using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Controllers;
using StockportWebappTests_Unit.Helpers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class ErrorControllerTest
    {
        private readonly ErrorController _controller;
        private readonly Mock<ILegacyRedirectsManager> _legacyRedirects;
        private readonly Mock<ILogger<ErrorController>> _logger;
        private readonly Mock<HttpContext> _httpContext;

        public ErrorControllerTest()
        {
            _logger = new Mock<ILogger<ErrorController>>();
            _httpContext = new Mock<HttpContext>();
            _legacyRedirects = new Mock<ILegacyRedirectsManager>();
           _controller = new ErrorController(_legacyRedirects.Object, _logger.Object);
        }

        [Fact]
        public void ShouldTellUsSomethingsMissingIfAPageWasNotFound()
        {
            const string url = "/a-url";
            SetUpUrl(_httpContext, url);
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).Returns(string.Empty);
            var result = _controller.Error("404") as ViewResult;;
            result.ViewData[@"ErrorHeading"].Should().Be("Something's missing");
        }
    
        [Fact]
        public void ShouldTellUsSomethingIsWrongIfADifferentErrorOccurred()
        {
            var result = _controller.Error("500") as ViewResult;;

            result.ViewData[@"ErrorHeading"].Should().Be("Something went wrong");
        }

        [Fact]
        public void ShouldRedirectIfPageNotFoundButMatchedALegacyRedirect()
        {
            const string url = "/a-url";
            SetUpUrl(_httpContext, url);
            var redirectedToLocation = @"/redirected-to-location-from-the-rule";
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).Returns(redirectedToLocation);
            var result = _controller.Error("404") as RedirectResult;;
            result.Url.Should().Be(redirectedToLocation);
            result.Permanent.Should().BeTrue();
        }

        [Fact]
        public void ShouldLogInformationIfLegacyUrlNotfound()
        {
            const string url = "/a-url";
            SetUpUrl(_httpContext, url);
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).Returns(string.Empty);
            _controller.Error("404");
            LogTesting.Assert(_logger, LogLevel.Information,
                $"No legacy url matching current url ({url}) found");
        }

        [Fact]
        public void ShouldLogInformationIfLegacyUrlfound()
        {
            const string url = "/a-url";
            SetUpUrl(_httpContext, url);
            var redirectedToLocation = @"/redirected-to-location-from-the-rule";
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).Returns(redirectedToLocation);

            _controller.Error("404");

            LogTesting.Assert(_logger, LogLevel.Information,
                $"A legacy redirect was found - redirecting to {redirectedToLocation}");
        }

        private void SetUpUrl(Mock<HttpContext> httpContext, string url)
        {
            httpContext.Setup(c => c.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                 .Returns(url);
        }
    }
}