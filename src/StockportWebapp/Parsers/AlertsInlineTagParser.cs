using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Parsers
{
    
    public class AlertsInlineTagParser : IDynamicTagParser<Alert>
    {
        private readonly IViewRender _viewRenderer;
        private readonly ILogger<Alert> _logger;

        public AlertsInlineTagParser(IViewRender viewRenderer, ILogger<Alert> logger)
        {
            _viewRenderer = viewRenderer;
            _logger = logger;
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
                    var AlertsInlineHtml = _viewRenderer.Render("AlertsInline", AlertsInline);
                    content = TagRegex.Replace(content, AlertsInlineHtml, 1);
                }
                else
                {
                    _logger.LogWarning($"The Alerts Title {AlertsInlineTitle} could not be found and will be removed");
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