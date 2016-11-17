using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Controllers;
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

           _controller = new ErrorController(_legacyRedirects.Object);
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

        // missing test:  don't do legacy redirects if not 404

        private void SetupThatNoLegacyRedirectMatches()
        {
            _legacyRedirects.Setup(o => o.RedirectUrl()).Returns(string.Empty);
        }

    }
}