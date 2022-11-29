using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class ContactUsAreaController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly ILogger<ContactUsAreaController> _logger;
        private readonly FeatureToggles _featureToggles;

        public ContactUsAreaController(IProcessedContentRepository repository, FeatureToggles featureToggles, ILogger<ContactUsAreaController> logger)
        {
            _repository = repository;
            _logger = logger;
            _featureToggles = featureToggles;
        }

        [Route("/contact")]
        public async Task<IActionResult> Index()
        {
            if (_featureToggles.ContactUsArea)
            {
                _logger.LogInformation("Contact Us Area: Getting Content from Repository");
                var contactUsAreaHttpResponse = await _repository.Get<ContactUsArea>();

                _logger.LogInformation($"Contact Us Area: Response from Repository: {contactUsAreaHttpResponse.StatusCode}");
                if (!contactUsAreaHttpResponse.IsSuccessful())
                    return contactUsAreaHttpResponse;

                _logger.LogInformation("Contact Us Area: Processing Repository content");
                var contactUsArea = contactUsAreaHttpResponse.Content as ProcessedContactUsArea;
                _logger.LogInformation($"Contact Us Area: Returning processed content to view");

                return View(contactUsArea);
            }

            _logger.LogInformation("Contact us area, feature toggle false, returning Not Found");
            return NotFound();
        }
    }
}