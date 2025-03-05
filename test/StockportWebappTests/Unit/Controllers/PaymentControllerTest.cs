using Microsoft.Extensions.Options;
using StockportWebapp.Configuration;

namespace StockportWebappTests_Unit.Unit.Controllers;

public class PaymentControllerTest
{
    private readonly Mock<IProcessedContentRepository> _fakeRepository = new();
    private readonly PaymentController _paymentController;
    private readonly PaymentController _paymentControllerWithServicePayPaymentPath;
    private readonly Mock<ICivicaPayGateway> _civicaPayGateway = new();
    private readonly Mock<IOptions<CivicaPayConfiguration>> _configuration = new();
    private readonly Mock<IObjectModelValidator> _objectValidator = new();
    private readonly Mock<IFeatureManager> _featureManager = new();

    private readonly ProcessedPayment _processedPayment = new("title",
                                                            "slug",
                                                            "teaser",
                                                            "description",
                                                            "default",
                                                            "payDetailsText",
                                                            "refLabel",
                                                            "fund",
                                                            "glCode",
                                                            null,
                                                            EPaymentReferenceValidation.None,
                                                            "meta",
                                                            "returnUrl",
                                                            "catId",
                                                            "accRef",
                                                            "payDesc",
                                                            null,
                                                            "15.00");

    private readonly ProcessedServicePayPayment _processedServicePayPayment = new("title",
                                                                                "slug",
                                                                                "teaser",
                                                                                "description",
                                                                                "payDetailsText",
                                                                                "refLabel",
                                                                                null,
                                                                                EPaymentReferenceValidation.None,
                                                                                "meta",
                                                                                "returnUrl",
                                                                                "catId",
                                                                                "accRef",
                                                                                "payDesc",
                                                                                null,
                                                                                "payAmount");

    public PaymentControllerTest()
    {
        DefaultHttpContext httpContextPayment = new();
        httpContextPayment.Request.Path = "/payment";
        ControllerContext mockControllerContextPayment = new()
        {
            HttpContext = httpContextPayment
        };

        DefaultHttpContext httpContextServicePayPayment = new();
        httpContextServicePayPayment.Request.Path = "/service-pay-payment";
        ControllerContext mockControllerContextServicePayPayment = new()
        {
            HttpContext = httpContextServicePayPayment
        };
        
        _fakeRepository
            .Setup(repo => repo.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, _processedPayment, string.Empty));

