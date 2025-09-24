namespace StockportWebapp.Models.ProcessedModels;

[ExcludeFromCodeCoverage]
public class ProcessedNews(string title,
                        string slug,
                        string teaser,
                        string image,
                        string thumbnailImage,
                        string imageCaption,
                        string body,
                        DateTime sunriseDate,
                        string publishingDate,
                        DateTime sunsetDate,
                        DateTime updatedAt,
                        List<Alert> alerts,
                        List<string> tags,
                        IEnumerable<InlineQuote> inlineQuotes,
                        CallToActionBanner callToAction,
                        string logoAreaTitle,
                        IEnumerable<TrustedLogo> trustedLogos,
                        TrustedLogo featuredLogo,
                        string eventsByTagOrCategory,
                        List<Event> events) : IProcessedContentType
{
    public readonly string Title = title;
    public readonly string Slug = slug;
    public readonly string Teaser = teaser;
    public readonly string Image = image;
    public readonly string ThumbnailImage = thumbnailImage;
    public readonly string ImageCaption = imageCaption;
    public readonly string Body = body;
    public readonly DateTime SunriseDate = sunriseDate;
    public readonly string PublishingDate = publishingDate;
    public readonly DateTime SunsetDate = sunsetDate;
    public readonly DateTime UpdatedAt = updatedAt;
    public readonly List<Alert> Alerts = alerts;
    public readonly List<string> Tags = tags;
    public readonly IEnumerable<InlineQuote> InlineQuotes = inlineQuotes;
    public readonly CallToActionBanner CallToAction = callToAction;
    public readonly string LogoAreaTitle = logoAreaTitle;
    public readonly IEnumerable<TrustedLogo> TrustedLogos = trustedLogos;
    public readonly TrustedLogo FeaturedLogo = featuredLogo;
    public readonly string EventsByTagOrCategory = eventsByTagOrCategory;
    public readonly List<Event> Events = events;
}