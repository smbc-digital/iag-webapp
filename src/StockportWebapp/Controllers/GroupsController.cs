using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebapp.Validation;
using StockportWebapp.ViewModels;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockportWebapp.Config;
using StockportWebapp.Exceptions;
using StockportWebapp.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class GroupsController : Controller
    {
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRepository _repository;
        private readonly GroupEmailBuilder _emailBuilder;
        private readonly IFilteredUrl _filteredUrl;
        private readonly FeatureToggles _featureToggle;
        private readonly IViewRender _viewRender;
        private readonly ILogger<GroupsController> _logger;
        private readonly List<Query> _managementQuery;
        private readonly IApplicationConfiguration _configuration;
      
        public GroupsController(IProcessedContentRepository processedContentRepository, IRepository repository, GroupEmailBuilder emailBuilder, IFilteredUrl filteredUrl, FeatureToggles featureToggle, IViewRender viewRender, ILogger<GroupsController> logger, IApplicationConfiguration configuration)
        {
            _processedContentRepository = processedContentRepository;
            _repository = repository;
            _filteredUrl = filteredUrl;
            _featureToggle = featureToggle;
            _viewRender = viewRender;
            _logger = logger;
            _configuration = configuration;
            _emailBuilder = emailBuilder;
            _managementQuery = new List<Query> { new Query("onlyActive", "false") };
        }

        [Route("/groups")]
        public async Task<IActionResult> Index()
        {
            var model = new GroupStartPage
            {
                PrimaryFilter = new PrimaryFilter
                {
                    Location = "Stockport",
                    Latitude = Defaults.Groups.StockportLatitude,
                    Longitude = Defaults.Groups.StockportLongitude
                }
            };

            var response = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = response.Content as List<GroupCategory>;

            if (listOfGroupCategories != null)
            {
                model.Categories = listOfGroupCategories;
                model.PrimaryFilter.Categories = listOfGroupCategories.OrderBy(c => c.Name).ToList();
            }

            return View(model);
        }

        [Route("/groups/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        [Route("groups/results")]
        public async Task<IActionResult> Results([FromQuery] string category, [FromQuery] int page, [FromQuery] double latitude, [FromQuery] double longitude, [FromQuery] string order = "", [FromQuery] string location = "Stockport")
        {
            var model = new GroupResults();
            var queries = new List<Query>();

            if (latitude != 0) queries.Add(new Query("latitude", latitude.ToString()));
            if (longitude != 0) queries.Add(new Query("longitude", longitude.ToString()));
            if (!string.IsNullOrEmpty(category)) queries.Add(new Query("Category", category == "all" ? "" : category));
            if (!string.IsNullOrEmpty(order)) queries.Add(new Query("Order", order));
            queries.Add(new Query("location", location));

            var response = await _repository.Get<GroupResults>(queries: queries);

            if (response.IsNotFound())
                return NotFound();

            model = response.Content as GroupResults;

            ViewBag.SelectedCategory = string.IsNullOrEmpty(category) ? "All" : (char.ToUpper(category[0]) + category.Substring(1)).Replace("-", " ");
            model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
            _filteredUrl.SetQueryUrl(model.CurrentUrl);
            model.AddFilteredUrl(_filteredUrl);

            DoPagination(model, page);

            if ((model.Categories != null) && model.Categories.Any())
            {
                ViewBag.Category = model.Categories.FirstOrDefault(c => c.Slug == category);
                model.PrimaryFilter.Categories = model.Categories.OrderBy(c => c.Name).ToList();
            }

            model.PrimaryFilter.Order = order;
            model.PrimaryFilter.Location = location;
            model.PrimaryFilter.Latitude = latitude != 0 ? latitude : Defaults.Groups.StockportLatitude;
            model.PrimaryFilter.Longitude = longitude != 0 ? longitude : Defaults.Groups.StockportLongitude;

            return View(model);
        }

        [Route("/groups/add-a-group")]
        public async Task<IActionResult> AddAGroup()
        {
            var groupSubmission = new GroupSubmission();

            groupSubmission.AvailableCategories = await GetAvailableGroupCategories();

            return View(groupSubmission);
        }

        [HttpPost]
        [Route("/groups/add-a-group")]
        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        public async Task<IActionResult> AddAGroup(GroupSubmission groupSubmission)
        {
            groupSubmission.AvailableCategories = await GetAvailableGroupCategories();

            if (!ModelState.IsValid)
            {
                ViewBag.SubmissionError = GetErrorsFromModelState(ModelState);
                return View(groupSubmission);
            }

            var successCode = await _emailBuilder.SendEmailAddNew(groupSubmission);
            if (successCode == HttpStatusCode.OK) return RedirectToAction("ThankYouMessage");

            ViewBag.SubmissionError = "There was a problem submitting the group, please try again.";

            return View(groupSubmission);
        }

        private async Task<List<string>> GetAvailableGroupCategories()
        {
            var response = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = response.Content as List<GroupCategory>;
            if (listOfGroupCategories != null)
                return listOfGroupCategories.Select(logc => logc.Name).OrderBy(c => c).ToList();

            return null;
        }

        [HttpGet]
        [Route("/groups/{slug}/change-group-info")]
        public ActionResult ChangeGroupInfo(string slug, string groupname)
        {
            var model = new ChangeGroupInfoViewModel
            {
                GroupName = groupname,
                Slug = slug
            };

            return View(model);
        }

        [HttpPost]
        [Route("/groups/{slug}/change-group-info")]
        public  IActionResult ChangeGroupInfo(string slug, ChangeGroupInfoViewModel submission)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubmissionError = GetErrorsFromModelState(ModelState);
                return View(submission);
            }

            var successCode = _emailBuilder.SendEmailChangeGroupInfo(submission).Result;
            if (successCode == HttpStatusCode.OK)
                return RedirectToAction("ChangeGroupInfoConfirmation", new { slug, groupName = submission.GroupName });

            ViewBag.SubmissionError = "There was a problem submitting the group, please try again.";

            return View(submission);
        }

        [Route("/groups/{slug}/change-group-info-confirmation")]
        public IActionResult ChangeGroupInfoConfirmation(string slug, string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                return NotFound();

            ViewBag.Slug = slug;
            ViewBag.GroupName = groupName;

            return View();
        }

        [HttpGet]
        [Route("/groups/exportpdf/{slug}")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> ExportPdf([FromServices] INodeServices nodeServices, [FromServices] CurrentEnvironment environment, string slug, [FromQuery] bool returnHtml = false)
        {
            _logger.LogInformation(string.Concat("Exporting group ", slug, " to pdf"));

            ViewBag.CurrentUrl = Request?.GetUri();

            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            var renderedExportStyles = _viewRender.Render("Shared/ExportStyles", _configuration.GetExportHost());
            var renderedHtml = _viewRender.Render("Shared/GroupDetail", group);
            var joinedHtml = string.Concat(renderedExportStyles, renderedHtml);

            // if raw html is requested, simply return the html instead
            if (returnHtml) return Content(joinedHtml, "text/html");

            var result = await nodeServices.InvokeAsync<byte[]>("./pdf", new { data = joinedHtml, delay = 1000 });

            if (result == null) _logger.LogError(string.Concat("Failed to export group ", slug, " to pdf"));

            return new FileContentResult(result, "application/pdf");
        }

        [Route("/groups/manage/{slug}/users")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> Users(string slug, LoggedInPerson loggedInPerson)
        {
            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            return View(group);
        }

        [HttpGet]
        [Route("/groups/manage/{slug}/newuser")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> NewUser(string slug, LoggedInPerson loggedInPerson)
        {
            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            var model = new AddEditUserViewModel
            {
                Slug = slug,
                Name = @group.Name
            };

            return View(model);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/newuser")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> NewUser(AddEditUserViewModel model, LoggedInPerson loggedInPerson)
        {
            var response = await _processedContentRepository.Get<Group>(model.Slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }
            else if (!ModelState.IsValid)
            {
                ViewBag.SubmissionError = GetErrorsFromModelState(ModelState);
            }
            else if (group.GroupAdministrators.Items.Any(u => u.Email.ToUpper() == model.GroupAdministratorItem.Email.ToUpper()))
            {
                ViewBag.SubmissionError = "Sorry, this email already exists for this group. You can only assign an email to a group once.";
            }
            else
            {
                var jsonContent = JsonConvert.SerializeObject(model.GroupAdministratorItem);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                response = await _repository.AddAdministrator(httpContent, model.Slug, model.GroupAdministratorItem.Email);
                if (!response.IsSuccessful()) return response;
                await _emailBuilder.SendEmailNewUser(model);
                return RedirectToAction("NewUserConfirmation", new { slug = model.Slug, name = model.GroupAdministratorItem.Name, groupName = group.Name });
            }

            return View(model);
        }

        [Route("/groups/manage/{slug}/newuserconfirmation")]
        public IActionResult NewUserConfirmation(string slug, string name, string groupName)
        {
            if (!_featureToggle.GroupManagement)
                return NotFound();

            if (string.IsNullOrWhiteSpace(groupName) || string.IsNullOrWhiteSpace(name))
                return NotFound();

            ViewBag.Slug = slug;
            ViewBag.Name = name;
            ViewBag.GroupName = groupName;

            return View();
        }

        [HttpGet]
        [Route("/groups/manage/{slug}/edituser")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> EditUser(string slug, string email, LoggedInPerson loggedInPerson)
        {
            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            var groupAdministrator = group.GroupAdministrators.Items.FirstOrDefault(i => i.Email == email);
            if (groupAdministrator == null)
                return NotFound();

            var model = new AddEditUserViewModel
            {
                Slug = slug,
                Name = @group.Name,
                GroupAdministratorItem = groupAdministrator,
                Previousrole = groupAdministrator.Permission
            };

            return View(model);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/edituser")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> EditUser(AddEditUserViewModel model, LoggedInPerson loggedInPerson)
        {
            var response = await _processedContentRepository.Get<Group>(model.Slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            var administratorItem = group.GroupAdministrators.Items.Where(i => i.Email == model.GroupAdministratorItem.Email);

            if (!administratorItem.Any() || !HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }
            else if (!ModelState.IsValid)
            {
                ViewBag.SubmissionError = GetErrorsFromModelState(ModelState);
            }
            else
            {
                var jsonContent = JsonConvert.SerializeObject(model.GroupAdministratorItem);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                
                response = await _repository.UpdateAdministrator(httpContent, model.Slug, model.GroupAdministratorItem.Email);
                if (!response.IsSuccessful()) return response;
                await _emailBuilder.SendEmailEditUser(model);
                return RedirectToAction("EditUserConfirmation", new { slug = model.Slug, name = model.GroupAdministratorItem.Name, groupName = group.Name });
            }
            
            return View(model);
        }

        [Route("/groups/manage/{slug}/edituserconfirmation")]
        public  IActionResult EditUserConfirmation(string slug, string name, string groupName)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(groupName) || string.IsNullOrWhiteSpace(name))
            {
                return NotFound();
            }

            ViewBag.Slug = slug;
            ViewBag.Name = name;
            ViewBag.GroupName = groupName;

            return View();
        }

        [HttpGet]
        [Route("/groups/manage/{slug}/removeuser")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> Remove(string slug, string email, LoggedInPerson loggedInPerson)
        {
            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            var groupAdministrator = group.GroupAdministrators.Items.FirstOrDefault(i => i.Email == email);
            if (groupAdministrator == null)
            {
                return NotFound();
            }

            var model = new RemoveUserViewModel()
            {
                Slug = slug,
                Email = email,
                Name = groupAdministrator.Name,
                GroupName = group.Name,
            };

            return View(model);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/removeuser")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> RemoveUser(RemoveUserViewModel model, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(model.Slug, _managementQuery);

            if (!response.IsSuccessful()) return response;
            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            response = await _repository.RemoveAdministrator(model.Slug, model.Email);

            if (!response.IsSuccessful()) return response;

            await _emailBuilder.SendEmailDeleteUser(model);
            return RedirectToAction("RemoveUserConfirmation", new { group = model.GroupName, slug = model.Slug, name = model.Name });
        }

        [Route("/groups/manage/removeconfirmation")]
        public IActionResult RemoveUserConfirmation(string group, string slug, string name)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(group))
            {
                return NotFound();
            }

            var model = new RemoveUserViewModel()
            {
                Slug = slug,
                Name = name,
                GroupName = group,
            };           

            return View(model);
        }

        [Route("/groups/thank-you-message")]
        public IActionResult ThankYouMessage()
        {
            return View();
        }

        [Route("/groups/manage")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> Manage(LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement || (loggedInPerson == null))
            {
                return NotFound();
            }

            var response = await _repository.GetAdministratorsGroups(loggedInPerson.Email);

            if (!response.IsSuccessful()) return response;

            var groups = response.Content as List<Group>;

            var result = new GroupManagePage
            {
                Groups = groups,
                Email = loggedInPerson.Email
            };

            return View(result);
        }

        [Route("/groups/manage/{slug}")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> ManageGroup(string slug, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            {
                return NotFound();
            }

           var result = new ManageGroupViewModel
            {
                Name = group.Name,
                Slug = slug,
                Administrator = HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"),
               IsArchived = DateNowIsNotBetweenHiddenRange(group.DateHiddenFrom, group.DateHiddenTo)
        };

            return View(result);
        }

        [Route("/groups/manage/{slug}/delete")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> Delete(string slug, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/delete")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> DeleteGroup(string slug, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;
            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            response = await _repository.Delete<Group>(slug);

            if (!response.IsSuccessful()) return response;

            _emailBuilder.SendEmailDelete(group);
           return RedirectToAction("DeleteConfirmation", new { group = group.Slug });
        }

        [Route("/groups/manage/deleteconfirmation")]
        public IActionResult DeleteConfirmation(string group)
        {
            if (!_featureToggle.GroupManagement)
                return NotFound();

            if (string.IsNullOrWhiteSpace(group))
                return NotFound();

            ViewBag.GroupName = group;

            return View();
        }

        [Route("/groups/manage/{slug}/archive")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> Archive(string slug, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/archive")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> ArchiveGroup(string slug, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;
            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            group.DateHiddenFrom = DateTime.Now;
            group.DateHiddenTo = null;

            var jsonContent = JsonConvert.SerializeObject(group);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var putResponse = await _repository.Archive<Group>(httpContent, slug);

            if (putResponse.StatusCode == (int)HttpStatusCode.OK)
            {
                _emailBuilder.SendEmailArchive(group);
                return RedirectToAction("ArchiveConfirmation", new { group = group.Slug });
            }
            else
            {
                throw new ContentfulUpdateException($"There was an error updating the group{group.Name}");
            }
        }

        [Route("/groups/manage/{slug}/archiveconfirmation")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> ArchiveConfirmation(string slug, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        [Route("/groups/manage/{slug}/publish")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> Publish(string slug, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/publish")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> PublishGroup(string slug, LoggedInPerson loggedInPerson)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;
            var group = response.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            group.DateHiddenFrom = DateTime.MaxValue;
            group.DateHiddenTo = DateTime.MaxValue; ;

            var jsonContent = JsonConvert.SerializeObject(group);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var putResponse = await _repository.Publish<Group>(httpContent, slug);

            if (putResponse.StatusCode == (int)HttpStatusCode.OK)
            {
                _emailBuilder.SendEmailPublish(group);
                return RedirectToAction("PublishConfirmation", new { slug = group.Slug, name = group.Name });
            }
            else
            {
                throw new ContentfulUpdateException($"There was an error updating the group{group.Name}");
            }
        }

        [Route("/groups/manage/publishconfirmation")]
        public IActionResult PublishConfirmation(string slug, string name)
        {
            if (!_featureToggle.GroupManagement)
                return NotFound();

            if (string.IsNullOrWhiteSpace(slug))
                return NotFound();

            ViewBag.GroupName = name;

            ViewBag.Slug = slug;

            return View();
        }

        [HttpGet]
        [Route("/groups/manage/{slug}/update")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> EditGroup(string slug, LoggedInPerson loggedInPerson)
        {
            var response = await _repository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as Group;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            {
                return NotFound();
            }

            var model = new GroupSubmission();
            model.Address = group.Address;
            model.Categories = group.CategoriesReference.Select(g => g.Name).ToList();
            model.CategoriesList = string.Join(",", model.Categories);
            model.Description = group.Description;
            model.Email = group.Email;
            model.Facebook = group.Facebook;
            model.Name = group.Name;
            model.PhoneNumber = group.PhoneNumber;
            model.Twitter = group.Twitter;
            model.Website = group.Website;
            model.Slug = group.Slug;
            model.Longitude = group.MapPosition.Lon;
            model.Latitude = group.MapPosition.Lat;
            model.Volunteering = group.Volunteering;

            model.AvailableCategories = await GetAvailableGroupCategories();

           return View(model);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/update")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> EditGroup(string slug, GroupSubmission model, LoggedInPerson loggedInPerson)
        {
            var response = await _repository.Get<Group>(slug, _managementQuery);
            var validationErrors = new StringBuilder();
            ViewBag.DisplayContentapiUpadteError = false;
            if (!response.IsSuccessful()) return response;

            var group = response.Content as Group;

            model.AvailableCategories = await GetAvailableGroupCategories();
            model.Slug = group.Slug;

            var categoryResponse = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = categoryResponse.Content as List<GroupCategory>;
            if (listOfGroupCategories != null)
            {
                model.Categories = listOfGroupCategories.Select(logc => logc.Name).ToList();
            }

            group.Address = model.Address;
            group.Description = model.Description;
            group.Email = model.Email;
            group.Facebook = model.Facebook;
            group.Name = model.Name;
            group.PhoneNumber = model.PhoneNumber;
            group.Twitter = model.Twitter;
            group.Website = model.Website;
            group.Volunteering = model.Volunteering;
            group.MapPosition = new MapPosition { Lon = model.Longitude, Lat = model.Latitude };

            group.CategoriesReference = new List<GroupCategory>();
            group.CategoriesReference.AddRange(listOfGroupCategories.Where(c => model.CategoriesList.Split(',').Contains(c.Name)));

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            {
                return NotFound();
            }
            else if (!ModelState.IsValid)
            {
                validationErrors.Append(GetErrorsFromModelState(ModelState));
                
            }
            else
            {
                var jsonContent = JsonConvert.SerializeObject(group);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var putResponse = await _repository.Put<Group>(httpContent, slug);

                if (putResponse.StatusCode == (int)HttpStatusCode.OK)
                {
                    _emailBuilder.SendEmailEditGroup(model, loggedInPerson.Email);
                    return RedirectToAction("EditGroupConfirmation", new {slug = slug, groupName = group.Name});
                }
                else
                {
                    _logger.LogError($"There was an error updating the group {group.Name}");
                    ViewBag.DisplayContentapiUpadteError = true;
                }
            }

            ViewBag.SubmissionError = validationErrors.Length > 0 ? validationErrors : null;

            return View(model);
        }

        [Route("/groups/manage/{slug}/updateconfirmation")]
        public IActionResult EditGroupConfirmation(string slug, string groupName)
        {
            if (!_featureToggle.GroupManagement)
                return NotFound();

            if (string.IsNullOrWhiteSpace(groupName))
                return NotFound();

            ViewBag.Slug = slug;
            ViewBag.GroupName = groupName;

            return View();
        }

        private void DoPagination(GroupResults groupResults, int currentPageNumber)
        {
            if ((groupResults != null) && groupResults.Groups.Any())
            {
                var paginatedGroups = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                    groupResults.Groups,
                    currentPageNumber,
                    "groups",
                    9);

                groupResults.Groups = paginatedGroups.Items;
                groupResults.Pagination = paginatedGroups.Pagination;
                groupResults.Pagination.CurrentUrl = groupResults.CurrentUrl;
            }
            else
            {
                groupResults.Pagination = new Pagination();
            }
        }
        

        private string GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            var message = new StringBuilder();

            foreach (var state in modelState)
            {
                if (state.Value.Errors.Count > 0 && state.Key != "Email")
                {
                    message.Append(state.Value.Errors.First().ErrorMessage + Environment.NewLine);
                }
            }

            return message.ToString();
        }

        public bool DateNowIsNotBetweenHiddenRange(DateTime? hiddenFrom, DateTime? hiddenTo)
        {
            var now = DateTime.Now;
            return hiddenFrom > now || (hiddenTo < now && hiddenTo != DateTime.MinValue) || (hiddenFrom == DateTime.MinValue && hiddenTo == DateTime.MinValue) || (hiddenFrom == null && hiddenTo == null);
        }

        public bool HasGroupPermission(string email, List<GroupAdministratorItems> groupAdministrators, string permission = "E")
        {
            var userPermission = groupAdministrators.FirstOrDefault(a => a.Email.ToUpper() == email.ToUpper())?.Permission;

            if ((userPermission == permission) || (userPermission == "A"))
            {
                return true;
            }

            return false;
        }
    }
}
