﻿namespace StockportWebapp.TagParsers;

public class AlertsInlineTagParser : IDynamicTagParser<Alert>
{
    private readonly IViewRender _viewRenderer;

    public AlertsInlineTagParser(IViewRender viewRenderer)
        => _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{Alerts-Inline:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

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
                var alertsInlineHtml = String.Empty;

                if (AlertsInline.Severity.Equals("Warning") || AlertsInline.Severity.Equals("Error"))
                {
                    alertsInlineHtml = _viewRenderer.Render("AlertsInlineWarning", AlertsInline);
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