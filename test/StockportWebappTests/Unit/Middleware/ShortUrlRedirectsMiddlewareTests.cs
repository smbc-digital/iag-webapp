namespace StockportWebappTests_Unit.Unit.Middleware;

public class ShortUrlRedirectsMiddlewareTests
{
    private readonly Mock<ILogger<ShortUrlRedirectsMiddleware>> _logger = new();
    private readonly ShortUrlRedirectsMiddleware _middleware;
    private readonly BusinessId _businessId = new BusinessId("unittest");
    private readonly Mock<IRepository> _mockRepository = new();

    public ShortUrlRedirectsMiddlewareTests()
    {
        Mock<RequestDelegate> next = new();
        BusinessIdRedirectDictionary shortItems = new() { { "unittest", new RedirectDictionary { { "/short-test", "short-redirect-url" } } } };
        BusinessIdRedirectDictionary legacyItems = new() { { "unittest", new RedirectDictionary { { "/legacy-test", "legacy-redirect-url" } } } };
        ShortUrlRedirects shortUrlRedirect = new(shortItems);
        shortUrlRedirect.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 5, 0));
        LegacyUrlRedirects legacyUrlRedirects = new(legacyItems);
        legacyUrlRedirects.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 5, 0));

        _middleware = new(next.Object, shortUrlRedirect, legacyUrlRedirects, _logger.Object, _mockRepository.Object);
    }

    [Fact]
    public async Task Invoke_ShouldNotCallRepository_IfCachedRedirectsNotExpired()
    {
        // Arrange
        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/short-test";

        // Act
        await _middleware.Invoke(httpContext, _businessId);

        // Assert
        _mockRepository.Verify(repo => repo.GetRedirects(), Times.Never);
    }

    [Fact]
    public async Task Invoke_ShouldCallRepository_IfCachedRedirectsHaveExpired()
    {
        // Arrange
        Mock<RequestDelegate> next = new();
        BusinessIdRedirectDictionary shortItems = new() { { "unittest", new RedirectDictionary { { "/short-test", "short-redirect-url" } } } };
        BusinessIdRedirectDictionary legacyItems = new() { { "unittest", new RedirectDictionary { { "/legacy-test", "legacy-redirect-url" } } } };
        ShortUrlRedirects shortUrlRedirect = new(shortItems);
        shortUrlRedirect.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 50, 0));
        LegacyUrlRedirects legacyUrlRedirects = new(legacyItems);
        legacyUrlRedirects.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 50, 0));
        ShortUrlRedirectsMiddleware middleware = new(next.Object, shortUrlRedirect, legacyUrlRedirects, _logger.Object, _mockRepository.Object);

        _mockRepository
            .Setup(repo => repo.GetRedirects())
            .ReturnsAsync(new HttpResponse(200, new Redirects(shortItems, legacyItems), string.Empty));

        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/short-test";

        // Act
        await middleware.Invoke(httpContext, _businessId);

        // Assert
        _mockRepository.Verify(repo => repo.GetRedirects(), Times.Once);
    }

    [Fact]
    public async Task ItReturns302ForCorrectHttpRedirect_ForShortUrlRedirect()
    {
        // Arrange
        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/short-test";

        // Act
        await _middleware.Invoke(httpContext, _businessId);

        // Assert
        Assert.Equal(302, httpContext.Response.StatusCode);
        Assert.Equal("short-redirect-url", httpContext.Response.Headers["Location"]);
        LogTesting.Assert(_logger, LogLevel.Information, "Redirecting from: /short-test, to: short-redirect-url");
    }

    [Fact]
    public async Task ItReturns302ForCorrectHttpRedirectIgnoringCase()
    {
        // Arrange
        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/SHORT-TEST";

        // Act
        await _middleware.Invoke(httpContext, _businessId);

        // Assert
        Assert.Equal(302, httpContext.Response.StatusCode);
        Assert.Equal("short-redirect-url", httpContext.Response.Headers["Location"][0]);
    }

    [Fact]
    public async Task ItReturns200ForKeyNotInRedirects()
    {
        // Arrange
        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/not-in-redirects";

        // Act
        await _middleware.Invoke(httpContext, _businessId);

        // Assert
        Assert.Equal(200, httpContext.Response.StatusCode);
        Assert.Empty(httpContext.Response.Headers);
    }

    [Fact]
    public async Task ItReturns200ForRootPath()
    {
        // Arrange
        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/";

        // Act
        await _middleware.Invoke(httpContext, _businessId);

        // Assert
        Assert.Equal(200, httpContext.Response.StatusCode);
        Assert.Empty(httpContext.Response.Headers);
    }

    [Fact]
    public async Task ItShouldReturn200ForBusinessIdNotInRedirects()
    {
        // Arrange
        Mock<ILogger<ShortUrlRedirectsMiddleware>> logger = new();
        Mock<RequestDelegate> next = new();
        BusinessIdRedirectDictionary shortItems = new() { { "unittest", new RedirectDictionary { { "/short-test", "short-redirect-url" } } } };
        BusinessIdRedirectDictionary legacyItems = new() { { "unittest", new RedirectDictionary { { "/legacy-test", "legacy-redirect-url" } } } };
        ShortUrlRedirects shortUrlRedirect = new(shortItems);
        shortUrlRedirect.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 3, 0));
        LegacyUrlRedirects legacyUrlRedirects = new(legacyItems);
        legacyUrlRedirects.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 3, 0));
        BusinessId businessId = new("not-in-redirects");
        ShortUrlRedirectsMiddleware middleware = new(next.Object, shortUrlRedirect, legacyUrlRedirects, _logger.Object, _mockRepository.Object); ;
        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/short-test";

        // Act
        await middleware.Invoke(httpContext, businessId);

        // Assert
        Assert.Equal(200, httpContext.Response.StatusCode);
        Assert.Empty(httpContext.Response.Headers);
    }
}