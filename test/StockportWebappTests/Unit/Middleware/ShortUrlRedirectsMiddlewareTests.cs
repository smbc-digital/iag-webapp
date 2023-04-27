namespace StockportWebappTests_Unit.Unit.Middleware;

public class ShortUrlRedirectsMiddlewareTests
{
    private readonly Mock<ILogger<ShortUrlRedirectsMiddleware>> _logger;
    private readonly ShortUrlRedirectsMiddleware _middleware;
    private readonly BusinessId _businessId = new BusinessId("unittest");
    private readonly Mock<IRepository> _mockRepository = new();

    public ShortUrlRedirectsMiddlewareTests()
    {
        _logger = new Mock<ILogger<ShortUrlRedirectsMiddleware>>();
        var next = new Mock<RequestDelegate>();
        var shortItems = new BusinessIdRedirectDictionary { { "unittest", new RedirectDictionary { { "/short-test", "short-redirect-url" } } } };
        var legacyItems = new BusinessIdRedirectDictionary { { "unittest", new RedirectDictionary { { "/legacy-test", "legacy-redirect-url" } } } };
        var shortUrlRedirect = new ShortUrlRedirects(shortItems);
        shortUrlRedirect.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 5, 0));
        var legacyUrlRedirects = new LegacyUrlRedirects(legacyItems);
        legacyUrlRedirects.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 5, 0));
        _middleware = new ShortUrlRedirectsMiddleware(next.Object, shortUrlRedirect, legacyUrlRedirects, _logger.Object, _mockRepository.Object);
    }

    [Fact]
    public void Invoke_ShouldNotCallRepository_IfCachedRedirectsNotExpired()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/short-test";

        _middleware.Invoke(httpContext, _businessId).Wait();

        _mockRepository.Verify(_ => _.GetRedirects(), Times.Never);
    }

    [Fact]
    public void Invoke_ShouldCallRepository_IfCachedRedirectsHaveExpired()
    {
        var next = new Mock<RequestDelegate>();
        var shortItems = new BusinessIdRedirectDictionary { { "unittest", new RedirectDictionary { { "/short-test", "short-redirect-url" } } } };
        var legacyItems = new BusinessIdRedirectDictionary { { "unittest", new RedirectDictionary { { "/legacy-test", "legacy-redirect-url" } } } };
        var shortUrlRedirect = new ShortUrlRedirects(shortItems);
        shortUrlRedirect.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 50, 0));
        var legacyUrlRedirects = new LegacyUrlRedirects(legacyItems);
        legacyUrlRedirects.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 50, 0));
        var middleware = new ShortUrlRedirectsMiddleware(next.Object, shortUrlRedirect, legacyUrlRedirects, _logger.Object, _mockRepository.Object);

        _mockRepository.Setup(_ => _.GetRedirects()).ReturnsAsync(new StockportWebapp.Http.HttpResponse(200, new Redirects(shortItems, legacyItems), string.Empty));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/short-test";

        middleware.Invoke(httpContext, _businessId).Wait();

        _mockRepository.Verify(_ => _.GetRedirects(), Times.Once);
    }

    [Fact]
    public void ItReturns302ForCorrectHttpRedirect_ForShortUrlRedirect()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/short-test";

        _middleware.Invoke(httpContext, _businessId).Wait();

        httpContext.Response.StatusCode.Should().Be(302);
        httpContext.Response.Headers["Location"][0].Should().Be("short-redirect-url");

        LogTesting.Assert(_logger, LogLevel.Information, "Redirecting from: /short-test, to: short-redirect-url");
    }

    [Fact]
    public void ItReturns302ForCorrectHttpRedirectIgnoringCase()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/SHORT-TEST";

        _middleware.Invoke(httpContext, _businessId).Wait();

        httpContext.Response.StatusCode.Should().Be(302);
        httpContext.Response.Headers["Location"][0].Should().Be("short-redirect-url");
    }

    [Fact]
    public void ItReturns200ForKeyNotInRedirects()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/not-in-redirects";

        _middleware.Invoke(httpContext, _businessId).Wait();

        httpContext.Response.StatusCode.Should().Be(200);
        httpContext.Response.Headers.Count.Should().Be(0);
    }

    [Fact]
    public void ItReturns200ForRootPath()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/";

        _middleware.Invoke(httpContext, _businessId).Wait();

        httpContext.Response.StatusCode.Should().Be(200);
        httpContext.Response.Headers.Count.Should().Be(0);
    }

    [Fact]
    public void ItShouldReturn200ForBusinessIdNotInRedirects()
    {
        var logger = new Mock<ILogger<ShortUrlRedirectsMiddleware>>();
        var next = new Mock<RequestDelegate>();
        var shortItems = new BusinessIdRedirectDictionary { { "unittest", new RedirectDictionary { { "/short-test", "short-redirect-url" } } } };
        var legacyItems = new BusinessIdRedirectDictionary { { "unittest", new RedirectDictionary { { "/legacy-test", "legacy-redirect-url" } } } };
        var shortUrlRedirect = new ShortUrlRedirects(shortItems);
        shortUrlRedirect.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 3, 0));
        var legacyUrlRedirects = new LegacyUrlRedirects(legacyItems);
        legacyUrlRedirects.LastUpdated = DateTime.Now.Subtract(new TimeSpan(0, 3, 0));
        var businessId = new BusinessId("not-in-redirects");
        var middleware = new ShortUrlRedirectsMiddleware(next.Object, shortUrlRedirect, legacyUrlRedirects, _logger.Object, _mockRepository.Object); ;
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/short-test";

        middleware.Invoke(httpContext, businessId).Wait();

        httpContext.Response.StatusCode.Should().Be(200);
        httpContext.Response.Headers.Count.Should().Be(0);
    }
}
