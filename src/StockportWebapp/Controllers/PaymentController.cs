using Microsoft.Extensions.Options;
using StockportWebapp.Configuration;
using StockportWebapp.Models;

namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
public class PaymentController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly ICivicaPayGateway _civicaPayGateway;
    private readonly CivicaPayConfiguration _civicaPayConfiguration;
    private readonly IFeatureManager _featureManager;
    private readonly ILogger<PaymentController> _logger;

    private const string PAYMENTS_TOGGLE = "PaymentPages";
    private const string CIVICA_PAY_SUCCESS = "00000";
    private const string CIVICA_PAY_INVALID_DETAILS = "00001";
    private const string CIVICA_PAY_DECLINED = "00022";
    private const string CIVICA_PAY_DECLINED_OTHER = "00023";

    public PaymentController(
        IProcessedContentRepository repository,
        ICivicaPayGateway civicaPayGateway,
        IOptions<CivicaPayConfiguration> configuration,
        IFeatureManager featureManager,
        ILogger<PaymentController> logger)
    {
        _repository = repository;
        _civicaPayGateway = civicaPayGateway;
        _civicaPayConfiguration = configuration.Value;
        _featureManager = featureManager;
        _logger = logger;
}

    [Route("/payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, string error, string serviceprocessed)
    {
        var response = await _repository.Get<Payment>(slug);
        if (!response.IsSuccessful())
            return response;

        PaymentSubmission paymentSubmission = new(response.Content as ProcessedPayment);

        if (!string.IsNullOrEmpty(error) 
                && !string.IsNullOrEmpty(serviceprocessed) 
                && serviceprocessed.ToUpper().Equals("FALSE"))
                ModelState.AddModelError(nameof(PaymentSubmission.Reference), error);

        if (_featureManager.IsEnabledAsync(PAYMENTS_TOGGLE).Result)
            return View("Details2024", paymentSubmission);

        return View(paymentSubmission);
    }

    [HttpPost]
    [Route("/payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, PaymentSubmission paymentSubmission)
    {
        TryValidateModel(paymentSubmission);

        var response = await _repository.Get<Payment>(slug);
        if (!response.IsSuccessful())
            return response;

        paymentSubmission.Payment = response.Content as ProcessedPayment;

        if (!ModelState.IsValid)
        {
            if (_featureManager.IsEnabledAsync(PAYMENTS_TOGGLE).Result)
                return View("Details2024", paymentSubmission);

            return View(paymentSubmission);
        }

        var civicaPayRequest = GetCreateImmediateBasketRequest(slug, paymentSubmission, $"WEB {Guid.NewGuid()}");
        var civicaPayResponse = await _civicaPayGateway.CreateImmediateBasketAsync(civicaPayRequest);

        if (civicaPayResponse.IsSuccessStatusCode && civicaPayResponse.ResponseContent.ResponseCode == CIVICA_PAY_SUCCESS)
            return Redirect(_civicaPayGateway.GetPaymentUrl(civicaPayResponse.ResponseContent.BasketReference, civicaPayResponse.ResponseContent.BasketToken, civicaPayRequest.CallingAppTranReference));

        // TODO: NEED TO CHECK THE LOGIC ON THE BELOW
        if (civicaPayResponse.StatusCode == HttpStatusCode.BadRequest 
            && civicaPayResponse.ResponseContent.ResponseCode == CIVICA_PAY_INVALID_DETAILS)
        {
                ModelState.AddModelError("Reference", $"Check {paymentSubmission.Payment.ReferenceLabel.ToLower()} and try again.");
                return View(paymentSubmission);   
        }

        return View("Error", response);
    }

    [Route("/payment/{slug}/result")]
    [Route("/service-pay-payment/{slug}/result")]
    public async Task<IActionResult> Success([FromRoute] string slug, [FromQuery] string callingAppTxnRef, [FromQuery] string responseCode)
    {
        var isServicePay = Request.Path.Value.Contains("service-pay-payment");
        var response = isServicePay 
                        ? await _repository.Get<ServicePayPayment>(slug) 
                        : await _repository.Get<Payment>(slug);

        if (!response.IsSuccessful())
            return response;

        dynamic payment = isServicePay 
                            ? response.Content as ProcessedServicePayPayment
                            : response.Content as ProcessedPayment;

        if (responseCode != "00000") { 
            if(!isServicePay) { 
                if (_featureManager.IsEnabledAsync(PAYMENTS_TOGGLE).Result)
                {
                    return responseCode == "00022" || responseCode == "00023"
                    ? View("Declined2024", new PaymentResult() { Slug = slug, Title  = payment.Title, Breadcrumbs = payment.Breadcrumbs })
                    : View("Failure2024", new PaymentResult() { Slug = slug, Title = payment.Title, Breadcrumbs = payment.Breadcrumbs });
                }

                return responseCode == "00022" || responseCode == "00023"
                    ? View("Declined", slug)
                    : View("Failure", slug);
            }

            return responseCode == "00022" || responseCode == "00023"
                    ? View("../ServicePayPayment/Declined", slug) 
                    : View("../ServicePayPayment/Failure", slug);
        }
            
        return View(new PaymentSuccess
            {
                Title = payment.Title,
                ReceiptNumber = callingAppTxnRef,
                MetaDescription = payment.MetaDescription
            });
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