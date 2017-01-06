using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Middleware;
using Xunit;

namespace StockportWebappTests.Unit.Middleware
{
    public class SecurityHeaderMiddlewareTest
    {

        private readonly SecurityHeaderMiddleware _middleware;
        private readonly FeatureToggles _featureToggles;
        private readonly Mock<RequestDelegate> _requestDelegate;

        public SecurityHeaderMiddlewareTest()
        {
            _requestDelegate = new Mock<RequestDelegate>();
            _featureToggles = new FeatureToggles { SecurityHeaders = true };

            _middleware = new SecurityHeaderMiddleware(_requestDelegate.Object, _featureToggles);
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
        public void ShouldContainContentSecurityPolicyHeaderForSrcReferences()
        {
            var context = new DefaultHttpContext();
            context.Request.Host = new HostString("www.domain.com");

            _middleware.Invoke(context);
            var header = context.Response.Headers["Content-Security-Policy"].ToString();

            header.Should().Contain("font-src", "'self' font.googleapis.com maxcdn.bootstrapcdn.com/font-awesome/ fonts.gstatic.com/");
            header.Should().Contain("img-src", "'self' images.contentful.com/ s3-eu-west-1.amazonaws.com/live-iag-static-assets/ uk1.siteimprove.com/ stockportb.logo-net.co.uk/ *.cloudfront.net/butotv/");
            header.Should().Contain("style-src", "'self' 'unsafe-inline' *.cludo.com/css/ maxcdn.bootstrapcdn.com/font-awesome/ fonts.googleapis.com/ *.cloudfront.net/butotv/");
            header.Should().Contain("script-src", "'self' 'unsafe-inline' 'unsafe-eval' https://ajax.googleapis.com/ajax/libs/jquery/ api.cludo.com/scripts/ customer.cludo.com/scripts/ cdnjs.cloudflare.com/ajax/libs/cookieconsent2/ s3-eu-west-1.amazonaws.com/share.typeform.com/ js.buto.tv/video/ s7.addthis.com/js/300/addthis_widget.js m.addthis.com/live/ siteimproveanalytics.com/js/ *.logo-net.co.uk/Delivery/");
            header.Should().Contain("connect-src", "'self' https://api.cludo.com/ buto-ping-middleman.buto.tv/ m.addthis.com/live/");
            header.Should().Contain("media-src", "'self' https://www.youtube.com/ *.cloudfront.net/butotv/live/videos/");
            header.Should().Contain("object-src", "'self'  https://www.youtube.com http://www.youtube.com");
        }
    }
}
