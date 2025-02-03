using Microsoft.Extensions.Options;
using StockportWebapp.Configuration;

namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
public class PaymentController(IProcessedContentRepository repository,
                            ICivicaPayGateway civicaPayGateway,
                            IOptions<CivicaPayConfiguration> configuration,
                            IFeatureManager featureManager) : Controller
{
    private readonly IProcessedContentRepository _repository = repository;
    private readonly ICivicaPayGateway _civicaPayGateway = civicaPayGateway;
    private readonly CivicaPayConfiguration _civicaPayConfiguration = configuration.Value;
    private readonly IFeatureManager _featureManager = featureManager;

    private const string PAYMENTS_TOGGLE = "PaymentPages";
    private const string CIVICA_PAY_SUCCESS = "00000";
    private const string CIVICA_PAY_INVALID_DETAILS = "00001";
    private const string CIVICA_PAY_DECLINED = "00022";
    private const string CIVICA_PAY_DECLINED_OTHER = "00023";


    [Route("/payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, string error, string serviceprocessed)
    {
        HttpResponse response = await _repository.Get<Payment>(slug);

        if (!response.IsSuccessful())
            return response;

        PaymentSubmission paymentSubmission = new(response.Content as ProcessedPayment);

        if (!string.IsNullOrEmpty(error)
                && !string.IsNullOrEmpty(serviceprocessed)
                && serviceprocessed.ToUpper().Equals("FALSE"))
            ModelState.AddModelError(nameof(PaymentSubmission.Reference), error);

        if (await _featureManager.IsEnabledAsync("PaymentPage"))
            return View("Details2024", paymentSubmission);

        return View(paymentSubmission);
    }

    [HttpPost]
    [Route("/payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, PaymentSubmission paymentSubmission)
    {
        TryValidateModel(paymentSubmission);
        HttpResponse response = await _repository.Get<Payment>(slug);
        
        if (!response.IsSuccessful())
            return response;

        paymentSubmission.Payment = response.Content as ProcessedPayment;

        if (!ModelState.IsValid)
        {
            if (await _featureManager.IsEnabledAsync("PaymentPage"))
                return View("Details2024", paymentSubmission);

            return View(paymentSubmission);
        }

        CreateImmediateBasketRequest civicaPayRequest = GetCreateImmediateBasketRequest(slug, paymentSubmission, $"WEB {Guid.NewGuid()}");
        StockportGovUK.NetStandard.Gateways.Response.HttpResponse<StockportGovUK.NetStandard.Gateways.Models.Civica.Pay.Response.CreateImmediateBasketResponse> civicaPayResponse = await _civicaPayGateway.CreateImmediateBasketAsync(civicaPayRequest);

        if (civicaPayResponse.IsSuccessStatusCode && civicaPayResponse.ResponseContent.ResponseCode == CIVICA_PAY_SUCCESS)
            return Redirect(_civicaPayGateway.GetPaymentUrl(civicaPayResponse.ResponseContent.BasketReference, civicaPayResponse.ResponseContent.BasketToken, civicaPayRequest.CallingAppTranReference));

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
        bool isServicePay = Request.Path.Value.Contains("service-pay-payment");
        HttpResponse response = isServicePay
                        ? await _repository.Get<ServicePayPayment>(slug)
                        : await _repository.Get<Payment>(slug);

        if (!response.IsSuccessful())
            return response;

        dynamic payment = isServicePay
                            ? response.Content as ProcessedServicePayPayment
                            : response.Content as ProcessedPayment;

        PaymentResult paymentResult = new PaymentResult(slug, payment.Title, payment.Breadcrumbs, callingAppTxnRef);

        if (responseCode != "00000")
        {
            if (_featureManager.IsEnabledAsync(PAYMENTS_TOGGLE).Result)
            {
                paymentResult.PaymentResultType = responseCode == CIVICA_PAY_DECLINED
                                            || responseCode == CIVICA_PAY_DECLINED_OTHER
                                                ? PaymentResultType.Declined
                                                : PaymentResultType.Failure;
                return View("Result", paymentResult);
            }

            if (isServicePay)
            {
                return responseCode == CIVICA_PAY_DECLINED || responseCode == CIVICA_PAY_DECLINED_OTHER
                        ? View("../ServicePayPayment/Declined", slug)
                        : View("../ServicePayPayment/Failure", slug);
            }

            return responseCode == CIVICA_PAY_DECLINED || responseCode == CIVICA_PAY_DECLINED_OTHER
                    ? View("Declined", slug)
                    : View("Failure", slug);
        }

        return await _featureManager.IsEnabledAsync("PaymentPage")
            ? View("Result", paymentResult)
            : View("Success", new PaymentSuccess
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
                new()
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