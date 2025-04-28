namespace StockportWebapp.Models.ProcessedModels;
[ExcludeFromCodeCoverage]
public class ProcessedNews(string title,
                        string slug,
                        string teaser,
                        string purpose,
                        string image,
                        string thumbnailImage,
                        string body,
                        List<Crumb> breadcrumbs,
                        DateTime sunriseDate,
                        DateTime sunsetDate,
                        DateTime updatedAt,
                        List<Alert> alerts,
                        List<string> tags,
                        IEnumerable<InlineQuote> inlineQuotes,
                        CallToActionBanner callToAction) : IProcessedContentType
{
    public readonly string Title = title;
    public readonly string Slug = slug;
    public readonly string Teaser = teaser;
    public readonly string Purpose = purpose;
    public readonly string Image = image;
    public readonly string ThumbnailImage = thumbnailImage;
    public readonly string Body = body;
    public readonly List<Crumb> Breadcrumbs = breadcrumbs;
    public readonly DateTime SunriseDate = sunriseDate;
    public readonly DateTime SunsetDate = sunsetDate;
    public readonly DateTime UpdatedAt = updatedAt;
    public readonly List<Alert> Alerts = alerts;
    public readonly List<string> Tags = tags;
    public readonly IEnumerable<InlineQuote> InlineQuotes = inlineQuotes;
    public readonly CallToActionBanner CallToAction = callToAction;
}