namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class Homepage(string title,
                    IEnumerable<string> popularSearchTerms,
                    string featuredTasksHeading,
                    string featuredTasksSummary,
                    IEnumerable<SubItem> featuredTasks,
                    IEnumerable<SubItem> featuredTopics,
                    IEnumerable<Alert> alerts,
                    IEnumerable<CarouselContent> carouselContents,
                    string backgroundImage,
                    string foregroundImage,
                    string foregroundImageLocation,
                    string foregroundImageLink,
                    string foregroundImageAlt,
                    string freeText,
                    string eventCategory,
                    string metaDescription,
                    CarouselContent campaignBanner,
                    CallToActionBanner callToAction,
                    CallToActionBanner callToActionPrimary,
                    IEnumerable<SpotlightOnBanner> spotlightOnBanner,
                    string imageOverlayText)
{
    public string Title { get; } = title;
    public IEnumerable<string> PopularSearchTerms { get; } = popularSearchTerms;
    public string FeaturedTasksHeading { get; } = featuredTasksHeading;
    public string FeaturedTasksSummary { get; } = featuredTasksSummary;
    public IEnumerable<SubItem> FeaturedTasks = featuredTasks;
    public IEnumerable<SubItem> FeaturedTopics { get; } = featuredTopics;
    public IEnumerable<Alert> Alerts { get; } = alerts;
    public IEnumerable<CarouselContent> CarouselContents { get; } = carouselContents;
    public string BackgroundImage { get; } = backgroundImage;
    public string ImageOverlayText { get; } = imageOverlayText;
    public string ForegroundImage { get; } = foregroundImage;
    public string ForegroundImageLocation { get; } = foregroundImageLocation;
    public string ForegroundImageLink { get; } = foregroundImageLink;
    public string ForegroundImageAlt { get; } = foregroundImageAlt;
    public IEnumerable<News> LastNews { get; set; }
    public string FreeText { get; } = freeText;
    public string EventCategory { get; } = eventCategory;
    public string MetaDescription { get; set; } = metaDescription;
    public CarouselContent CampaignBanner { get; set; } = campaignBanner;
    public CallToActionBanner CallToAction { get; set; } = callToAction;
    public CallToActionBanner CallToActionPrimary { get; set; } = callToActionPrimary;
    public IEnumerable<SpotlightOnBanner> SpotlightOnBanner { get; set; } = spotlightOnBanner;
}