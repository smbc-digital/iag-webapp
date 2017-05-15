using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Helpers;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class PaymentController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly IApplicationConfiguration _applicationConfiguration;

        public PaymentController(IProcessedContentRepository repository, IApplicationConfiguration applicationConfiguration)
        {
            _repository = repository;
            _applicationConfiguration = applicationConfiguration;
        }

        [Route("/payment/{slug}")]
        public async Task<IActionResult> Detail(string slug, string error, string serviceprocessed)
        {
            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            var paymentSubmission = new PaymentSubmission();
            paymentSubmission.Payment = payment;

            if (!string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(serviceprocessed) && serviceprocessed.ToUpper().Equals("FALSE"))
                ModelState.AddModelError(nameof(PaymentSubmission.Reference), error);

            return View(paymentSubmission);
        }

        [HttpPost]
        [Route("/payment/{slug}")]
        public async Task<IActionResult> Detail(string slug, PaymentSubmission paymentSubmission)
        {
            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            paymentSubmission.Payment = payment;

             var currentPath = Request.GetUri().AbsoluteUri;

            if (!ModelState.IsValid)
                return View(paymentSubmission);
            else
                return Redirect(ParisLinkHelper.CreateParisLink(paymentSubmission, _applicationConfiguration, currentPath));
        }
    }
}
