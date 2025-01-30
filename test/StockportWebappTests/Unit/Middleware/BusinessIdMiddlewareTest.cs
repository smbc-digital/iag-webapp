namespace StockportWebappTests_Unit.Unit.Middleware;

public class BusinessIdMiddlewareTest
{
    private readonly BusinessIdMiddleware _businessIdMiddleware;
    private readonly BusinessId _businessId = new();
    private readonly Mock<ILogger<BusinessIdMiddleware>> _logger = new();
    private readonly Mock<RequestDelegate> requestDelegate = new();

    public BusinessIdMiddlewareTest() =>
        _businessIdMiddleware = new(requestDelegate.Object, _logger.Object);

    [Fact]
    public void ShouldSetBusinessIdIfBusinessIdIsInHeader()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Headers["BUSINESS-ID"] = "business-id";

        // Act
        _businessIdMiddleware.Invoke(context, _businessId);

        // Assert
        Assert.Equal("business-id", _businessId.ToString());
    }

    [Fact]
    public void ShouldSetToDefaultBusinessIdIfBusinessIdIsNoInHeader()
    {
        // Arrange
        DefaultHttpContext context = new();
        
        // Act
        _businessIdMiddleware.Invoke(context, _businessId);

        // Assert
        Assert.Equal("stockportgov", _businessId.ToString());
    }

    [Fact]
    public void ShouldLogWarningIfNoBusinessIdIsSet()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Path = "/";
        
        // Act
        _businessIdMiddleware.Invoke(context, _businessId);

        // Assert
        LogTesting.Assert(_logger, LogLevel.Information, "BUSINESS-ID has not been set, setting to default");
    }

    [Fact]
    public void ShouldNotLogIfNoBusinessIdIsSetAndIsAsset()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Path = "/assets/test.js";
        
        // Act
        _businessIdMiddleware.Invoke(context, _businessId);

        // Assert
        LogTesting.DoesNotContain(_logger, LogLevel.Information, "BUSINESS-ID has not been set, setting to default");
    }

    [Fact]
    public void ShouldNotLogIfNoBusinessIdIsSetAndIsHealthCheck()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Path = "/_healthcheck";

        // Act
        _businessIdMiddleware.Invoke(context, _businessId);

        // Assert
        LogTesting.DoesNotContain(_logger, LogLevel.Information, "BUSINESS-ID has not been set, setting to default");
    }

    [Fact]
    public void ShouldLogInformationIfBusniessIdIsSet()
    {
        // Arrange
        DefaultHttpContext context = new();
        context.Request.Headers["BUSINESS-ID"] = "business-id";

        // Act
        _businessIdMiddleware.Invoke(context, _businessId);

        // Assert
        LogTesting.Assert(_logger, LogLevel.Information, "BUSINESS-ID has been set to: business-id");
    }
}