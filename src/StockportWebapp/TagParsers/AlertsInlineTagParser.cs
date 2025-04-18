﻿namespace StockportWebapp.TagParsers;

public class AlertsInlineTagParser(IViewRender viewRenderer) : IDynamicTagParser<Alert>
{
    private readonly IViewRender _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{Alerts-Inline:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string content, IEnumerable<Alert> alertsInline, bool redesigned = false)
    {
        MatchCollection matches = TagRegex.Matches(content);

        foreach (Match match in matches)
        {
            string AlertsInlineTitle = match.Groups[1].Value;
            Alert AlertsInline = GetMatchingInlineAlert(alertsInline, AlertsInlineTitle);

            if (AlertsInline is not null)
            {
                string alertsInlineHtml = string.Empty;

                if (AlertsInline.Severity.Equals(Severity.Warning) || AlertsInline.Severity.Equals(Severity.Error))
                    alertsInlineHtml = _viewRenderer.Render("AlertsInlineWarning", AlertsInline);
                else
                    alertsInlineHtml = _viewRenderer.Render("AlertsInline", AlertsInline);

                content = TagRegex.Replace(content, alertsInlineHtml, 1);
            }
        }

        return RemoveEmptyTags(content);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);

    private static Alert GetMatchingInlineAlert(IEnumerable<Alert> alertsInline, string reference) =>
        alertsInline?.FirstOrDefault(s => s.Title.Equals(reference) || s.Slug.Equals(reference));
}