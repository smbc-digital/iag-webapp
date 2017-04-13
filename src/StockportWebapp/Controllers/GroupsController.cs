using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class GroupsController : Controller
    {
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRepository _repository;
        private readonly IGroupRepository _groupRepository;
        private readonly IFilteredUrl _filteredUrl;

        FeatureToggles _featuretoggles;

        public GroupsController(IProcessedContentRepository processedContentRepository, IRepository repository, IGroupRepository groupRepository,
            FeatureToggles featureToggles, IFilteredUrl filteredUrl)
        {
            _processedContentRepository = processedContentRepository;
            _repository = repository;
            _featuretoggles = featureToggles;
            _groupRepository = groupRepository;
            _filteredUrl = filteredUrl;
        }

        [Route("/groups")]
        public async Task<IActionResult> Index()
        {
            if (_featuretoggles.GroupStartPage)
            {
                GroupStartPage model = new GroupStartPage();
                var response = await _repository.Get<List<GroupCategory>>();
                var listOfGroupCategories = response.Content as List<GroupCategory>;

                if (listOfGroupCategories != null)
                {
                    model.Categories = listOfGroupCategories;
                }

                return View(model);
            }

            return NotFound();

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
        public async Task<IActionResult> Results([FromQuery] string category, [FromQuery] int page,[FromQuery] double lat,[FromQuery] double lon, [FromQuery] string order)
        {
            if (_featuretoggles.GroupResultsPage)
            {
                GroupResults model = new GroupResults();
                var queries = new List<Query>();
                if (!string.IsNullOrEmpty(category)) queries.Add(new Query("Category", category));
                if (lat != 0) queries.Add(new Query("Lat", lat.ToString()));
                if (lon != 0) queries.Add(new Query("Lon", lon.ToString()));
                if (!string.IsNullOrEmpty(order)) queries.Add(new Query("Order", order));
                var response = await _repository.Get<GroupResults>(queries: queries);

                if (response.IsNotFound())
                    return NotFound();

                model = response.Content as GroupResults;

                model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
                _filteredUrl.SetQueryUrl(model.CurrentUrl);
                model.AddFilteredUrl(_filteredUrl);

                DoPagination(model, page);

                if (model.Categories != null && model.Categories.Any())
                    ViewBag.Category = model.Categories.FirstOrDefault(c => c.Slug == category);

                return View(model);
            }

            return NotFound();
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
        public async Task<IActionResult> AddAGroup(GroupSubmission groupSubmission)
        {
            var response = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = response.Content as List<GroupCategory>;
            if (listOfGroupCategories != null)
            {
                groupSubmission.Categories = listOfGroupCategories.Select(logc => logc.Name).ToList();
            }
            
            if (!ModelState.IsValid) return View(groupSubmission);

            var successCode = await _groupRepository.SendEmailMessage(groupSubmission);
            if (successCode == HttpStatusCode.OK) return RedirectToAction("ThankYouMessage");

            ViewBag.SubmissionError = "There was a problem submitting the group, please try again.";

            return View(groupSubmission);
        }

        [Route("/groups/thank-you-message")]
        public IActionResult ThankYouMessage()
        {
            return View();
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
    }
}
