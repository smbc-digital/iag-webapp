using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Helpers;
using StockportWebapp.ProcessedModels;
using Microsoft.AspNetCore.NodeServices;
using StockportWebapp.Utils;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class PaymentController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IViewRender _viewRender;
        private readonly ParisHashHelper _hashHelper;

        public PaymentController(IProcessedContentRepository repository, IApplicationConfiguration applicationConfiguration, IViewRender viewRender, ParisHashHelper hashHelper)
        {
            _repository = repository;
            _applicationConfiguration = applicationConfiguration;
            _viewRender = viewRender;
            _hashHelper = hashHelper;
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
            var confirmationReturn =  $"{currentPath}/thanks";

            if (!ModelState.IsValid)
                return View(paymentSubmission);
            else
                return Redirect(ParisLinkHelper.CreateParisLink(paymentSubmission, _applicationConfiguration, confirmationReturn));
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

            var model = new PaymentResponse();

            model.Title = payment.Title;
            model.TransactionType = transactionType;
            model.AdministrationCharge = administrationCharge;
            model.Amount = amount;
            model.Data = data;
            model.ServiceProcessed = serviceProcessed;
            model.MerchantNumber = merchantNumber;
            model.AuthorisationCode = authorisationCode;
            model.Date = date;
            model.MerchantTid = merchantTid;
            model.ReceiptNumber = receiptNumber;
            model.Hash = hash;
            model.Slug = slug;
      
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

            var model = new PaymentResponse();

            model.Title = payment.Title;
            model.TransactionType = transactionType;
            model.AdministrationCharge = administrationCharge;
            model.Amount = amount;
            model.Data = data;
            model.ServiceProcessed = serviceProcessed;
            model.MerchantNumber = merchantNumber;
            model.AuthorisationCode = authorisationCode;
            model.Date = date;
            model.MerchantTid = merchantTid;
            model.ReceiptNumber = receiptNumber;
            model.Hash = hash;
            model.Slug = slug;

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

            var model = new PaymentResponse();

            model.Title = payment.Title;
            model.TransactionType = transactionType;
            model.AdministrationCharge = administrationCharge;
            model.Amount = amount;
            model.Data = data;
            model.ServiceProcessed = serviceProcessed;
            model.MerchantNumber = merchantNumber;
            model.AuthorisationCode = authorisationCode;
            model.Date = date;
            model.MerchantTid = merchantTid;
            model.ReceiptNumber = receiptNumber;
            model.Hash = hash;
            model.Slug = slug;

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
