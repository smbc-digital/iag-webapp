using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Middleware;
using Xunit;

namespace StockportWebappTests.Unit.Middleware
{
    public class BetaToWwwMiddlewareTest
    {
        private readonly BetaToWwwMiddleware _middleware;
        private readonly Mock<ILogger<BetaToWwwMiddleware>> _logger;
        private readonly FeatureToggles _featureToggles;
        private readonly Mock<RequestDelegate> _requestDelegate;

        public BetaToWwwMiddlewareTest()
        {
            _requestDelegate = new Mock<RequestDelegate>();
            _logger = new Mock<ILogger<BetaToWwwMiddleware>>();
            _featureToggles = new FeatureToggles { BetaToWwwRedirect = true };
            _middleware = new BetaToWwwMiddleware(_requestDelegate.Object, _logger.Object, _featureToggles);
        }

        [Fact]
        public void ShouldRedirectBetaHostToWwwHost()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("beta.domain.notwww");

            _middleware.Invoke(context);

            context.Response.Headers["Location"].ToString().Should().Be("://www.domain.notwww");
            context.Response.StatusCode.Should().Be(302);
        }

        [Fact]
        public void ShouldNotRedirectAlphaHostToWwwHost()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("alpha.domain.notwww");

            _middleware.Invoke(context);

            context.Response.StatusCode.Should().Be(200);
        }

        [Fact]
        public void ShouldNotRedirectWwwHost()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("www.domain.www");

            _middleware.Invoke(context);

            context.Response.StatusCode.Should().Be(200);
        }

        [Fact]
        public void ShouldRedirectBetaHostToWwwHostAndKeepPath()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("beta.domain.com");
            context.Request.Path = "/test";

            _middleware.Invoke(context);

            context.Response.Headers["Location"].ToString().Should().Be("://www.domain.com/test");
        }

        [Fact]
        public void ShouldLogRedirect()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("beta.domain.com");
            context.Request.Path = "/test";
            _middleware.Invoke(context);
            LogTesting.Assert(_logger, LogLevel.Information, "beta.domain.com redirected to www.domain.com for path: /test");
        }

        [Fact]
        public void ShouldNotRedirectFromBetaToWwwWhenFeatureToggleIsOff()
        {
            var featureToggles = new FeatureToggles { BetaToWwwRedirect = false };
            var middleware = new BetaToWwwMiddleware(_requestDelegate.Object, _logger.Object, featureToggles);
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("beta.domain.com");
            context.Request.Path = "/test";
            middleware.Invoke(context);

            context.Response.StatusCode.Should().Be(200);
        }
    }
}
