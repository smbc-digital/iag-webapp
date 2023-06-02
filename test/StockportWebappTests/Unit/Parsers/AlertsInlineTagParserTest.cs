namespace StockportWebappTests_Unit.Unit.Parsers;

public class AlertsInlineTagParserTest
{
    private readonly Mock<IViewRender> _viewRenderer;
    private readonly AlertsInlineTagParser _alertsInlineTagParser;

    public AlertsInlineTagParserTest()
    {
        _viewRenderer = new Mock<IViewRender>();
        _alertsInlineTagParser = new AlertsInlineTagParser(_viewRenderer.Object);
    }

    [Fact]
    public void ShouldRemoveAlertsInlineTagsThatDontExistFromTheBody()
    {
        var content = "this is some test {{Alerts-Inline:some-title}}";
        var parsedHtml = _alertsInlineTagParser.Parse(content, new List<Alert>());
        _viewRenderer.Verify(o => o.Render("AlertsInline", It.IsAny<Alert>()), Times.Never);
        parsedHtml.Should().Be("this is some test ");
    }
}
