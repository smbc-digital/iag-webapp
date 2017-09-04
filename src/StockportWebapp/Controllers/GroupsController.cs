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
using ReverseMarkdown;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class GroupsController : Controller
    {
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRepository _repository;
        private readonly GroupEmailBuilder _emailBuilder;
        private readonly EventEmailBuilder _eventEmailBuilder;
        private readonly IFilteredUrl _filteredUrl;
        private readonly IViewRender _viewRender;
        private readonly ILogger<GroupsController> _logger;
        private readonly List<Query> _managementQuery;
        private readonly IApplicationConfiguration _configuration;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly ViewHelpers _viewHelpers;
        private readonly IDateCalculator _dateCalculator;
        private readonly FavouritesHelper favouritesHelper;

        public GroupsController(IProcessedContentRepository processedContentRepository, IRepository repository, GroupEmailBuilder emailBuilder, EventEmailBuilder eventEmailBuilder, IFilteredUrl filteredUrl, IViewRender viewRender, ILogger<GroupsController> logger, IApplicationConfiguration configuration, MarkdownWrapper markdownWrapper, ViewHelpers viewHelpers, IDateCalculator dateCalculator, IHttpContextAccessor httpContextAccessor)
        {
            _processedContentRepository = processedContentRepository;
            _repository = repository;
            _filteredUrl = filteredUrl;
            _viewRender = viewRender;
            _logger = logger;
            _configuration = configuration;
            _emailBuilder = emailBuilder;
            _eventEmailBuilder = eventEmailBuilder;
            _managementQuery = new List<Query> { new Query("onlyActive", "false") };
            _markdownWrapper = markdownWrapper;
            _viewHelpers = viewHelpers;
            _dateCalculator = dateCalculator;
            favouritesHelper = new FavouritesHelper(httpContextAccessor);
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

        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("/groups/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            favouritesHelper.PopulateFavourites(new List<ProcessedGroup>
            {
                group
            });

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("groups/results")]
        public async Task<IActionResult> Results([FromQuery] string category, [FromQuery] int page, [FromQuery] double latitude, [FromQuery] int pageSize, [FromQuery] double longitude, [FromQuery] string order = "", [FromQuery] string location = "Stockport")
        {
            var model = new GroupResults();
            var queries = new List<Query>();

            if (latitude != 0) queries.Add(new Query("latitude", latitude.ToString()));
            if (longitude != 0) queries.Add(new Query("longitude", longitude.ToString()));
            if (!string.IsNullOrEmpty(category)) queries.Add(new Query("Category", category == "all" ? "" : category));
            if (!string.IsNullOrEmpty(order)) queries.Add(new Query("Order", order));
            if (!string.IsNullOrEmpty(location)) queries.Add(new Query("location", location));

            var response = await _repository.Get<GroupResults>(queries: queries);

            if (response.IsNotFound())
                return NotFound();

            model = response.Content as GroupResults;

            ViewBag.SelectedCategory = string.IsNullOrEmpty(category) ? "All" : (char.ToUpper(category[0]) + category.Substring(1)).Replace("-", " ");
            model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
            _filteredUrl.SetQueryUrl(model.CurrentUrl);
            model.AddFilteredUrl(_filteredUrl);

            DoPagination(model, page, pageSize);

            if ((model.Categories != null) && model.Categories.Any())
            {
                ViewBag.Category = model.Categories.FirstOrDefault(c => c.Slug == category);
                model.PrimaryFilter.Categories = model.Categories.OrderBy(c => c.Name).ToList();
            }

            favouritesHelper.PopulateFavourites(model.Groups);

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
            {
                return listOfGroupCategories.Select(logc => logc.Name).OrderBy(c => c).ToList();
            }

            return null;
        }

        private async Task<List<string>> GetAvailableEventCategories()
        {
            var response = await _repository.Get<List<EventCategory>>();
            var listOfEventCategories = response.Content as List<EventCategory>;
            if (listOfEventCategories != null)
            {
                return listOfEventCategories.Select(logc => logc.Name).OrderBy(c => c).ToList();
            }

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
        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
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

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet]
        [Route("/groups/exportpdf/{slug}")]
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

        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("/groups/manage")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> Manage(LoggedInPerson loggedInPerson)
        {
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



        [Route("/groups/manage/{groupSlug}/events/{eventSlug}/delete")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> DeleteEvent(string groupSlug, string eventSlug, LoggedInPerson loggedInPerson)
        {
            var eventResponse = await _processedContentRepository.Get<Event>(eventSlug, _managementQuery);
            var groupResponse = await _processedContentRepository.Get<Group>(groupSlug, _managementQuery);

            if (!eventResponse.IsSuccessful()) return eventResponse;

            var eventItem = eventResponse.Content as ProcessedEvents;
            var group = groupResponse.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            ViewBag.CurrentUrl = Request?.GetUri();
            ViewBag.GroupName = group.Name;
            ViewBag.GroupSlug = group.Slug;

            return View(eventItem);
        }

        [HttpPost]
        [Route("/groups/manage/{groupSlug}/events/{eventSlug}/delete")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> DeleteAnEvent(string groupSlug, string eventSlug, LoggedInPerson loggedInPerson)
        {
            var eventResponse = await _processedContentRepository.Get<Event>(eventSlug, _managementQuery);
            var groupResponse = await _processedContentRepository.Get<Group>(groupSlug, _managementQuery);

            if (!eventResponse.IsSuccessful()) return eventResponse;

            var eventItem = eventResponse.Content as ProcessedEvents;
            var group = groupResponse.Content as ProcessedGroup;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            {
                return NotFound();
            }

            var deleteResponse = await _repository.Delete<Event>(eventSlug);

            if (!deleteResponse.IsSuccessful()) return deleteResponse;

            _emailBuilder.SendEmailEventDelete(eventItem, group);
            return RedirectToAction("DeleteEventConfirmation", new { eventName = eventItem.Title, eventSlug = eventItem.Slug, groupSlug = group.Slug, groupName = group.Name  });
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/delete")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> DeleteGroup(string slug, LoggedInPerson loggedInPerson)
        {
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
           return RedirectToAction("DeleteConfirmation", new { group = group.Name });
        }

        [Route("/groups/manage/deleteconfirmation")]
        public IActionResult DeleteConfirmation(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                return NotFound();

            ViewBag.GroupName = group;

            return View();
        }

        [Route("/groups/manage/deleteeventconfirmation")]
        public IActionResult DeleteEventConfirmation(string eventName, string eventSlug, string groupSlug, string groupName)
        {
            if (string.IsNullOrWhiteSpace(eventName))
                return NotFound();

            ViewBag.EventName = eventName;
            ViewBag.GroupName = groupName;
            ViewBag.GroupSlug = groupSlug;
            ViewBag.EventSlug = eventSlug;

            return View();
        }

        [Route("/groups/manage/{slug}/archive")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> Archive(string slug, LoggedInPerson loggedInPerson)
        {
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
            model.Description = _markdownWrapper.ConvertToHtml(group.Description);
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
            model.Categories = group.CategoriesReference.Select(g => g.Name).ToList();
            model.CategoriesList = string.Join(",", model.Categories);
            model.VolunteeringText = GetVolunteeringText(group.VolunteeringText);
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
            ViewBag.DisplayContentapiUpdateError = false;
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

            var converter = new Converter();

            group.Address = model.Address;
            group.Description = converter.Convert(_viewHelpers.StripUnwantedHtml(model.Description));
            group.Email = model.Email;
            group.Facebook = model.Facebook;
            group.Name = model.Name;
            group.PhoneNumber = model.PhoneNumber;
            group.Twitter = model.Twitter;
            group.Website = model.Website;
            group.Volunteering = model.Volunteering;
            group.MapPosition = new MapPosition { Lon = model.Longitude, Lat = model.Latitude };
            group.Volunteering = model.Volunteering;

           

            group.VolunteeringText = GetVolunteeringText(model.VolunteeringText);

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
                    ViewBag.DisplayContentapiUpdateError = true;
                }
            }

            ViewBag.SubmissionError = validationErrors.Length > 0 ? validationErrors : null;

            return View(model);
        }


        [HttpGet]
        [Route("/groups/favourites/clearall")]
        public IActionResult FavouriteGroupsClearAll()
        {
            var model = new Favourites
            {
                Type = "groups",
                Crumbs = new List<Crumb> { new Crumb("Find a local group", "groups", "groups") },
                FavouritesUrl = "/groups/favourites"
            };

            return View("~/views/stockportgov/favourites/confirmclearall.cshtml", model);
        }

        [HttpPost]
        [Route("/groups/favourites/clearall")]
        public IActionResult FavouriteGroupsClearAll(Favourites model)
        {
            favouritesHelper.RemoveAllFromFavourites<Group>();
            return RedirectToAction("FavouriteGroups");
        }

        private async Task<Http.HttpResponse> GetFavouriteGroupResults()
        {
            var model = new GroupResults();
            var queries = new List<Query>();

            var favouritesList = favouritesHelper.GetFavourites<Group>();
            var favourites = "-NO-FAVOURITES-SET-";
            if (favouritesList != null && favouritesList.Any())
            {
                favourites = string.Join(",", favouritesHelper.GetFavourites<Group>());
            }

            queries.Add(new Query("slugs", favourites));

            return await _repository.Get<GroupResults>(queries: queries);
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet]
        [Route("/groups/favourites")]
        public async Task<IActionResult> FavouriteGroups([FromQuery] int page, [FromQuery] int pageSize)
        {
            var response = await GetFavouriteGroupResults();

            if (response.IsNotFound())
            {
                return NotFound();
            }

            var model = response.Content as GroupResults;

            model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
            _filteredUrl.SetQueryUrl(model.CurrentUrl);
            model.AddFilteredUrl(_filteredUrl);

            favouritesHelper.PopulateFavourites(model.Groups);

            //Temporarily removed pagination
            //DoPagination(model, page, pageSize);

            if (pageSize == -1)
            {
                return View("FavouriteGroupsPrint", model);
            }
            else
            {
                return View(model);
            }
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpGet]
        [Route("/groups/exportpdf/favourites")]
        public async Task<IActionResult> FavouriteGroupsPDF([FromServices] INodeServices nodeServices)
        {
            _logger.LogInformation("Exporting group favourites to pdf");

            var response = await GetFavouriteGroupResults();

            if (response.IsNotFound()) return NotFound();

            var model = response.Content as GroupResults;

            var groupList = model.Groups;

            //Temporarily removed pagination
            //DoPagination(model, 0, -1);

            var renderedExportStyles = _viewRender.Render("Shared/ExportStyles", _configuration.GetExportHost());
            var renderedHtml = _viewRender.Render("Shared/Groups/FavouriteGroupsPrint", groupList);

            var result = await nodeServices.InvokeAsync<byte[]>("./pdf", new { data = string.Concat(renderedExportStyles, renderedHtml), delay = 1000 });

            if (result == null) _logger.LogError("Failed to export group favourites to pdf");

            return new FileContentResult(result, "application/pdf");
        }

        [HttpGet]
        [Route("/groups/manage/{groupslug}/events/{eventslug}/update")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> EditEvent(string groupslug, string eventslug, LoggedInPerson loggedInPerson)
        {
            var response = await _repository.Get<Group>(groupslug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as Group;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            {
                return NotFound();
            }

            var eventResponse = await _repository.Get<Event>(eventslug, _managementQuery);

            if (!response.IsSuccessful()) return eventResponse;

            var eventDetail = eventResponse.Content as Event;

            var model = new EventSubmission();

            model.AvailableCategories = await GetAvailableEventCategories();

            if (eventDetail.EventCategories.Any())
            {
                model.CategoriesList = eventDetail.EventCategories[0].Name;

                if (eventDetail.EventCategories.Count() > 1)
                {
                    model.CategoriesList += $",{eventDetail.EventCategories[1].Name}";
                }

                if (eventDetail.EventCategories.Count() > 2)
                {
                    model.CategoriesList += $",{eventDetail.EventCategories[2].Name}";
                }
            }

            model.Description = _markdownWrapper.ConvertToHtml(eventDetail.Description);
            if (eventDetail.EventFrequency != EventFrequency.None)
            {
                model.EndDate = _dateCalculator.GetEventEndDate(eventDetail);
            }

            model.EndTime = DateTime.ParseExact(eventDetail.EndTime, "HH:mm", null);
            model.EventDate = eventDetail.EventDate;
            model.Fee = eventDetail.Fee;
            model.Frequency = eventDetail.EventFrequency.ToString();
            model.Location = eventDetail.Location;
            model.RecurringEventYn = eventDetail.EventFrequency == EventFrequency.None ? "No" : "Yes";
            model.StartTime = DateTime.ParseExact(eventDetail.StartTime, "HH:mm", null);
            model.Title = eventDetail.Title;
            model.SubmittedBy = eventDetail.SubmittedBy;
            model.SubmitterEmail = loggedInPerson.Email;

            model.Slug = eventDetail.Slug;
            model.GroupName = group.Name;
            model.GroupSlug = group.Slug;

            return View(model);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/events/{eventslug}/update")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> EditEvent(string slug, string eventslug, EventSubmission model, LoggedInPerson loggedInPerson)
        {
            var response = await _repository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as Group;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            {
                return NotFound();
            }

            var eventResponse = await _repository.Get<Event>(eventslug, _managementQuery);
            var validationErrors = new StringBuilder();
            ViewBag.DisplayContentapiUpdateError = false;

            if (!eventResponse.IsSuccessful()) return eventResponse;

            var eventDetail = eventResponse.Content as Event;

            model.Slug = eventDetail.Slug;
            model.GroupSlug = group.Slug;
            model.GroupName = group.Name;

            var converter = new Converter();

            eventDetail.Title = model.Title;
            eventDetail.Description = converter.Convert(_viewHelpers.StripUnwantedHtml(model.Description));
            eventDetail.EndTime = ((DateTime)model.EndTime).ToString("HH:mm");
            eventDetail.EventDate = (DateTime)model.EventDate;
            eventDetail.Fee = model.Fee;
            eventDetail.Location = model.Location;

            if (model.RecurringEventYn == "Yes")
            {
                switch (model.Frequency)
                {
                    case "Daily":
                        eventDetail.EventFrequency = EventFrequency.Daily;
                        break;
                    case "Fortnightly":
                        eventDetail.EventFrequency = EventFrequency.Fortnightly;
                        break;
                    case "Weekly":
                        eventDetail.EventFrequency = EventFrequency.Weekly;
                        break;
                    case "Monthly":
                        eventDetail.EventFrequency = EventFrequency.Monthly;
                        break;
                    case "MonthlyDate":
                        eventDetail.EventFrequency = EventFrequency.MonthlyDate;
                        break;
                    case "MonthlyDay":
                        eventDetail.EventFrequency = EventFrequency.MonthlyDay;
                        break;
                    case "Yearly":
                        eventDetail.EventFrequency = EventFrequency.Yearly;
                        break;
                    default:
                        eventDetail.EventFrequency = EventFrequency.None;
                        break;
                }

                eventDetail.Occurences = _dateCalculator.GetEventOccurences(eventDetail.EventFrequency, (DateTime)model.EventDate, (DateTime)model.EndDate);
            }
            else
            {
                eventDetail.EventFrequency = EventFrequency.None;
                eventDetail.Occurences = 1;
            }

            eventDetail.StartTime = model.StartTime?.ToString("HH:mm");
            eventDetail.UpdatedAt = DateTime.Now;

            model.Occurrences = eventDetail.Occurences;

            var categoryResponse = await _repository.Get<List<EventCategory>>();
            var listOfEventCategories = categoryResponse.Content as List<EventCategory>;

            if (!string.IsNullOrEmpty(model.CategoriesList))
            {
                eventDetail.EventCategories = listOfEventCategories.Where(c => model.CategoriesList.Split(',').Contains(c.Name)).ToList();
            }

            model.AvailableCategories = await GetAvailableEventCategories();

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
                var jsonContent = JsonConvert.SerializeObject(eventDetail);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var putResponse = await _repository.Put<Event>(httpContent, eventslug);

                if (putResponse.StatusCode == (int)HttpStatusCode.OK)
                {
                    _eventEmailBuilder.SendEmailEditEvent(model, loggedInPerson.Email);
                    return RedirectToAction("EditEventConfirmation", new { groupslug = group.Slug, groupName = group.Name, eventslug = eventDetail.Slug, eventname = eventDetail.Title });
                }
                else
                {
                    _logger.LogError($"There was an error updating the event {eventDetail.Title}");
                    ViewBag.DisplayContentapiUpdateError = true;
                }
            }

            ViewBag.SubmissionError = validationErrors.Length > 0 ? validationErrors : null;

            return View(model);
        }

        [Route("/groups/manage/{slug}/updateconfirmation")]
        public IActionResult EditGroupConfirmation(string slug, string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                return NotFound();

            ViewBag.Slug = slug;
            ViewBag.GroupName = groupName;

            return View();
        }

        [Route("/groups/manage/{groupslug}/events/{eventslug}/updateconfirmation")]
        public IActionResult EditEventConfirmation(string groupslug, string eventslug, string groupName, string eventName)
        {
            if (string.IsNullOrWhiteSpace(groupName) || string.IsNullOrWhiteSpace(eventName))
                return NotFound();

            ViewBag.Slug = eventslug;
            ViewBag.EventName = eventName;
            ViewBag.GroupSlug = groupslug;
            ViewBag.GroupName = groupName;

            return View();
        }

        [HttpGet]
        [Route("/groups/manage/{slug}/events")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> ViewGroupsEvents(string slug, LoggedInPerson loggedInPerson)
        {
            var response = await _repository.Get<Group>(slug, _managementQuery);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as Group;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            {
                return NotFound();
            }

            return View(group);
        }


        [HttpGet]
        [Route("/groups/manage/{groupSlug}/events/{eventSlug}")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> GroupEventsDetails(string groupSlug, string eventSlug, LoggedInPerson loggedInPerson)
        {
            var responseEvent = await _repository.Get<Event>(eventSlug, _managementQuery);
            var responseGroup = await _repository.Get<Group>(groupSlug, _managementQuery);

            if (!responseEvent.IsSuccessful()) return responseEvent;
            if (!responseGroup.IsSuccessful()) return responseGroup;

            var eventItem = responseEvent.Content as Event;
            var group = responseGroup.Content as Group;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            {
                return NotFound();
            }

            ViewBag.GroupName = group.Name;
            ViewBag.GroupSlug = group.Slug;

            return View(eventItem);
        }

        [HttpGet]
        [Route("/groups/manage/{groupSlug}/events/add-your-event")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        public async Task<IActionResult> AddYourEvent(string groupSlug, LoggedInPerson loggedInPerson)
        {
            var responseGroup = await _repository.Get<Group>(groupSlug, _managementQuery);
            if (!responseGroup.IsSuccessful()) return responseGroup;
            var group = responseGroup.Content as Group;

            if (!HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            {
                return NotFound();
            }
                       
            var eventSubmission = new EventSubmission();
            eventSubmission.GroupName = group.Name;
            eventSubmission.GroupSlug = group.Slug;
            eventSubmission.SubmittedBy = loggedInPerson.Name;
            eventSubmission.SubmitterEmail = loggedInPerson.Email;

            return View("Add-Your-Event", eventSubmission);
        }

        [HttpPost]
        [Route("/groups/manage/{groupSlug}/events/add-your-event")]
        [ServiceFilter(typeof(GroupAuthorisation))]
        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        public async Task<IActionResult> AddYourEvent(EventSubmission eventSubmission)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubmissionError = GetErrorsFromModelState(ModelState);
                return View("Add-Your-Event", eventSubmission);
            }

            Enum.TryParse(eventSubmission.Frequency, out EventFrequency frequency);
            if (frequency != EventFrequency.None)
            {
                eventSubmission.Occurrences = _dateCalculator.GetEventOccurences(frequency, (DateTime)eventSubmission.EventDate, (DateTime)eventSubmission.EndDate);
            }

            var successCode = await _eventEmailBuilder.SendEmailAddNew(eventSubmission);
            if (successCode == HttpStatusCode.OK) return RedirectToAction("EventsThankYouMessage", eventSubmission);

            ViewBag.SubmissionError = "There was a problem submitting the event, please try again.";

            return View("Add-Your-Event", eventSubmission);
        }

        [Route("/groups/events-thank-you-message")]
        public IActionResult EventsThankYouMessage(EventSubmission eventSubmission)
        {
            return View(eventSubmission);
        }

        private void DoPagination(GroupResults groupResults, int currentPageNumber ,int pageSize)
        {
            if ((groupResults != null) && groupResults.Groups.Any())
            {
                var paginatedGroups = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                    groupResults.Groups,
                    currentPageNumber,
                    "groups",
                    pageSize,
                    _configuration.GetGroupsDefaultPageSize("stockportgov"));

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

        private string GetVolunteeringText(string volunteeringText)
        {
            return string.IsNullOrEmpty(volunteeringText) ? "If you would like to find out more about being a volunteer with us, please e-mail with your interest and we’ll be in contact as soon as possible." : volunteeringText;
        }
    }
}
