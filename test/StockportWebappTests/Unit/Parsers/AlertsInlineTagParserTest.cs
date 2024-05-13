namespace StockportWebappTests_Unit.Unit.Parsers;

public class AlertsInlineTagParserTest
{
    private readonly Mock<IViewRender> _viewRenderer;
    private readonly AlertsInlineTagParser _alertsInlineTagParser;

    public AlertsInlineTagParserTest()
    {
        _viewRenderer = new Mock<IViewRender>();
        _alertsInlineTagParser = new AlertsInlineTagParser(_viewRenderer.Object);

        _viewRenderer.Setup(viewRender => viewRender.Render(It.IsAny<string>(), It.IsAny<Alert>())).Returns("Test Parser Result");
    }

    [Fact]
    public void ShouldRemoveAlertsInlineTagsThatDontExistFromTheBody()
    {
        var content = "this is some test {{Alerts-Inline:some-title}}";
        var parsedHtml = _alertsInlineTagParser.Parse(content, new List<Alert>());
        _viewRenderer.Verify(o => o.Render("AlertsInline", It.IsAny<Alert>()), Times.Never);
        parsedHtml.Should().Be("this is some test ");
    }


    [Fact]
    public void ShoudlCallRendererWithPartialDefaultStyle()
    {
        var testAlert = new Alert("Test Alert", "Test Heading", "Test Alert Body", Severity.Information, DateTime.Today.AddDays(-7), DateTime.Today.AddDays(7), "test-alert", true, string.Empty);
        var alerts = new List<Alert>
        {
            testAlert
        };

        var content = "this is some test {{Alerts-Inline:Test Alert}}";
        var parsedHtml = _alertsInlineTagParser.Parse(content, alerts);
        _viewRenderer.Verify(o => o.Render("AlertsInline", It.IsAny<Alert>()), Times.Once);
    }

    [Fact]
    public void ShoudlCallRendererWithPartialWarningStyle()
    {
        var testAlert = new Alert("Test Alert", "Test Heading", "Test Alert Body", Severity.Warning, DateTime.Today.AddDays(-7), DateTime.Today.AddDays(7), "test-alert", true, string.Empty);
        var alerts = new List<Alert>
        {
            testAlert    
        };

        var content = "this is some test {{Alerts-Inline:Test Alert}}";
        var parsedHtml = _alertsInlineTagParser.Parse(content, alerts);
        _viewRenderer.Verify(o => o.Render("AlertsInlineWarning", It.IsAny<Alert>()), Times.Once);
    }

    [Fact]
    public void ShoudlNotCallRendererIfAttachedAlertsIsNull()
    {
        var testAlert = new Alert("Not Test Alert", "Test Heading", "Test Alert Body", Severity.Warning, DateTime.Today.AddDays(-7), DateTime.Today.AddDays(7), "test-alert", true, string.Empty);
        var alerts = new List<Alert>
        {
            testAlert
        };

        var content = "this is some test {{Alerts-Inline:Test Alert}}";
        var parsedHtml = _alertsInlineTagParser.Parse(content, alerts);
        _viewRenderer.Verify(o => o.Render("AlertsInlineWarning", It.IsAny<Alert>()), Times.Never);
        _viewRenderer.Verify(o => o.Render("AlertsInline", It.IsAny<Alert>()), Times.Never);
        parsedHtml.Should().Be("this is some test ");
    }
}
