using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class ErrorControllerTest
    {
        private readonly ErrorController _controller;
        private Mock<ILegacyRedirectsManager> _legacyRedirects;

        public ErrorControllerTest()
        {
            _legacyRedirects = new Mock<ILegacyRedirectsManager>();
            SetupThatNoLegacyRedirectMatches();

           _controller = new ErrorController(_legacyRedirects.Object, new FeatureToggles() { LegacyUrlRedirects = true });
        }

        [Fact]
        public void ShouldTellUsSomethingsMissingIfAPageWasNotFound()
        {
            var result = AsyncTestHelper.Resolve(_controller.Error("404")) as ViewResult;

            result.ViewData[@"ErrorHeading"].Should().Be("Something's missing");
        }

        [Fact]
        public void ShouldTellUsSomethingIsWrongIfADifferentErrorOccurred()
        {
            var result = AsyncTestHelper.Resolve(_controller.Error("500")) as ViewResult;

            result.ViewData[@"ErrorHeading"].Should().Be("Something went wrong");
        }

        [Fact]
        public void ShouldRedirectIfPageNotFoundButMatchedALegacyRedirect()
        {
            var redirectedToLocation = @"/redirected-to-location-from-the-rule";

            _legacyRedirects.Setup(o => o.RedirectUrl()).Returns(redirectedToLocation);

            var result = AsyncTestHelper.Resolve(_controller.Error("404")) as RedirectResult;

            result.Url.Should().Be(redirectedToLocation);
        }

        [Fact]
        public void ShouldNotSearchForRedirectsIfLegacyUrlRedirectsFeatureTogglesAreOff()
        {
            var controller = new ErrorController(_legacyRedirects.Object, new FeatureToggles() { LegacyUrlRedirects = false });

            AsyncTestHelper.Resolve(controller.Error("404"));

            _legacyRedirects.Verify(o => o.RedirectUrl(), Times.Never);
        }
        
        [Fact]
        public void ShouldNotSearchForLegacyRedirectsIfNot404Error()
        {
            AsyncTestHelper.Resolve(_controller.Error("500"));

            _legacyRedirects.Verify(o => o.RedirectUrl(), Times.Never);
        }

        private void SetupThatNoLegacyRedirectMatches()
        {
            _legacyRedirects.Setup(o => o.RedirectUrl()).Returns(string.Empty);
        }

    }
}