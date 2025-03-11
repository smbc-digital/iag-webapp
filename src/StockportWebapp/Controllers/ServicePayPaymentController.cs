using Microsoft.Extensions.Options;
using StockportGovUK.NetStandard.Gateways.Models.Civica.Pay.Response;
using StockportGovUK.NetStandard.Gateways.Response;
using StockportWebapp.Configuration;

namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
public class ServicePayPaymentController(IProcessedContentRepository repository,
                                        ICivicaPayGateway civicaPayGateway,
                                        IOptions<CivicaPayConfiguration> configuration,
                                        ILogger<ServicePayPaymentController> logger,
                                        IFeatureManager featureManager) : Controller
{
    private readonly IProcessedContentRepository _repository = repository;
    private readonly ICivicaPayGateway _civicaPayGateway = civicaPayGateway;
    private readonly CivicaPayConfiguration _civicaPayConfiguration = configuration.Value;
    private readonly ILogger<ServicePayPaymentController> _logger = logger;
    private readonly IFeatureManager _featureManager = featureManager;

    private const string CIVICA_PAY_SUCCESS = "00000";
    private const string CIVICA_PAY_INVALID_DETAILS = "00001";
    private const string CIVICA_PAY_DECLINED = "00022";
    private const string CIVICA_PAY_DECLINED_OTHER = "00023";

    [Route("/service-pay-payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, string error, string serviceProcessed)
    {
        HttpResponse response = await _repository.Get<ServicePayPayment>(slug);

        if (!response.IsSuccessful())
            return response;

        ProcessedServicePayPayment payment = response.Content as ProcessedServicePayPayment;

        ServicePayPaymentSubmissionViewModel paymentSubmission = new()
        {
            Payment = payment
        };

        if (!string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(serviceProcessed) && serviceProcessed.ToUpper().Equals("FALSE"))
        {
            ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Reference), error);
            ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.EmailAddress), error);
            ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Name), error);
            ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Amount), error);
        }

        return await _featureManager.IsEnabledAsync("ServicePaymentPage")
            ? View("Detail2025", paymentSubmission)
            : View(paymentSubmission);
    }

    [HttpPost]
    [Route("/service-pay-payment/{slug}")]
    public async Task<IActionResult> Detail(string slug, ServicePayPaymentSubmissionViewModel paymentSubmission)
    {
        HttpResponse response = await _repository.Get<ServicePayPayment>(slug);

        if (!response.IsSuccessful())
            return response;

        ProcessedServicePayPayment payment = response.Content as ProcessedServicePayPayment;
        paymentSubmission.Payment = payment;

        TryValidateModel(paymentSubmission);

        if (!ModelState.IsValid)
            return await _featureManager.IsEnabledAsync("PaymentPage")
                ? View("Detail2025", paymentSubmission)
                : View(paymentSubmission);

        CreateImmediateBasketRequest civicaPayRequest = GetCreateImmediateBasketRequest(slug, paymentSubmission);

        HttpResponse<CreateImmediateBasketResponse> civicaResponse = await _civicaPayGateway.CreateImmediateBasketAsync(civicaPayRequest);

        if (civicaResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
        {
            string responseCode = civicaResponse.ResponseContent.ResponseCode;

            if (responseCode.Equals(CIVICA_PAY_INVALID_DETAILS))
            {
                ModelState.AddModelError("Reference", $"Check {payment.ReferenceLabel.ToLower()} and try again");
                
                return await _featureManager.IsEnabledAsync("PaymentPage")
                    ? View("Detail2025", paymentSubmission)
                    : View(paymentSubmission);
            }

            _logger.LogError($"ServicePayPaymentController:: Unable to create ImmediateBasket:: " +
                            $"CivicaPay response code: {responseCode}, " +
                            $"CivicaPay error message - {civicaResponse.ResponseContent.ErrorMessage}");

            return View("Error", response);
        }

        return Redirect(_civicaPayGateway.GetPaymentUrl(
            civicaResponse.ResponseContent.BasketReference, 
            civicaResponse.ResponseContent.BasketToken, 
            paymentSubmission.Reference));
    }

    private CreateImmediateBasketRequest GetCreateImmediateBasketRequest(string slug, ServicePayPaymentSubmissionViewModel paymentSubmission) =>
        new()
        {
            CallingAppIdentifier = _civicaPayConfiguration.CallingAppIdentifier,
            CustomerID = _civicaPayConfiguration.CustomerID,
            ApiPassword = _civicaPayConfiguration.ApiPassword,
            ReturnURL = !string.IsNullOrEmpty(paymentSubmission.Payment.ReturnUrl)
                ? paymentSubmission.Payment.ReturnUrl
                : $"{Request.Scheme}://{Request.Host}/service-pay-payment/{slug}/result",
            NotifyURL = string.Empty,
            CallingAppTranReference =paymentSubmission.Reference,
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
                        CallingAppTranReference = paymentSubmission.Reference,
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