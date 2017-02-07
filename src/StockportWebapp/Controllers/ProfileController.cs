using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
    public class ProfileController : Controller
    {
        private readonly IProcessedContentRepository _repository;

        public ProfileController(IProcessedContentRepository repository)
        {
            _repository = repository;
        }

        [Route("/profile/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var response = await _repository.Get<Profile>(slug);

            if (!response.IsSuccessful()) return response;

            var profile = response.Content as ProcessedProfile;

            return View(profile);
        }
    }
}
