namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class News(string title,
                string slug,
                string teaser,
                string purpose,
                string heroImage,
                string image,
                string thumbnailImage,
                string heroImageCaption,
                string body,
                List<Crumb> breadcrumbs,
                DateTime sunriseDate,
                string publishingDate,
                DateTime sunsetDate,
                DateTime updatedAt,
                List<Alert> alerts,
                List<string> tags,
                IEnumerable<Document> documents,
                IEnumerable<Profile> profiles,
                IEnumerable<InlineQuote> inlineQuotes,
                CallToActionBanner callToAction,
                string logoAreaTitle,
                IEnumerable<TrustedLogo> trustedLogos,
                TrustedLogo featuredLogo,
                string eventsByTagOrCategory,
                List<Event> events)
{
    public string Title { get; } = title;
    public string Slug { get; } = slug;
    public string Teaser { get; } = teaser;
    public string Purpose { get; set; } = purpose;
    public string HeroImage { get; } = heroImage;
    public string Image { get; } = image;
    public string ThumbnailImage { get; } = thumbnailImage;
    public string HeroImageCaption { get; } = heroImageCaption;
    public string Body { get; } = body;
    public List<Crumb> Breadcrumbs { get; } = breadcrumbs;
    public DateTime SunriseDate { get; } = sunriseDate;
    public string PublishingDate { get; } = publishingDate;
    public DateTime SunsetDate { get; } = sunsetDate;
    public DateTime UpdatedAt { get; } = updatedAt;
    public List<Alert> Alerts { get; } = alerts;
    public List<string> Tags { get; } = tags;
    public IEnumerable<Document> Documents { get; set; } = documents;
    public IEnumerable<Profile> Profiles { get; set; } = profiles;
    public IEnumerable<InlineQuote> InlineQuotes { get; set; } = inlineQuotes;
    public CallToActionBanner CallToAction { get; set; } = callToAction;
    public string LogoAreaTitle { get; } = logoAreaTitle;
    public IEnumerable<TrustedLogo> TrustedLogos { get; set; } = trustedLogos;
    public TrustedLogo FeaturedLogo { get; set; } = featuredLogo;
    public string EventsByTagOrCategory { get; set; } = eventsByTagOrCategory;
    public List<Event> Events { get; set; } = events;
}