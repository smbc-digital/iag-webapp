namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class StartPage(string slug,
                    string title,
                    string teaser,
                    string summary,
                    string upperBody,
                    string formLink,
                    string lowerBody,
                    IEnumerable<Crumb> breadcrumbs,
                    string backgroundImage,
                    string icon,
                    List<Alert> alerts,
                    IEnumerable<Alert> inlineAlerts)
{
    public string Slug { get; } = slug;
    public string Title { get; } = title;
    public string Teaser { get; } = teaser;
    public string Summary { get; } = summary;
    public string UpperBody { get; } = MarkdownWrapper.ToHtml(upperBody);
    public string FormLink { get; } = formLink;
    public string LowerBody { get; } = MarkdownWrapper.ToHtml(lowerBody);
    public IEnumerable<Crumb> Breadcrumbs { get; } = breadcrumbs;
    public string BackgroundImage { get; } = backgroundImage;
    public string Icon { get; } = icon;
    public List<Alert> Alerts { get; private set; } = alerts;
    public IEnumerable<Alert> AlertsInline { get; set; } = inlineAlerts;
}