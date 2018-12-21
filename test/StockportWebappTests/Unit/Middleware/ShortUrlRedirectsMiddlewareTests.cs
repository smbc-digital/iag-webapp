using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Scheduler;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Middleware
{
    public class ShortUrlRedirectsMiddlewareTests
    {
        private readonly Mock<ILogger<ShortUrlRedirectsMiddleware>> _logger;
        private readonly ShortUrlRedirectsMiddleware _middleware;
        private readonly BusinessId _businessId = new BusinessId("unittest");

        public ShortUrlRedirectsMiddlewareTests()
        {
            _logger = new Mock<ILogger<ShortUrlRedirectsMiddleware>>();
            var next = new Mock<RequestDelegate>();
            var items = new BusinessIdRedirectDictionary {{"unittest", new RedirectDictionary {{"/test", "redirect-url"}}}};
            var urlRedirect = new ShortUrlRedirects(items);
            _middleware = new ShortUrlRedirectsMiddleware(next.Object, urlRedirect, _logger.Object);
        }

        [Fact]
        public void ItReturns302ForCorrectHttpRedirect()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";

            _middleware.Invoke(httpContext, _businessId).Wait();

            httpContext.Response.StatusCode.Should().Be(302);
            httpContext.Response.Headers["Location"][0].Should().Be("redirect-url");

            LogTesting.Assert(_logger, LogLevel.Information, "Redirecting from: /test, to: redirect-url");
        }

        [Fact]
        public void ItReturns302ForCorrectHttpRedirectIgnoringCase()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/TEST";

            _middleware.Invoke(httpContext, _businessId).Wait();

            httpContext.Response.StatusCode.Should().Be(302);
            httpContext.Response.Headers["Location"][0].Should().Be("redirect-url");
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
            var items = new BusinessIdRedirectDictionary { { "unittest", new RedirectDictionary { { "/test", "redirect-url" } } } };
            var urlRedirect = new ShortUrlRedirects(items);
            var businessId = new BusinessId("not-in-redirects");
            var middleware = new ShortUrlRedirectsMiddleware(next.Object, urlRedirect, logger.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";

            middleware.Invoke(httpContext, businessId).Wait();

            httpContext.Response.StatusCode.Should().Be(200);
            httpContext.Response.Headers.Count.Should().Be(0);
        }
    }
}
