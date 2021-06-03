using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StockportGovUK.NetStandard.Gateways.Civica.Pay;
using StockportGovUK.NetStandard.Models.Civica.Pay.Request;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public class ServicePayPaymentController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly ICivicaPayGateway _civicaPayGateway;
        private readonly IConfiguration _configuration;

        public ServicePayPaymentController(IProcessedContentRepository repository, ICivicaPayGateway civicaPayGateway, IConfiguration configuration)
        {
            _repository = repository;
            _civicaPayGateway = civicaPayGateway;
            _configuration = configuration;
        }

        [Route("/service-pay-payment/{slug}")]
        public async Task<IActionResult> Detail(string slug, string error, string serviceProcessed)
        {
            var response = await _repository.Get<ServicePayPayment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedServicePayPayment;

            var paymentSubmission = new ServicePayPaymentSubmissionViewModel
            {
                Payment = payment
            };

            if (!string.IsNullOrEmpty(payment?.PaymentAmount))
                paymentSubmission.Amount = payment.PaymentAmount;

            if (!string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(serviceProcessed) && serviceProcessed.ToUpper().Equals("FALSE"))
            {
                ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Reference), error);
                ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.EmailAddress), error);
                ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Name), error);
                ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Amount), error);
            }

            return View(paymentSubmission);
        }

        [HttpPost]
        [Route("/service-pay-payment/{slug}")]
        public async Task<IActionResult> Detail(string slug, ServicePayPaymentSubmissionViewModel paymentSubmission)
        {
            var response = await _repository.Get<ServicePayPayment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedServicePayPayment;

            paymentSubmission.Payment = payment;

            if (!ModelState.IsValid)
                return View(paymentSubmission);

            var immediateBasketResponse = new CreateImmediateBasketRequest
            {
                CallingAppIdentifier = _configuration.GetValue<string>("CivicaPayCallingAppIdentifier"),
                CustomerID = _configuration.GetValue<string>("CivicaPayCustomerID"),
                ApiPassword = _configuration.GetValue<string>("CivicaPayApiPassword"),
                ReturnURL = !string.IsNullOrEmpty(payment.ReturnUrl) ? payment.ReturnUrl : $"{Request.Scheme}://{Request.Host}/service-pay-payment/{slug}/result",
                NotifyURL = string.Empty,
                CallingAppTranReference = paymentSubmission.Reference,
                PaymentItems = new List<PaymentItem>
                {
                    new PaymentItem
                    {
                        PaymentDetails = new PaymentDetail
                        {
                            CatalogueID = payment.CatalogueId,
                            AccountReference = !string.IsNullOrEmpty(payment.AccountReference) ? payment.AccountReference : paymentSubmission.Reference,
                            PaymentAmount = paymentSubmission.Amount,
                            PaymentNarrative = $"{payment.PaymentDescription} - {paymentSubmission.Reference}",
                            CallingAppTranReference = paymentSubmission.Reference,
                            Quantity = "1",
                            ServicePayReference = paymentSubmission.Reference,
                            ServicePayNarrative = $"{payment.PaymentDescription} - Name: {paymentSubmission.Name} - Email: {paymentSubmission.EmailAddress}",
                            EmailAddress = paymentSubmission.EmailAddress,
                            TelephoneNumber = "0"
                        },
                        AddressDetails = new AddressDetail
                        {
                            Name = paymentSubmission.Name
                        }
                    }
                }
            };

            var civicaResponse = await _civicaPayGateway.CreateImmediateBasketAsync(immediateBasketResponse);
            if (civicaResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                if (civicaResponse.ResponseContent.ResponseCode == "00001")
                {
                    ModelState.AddModelError("Reference", $"Check {payment.ReferenceLabel.ToLower()} and try again.");
                    return View(paymentSubmission);
                }
                return View("Error", response);
            }

            return Redirect(_civicaPayGateway.GetPaymentUrl(civicaResponse.ResponseContent.BasketReference, civicaResponse.ResponseContent.BasketToken, paymentSubmission.Reference));
        }
    }
}
