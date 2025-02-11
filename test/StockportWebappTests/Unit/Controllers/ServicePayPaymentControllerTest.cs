using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using StockportWebapp.Configuration;

namespace StockportWebappTests_Unit.Unit.Controllers;

public class ServicePayPaymentControllerTest
{
    private readonly ServicePayPaymentController _paymentController;
    private readonly Mock<IProcessedContentRepository> _fakeRepository = new();
    private readonly Mock<ICivicaPayGateway> _civicaPayGateway = new();
    private readonly Mock<IOptions<CivicaPayConfiguration>> _configuration = new();
    private readonly Mock<IFeatureManager> _featureManager = new();
    private readonly ProcessedServicePayPayment _processedPayment = new("title",
                                                                        "slug",
                                                                        "teaser",
                                                                        "description",
                                                                        "paymentDetailsText",
                                                                        "Reference Number",
                                                                        new List<Crumb>(),
                                                                        EPaymentReferenceValidation.None,
                                                                        "metaDescription",
                                                                        "returnUrl",
                                                                        "1233455",
                                                                        "40000000",
                                                                        "paymentDescription",
                                                                        new List<Alert>(),
                                                                        "20.65");


    public ServicePayPaymentControllerTest()
    {
        _civicaPayGateway
            .Setup(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
            .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
            {
                StatusCode = HttpStatusCode.OK,
                ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00000" }
            });

        _civicaPayGateway
            .Setup(gateway => gateway.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("redirectUrl");

        _fakeRepository
            .Setup(repo => repo.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, _processedPayment, string.Empty));

        _configuration
            .Setup(conf => conf.Value)
            .Returns(new CivicaPayConfiguration{
                ApiPassword="Password",
                CustomerID = "CustomerId",
                CallingAppIdentifier = "WebApp"
            });

        _paymentController = new(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object, new Mock<ILogger<ServicePayPaymentController>>().Object, _featureManager.Object);

        Mock<IObjectModelValidator> objectValidator = new();
        objectValidator.Setup(validator => validator.Validate(It.IsAny<ActionContext>(),
                                                            It.IsAny<ValidationStateDictionary>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>()));
        
        _paymentController.ObjectValidator = objectValidator.Object;
    }

    [Fact]
    public async Task DetailShouldReturnAPaymentWithProcessedBody()
    {
        // Act
        ViewResult view = await _paymentController.Detail("slug", null, null) as ViewResult; ;
        ServicePayPaymentSubmissionViewModel model = view.ViewData.Model as ServicePayPaymentSubmissionViewModel;

        // Assert
        Assert.Equal(_processedPayment, model.Payment);
    }

    [Fact]
    public async Task DetailShouldGetA404NotFoundPayment()
    {
        // Arrange
        _fakeRepository
            .Setup(repo => repo.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        HttpResponse response = await _paymentController.Detail("not-found-slug", null, null) as HttpResponse;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task DetailShouldReturnViewWithErrorsIfErrors()
    {
        ViewResult result = await _paymentController.Detail("slug", "Test Error", "false") as ViewResult;
        ServicePayPaymentSubmissionViewModel model = result.ViewData.Model as ServicePayPaymentSubmissionViewModel;

        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.True(_paymentController.ModelState.ContainsKey(nameof(ServicePayPaymentSubmissionViewModel.Reference)));
        Assert.True(_paymentController.ModelState.ContainsKey(nameof(ServicePayPaymentSubmissionViewModel.EmailAddress)));
        Assert.True(_paymentController.ModelState.ContainsKey(nameof(ServicePayPaymentSubmissionViewModel.Name)));
        Assert.True(_paymentController.ModelState.ContainsKey(nameof(ServicePayPaymentSubmissionViewModel.Amount)));
    }

    [Fact]
    public async Task DetailPostShouldCallGatewayCreateImmediateBasket()
    {
        // Act
        await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Reference = "12346",
            Amount = "23.5",
            Name = "name",
            EmailAddress = "test-email-address",
            Payment = _processedPayment
        });

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
    }

    [Fact]
    public async Task DetailPostShouldReturnDetailsViewIfModelStateInvalid()
    {
        // Arrange
        _paymentController.ModelState.AddModelError("Reference", "error");

        // Act
        ViewResult result = await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Payment = _processedPayment,
            Amount = "12.00",
            Reference = "123456"
        }) as ViewResult;
        
        // Assert
        _civicaPayGateway.Verify(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Never);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task DetailPostShouldReturnErrorViewIfBadRequest()
    {
        // Arrange
        _civicaPayGateway
            .Setup(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
            .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00000" }
            });

        // Act
        ViewResult result = await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Payment = _processedPayment,
            Amount = "12.00",
            Reference = "123456789"
        }) as ViewResult;

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", result.ViewName);
    }

    [Fact]
    public async Task DetailPostShouldReturn404IfNotFound()
    {
        // Arrange
        _fakeRepository
            .Setup(repo => repo.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        HttpResponse response = await _paymentController.Detail("not-found-slug", new ServicePayPaymentSubmissionViewModel()) as HttpResponse;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task DetailPostShouldReturnViewIfResponseCodeIs00001()
    {
        // Arrange
        _civicaPayGateway
            .Setup(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
            .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
            {
                StatusCode = HttpStatusCode.BadRequest,
                ResponseContent = new CreateImmediateBasketResponse { BasketReference = "testRef", BasketToken = "testBasketToken", ResponseCode = "00001" }
            });

        // Act
        ViewResult result = await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Payment = _processedPayment,
            Reference = "123456789",
            Amount = "12.00",
            EmailAddress = "test-email-address"
        }) as ViewResult;

        // Assert
        _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task DetailPostShouldCallGatewayGetPaymentUrl()
    {
        // Act
        await _paymentController.Detail("slug", new ServicePayPaymentSubmissionViewModel
        {
            Payment = _processedPayment,
            Amount = "12.00",
            Reference = "123456789"
        });

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}