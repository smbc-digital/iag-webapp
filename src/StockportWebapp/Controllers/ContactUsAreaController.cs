using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.ProcessedModels;
using StockportWebapp.FeatureToggling;

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

        [Route("/contactusarea")]
        public async Task<IActionResult> Index()
        {
            if (_featureToggles.ContactUsArea)
            {
                var contactUsAreaHttpResponse = await _repository.Get<ContactUsArea>();

                if (!contactUsAreaHttpResponse.IsSuccessful())
                    return contactUsAreaHttpResponse;

                var contactUsArea = contactUsAreaHttpResponse.Content as ProcessedContactUsArea;

                return View(contactUsArea);
            }

            _logger.LogInformation("Contact us area, feature toggle false, returning Not Found");
            return NotFound();
        }
    }
}