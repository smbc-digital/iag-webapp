using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
    public class ProfileController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly FeatureToggles _featuretogles;

        public ProfileController(IProcessedContentRepository repository, FeatureToggles featureToggles)
        {
            _repository = repository;
            _featuretogles = featureToggles;
        }

        [Route("/profile/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var response = await _repository.Get<Profile>(slug);

            if (!response.IsSuccessful()) return response;

            var profile = response.Content as ProcessedProfile;

            if (_featuretogles.SemanticProfile)
            {
                return View("Semantic/Index", profile);
            }

            return View(profile);
        }
    }
}
