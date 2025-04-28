namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class News(string title,
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
                IEnumerable<Document> documents,
                IEnumerable<Profile> profiles,
                IEnumerable<InlineQuote> inlineQuotes,
                CallToActionBanner callToAction)
{
    public string Title { get; } = title;
    public string Slug { get; } = slug;
    public string Teaser { get; } = teaser;
    public string Purpose { get; set; } = purpose;
    public string Image { get; } = image;
    public string ThumbnailImage { get; } = thumbnailImage;
    public string Body { get; } = body;
    public List<Crumb> Breadcrumbs { get; } = breadcrumbs;
    public DateTime SunriseDate { get; } = sunriseDate;
    public DateTime SunsetDate { get; } = sunsetDate;
    public DateTime UpdatedAt { get; } = updatedAt;
    public List<Alert> Alerts { get; } = alerts;
    public List<string> Tags { get; } = tags;
    public IEnumerable<Document> Documents { get; set; } = documents;
    public IEnumerable<Profile> Profiles { get; set; } = profiles;
    public IEnumerable<InlineQuote> InlineQuotes { get; set; } = inlineQuotes;
    public CallToActionBanner CallToAction { get; set; } = callToAction;
}