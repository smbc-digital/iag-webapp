using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebapp.ProcessedModels;
using Microsoft.AspNetCore.NodeServices;
using StockportGovUK.NetStandard.Gateways.Civica.Pay;
using StockportGovUK.NetStandard.Models.Civica.Pay.Request;
using System;
using Microsoft.Extensions.Configuration;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Config;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public class PaymentController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly IViewRender _viewRender;
        private readonly ParisHashHelper _hashHelper;
        private readonly ICivicaPayGateway _civicaPayGateway;
        private readonly IConfiguration _configuration;
        private readonly FeatureToggles _featureToggles;
        private readonly IApplicationConfiguration _applicationConfiguration;

        public PaymentController(IProcessedContentRepository repository, IViewRender viewRender, ParisHashHelper hashHelper, ICivicaPayGateway civicaPayGateway, IConfiguration configuration, FeatureToggles featureToggles, IApplicationConfiguration applicationConfiguration)
        {
            _repository = repository;
            _viewRender = viewRender;
            _hashHelper = hashHelper;
            _civicaPayGateway = civicaPayGateway;
            _configuration = configuration;
            _featureToggles = featureToggles;
            _applicationConfiguration = applicationConfiguration;
        }

        [Route("/payment/{slug}")]
        public async Task<IActionResult> Detail(string slug, string error, string serviceprocessed)
        {
            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            var paymentSubmission = new PaymentSubmission
            {
                Payment = payment
            };

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

            TryValidateModel(paymentSubmission);

            // if (!ModelState.IsValid)
            // {
            //    return View(paymentSubmission);
            // }

            if (_featureToggles.CivicaPay)
            {
                var transactionReference = Guid.NewGuid().ToString();

                var immediateBasketResponse = new CreateImmediateBasketRequest
                {
                    CallingAppIdentifier = _configuration.GetValue<string>("CivicaPayCallingAppIdentifier"),
                    CustomerID = _configuration.GetValue<string>("CivicaPayCustomerID"),
                    ApiPassword = _configuration.GetValue<string>("CivicaPayApiPassword"),
                    ReturnURL = !string.IsNullOrEmpty(payment.ReturnUrl) ? payment.ReturnUrl : $"{Request.Scheme}://{Request.Host}/payment/{slug}/result",
                    NotifyURL = string.Empty,
                    CallingAppTranReference = transactionReference,
                    PaymentItems = new System.Collections.Generic.List<PaymentItem>
                    {
                        new PaymentItem
                        {
                            PaymentDetails = new PaymentDetail
                            {
                                CatalogueID = payment.CatalogueId,
                                AccountReference = !string.IsNullOrEmpty(payment.AccountReference) ? payment.AccountReference : paymentSubmission.Reference,
                                PaymentAmount = paymentSubmission.Amount.ToString(),
                                PaymentNarrative = $"{payment.PaymentDescription} - {paymentSubmission.Reference}",
                                CallingAppTranReference = transactionReference,
                                Quantity = "1"
                            },
                            AddressDetails = new AddressDetail()
                        }
                    }
                };

                var civicaResponse = await _civicaPayGateway.CreateImmediateBasketAsync(immediateBasketResponse);
                if (civicaResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    return View("Error", response);
                }

                return Redirect(_civicaPayGateway.GetPaymentUrl(civicaResponse.ResponseContent.BasketReference, civicaResponse.ResponseContent.BasketToken, transactionReference));
            }

            var currentPath = Request.GetUri().AbsoluteUri;
            var confirmationReturn = $"{currentPath}";

            return Redirect(ParisLinkHelper.CreateParisLink(paymentSubmission, _applicationConfiguration, confirmationReturn));
        }

        [Route("/payment/{slug}/result")]
        public async Task<IActionResult> Success([FromRoute]string slug, [FromQuery]string callingAppTxnRef, [FromQuery] string responseCode)
        {
            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            if (responseCode != "00000")
            {
                return View("Failure", slug);
            }

            var model = new PaymentSuccess
            {
                Title = payment.Title,
                ReceiptNumber = callingAppTxnRef,
                MetaDescription = payment.MetaDescription
            };

            return View(model);
        }

        [Route("/payment/{slug}/thanks")]
        public async Task<IActionResult> Confirmation(string slug, [FromQuery] string error, [FromQuery] string transactionType, [FromQuery] string amount, [FromQuery] string administrationCharge, [FromQuery] string data,
                                            [FromQuery] string serviceProcessed, [FromQuery] string merchantNumber, [FromQuery] string authorisationCode, [FromQuery] string date, [FromQuery] string merchantTid,
                                            [FromQuery] string receiptNumber, [FromQuery] string hash)
        {
            if (!string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Detail", new { slug = slug, error = error, serviceprocessed = "false" });
            }

            if (!_hashHelper.IsValidRequest(Request))
            {
                return NotFound();
            }

            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            var model = new PaymentResponse
            {
                Title = payment.Title,
                TransactionType = transactionType,
                AdministrationCharge = administrationCharge,
                Amount = amount,
                Data = data,
                ServiceProcessed = serviceProcessed,
                MerchantNumber = merchantNumber,
                AuthorisationCode = authorisationCode,
                Date = date,
                MerchantTid = merchantTid,
                ReceiptNumber = receiptNumber,
                Hash = hash,
                Slug = slug,
                MetaDescription = payment.MetaDescription
            };

            return View(model);
        }

        [Route("/payment/{slug}/printthanks")]
        public async Task<IActionResult> ConfirmationPrint(string slug, [FromQuery] string transactionType, [FromQuery] string amount, [FromQuery] string administrationCharge, [FromQuery] string data,
                                            [FromQuery] string serviceProcessed, [FromQuery] string merchantNumber, [FromQuery] string authorisationCode, [FromQuery] string date, [FromQuery] string merchantTid,
                                            [FromQuery] string receiptNumber, [FromQuery] string hash)
        {
            if (!_hashHelper.IsValidRequest(Request))
            {
                return NotFound();
            }

            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            var model = new PaymentResponse
            {
                Title = payment.Title,
                TransactionType = transactionType,
                AdministrationCharge = administrationCharge,
                Amount = amount,
                Data = data,
                ServiceProcessed = serviceProcessed,
                MerchantNumber = merchantNumber,
                AuthorisationCode = authorisationCode,
                Date = date,
                MerchantTid = merchantTid,
                ReceiptNumber = receiptNumber,
                Hash = hash,
                Slug = slug
            };

            return View(model);
        }

        [HttpGet]
        [Route("/payment/{slug}/exportpdf")]
        public async Task<IActionResult> ExportPdf([FromServices] INodeServices nodeServices, string slug, [FromQuery] string transactionType, [FromQuery] string amount, [FromQuery] string administrationCharge, [FromQuery] string data,
                                            [FromQuery] string serviceProcessed, [FromQuery] string merchantNumber, [FromQuery] string authorisationCode, [FromQuery] string date, [FromQuery] string merchantTid,
                                            [FromQuery] string receiptNumber, [FromQuery] string hash)
        {
            if (!_hashHelper.IsValidRequest(Request))
            {
                return NotFound();
            }

            ViewBag.CurrentUrl = Request?.GetUri();

            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            var model = new PaymentResponse
            {
                Title = payment.Title,
                TransactionType = transactionType,
                AdministrationCharge = administrationCharge,
                Amount = amount,
                Data = data,
                ServiceProcessed = serviceProcessed,
                MerchantNumber = merchantNumber,
                AuthorisationCode = authorisationCode,
                Date = date,
                MerchantTid = merchantTid,
                ReceiptNumber = receiptNumber,
                Hash = hash,
                Slug = slug
            };

            var renderedExportStyles = _viewRender.Render("Shared/ExportStyles", string.Concat(Request?.Scheme, "://", Request?.Host));
            var renderedHtml = _viewRender.Render("Shared/PaymentConfirmationPDF", model);

            var result = await nodeServices.InvokeAsync<byte[]>("./pdf", new { data = string.Concat(renderedExportStyles, renderedHtml), delay = 500 });

            HttpContext.Response.ContentType = "application/pdf";

            string filename = @"receipt.pdf";
            HttpContext.Response.Headers.Add("x-filename", filename);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "x-filename");
            HttpContext.Response.Body.Write(result, 0, result.Length);
            return new ContentResult();
        }
    }
}