        _fakeRepository
            .Setup(repo => repo.Get<ServicePayPayment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, _processedServicePayPayment, string.Empty));

        _configuration
            .Setup(conf => conf.Value)
            .Returns(new CivicaPayConfiguration{
                ApiPassword="Password",
                CustomerID = "CustomerId",
                CallingAppIdentifier = "WebApp"
            });

        _objectValidator
            .Setup(validator => validator.Validate(
                It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));

        _paymentController = new(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object, _featureManager.Object)
        {
            ControllerContext = mockControllerContextPayment
        };

        _paymentControllerWithServicePayPaymentPath = new(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object, _featureManager.Object)
        {
            ControllerContext = mockControllerContextServicePayPayment
        };
    }

    [Fact]
    public async Task DetailShouldReturnAPaymentWithProcessedBody()
    {
        // Arrange
        ViewResult view = await _paymentController.Detail("slug", null, null) as ViewResult;

        // Act
        PaymentSubmission model = view.ViewData.Model as PaymentSubmission;
        
        // Assert
        Assert.Equal(_processedPayment, model.Payment);
    }

    [Fact]
    public async Task DetailShouldReturnViewWithErrorsIfErrors()
    {
        // Act & Assert
        ViewResult result = await _paymentController.Detail("slug", "Test Error", "false") as ViewResult;
        PaymentSubmission model = result.ViewData.Model as PaymentSubmission;

        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.True(_paymentController.ModelState.ContainsKey(nameof(PaymentSubmission.Reference)));
    }

    [Fact]
    public async Task DetailShouldGetA404NotFoundPayment()
    {
        // Arrange
        _fakeRepository
            .Setup(repo => repo.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        HttpResponse response = await _paymentController.Detail("not-found-slug", null, null) as HttpResponse; ;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task DetailPostShouldGetA404NotFoundPayment()
    {
        // Arrange
        _fakeRepository
            .Setup(repo => repo.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        _paymentController.ObjectValidator = _objectValidator.Object;

        HttpResponse response = await _paymentController.Detail("not-found-slug", new PaymentSubmission()) as HttpResponse;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task DetailPostShouldCallGatewayCreateImmediateBasket()
    {
        // Arrange
        _civicaPayGateway
            .Setup(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
            .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
            {
                StatusCode = HttpStatusCode.OK,
                ResponseContent = new CreateImmediateBasketResponse
                {
                    BasketReference = "testRef",
                    BasketToken = "testBasketToken",
                    ResponseCode = "00000"
                }
            });

        _civicaPayGateway
            .Setup(gateway => gateway.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("redirectUrl");

        _paymentController.ObjectValidator = _objectValidator.Object;

        // Act
        await _paymentController.Detail("slug", new PaymentSubmission
        {
            Payment = _processedPayment,
            Amount = 12.00m,
            Reference = "123456789"
        });

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
    }

    [Fact]
    public async Task DetailPostShouldReturnDetailsViewIfModelStateInvalid()
    {
        // Arrange
        _paymentController.ObjectValidator = _objectValidator.Object;
        _paymentController.ModelState.AddModelError("Reference", "error");
        
        // Act
        ViewResult result = await _paymentController.Detail("slug", new PaymentSubmission
        {
            Payment = _processedPayment,
            Amount = 12.00m,
            Reference = "123456"
        }) as ViewResult;

        // Asssert
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
                ResponseContent = new CreateImmediateBasketResponse
                {
                    BasketReference = "testRef",
                    BasketToken = "testBasketToken",
                    ResponseCode = "00000"
                }
            });

        _paymentController.ObjectValidator = _objectValidator.Object;

        // Act
        ViewResult result = await _paymentController.Detail("slug", new PaymentSubmission
        {
            Payment = _processedPayment,
            Amount = 12.00m,
            Reference = "123456789"
        }) as ViewResult;
        
        // Assert
        _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", result.ViewName);
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
                ResponseContent = new CreateImmediateBasketResponse
                {
                    BasketReference = "testRef",
                    BasketToken = "testBasketToken",
                    ResponseCode = "00001"
                }
            });

        _paymentController.ObjectValidator = _objectValidator.Object;

        // Act
        ViewResult result = await _paymentController.Detail("slug", new PaymentSubmission
        {
            Payment = _processedPayment,
            Amount = 12.00m,
            Reference = "123456789"
        }) as ViewResult;

        // Assert
        _civicaPayGateway.Verify(_ => _.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task DetailPostShouldCallGatewayGetPaymentUrl()
    {
        // Arrange
        _civicaPayGateway
            .Setup(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()))
            .ReturnsAsync(new HttpResponse<CreateImmediateBasketResponse>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                ResponseContent = new CreateImmediateBasketResponse
                {
                    BasketReference = "testRef",
                    BasketToken = "testBasketToken",
                    ResponseCode = "00000"
                }
            });

        _civicaPayGateway
            .Setup(gateway => gateway.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("redirectUrl");

        _paymentController.ObjectValidator = _objectValidator.Object;

        // Act
        await _paymentController.Detail("slug", new PaymentSubmission
        {
            Payment = _processedPayment,
            Amount = 12.00m,
            Reference = "123456789"
        });

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SuccessShouldCallRepositoryForPaymentIfPathIsPayment()
    {
        // Act
        await _paymentController.Success("slug", "callingAppTxnRef", "00000");

        // Assert
        _fakeRepository.Verify(repo => repo.Get<Payment>("slug", null), Times.Once);
    }

    [Fact]
    public async Task SuccessShouldCallRepositoryForServicePayPaymentIfPathIsServicePayPayment()
    {
        // Act
        await _paymentControllerWithServicePayPaymentPath.Success("slug", "callingAppTxnRef", "00000");

        // Assert
        _fakeRepository.Verify(repo => repo.Get<ServicePayPayment>("slug", null), Times.Once);
    }

    [Fact]
    public async Task SuccessShouldReturnResponseIfNotSuccessful()
    {
        // Arrange
        _fakeRepository
            .Setup(repo => repo.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, _processedPayment, "Not found"));

        // Act
        HttpResponse result = await _paymentController.Success("slug", "callingAppTxnRef", "00000") as HttpResponse;

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Equal(_processedPayment, result.Content);
    }

    [Theory]
    [InlineData(true, "00022", "../ServicePayPayment/Declined")]
    [InlineData(true, "99999", "../ServicePayPayment/Failure")]
    [InlineData(false, "00023", "Declined")]
    [InlineData(false, "99999", "Failure")]
    public async Task SuccessShouldReturnCorrectErrorView(bool isServicePayPaymentPath, string responseCode, string expectedView)
    {
        // Act
        ViewResult result = isServicePayPaymentPath
            ? await _paymentControllerWithServicePayPaymentPath.Success("slug", "callingAppTxnRef", responseCode) as ViewResult
            : await _paymentController.Success("slug", "callingAppTxnRef", responseCode) as ViewResult;

        // Assert
        Assert.Equal(expectedView, result.ViewName);
    }

    [Fact]
    public async Task SuccessShouldReturnViewWithModel()
    {
        // Arrange
        PaymentSuccess model = new()
        {
            Title = _processedPayment.Title,
            ReceiptNumber = "123456",
            MetaDescription = _processedPayment.MetaDescription
        };

        // Act
        ViewResult result = await _paymentController.Success("slug", "123456", "00000") as ViewResult;
        
        // Assert
        Assert.Equivalent(model, result.Model);
    }
}