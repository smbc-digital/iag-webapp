using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.ProcessedModels;
using StockportWebapp.ViewModels;
using System.Linq;
using StockportWebapp.Utils;
using System;
using StockportWebapp.Config;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class ContactUsAreaController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly ILogger<ShowcaseController> _logger;
        private readonly IApplicationConfiguration _config;

        public ContactUsAreaController(IProcessedContentRepository repository, ILogger<ShowcaseController> logger, IApplicationConfiguration config)
        {
            _repository = repository;
            _logger = logger;
            _config = config;
        }

        [Route("/contactusarea")]
        public async Task<IActionResult> Index()
        { 
            var contactUsAreaHttpResponse = await _repository.Get<ContactUsArea>();

            if (!contactUsAreaHttpResponse.IsSuccessful())
                return contactUsAreaHttpResponse;

            var contactUsArea = contactUsAreaHttpResponse.Content as ProcessedContactUsArea;

            return View(contactUsArea);
        }
    }
}