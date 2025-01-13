namespace StockportWebappTests_Unit.Unit.Controllers;

public class ErrorControllerTest
{
    private readonly Mock<ILegacyRedirectsManager> _legacyRedirects = new();
    private readonly Mock<ILogger<ErrorController>> _logger = new();

    [Fact]
    public async Task ShouldTellUsSomethingsMissingIfAPageWasNotFound()
    {
        // Arrange
        DefaultHttpContext httpContext = new();

        httpContext.Request.Path = "/pathThatDoesntExist";
        httpContext.Response.StatusCode = 404;

        ControllerContext mockHttpContext = new()
        {
            HttpContext = httpContext
        };

        ErrorController controller = new(_legacyRedirects.Object, _logger.Object)
        {
            ControllerContext = mockHttpContext
        };

        controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
        {
            OriginalPath = "/OriginalPath"
        });

        _legacyRedirects
            .Setup(redirects => redirects.RedirectUrl("/a-url"))
            .ReturnsAsync(string.Empty);

        // Act
        ViewResult result = await controller.Error() as ViewResult;

        // Assert
        Assert.Equal("Something's missing", result.ViewData[@"ErrorHeading"]);
    }

    [Fact]
    public async Task ShouldTellUsSomethingIsWrongIfADifferentErrorOccurred()
    {
        // Arrange
        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/test";
        httpContext.Response.StatusCode = 500;
        
        ControllerContext mockHttpContext = new()
        {
            HttpContext = httpContext
        };

        ErrorController controller = new(_legacyRedirects.Object, _logger.Object) { ControllerContext = mockHttpContext };
        controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
        {
            OriginalPath = "/OriginalPath"
        });

        // Act
        ViewResult result = await controller.Error() as ViewResult; ;

        // Assert
        Assert.Equal("Something went wrong", result.ViewData[@"ErrorHeading"]);
    }

    [Fact]
    public async Task ShouldRedirectIfPageNotFoundButMatchedALegacyRedirect()
    {
        _legacyRedirects
            .Setup(redirects => redirects.RedirectUrl("/a-url"))
            .ReturnsAsync(@"/redirected-to-location-from-the-rule");

        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/test";
        httpContext.Response.StatusCode = 404;
        ControllerContext mockHttpContext = new()
        {
            HttpContext = httpContext
        };

        ErrorController controller = new(_legacyRedirects.Object, _logger.Object) { ControllerContext = mockHttpContext };

        controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
        {
            OriginalPath = "/a-url"
        });

        RedirectResult result = await controller.Error() as RedirectResult;
        result.Url.Should().Be(@"/redirected-to-location-from-the-rule");
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