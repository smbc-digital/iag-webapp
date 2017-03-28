using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;
using StockportWebapp.Http;
using System.Linq;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class ShowcaseController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly ILogger<ShowcaseController> _logger;
        private readonly IContactUsMessageTagParser _contactUsMessageParser;

        public ShowcaseController(IProcessedContentRepository repository, ILogger<ShowcaseController> logger, IContactUsMessageTagParser contactUsMessageParser)
        {
            _repository = repository;
            _logger = logger;
            _contactUsMessageParser = contactUsMessageParser;
        }

        [Route("/showcase/{ShowcaseSlug}")]
        public async Task<IActionResult> Showcase(string ShowcaseSlug)
        { 
            var ShowcaseHttpResponse = await _repository.Get<Showcase>(ShowcaseSlug);

            if (!ShowcaseHttpResponse.IsSuccessful())
                return ShowcaseHttpResponse;

            var Showcase = ShowcaseHttpResponse.Content as ProcessedShowcase;

            return View(Showcase);
        }
    }
}