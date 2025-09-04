namespace StockportWebapp.Models.ProcessedModels;

[ExcludeFromCodeCoverage]
public class ProcessedStartPage(string slug,
                                string title,
                                string teaser,
                                string summary,
                                string upperBody,
                                string formLink,
                                string lowerBody,
                                IEnumerable<Crumb> breadcrumbs,
                                string backgroundImage,
                                string icon,
                                List<Alert> alerts) : IProcessedContentType
{
    public string Slug { get; } = slug;
    public string Title { get; } = title;
    public string Teaser { get; } = teaser;
    public string Summary { get; } = summary;
    public string UpperBody { get; } = upperBody;
    public string FormLink { get; } = formLink;
    public string LowerBody { get; } = lowerBody;
    public IEnumerable<Crumb> Breadcrumbs { get; } = breadcrumbs;
    public string BackgroundImage { get; } = backgroundImage;
    public string Icon { get; } = icon;
    public List<Alert> Alerts { get; private set; } = alerts;
}