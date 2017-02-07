using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StockportWebapp.Middleware;
using Xunit;

namespace StockportWebappTests.Unit.Middleware
{
    public class SecurityHeaderMiddlewareTest
    {

        private readonly SecurityHeaderMiddleware _middleware;

        public SecurityHeaderMiddlewareTest()
        {
            var requestDelegate = new Mock<RequestDelegate>();

            _middleware = new SecurityHeaderMiddleware(requestDelegate.Object);
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
