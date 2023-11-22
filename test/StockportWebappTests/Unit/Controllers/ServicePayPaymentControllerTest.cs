﻿using Microsoft.Extensions.Options;
using StockportWebapp.Configuration;

namespace StockportWebappTests_Unit.Unit.Controllers;

public class ServicePayPaymentControllerTest
{
    private readonly Mock<IProcessedContentRepository> _fakeRepository = new();
    private readonly ServicePayPaymentController _paymentController;
    private readonly Mock<ICivicaPayGateway> _civicaPayGateway = new();
    private readonly Mock<IOptions<CivicaPayConfiguration>> _configuration = new();
    private readonly ProcessedServicePayPayment _processedPayment = new ProcessedServicePayPayment("title", "slug", "teaser", "description", "paymentDetailsText",
        "Reference Number", new List<Crumb>(), EPaymentReferenceValidation.None,
        "metaDescription", "returnUrl", "1233455", "40000000", "paymentDescription", new List<Alert>(), "20.65");


    public ServicePayPaymentControllerTest()
    {
        _civicaPayGateway.Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
            .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse> { StatusCode = HttpStatusCode.OK, ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00000" } });

        _civicaPayGateway
            .Setup(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("redirectUrl");

        _fakeRepository
            .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, _processedPayment, string.Empty));

        _configuration
            .Setup(_ => _.Value)
            .Returns(new CivicaPayConfiguration{
                ApiPassword="Password",
                CustomerID = "CustomerId",
                CallingAppIdentifier = "WebApp"
            });

        _paymentController = new ServicePayPaymentController(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object, new Mock<ILogger<ServicePayPaymentController>>().Object);

        var objectValidator = new Mock<IObjectModelValidator>();
        objectValidator.Setup(_ => _.Validate(
            It.IsAny<ActionContext>(),
            It.IsAny<ValidationStateDictionary>(),
            It.IsAny<string>(),
            It.IsAny<object>()));
        _paymentController.ObjectValidator = objectValidator.Object;
    }

    [Fact]
    public async Task DetailShouldReturnAPaymentWithProcessedBody()
    {
        var view = await _paymentController.Detail("slug", null, null) as ViewResult; ;
        var model = view.ViewData.Model as ServicePayPaymentSubmissionViewModel;

        model.Payment.Should().Be(_processedPayment);
    }

    [Fact]
    public async Task DetailShouldGetA404NotFoundPayment()
    {
        _fakeRepository
            .Setup(_ => _.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        var response = await _paymentController.Detail("not-found-slug", null, null) as HttpResponse;

        response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DetailPostShouldCallGatewayCreateImmediateBasket()
    {
        await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Reference = "12346",
            Amount = "23.5",
            Name = "name",
            EmailAddress = "test-email-address",
            Payment = _processedPayment
        });

        _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
    }

    [Fact]
    public async Task DetailPostShouldReturnDetailsViewIfModelStateInvalid()
    {
        _paymentController.ModelState.AddModelError("Reference", "error");

        var result = await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Payment = _processedPayment,
            Amount = "12.00",
            Reference = "123456"
        }) as ViewResult;

        _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Never);
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task DetailPostShouldReturnErrorViewIfBadRequest()
    {
        _civicaPayGateway.Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
            .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse> { StatusCode = HttpStatusCode.BadRequest, ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00000" } });

        var result = await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Payment = _processedPayment,
            Amount = "12.00",
            Reference = "123456789"
        }) as ViewResult;

        _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        result.Should().BeOfType<ViewResult>();
        result.ViewName.Should().Be("Error");
    }

    [Fact]
    public async Task DetailPostShouldReturnViewIfResponseCodeIs00001()
    {
        _civicaPayGateway.Setup(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
            .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse> { StatusCode = HttpStatusCode.BadRequest, ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00001" } });

        var result = await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Payment = _processedPayment,
            Reference = "123456789",
            Amount = "12.00",
            EmailAddress = "test-email-address"
        }) as ViewResult;

        _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task DetailPostShouldCallGatewayGetPaymentUrl()
    {
        await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Payment = _processedPayment,
            Amount = "12.00",
            Reference = "123456789"
        });

        _civicaPayGateway.Verify(_ => _.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
