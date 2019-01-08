using System.Collections.Generic;
using Moq;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using StockportWebapp.FeatureToggling;
using StockportWebappTests_Unit.Helpers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Parsers
{
    public class AlertsInlineTagParserTest
    {
        private readonly Mock<IViewRender> _viewRenderer;
        private readonly AlertsInlineTagParser _alertsInlineTagParser;
        private readonly Mock<ILogger<Alert>> _mockLogger;
        private readonly FeatureToggles _featureToggles;

        public AlertsInlineTagParserTest()
        {
            _featureToggles =  new FeatureToggles(){SemanticInlineAlert = true};
            _viewRenderer = new Mock<IViewRender>();
            _mockLogger = new Mock<ILogger<Alert>>();
            _alertsInlineTagParser = new AlertsInlineTagParser(_viewRenderer.Object, _mockLogger.Object, _featureToggles);
        }

        [Fact]
        public void ShouldReplaceAlertsInlineTagWithAlertsInlineView()
        {
            var content = "this is some test {{Alerts-Inline:some-title}}";
            var alertsInline = new Alert("some-title", "some-image-url", "some-subheading", "some-severity", System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty);
            var alertsInlineList = new List<Alert>() { alertsInline };
            var renderResult = "RENDERED ARTICLE CONTENT";

            _viewRenderer.Setup(o => o.Render("AlertsInline", alertsInline)).Returns(renderResult);

            var parsedHtml = _alertsInlineTagParser.Parse(content, alertsInlineList);

            _viewRenderer.Verify(o => o.Render("AlertsInline", alertsInline), Times.Once);
            parsedHtml.Should().Contain(renderResult);
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
        public void ShouldLogWhenAlertsInlineIsNotFound()
        {
            var content = "this is some test {{Alerts-Inline:some-title}}";
            _alertsInlineTagParser.Parse(content, new List<Alert>());
            LogTesting.Assert(_mockLogger, LogLevel.Warning,
             "The Alerts Title some-title could not be found and will be removed");
        }
    }
}
