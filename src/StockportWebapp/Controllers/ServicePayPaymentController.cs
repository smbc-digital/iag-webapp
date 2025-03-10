﻿using Microsoft.Extensions.Options;
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

        if (!string.IsNullOrEmpty(payment?.PaymentAmount))
            paymentSubmission.Amount = payment.PaymentAmount;

        if (!string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(serviceProcessed) && serviceProcessed.ToUpper().Equals("FALSE"))
        {
            ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Reference), error);
            ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.EmailAddress), error);
            ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Name), error);
            ModelState.AddModelError(nameof(ServicePayPaymentSubmissionViewModel.Amount), error);
        }

        if (await _featureManager.IsEnabledAsync("ServicePaymentPage"))
            return View("Detail2025", paymentSubmission);

        return View(paymentSubmission);
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

        bool featureToggle = await _featureManager.IsEnabledAsync("PaymentPage");

        if (!ModelState.IsValid)
            return View(featureToggle ? "Detail2025" : "Detail", paymentSubmission);

        CreateImmediateBasketRequest immediateBasketResponse = new()
        {
            CallingAppIdentifier = _civicaPayConfiguration.CallingAppIdentifier,
            CustomerID = _civicaPayConfiguration.CustomerID,
            ApiPassword = _civicaPayConfiguration.ApiPassword,
            ReturnURL = !string.IsNullOrEmpty(payment.ReturnUrl)
                ? payment.ReturnUrl
                : $"{Request.Scheme}://{Request.Host}/service-pay-payment/{slug}/result",
            NotifyURL = string.Empty,
            CallingAppTranReference = paymentSubmission.Reference,
            PaymentItems = new List<PaymentItem>
            {
                new PaymentItem
                {
                    PaymentDetails = new PaymentDetail
                    {
                        CatalogueID = payment.CatalogueId,
                        AccountReference = !string.IsNullOrEmpty(payment.AccountReference)
                            ? payment.AccountReference
                            : paymentSubmission.Reference,
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

        HttpResponse<CreateImmediateBasketResponse> civicaResponse = await _civicaPayGateway.CreateImmediateBasketAsync(immediateBasketResponse);

        if (civicaResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
        {
            string responseCode = civicaResponse.ResponseContent.ResponseCode;

            if (responseCode.Equals("00001"))
            {
                ModelState.AddModelError("Reference", $"Check {payment.ReferenceLabel.ToLower()} and try again.");
                
                return View(featureToggle ? "Detail2025" : "Detail", paymentSubmission);
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
}