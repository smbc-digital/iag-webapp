using System.Threading.Tasks;
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
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;


        public ErrorControllerTest()
        {
            _logger = new Mock<ILogger<ErrorController>>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _legacyRedirects = new Mock<ILegacyRedirectsManager>();
        }

        [Fact]
        public async Task ShouldTellUsSomethingsMissingIfAPageWasNotFound()
        {
            const string url = "/a-url";

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/pathThatDoesntExist";
            httpContext.Response.StatusCode = 404;
            var mockHttpContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ErrorController(_legacyRedirects.Object, _logger.Object) { ControllerContext = mockHttpContext };
            controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
            {
                OriginalPath = "/OriginalPath"
            });

            _legacyRedirects.Setup(o => o.RedirectUrl(url)).ReturnsAsync(string.Empty);
            var result = await controller.Error() as ViewResult;
            result.ViewData[@"ErrorHeading"].Should().Be("Something's missing");
        }

        [Fact]
        public async Task ShouldTellUsSomethingIsWrongIfADifferentErrorOccurred()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";
            httpContext.Response.StatusCode = 500;
            var mockHttpContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ErrorController(_legacyRedirects.Object, _logger.Object) { ControllerContext = mockHttpContext };
            controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
            {
                OriginalPath = "/OriginalPath"
            });

            var result = await controller.Error() as ViewResult; ;

            result.ViewData[@"ErrorHeading"].Should().Be("Something went wrong");
        }

        [Fact]
        public async Task ShouldRedirectIfPageNotFoundButMatchedALegacyRedirect()
        {
            const string url = "/a-url";
            var redirectedToLocation = @"/redirected-to-location-from-the-rule";
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).ReturnsAsync(redirectedToLocation);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";
            httpContext.Response.StatusCode = 404;
            var mockHttpContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ErrorController(_legacyRedirects.Object, _logger.Object) { ControllerContext = mockHttpContext };
            controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
            {
                OriginalPath = url
            });

            var result = await controller.Error() as RedirectResult;
            result.Url.Should().Be(redirectedToLocation);
            result.Permanent.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldLogInformationIfLegacyUrlNotfound()
        {
            const string url = "/a-url";
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).ReturnsAsync(string.Empty);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";
            httpContext.Response.StatusCode = 404;
            var mockHttpContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ErrorController(_legacyRedirects.Object, _logger.Object) { ControllerContext = mockHttpContext };
            controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
            {
                OriginalPath = url
            });

            await controller.Error();
            LogTesting.Assert(_logger, LogLevel.Information,
                $"No legacy url matching current url ({url}) found");
        }

        [Fact]
        public async Task ShouldLogInformationIfLegacyUrlfound()
        {
            const string url = "/a-url";
            var redirectedToLocation = @"/redirected-to-location-from-the-rule";
            _legacyRedirects.Setup(o => o.RedirectUrl(url)).ReturnsAsync(redirectedToLocation);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = "/test";
            httpContext.Response.StatusCode = 404;
            var mockHttpContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ErrorController(_legacyRedirects.Object, _logger.Object) { ControllerContext = mockHttpContext };
            controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
            {
                OriginalPath = url
            });

            await controller.Error();

            LogTesting.Assert(_logger, LogLevel.Information,
                $"A legacy redirect was found - redirecting to {redirectedToLocation}");
        }
    }
}