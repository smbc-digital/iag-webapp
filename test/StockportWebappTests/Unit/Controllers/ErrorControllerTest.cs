using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class ErrorControllerTest
    {
        private readonly ErrorController _controller;
        private readonly Mock<ILegacyRedirectsManager> _legacyRedirects;
        private readonly Mock<ILogger<ErrorController>> _logger;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public ErrorControllerTest()
        {
            _logger = new Mock<ILogger<ErrorController>>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _legacyRedirects = new Mock<ILegacyRedirectsManager>();
           _controller = new ErrorController(_legacyRedirects.Object, _httpContextAccessor.Object, _logger.Object);
        }

        [Fact]
        public void ShouldTellUsSomethingsMissingIfAPageWasNotFound()
        {
            const string url = "/a-url";
            SetUpUrl(_httpContextAccessor, url);
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
            SetUpUrl(_httpContextAccessor, url);
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
            SetUpUrl(_httpContextAccessor, url);
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).Returns(string.Empty);
            _controller.Error("404");
            LogTesting.Assert(_logger, LogLevel.Information,
                $"No legacy url matching current url ({url}) found");
        }

        [Fact]
        public void ShouldLogInformationIfLegacyUrlfound()
        {
            const string url = "/a-url";
            SetUpUrl(_httpContextAccessor, url);
            var redirectedToLocation = @"/redirected-to-location-from-the-rule";
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).Returns(redirectedToLocation);

            _controller.Error("404");

            LogTesting.Assert(_logger, LogLevel.Information,
                $"A legacy redirect was found - redirecting to {redirectedToLocation}");
        }

        private void SetUpUrl(Mock<IHttpContextAccessor> httpContextAccessor, string url)
        {
            httpContextAccessor.Setup(h => h.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath)
                 .Returns(url);
        }
    }
}