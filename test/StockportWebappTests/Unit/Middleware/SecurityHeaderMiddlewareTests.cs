using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Middleware;
using Xunit;

namespace StockportWebappTests.Unit.Middleware
{
    public class SecurityHeaderMiddlewareTests
    {

        private readonly SecurityHeaderMiddleware _middleware;
        private readonly Mock<FeatureToggles> _featureToggles;
        private readonly Mock<RequestDelegate> _requestDelegate;

        public SecurityHeaderMiddlewareTests()
        {
            _requestDelegate = new Mock<RequestDelegate>();
            _featureToggles = new Mock<FeatureToggles>();

            _featureToggles.Setup(x => x.SecurityHeaders).Returns(true);

            _middleware = new SecurityHeaderMiddleware(_requestDelegate.Object, _featureToggles.Object);
        }

        [Fact]
        public void ShouldReturnStrictTransportSecurityHeaderForWWW()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("www.domain.com");

            _middleware.Invoke(context);

            context.Response.Headers["Strict-Transport-Security"].ToString().Should().NotBeNullOrEmpty();
            context.Response.Headers["Strict-Transport-Security"].ToString().Should().Be("max-age=31536000");
        }

        [Fact]
        public void ShouldReturnStrictTransportSecurityHeaderForInt()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("int-iag.domain.com");

            _middleware.Invoke(context);

            context.Response.Headers["Strict-Transport-Security"].ToString().Should().NotBeNullOrEmpty();
            context.Response.Headers["Strict-Transport-Security"].ToString().Should().Be("max-age=31536000");
        }

        [Fact]
        public void ShouldReturnStrictTransportSecurityHeaderForQA()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("int-qa.domain.com");

            _middleware.Invoke(context);

            context.Response.Headers["Strict-Transport-Security"].ToString().Should().NotBeNullOrEmpty();
            context.Response.Headers["Strict-Transport-Security"].ToString().Should().Be("max-age=31536000");
        }

        [Fact]
        public void ShouldReturnStrictTransportSecurityHeaderForStage()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("int-stage.domain.com");

            _middleware.Invoke(context);

            context.Response.Headers["Strict-Transport-Security"].ToString().Should().NotBeNullOrEmpty();
            context.Response.Headers["Strict-Transport-Security"].ToString().Should().Be("max-age=31536000");
        }

        [Fact]
        public void ShouldNotReturnStrictTransportSecurityHeaderForLocalAddresses()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("stockportgov:5555");

            _middleware.Invoke(context);

            context.Response.Headers["Strict-Transport-Security"].ToString().Should().BeNullOrEmpty();
        }
    }
}
