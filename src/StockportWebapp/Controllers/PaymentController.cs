using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Gateways.Models.Civica.Pay.Response;
using StockportGovUK.NetStandard.Gateways.Response;
using StockportWebapp.Configuration;

namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
public class PaymentController(IProcessedContentRepository repository,
                            ICivicaPayGateway civicaPayGateway,
                            IOptions<CivicaPayConfiguration> configuration,
                            ILogger<PaymentController> logger) : Controller
{
    private readonly IProcessedContentRepository _repository = repository;
    private readonly ICivicaPayGateway _civicaPayGateway = civicaPayGateway;
    private readonly CivicaPayConfiguration _civicaPayConfiguration = configuration.Value;
    private readonly ILogger<PaymentController> _logger = logger;
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

        return View("PaymentDetail", paymentSubmission);
    }

    [HttpPost]
    [Route("/payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, PaymentSubmission paymentSubmission)
    {
        HttpResponse response = await _repository.Get<Payment>(slug);

        if (!response.IsSuccessful())
            return response;

        paymentSubmission.Payment = response.Content as ProcessedPayment;

        TryValidateModel(paymentSubmission);

        if (string.IsNullOrEmpty(paymentSubmission.Reference))
            ModelState.AddModelError("Reference", $"Enter the {paymentSubmission.Payment.ReferenceLabel.ToLower()}");

        if (!ModelState.IsValid)
            return View("PaymentDetail", paymentSubmission);

        CreateImmediateBasketRequest civicaPayRequest = GetCreateImmediateBasketRequest(slug, paymentSubmission, paymentSubmission.Payment.PaymentType.Equals("ServicePayPayment") ? paymentSubmission.Reference : $"WEB {Guid.NewGuid()}");

        HttpResponse<CreateImmediateBasketResponse> civicaPayResponse = await _civicaPayGateway.CreateImmediateBasketAsync(civicaPayRequest);


        string reference = paymentSubmission.Payment.PaymentType.Equals("ServicePayPayment")
            ? paymentSubmission.Reference
            : civicaPayRequest.CallingAppTranReference;
        
        if (civicaPayResponse.IsSuccessStatusCode && civicaPayResponse.ResponseContent.ResponseCode.Equals(CIVICA_PAY_SUCCESS))
            return Redirect(_civicaPayGateway.GetPaymentUrl(civicaPayResponse.ResponseContent.BasketReference, civicaPayResponse.ResponseContent.BasketToken, reference));

        if (civicaPayResponse.StatusCode.Equals(HttpStatusCode.BadRequest)
            && civicaPayResponse.ResponseContent.ResponseCode.Equals(CIVICA_PAY_INVALID_DETAILS))
        {
            _logger.LogInformation($"{nameof(PaymentController)}::{nameof(Detail)}: " +
                $"CivicaPay returned invalid details when creating immediate basket. " +
                $"Response code: {civicaPayResponse.ResponseContent.ResponseCode} " +
                $"Error : {civicaPayResponse.ResponseContent.ErrorMessage}");

            ModelState.AddModelError("Reference", $"Check {paymentSubmission.Payment.ReferenceLabel.ToLower()} and try again");

            return View("PaymentDetail", paymentSubmission);
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

        dynamic payment = response.Content;
        PaymentResult paymentResult = new(slug, payment.Title, payment.Breadcrumbs, callingAppTxnRef, isServicePay);

        if (!responseCode.Equals(CIVICA_PAY_SUCCESS))
        {
            paymentResult.PaymentResultType = responseCode.Equals(CIVICA_PAY_DECLINED) || responseCode.Equals(CIVICA_PAY_DECLINED_OTHER)
                ? PaymentResultType.Declined
                : PaymentResultType.Failure;

            return View("Result", paymentResult);
        }

        return View("Result", paymentResult);
    }

    private CreateImmediateBasketRequest GetCreateImmediateBasketRequest(string slug, PaymentSubmission paymentSubmission, string transactionReference) =>
        new()
        {
            CallingAppIdentifier = _civicaPayConfiguration.CallingAppIdentifier,
            CustomerID = _civicaPayConfiguration.CustomerID,
            ApiPassword = _civicaPayConfiguration.ApiPassword,
            ReturnURL = !string.IsNullOrEmpty(paymentSubmission.Payment.ReturnUrl)
                ? paymentSubmission.Payment.ReturnUrl
                : $"{Request.Scheme}://{Request.Host}/payment/{slug}/result",
            NotifyURL = string.Empty,
            CallingAppTranReference = transactionReference,
            PaymentItems = new List<PaymentItem>
            {
                new()
                {
                    PaymentDetails = new PaymentDetail
                    {
                        CatalogueID =  paymentSubmission.Payment.CatalogueId,
                        AccountReference = !string.IsNullOrEmpty(paymentSubmission.Payment.AccountReference)
                            ? paymentSubmission.Payment.AccountReference
                            : paymentSubmission.Reference,
                        PaymentAmount = paymentSubmission.Amount,
                        PaymentNarrative = $"{paymentSubmission.Payment.PaymentDescription} - {paymentSubmission.Reference}",
                        CallingAppTranReference = transactionReference,
                        Quantity = "1",
                        ServicePayReference = paymentSubmission.Reference,
                        ServicePayNarrative = $"{paymentSubmission.Payment.PaymentDescription} - Name: {paymentSubmission.Name} - Email: {paymentSubmission.EmailAddress}",
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
}