using Microsoft.Extensions.Options;
using StockportWebapp.Configuration;
using StockportWebapp.Models;

namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
public class PaymentController(IProcessedContentRepository repository,
                            ICivicaPayGateway civicaPayGateway,
                            IOptions<CivicaPayConfiguration> configuration) : Controller
{
    private readonly IProcessedContentRepository _repository = repository;
    private readonly ICivicaPayGateway _civicaPayGateway = civicaPayGateway;
    private readonly CivicaPayConfiguration _civicaPayConfiguration = configuration.Value;

    [Route("/payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, string error, string serviceprocessed)
    {
        HttpResponse response = await _repository.Get<Payment>(slug);

        if (!response.IsSuccessful())
            return response;

        ProcessedPayment payment = response.Content as ProcessedPayment;

        PaymentSubmission paymentSubmission = new()
        {
            Payment = payment
        };

        if (_featureManager.IsEnabledAsync(PAYMENTS_TOGGLE).Result)
            return View("Details2024", paymentSubmission);

        return View(paymentSubmission);
    }

    [HttpPost]
    [Route("/payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, PaymentSubmission paymentSubmission)
    {
        HttpResponse response = await _repository.Get<Payment>(slug);

        var response = await _repository.Get<Payment>(slug);
        if (!response.IsSuccessful())
            return response;

        ProcessedPayment payment = response.Content as ProcessedPayment;

        paymentSubmission.Payment = payment;

        TryValidateModel(paymentSubmission);

        if (!ModelState.IsValid)
            return View(paymentSubmission);

        string transactionReference = Guid.NewGuid().ToString();

        CreateImmediateBasketRequest immediateBasketResponse = new()
        {
            CallingAppIdentifier = _civicaPayConfiguration.CallingAppIdentifier,
            CustomerID = _civicaPayConfiguration.CustomerID,
            ApiPassword = _civicaPayConfiguration.ApiPassword,
            ReturnURL = !string.IsNullOrEmpty(payment.ReturnUrl)
                ? payment.ReturnUrl
                : $"{Request.Scheme}://{Request.Host}/payment/{slug}/result",
            NotifyURL = string.Empty,
            CallingAppTranReference = transactionReference,
            PaymentItems = new List<PaymentItem>
            {
                new() {
                    PaymentDetails = new PaymentDetail
                    {
                        CatalogueID = payment.CatalogueId,
                        AccountReference = !string.IsNullOrEmpty(payment.AccountReference)
                            ? payment.AccountReference
                            : paymentSubmission.Reference,
                        PaymentAmount = paymentSubmission.Amount.ToString(),
                        PaymentNarrative = $"{payment.PaymentDescription} - {paymentSubmission.Reference}",
                        CallingAppTranReference = transactionReference,
                        Quantity = "1"
                    },
                    AddressDetails = new AddressDetail()
                }
            }
        };

        StockportGovUK.NetStandard.Gateways.Response.HttpResponse<StockportGovUK.NetStandard.Gateways.Models.Civica.Pay.Response.CreateImmediateBasketResponse> civicaResponse = await _civicaPayGateway.CreateImmediateBasketAsync(immediateBasketResponse);
        if (civicaResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
        {
            if (civicaResponse.ResponseContent.ResponseCode.Equals("00001"))
            {
                ModelState.AddModelError("Reference", $"Check {payment.ReferenceLabel.ToLower()} and try again.");

                return View(paymentSubmission);
            }

            return View("Error", response);
        }

        return View("Error", response);
    }

    [Route("/payment/{slug}/result")]
    [Route("/service-pay-payment/{slug}/result")]
    public async Task<IActionResult> Success([FromRoute] string slug, [FromQuery] string callingAppTxnRef, [FromQuery] string responseCode)
    {
        bool pathIsServicePay = Request.Path.Value.Contains("service-pay-payment");

        HttpResponse response = pathIsServicePay
            ? await _repository.Get<ServicePayPayment>(slug)
            : await _repository.Get<Payment>(slug);

        if (!response.IsSuccessful())
            return response;

        dynamic payment;
        if (pathIsServicePay)
            payment = response.Content as ProcessedServicePayPayment;
        else
            payment = response.Content as ProcessedPayment;

        if (!responseCode.Equals("00000"))
            return responseCode.Equals("00022") || responseCode.Equals("00023")
                ? pathIsServicePay ? View("../ServicePayPayment/Declined", slug) : View("Declined", slug)
                : pathIsServicePay ? View("../ServicePayPayment/Failure", slug) : View("Failure", slug);

        PaymentSuccess model = new()
        {
            Title = payment.Title,
            ReceiptNumber = callingAppTxnRef,
            MetaDescription = payment.MetaDescription
        };

        return View(model);
    }

    private CreateImmediateBasketRequest GetCreateImmediateBasketRequest(string slug, PaymentSubmission paymentSubmission, string transactionReference) =>
    new CreateImmediateBasketRequest
    {
        CallingAppIdentifier = _civicaPayConfiguration.CallingAppIdentifier,
        CustomerID = _civicaPayConfiguration.CustomerID,
        ApiPassword = _civicaPayConfiguration.ApiPassword,
        ReturnURL = !string.IsNullOrEmpty(paymentSubmission.Payment.ReturnUrl) ? paymentSubmission.Payment.ReturnUrl : $"{Request.Scheme}://{Request.Host}/payment/{slug}/result",
        NotifyURL = string.Empty,
        CallingAppTranReference = transactionReference,
        PaymentItems = new System.Collections.Generic.List<PaymentItem>
        {
                new PaymentItem
                {
                    PaymentDetails = new PaymentDetail
                    {
                        CatalogueID =  paymentSubmission.Payment.CatalogueId,
                        AccountReference = !string.IsNullOrEmpty( paymentSubmission.Payment.AccountReference) ?  paymentSubmission.Payment.AccountReference : paymentSubmission.Reference,
                        PaymentAmount = paymentSubmission.Amount.ToString(),
                        PaymentNarrative = $"{ paymentSubmission.Payment.PaymentDescription} - {paymentSubmission.Reference}",
                        CallingAppTranReference = transactionReference,
                        Quantity = "1"
                    },
                    AddressDetails = new AddressDetail()
                }
        }
    };
}