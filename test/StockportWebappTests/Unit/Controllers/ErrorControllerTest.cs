namespace StockportWebappTests_Unit.Unit.Controllers;

public class ErrorControllerTest
{
    private readonly Mock<ILegacyRedirectsManager> _legacyRedirects = new();
    private readonly Mock<ILogger<ErrorController>> _logger = new();
    private readonly Mock<IFeatureManager> _featureManager = new();

    [Fact]
    public async Task ShouldTellUsSomethingsMissingIfAPageWasNotFound()
    {
        // Arrange
        DefaultHttpContext httpContext = new();

        httpContext.Request.Path = "/pathThatDoesntExist";
        httpContext.Response.StatusCode = 404;

        ErrorController controller = new(_legacyRedirects.Object, _logger.Object, _featureManager.Object, new BusinessId("stockportgov"))
        {
            ControllerContext = new()
            {
                HttpContext = httpContext
            }
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
        Assert.Equal("Something's missing", result.ViewData["ErrorHeading"]);
    }

    [Fact]
    public async Task ShouldTellUsSomethingIsWrongIfADifferentErrorOccurred()
    {
        // Arrange
        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/test";
        httpContext.Response.StatusCode = 500;
        
        ErrorController controller = new(_legacyRedirects.Object, _logger.Object, _featureManager.Object, new BusinessId("stockportgov"))
        {
            ControllerContext = new()
            {
                HttpContext = httpContext
            }
        };

        controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
        {
            OriginalPath = "/OriginalPath"
        });

        // Act
        ViewResult result = await controller.Error() as ViewResult; ;

        // Assert
        Assert.Equal("Something went wrong", result.ViewData["ErrorHeading"]);
    }

    [Fact]
    public async Task ShouldRedirectIfPageNotFoundButMatchedALegacyRedirect()
    {
        // Arrange
        _legacyRedirects
            .Setup(redirects => redirects.RedirectUrl("/a-url"))
            .ReturnsAsync("/redirected-to-location-from-the-rule");

        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/test";
        httpContext.Response.StatusCode = 404;

        // Act
        ErrorController controller = new(_legacyRedirects.Object, _logger.Object, _featureManager.Object, new BusinessId("stockportgov"))
        {
            ControllerContext = new()
            {
                HttpContext = httpContext
            }
        };

        controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
        {
            OriginalPath = "/a-url"
        });

        // Assert
        RedirectResult result = await controller.Error() as RedirectResult;
        Assert.Equal("/redirected-to-location-from-the-rule", result.Url);
        Assert.True(result.Permanent);
    }

    [Fact]
    public async Task ShouldLogInformationIfLegacyUrlNotfound()
    {
        // Arrange
        _legacyRedirects
            .Setup(redirects => redirects.RedirectUrl("/a-url"))
            .ReturnsAsync(string.Empty);

        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/test";
        httpContext.Response.StatusCode = 404;

        ControllerContext mockHttpContext = new()
        {
            HttpContext = httpContext
        };

        // Act
        ErrorController controller = new(_legacyRedirects.Object, _logger.Object, _featureManager.Object, new BusinessId("stockportgov"))
        {
            ControllerContext = new()
            {
                HttpContext = httpContext
            }
        };

        controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
        {
            OriginalPath = "/a-url"
        });

        // Assert
        await controller.Error();
        LogTesting.Assert(_logger, LogLevel.Information, $"No legacy url matching current url (\"/a-url\") found");
    }

    [Fact]
    public async Task ShouldLogInformationIfLegacyUrlfound()
    {
        // Arrange
        _legacyRedirects
            .Setup(redirects => redirects.RedirectUrl("/a-url"))
            .ReturnsAsync("/redirected-to-location-from-the-rule");

        DefaultHttpContext httpContext = new();
        httpContext.Request.Path = "/test";
        httpContext.Response.StatusCode = 404;

        // Act
        ErrorController controller = new(_legacyRedirects.Object, _logger.Object, _featureManager.Object, new BusinessId("stockportgov"))
        {
            ControllerContext = new()
            {
                HttpContext = httpContext
            }
        };
        
        controller.HttpContext.Features.Set<IStatusCodeReExecuteFeature>(new StatusCodeReExecuteFeature()
        {
            OriginalPath = "/a-url"
        });

        // Assert

        await controller.Error();
        LogTesting.Assert(_logger, LogLevel.Information, $"A legacy redirect was found - redirecting to \"/redirected-to-location-from-the-rule\"");
    }
}