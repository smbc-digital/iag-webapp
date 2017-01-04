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

        [Fact]
        public void ShouldContainContentSecurityPolicyHeader()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("www.domain.com");

            _middleware.Invoke(context);

            var header = context.Response.Headers["Content-Security-Policy"].ToString();

            header.Should().NotBeNullOrEmpty();
            header.Should().Contain("default-src https:");
        }

        [Fact]
        public void ShouldContainContentSecurityPolicyHeaderForSrcReferences()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("www.domain.com");

            _middleware.Invoke(context);
            var header = context.Response.Headers["Content-Security-Policy"].ToString();

            header.Should().Contain("font-src 'self' https://font.googleapis.com");
            header.Should().Contain("img-src 'self' https://images.contentful.com https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/");
            header.Should().Contain("style-src 'self' 'unsafe-inline' https://customer.cludo.com/css/112/1144/ https://maxcdn.bootstrapcdn.com/font-awesome/");
            header.Should().Contain("script-src 'self' 'unsafe-inline' https://ajax.googleapis.com/ajax/libs/jquery/ https://api.cludo.com/scripts/ https://cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ https://s3-eu-west-1.amazonaws.com/share.typeform.com/ https://js.buto.tv/video/ https://s7.addthis.com/js/300/addthis_widget.js");
            header.Should().Contain("form-action 'self'");
            header.Should().Contain("media-src 'self' https://www.youtube.com/ *.cloudfront.net/butotv/live/videos/");
        }
    }
}
