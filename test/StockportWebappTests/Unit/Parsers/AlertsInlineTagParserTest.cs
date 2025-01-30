namespace StockportWebappTests_Unit.Unit.Parsers;

public class AlertsInlineTagParserTest
{
    private readonly Mock<IViewRender> _viewRenderer = new();
    private readonly AlertsInlineTagParser _alertsInlineTagParser;

    public AlertsInlineTagParserTest()
    {
        _viewRenderer
            .Setup(viewRender => viewRender.Render(It.IsAny<string>(), It.IsAny<Alert>()))
            .Returns("Test Parser Result");
        
        _alertsInlineTagParser = new(_viewRenderer.Object);
    }

    [Fact]
    public void ShouldRemoveAlertsInlineTagsThatDontExistFromTheBody()
    {
        // Act
        string parsedHtml = _alertsInlineTagParser.Parse("this is some test {{Alerts-Inline:some-title}}", new List<Alert>());

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("AlertsInline", It.IsAny<Alert>()), Times.Never);
        Assert.Equal("this is some test ", parsedHtml);
    }

    [Fact]
    public void ShoudlCallRendererWithPartialDefaultStyle()
    {
        // Arrange
        Alert testAlert = new("Test Alert",
                            "Test Heading",
                            "Test Alert Body",
                            Severity.Information,
                            DateTime.Today.AddDays(-7),
                            DateTime.Today.AddDays(7),
                            "test-alert",
                            true,
                            string.Empty);
        
        List<Alert> alerts = new()
        {
            testAlert
        };

        // Act
        string parsedHtml = _alertsInlineTagParser.Parse("this is some test {{Alerts-Inline:Test Alert}}", alerts);
        
        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("AlertsInline", It.IsAny<Alert>()), Times.Once);
    }

    [Fact]
    public void ShoudlCallRendererWithPartialWarningStyle()
    {
        // Arrange
        Alert testAlert = new("Test Alert",
                            "Test Heading",
                            "Test Alert Body",
                            Severity.Warning,
                            DateTime.Today.AddDays(-7),
                            DateTime.Today.AddDays(7),
                            "test-alert",
                            true,
                            string.Empty);
        
        List<Alert> alerts = new()
        {
            testAlert    
        };

        // Act
        string parsedHtml = _alertsInlineTagParser.Parse("this is some test {{Alerts-Inline:Test Alert}}", alerts);

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("AlertsInlineWarning", It.IsAny<Alert>()), Times.Once);
    }

    [Fact]
    public void ShoudlNotCallRendererIfAttachedAlertsIsNull()
    {
        // Arrange
        Alert testAlert = new("Not Test Alert",
                            "Test Heading",
                            "Test Alert Body",
                            Severity.Warning,
                            DateTime.Today.AddDays(-7),
                            DateTime.Today.AddDays(7),
                            "test-alert",
                            true,
                            string.Empty);
        
        List<Alert> alerts = new()
        {
            testAlert
        };

        // Act
        string parsedHtml = _alertsInlineTagParser.Parse("this is some test {{Alerts-Inline:Test Alert}}", alerts);

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("AlertsInlineWarning", It.IsAny<Alert>()), Times.Never);
        _viewRenderer.Verify(renderer => renderer.Render("AlertsInline", It.IsAny<Alert>()), Times.Never);
        Assert.Equal("this is some test ", parsedHtml);
    }
}