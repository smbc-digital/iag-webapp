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
    private readonly Mock<ILogger<PaymentController>> _logger = new();

    private readonly ProcessedPayment _processedPayment = new("title",
                                                            "slug",
                                                            "teaser",
                                                            "description",
                                                            "default",
                                                            "payDetailsText",
                                                            "account number",
                                                            "fund",
                                                            "glCode",
                                                            null,
                                                            EPaymentReferenceValidation.None,
                                                            "meta",
                                                            "returnUrl",
                                                            "catId",
                                                            "accRef",
                                                            "payDesc",
                                                            null);

    private readonly ProcessedServicePayPayment _processedServicePayPayment = new("title",
                                                                                "slug",
                                                                                "teaser",
                                                                                "description",
                                                                                "payDetailsText",
                                                                                "account number",
                                                                                null,
                                                                                EPaymentReferenceValidation.None,
                                                                                "meta",
                                                                                "returnUrl",
                                                                                "catId",
                                                                                "accRef",
                                                                                "payDesc",
                                                                                null);

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

        _paymentController = new(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object, _logger.Object)
        {
            ControllerContext = mockControllerContextPayment
        };

        _paymentControllerWithServicePayPaymentPath = new(_fakeRepository.Object, _civicaPayGateway.Object, _configuration.Object, _logger.Object)
        {
            ControllerContext = mockControllerContextServicePayPayment
        };
    }

    [Fact]
    public async Task Detail_GET_ShouldReturnView_WhenResponseIsSuccessful()
    {
        // Act
        ViewResult view = await _paymentController.Detail("slug", null, null) as ViewResult;
        PaymentSubmission model = view.ViewData.Model as PaymentSubmission;
        
        // Assert
        Assert.Equal(_processedPayment, model.Payment);
    }

    [Fact]
    public async Task Detail_GET_ShouldReturnViewWithErrors_If_Errors()
    {
        // Act
        ViewResult result = await _paymentController.Detail("slug", "Test Error", "false") as ViewResult;
        PaymentSubmission model = result.ViewData.Model as PaymentSubmission;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(model);
        Assert.True(_paymentController.ModelState.ContainsKey(nameof(PaymentSubmission.Reference)));
    }

    [Fact]
    public async Task Detail_GET_ShouldReturnNotFoundPayment()
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
    public async Task Detail_POST_ShouldReturnNotFoundPayment()
    {
        // Arrange
        _fakeRepository
            .Setup(repo => repo.Get<Payment>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        _paymentController.ObjectValidator = _objectValidator.Object;

        // Act
        HttpResponse response = await _paymentController.Detail("not-found-slug", new PaymentSubmission()) as HttpResponse;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task Detail_POST_ShouldCallGatewayCreateImmediateBasket()
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
            Amount = "12.00",
            Reference = "123456789"
        });

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
    }

    [Fact]
    public async Task Detail_POST_ShouldReturnDetailsView_If_ModelStateInvalid()
    {
        // Arrange
        _paymentController.ObjectValidator = _objectValidator.Object;
        
        // Act
        ViewResult result = await _paymentController.Detail("slug", new PaymentSubmission
        {
            Payment = _processedPayment,
            Amount = "12.00",
            Reference = string.Empty
        }) as ViewResult;

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Never);
        Assert.IsType<ViewResult>(result);

        ModelError error = _paymentController.ModelState["Reference"].Errors.First();
        Assert.Equal("Enter the account number", error.ErrorMessage);
    }

    [Fact]
    public async Task Detail_POST_ShouldReturnErrorView_If_BadRequest()
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
            Amount = "12.00",
            Reference = "123456789"
        }) as ViewResult;
        
        // Assert
        _civicaPayGateway.Verify(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", result.ViewName);
    }

    [Fact]
    public async Task Detail_POST_ShouldReturnView_If_ResponseCodeIs00001()
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
            Amount = "12.00",
            Reference = "123456789"
        }) as ViewResult;

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.CreateImmediateBasketAsync(It.IsAny<CreateImmediateBasketRequest>()), Times.Once);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Detail_POST_ShouldCallGatewayGetPaymentUrl()
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
            Amount = "12.00",
            Reference = "123456789"
        });

        // Assert
        _civicaPayGateway.Verify(gateway => gateway.GetPaymentUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Success_ShouldCallRepositoryForPayment_If_PathIsPayment()
    {
        // Act
        await _paymentController.Success("slug", "callingAppTxnRef", "00000");

        // Assert
        _fakeRepository.Verify(repo => repo.Get<Payment>("slug", null), Times.Once);
    }

    [Fact]
    public async Task Success_ShouldCallRepositoryForServicePayPayment_If_PathIsServicePayPayment()
    {
        // Act
        await _paymentControllerWithServicePayPaymentPath.Success("slug", "callingAppTxnRef", "00000");

        // Assert
        _fakeRepository.Verify(repo => repo.Get<ServicePayPayment>("slug", null), Times.Once);
    }

    [Fact]
    public async Task Success_ShouldReturnNotFound_If_NotSuccessful()
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
    [InlineData(true, "00022", "Result")]
    [InlineData(true, "99999", "Result")]
    [InlineData(false, "00023", "Result")]
    [InlineData(false, "99999", "Result")]
    public async Task Success_ShouldReturnErrorView_If_StatusCodeIsNotSuccessful(bool isServicePayPaymentPath, string responseCode, string expectedView)
    {
        // Act
        ViewResult result = isServicePayPaymentPath
            ? await _paymentControllerWithServicePayPaymentPath.Success("slug", "callingAppTxnRef", responseCode) as ViewResult
            : await _paymentController.Success("slug", "callingAppTxnRef", responseCode) as ViewResult;

        // Assert
        Assert.Equal(expectedView, result.ViewName);
    }

    [Fact]
    public async Task Success_ShouldReturnViewWithModel()
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
        PaymentResult paymentResult = result.Model as PaymentResult;

        // Assert
        Assert.Equal(model.Title, paymentResult.Title);
        Assert.Equal(model.ReceiptNumber, paymentResult.ReceiptNumber);
    }

    [Fact]
    public async Task Success_ShouldReturnSuccessView_WithValidModel()
    {
        // Act
        ViewResult result = await _paymentController.Success("slug", "txnRef", "00000") as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PaymentResult>(result.Model);
    }
}