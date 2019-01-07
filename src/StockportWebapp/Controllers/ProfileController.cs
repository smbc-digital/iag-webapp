using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Services.Profile;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
    public class ProfileController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly FeatureToggles _featuretogles;
        private readonly IProfileService _profileService;

        public ProfileController(IProcessedContentRepository repository, FeatureToggles featureToggles, IProfileService profileService)
        {
            _repository = repository;
            _featuretogles = featureToggles;
            _profileService = profileService;
        }

        [Route("/profile/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            if (_featuretogles.SemanticProfile)
            {
                var profileEntity = await _profileService.GetProfile(slug);
                var model = new ProfileNew(profileEntity.Title, profileEntity.Slug, profileEntity.LeadParagraph, profileEntity.Teaser,
                profileEntity.Image, profileEntity.Body, profileEntity.Breadcrumbs, profileEntity.Alerts);

                return View("Semantic/Index", model);
            }

            var response = await _repository.Get<Profile>(slug);

            if (!response.IsSuccessful()) return response;

            return View(response.Content as ProcessedProfile);
        }
    }
}
