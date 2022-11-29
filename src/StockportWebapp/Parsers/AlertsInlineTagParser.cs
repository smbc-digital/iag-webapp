using System.Text.RegularExpressions;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Parsers
{

    public class AlertsInlineTagParser : IDynamicTagParser<Alert>
    {
        private readonly IViewRender _viewRenderer;
        private readonly ILogger<Alert> _logger;
        private readonly FeatureToggles _featureToggles;

        public AlertsInlineTagParser(IViewRender viewRenderer, ILogger<Alert> logger, FeatureToggles featureToggles)
        {
            _viewRenderer = viewRenderer;
            _logger = logger;
            _featureToggles = featureToggles;
        }

        protected Regex TagRegex => new Regex("{{Alerts-Inline:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

        public string Parse(string content, IEnumerable<Alert> alertsInline)
        {
            var matches = TagRegex.Matches(content);

            foreach (Match match in matches)
            {
                var tagDataIndex = 1;
                var AlertsInlineTitle = match.Groups[tagDataIndex].Value;
                var AlertsInline = GetAlertsInlineMatchingTitle(alertsInline, AlertsInlineTitle);
                if (AlertsInline != null)
                {
                    var alertsInlineHtml = "";
                    if (_featureToggles.SemanticInlineAlert)
                    {
                        alertsInlineHtml = _viewRenderer.Render("Semantic/AlertsInline", AlertsInline);
                    }
                    else
                    {
                        alertsInlineHtml = _viewRenderer.Render("AlertsInline", AlertsInline);
                    }

                    content = TagRegex.Replace(content, alertsInlineHtml, 1);
                }
            }
            return RemoveEmptyTags(content);
        }

        private string RemoveEmptyTags(string content)
        {
            return TagRegex.Replace(content, string.Empty);
        }

        private Alert GetAlertsInlineMatchingTitle(IEnumerable<Alert> alertsInline, string title)
        {
            return alertsInline.FirstOrDefault(s => s.Title == title);
        }
    }
}