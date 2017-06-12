using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.ComponentModel;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class GroupsController : Controller
    {
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRepository _repository;
        private readonly IGroupRepository _groupRepository;
        private readonly IFilteredUrl _filteredUrl;
        private readonly FeatureToggles _featureToggle;
        private readonly IViewRender _viewRender;

        public GroupsController(IProcessedContentRepository processedContentRepository, IRepository repository, IGroupRepository groupRepository, IFilteredUrl filteredUrl, FeatureToggles featureToggle, IViewRender viewRender)
        {
            _processedContentRepository = processedContentRepository;
            _repository = repository;
            _groupRepository = groupRepository;
            _filteredUrl = filteredUrl;
            _featureToggle = featureToggle;
            _viewRender = viewRender;
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

            if (model.Categories != null && model.Categories.Any())
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
            var response = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = response.Content as List<GroupCategory>;
            if (listOfGroupCategories != null)
            {
                groupSubmission.Categories = listOfGroupCategories.Select(logc => logc.Name).ToList();
            }

            return View(groupSubmission);
        }

        [HttpPost]
        [Route("/groups/add-a-group")]
        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        public async Task<IActionResult> AddAGroup(GroupSubmission groupSubmission)
        {
            var response = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = response.Content as List<GroupCategory>;
            if (listOfGroupCategories != null)
            {
                groupSubmission.Categories = listOfGroupCategories.Select(logc => logc.Name).ToList();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.SubmissionError = GetErrorsFromModelState(ModelState);
                return View(groupSubmission);
            }

            var successCode = await _groupRepository.SendEmailMessage(groupSubmission);
            if (successCode == HttpStatusCode.OK) return RedirectToAction("ThankYouMessage");

            ViewBag.SubmissionError = "There was a problem submitting the group, please try again.";

            return View(groupSubmission);
        }

        [HttpGet]
        [Route("/groups/exportpdf/{slug}")]
        public async Task<IActionResult> ExportPdf([FromServices] INodeServices nodeServices, string slug)
        {
            ViewBag.CurrentUrl = Request?.GetUri();

            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            var renderedExportStyles = _viewRender.Render("Shared/ExportStyles", string.Concat(Request?.Scheme, "://", Request?.Host));
            var renderedHtml = _viewRender.Render("Shared/GroupDetail", group);

            var result = await nodeServices.InvokeAsync<byte[]>("./pdf", string.Concat(renderedExportStyles, renderedHtml));

            HttpContext.Response.ContentType = "application/pdf";

            string filename = @"report.pdf";
            HttpContext.Response.Headers.Add("x-filename", filename);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "x-filename");
            HttpContext.Response.Body.Write(result, 0, result.Length);
            return new ContentResult();
        }

        [Route("/groups/thank-you-message")]
        public IActionResult ThankYouMessage()
        {
            return View();
        }

        [Route("/groups/manage")]
        public IActionResult Manage()
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var result = new GroupManagePage();

            var groups = new List<Tuple<string, string, string, string>>();
            groups.Add(new Tuple<string, string, string, string>("6th Altrincham Scouts Group 6th Altrincham Scouts Group 6th Altrincham Scouts Group 6th Altrincham Scouts Group", "Published", "green", "alt-6"));
            groups.Add(new Tuple<string, string, string, string>("7th Altrincham Scout Group", "Archived", "red", "alt-7"));
            groups.Add(new Tuple<string, string, string, string>("Little Bees Dance Group", "Published", "green", "little-bees"));
            groups.Add(new Tuple<string, string, string, string>("Pannal Sports Football Club", "Pending Deletion", "yellow", "pannal"));
            groups.Add(new Tuple<string, string, string, string>("Kersel Rugby Club", "Archived", "red", "kersel"));
            groups.Add(new Tuple<string, string, string, string>("Middleton Model Railway Club", "Published", "green", "trains"));

            result.Groups = groups;

            return View(result);
        }

        [Route("/groups/manage/{slug}")]
        public async Task<IActionResult> ManageGroup(string slug)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            var result = new ManageGroupViewModel
            {
                Name = group.Name,
                Slug = slug
            };

            return View(result);
        }

        [HttpGet]
        [Route("/groups/manage/{slug}/delete")]
        public async Task<IActionResult> Delete(string slug)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        [HttpPost]
        [Route("/groups/manage/{slug}/delete")]
        public async Task<IActionResult> DeleteGroup(string slug)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;
            var group = response.Content as ProcessedGroup;

            response = await _processedContentRepository.Delete<Group>(slug);

            if (!response.IsSuccessful()) return response;

            // TODO - Send emails

            return RedirectToAction("DeleteConfirmation", new { group = group.Name });
        }

        [Route("/groups/manage/deleteconfirmation")]
        public async Task<IActionResult> DeleteConfirmation(string group)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(group))
            {
                return NotFound();
            }

            ViewBag.GroupName = group;

            return View();
        }

        [Route("/groups/manage/{slug}/archive")]
        public async Task<IActionResult> Archive(string slug)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        [Route("/groups/manage/{slug}/archiveconfirmation")]
        public async Task<IActionResult> ArchiveConfirmation(string slug)
        {
            if (!_featureToggle.GroupManagement)
            {
                return NotFound();
            }

            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(group);
        }

        private void DoPagination(GroupResults groupResults, int currentPageNumber)
        {
            if (groupResults != null && groupResults.Groups.Any())
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
                if (state.Value.Errors.Count > 0)
                {
                    message.Append(state.Value.Errors.First().ErrorMessage + Environment.NewLine);
                }
            }
            return message.ToString();
        }
    }
}
