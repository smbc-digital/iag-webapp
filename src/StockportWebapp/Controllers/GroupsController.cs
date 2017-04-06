using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class GroupsController : Controller
    {
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRepository _repository;
        FeatureToggles _featuretoggles;

        public GroupsController(IProcessedContentRepository processedContentRepository, IRepository repository,
            FeatureToggles featureToggles)
        {
            _processedContentRepository = processedContentRepository;
            _repository = repository;
            _featuretoggles = featureToggles;
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

            return View(group);
        }

        [Route("groups/results")]
        public async Task<IActionResult> Results([FromQuery] string category)
        {
            if (_featuretoggles.GroupResultsPage)
            {
                GroupResults model = new GroupResults();
                var queries = new List<Query>();
                if (!string.IsNullOrEmpty(category)) queries.Add(new Query("Category", category));
                var response = await _repository.Get<GroupResults>(queries: queries);

                if (response.IsNotFound())
                    return NotFound();

                model = response.Content as GroupResults;

                return View(model);
            }

            return NotFound();
        }
    }
}
