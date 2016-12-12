using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Middleware;
using Xunit;

namespace StockportWebappTests.Unit.Middleware
{
    public class OldEventsMiddlewareTest
    {
        private readonly OldEventsMiddleware _oldEventsMiddleware;
        private readonly Mock<ILegacyRedirectsManager> _legacyRedirects;

        public OldEventsMiddlewareTest()
        {
            var requestDelegate = new Mock<RequestDelegate>();
            _legacyRedirects = new Mock<ILegacyRedirectsManager>();
            _oldEventsMiddleware = new OldEventsMiddleware(requestDelegate.Object,
                new FeatureToggles {EventCalendar = true});
        }

        [Theory]
        [InlineData("/events")]
        [InlineData("/events/one")]
        public void ShouldReturnNewEventIfFeatureToggleIsOn(string path)
        {
            var context = new DefaultHttpContext();
            context.Request.Path = path;

            _oldEventsMiddleware.Invoke(context, _legacyRedirects.Object);

            context.Response.StatusCode.Should().Be(200);
        }

        [Theory]
        [InlineData("/events")]
        [InlineData("/events/one")]
        [InlineData("/events/one/two")]
        [InlineData("/events/one/two/three")]
        [InlineData("/events/one/two/three/four")]
        [InlineData("/events/one/two/three/four/five")]
        public void ShouldReturnOldEventIfFeatureToggleIsOff(string path)
        {
            var requestDelegate = new Mock<RequestDelegate>();
            var oldEventsMiddleware = new OldEventsMiddleware(requestDelegate.Object,
                new FeatureToggles { EventCalendar = false });

            var context = new DefaultHttpContext();
            context.Request.Path = path;

            _legacyRedirects.Setup(o => o.RedirectUrl(path)).Returns("redirected-url");
            oldEventsMiddleware.Invoke(context, _legacyRedirects.Object);

            context.Response.StatusCode.Should().Be(302);
            context.Response.Headers["Location"].Should().BeEquivalentTo("redirected-url");
        }

        [Theory]
        [InlineData("/events")]
        [InlineData("/events/one")]
        public void ShouldReturnToErrorControllerIfLegacyRedirectDoesNotExist(string path)
        {
            var requestDelegate = new Mock<RequestDelegate>();
            var oldEventsMiddleware = new OldEventsMiddleware(requestDelegate.Object,
                new FeatureToggles { EventCalendar = false });

            var context = new DefaultHttpContext();
            context.Request.Path = path;

            _legacyRedirects.Setup(o => o.RedirectUrl(path)).Returns(string.Empty);
            oldEventsMiddleware.Invoke(context, _legacyRedirects.Object);

            context.Response.StatusCode.Should().Be(302);
            context.Response.Headers["Location"].Should().BeEquivalentTo("/Error/404");
        }
    }
}
