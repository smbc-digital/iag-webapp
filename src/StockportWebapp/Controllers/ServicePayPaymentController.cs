﻿using Microsoft.Extensions.Options;
using StockportWebapp.Configuration;

namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
public class ServicePayPaymentController : Controller
{
    private readonly IProcessedContentRepository _repository;
    private readonly ICivicaPayGateway _civicaPayGateway;
    private readonly CivicaPayConfiguration _civicaPayConfiguration;
    private readonly ILogger<ServicePayPaymentController> _logger;

    public ServicePayPaymentController(
        IProcessedContentRepository repository,
        ICivicaPayGateway civicaPayGateway,
        IOptions<CivicaPayConfiguration> configuration,
        ILogger<ServicePayPaymentController> logger)
    {
        _repository = repository;
        _civicaPayGateway = civicaPayGateway;
        _civicaPayConfiguration = configuration.Value;
        _logger = logger;
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Detail(string slug, ServicePayPaymentSubmissionViewModel paymentSubmission)
    {
        var response = await _repository.Get<ServicePayPayment>(slug);

        if (!response.IsSuccessful())
            return response;

        var payment = response.Content as ProcessedServicePayPayment;

        paymentSubmission.Payment = payment;
        TryValidateModel(paymentSubmission);

        if (!ModelState.IsValid)
            return View(paymentSubmission);

        var immediateBasketResponse = new CreateImmediateBasketRequest
        {
            CallingAppIdentifier = _civicaPayConfiguration.CallingAppIdentifier,
            CustomerID = _civicaPayConfiguration.CustomerID,
            ApiPassword = _civicaPayConfiguration.ApiPassword,
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

            _logger.LogError($"ServicePayPaymentController:: Unable to create ImmediateBasket:: CivicaPay response code: {civicaResponse.ResponseContent.ResponseCode}, CivicaPay error message - {civicaResponse.ResponseContent.ErrorMessage}");
            return View("Error", response);
        }

        return Redirect(_civicaPayGateway.GetPaymentUrl(civicaResponse.ResponseContent.BasketReference, civicaResponse.ResponseContent.BasketToken, paymentSubmission.Reference));
    }
}
