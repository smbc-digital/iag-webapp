using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class ShowcaseController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly ILogger<ShowcaseController> _logger;

        public ShowcaseController(IProcessedContentRepository repository, ILogger<ShowcaseController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [Route("/showcase/{slug}")]
        public async Task<IActionResult> Showcase(string slug)
        { 
            var showcaseHttpResponse = await _repository.Get<Showcase>(slug);

            if (!showcaseHttpResponse.IsSuccessful())
                return showcaseHttpResponse;

            var showcase = showcaseHttpResponse.Content as ProcessedShowcase;

            return View(showcase);
        }
    }
}