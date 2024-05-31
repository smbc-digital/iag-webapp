namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class Homepage
{
    public IEnumerable<string> PopularSearchTerms { get; }
    public string FeaturedTasksHeading { get; }
    public string FeaturedTasksSummary { get; }
    public IEnumerable<SubItem> FeaturedTasks;
    public IEnumerable<SubItem> FeaturedTopics { get; }
    public IEnumerable<Alert> Alerts { get; }
    public IEnumerable<CarouselContent> CarouselContents { get; }
    public string BackgroundImage { get; }
    public string ForegroundImage { get; }
    public string ForegroundImageLocation { get; }
    public string ForegroundImageLink { get; }
    public string ForegroundImageAlt { get; }
    public IEnumerable<News> LastNews { get; set; }
    public string FreeText { get; }
    public Group FeaturedGroup { get; }
    public string EventCategory { get; }
    public string MetaDescription { get; set; }
    public CarouselContent CampaignBanner { get; set; }
    public CallToActionBanner CallToAction { get; set; }
    public CallToActionBanner CallToActionPrimary { get; set; }

    public IEnumerable<SpotlightOnBanner> SpotlightOnBanner { get; set; }

    public Homepage(IEnumerable<string> popularSearchTerms, string featuredTasksHeading, string featuredTasksSummary, IEnumerable<SubItem> featuredTasks, IEnumerable<SubItem> featuredTopics, IEnumerable<Alert> alerts, IEnumerable<CarouselContent> carouselContents, string backgroundImage, string foregroundImage, string foregroundImageLocation, string foregroundImageLink, string foregroundImageAlt, string freeText, Group featuredGroup, string eventCategory, string metaDescription, CarouselContent campaignBanner, CallToActionBanner callToAction, CallToActionBanner callToActionPrimary, IEnumerable<SpotlightOnBanner> spotlightOnBanner)
    {
        PopularSearchTerms = popularSearchTerms;
        FeaturedTasksHeading = featuredTasksHeading;
        FeaturedTasksSummary = featuredTasksSummary;
        FeaturedTasks = featuredTasks;
        FeaturedTopics = featuredTopics;
        Alerts = alerts;
        CarouselContents = carouselContents;
        BackgroundImage = backgroundImage;
        ForegroundImage = foregroundImage;
        ForegroundImageLocation = foregroundImageLocation;
        ForegroundImageLink = foregroundImageLink;
        ForegroundImageAlt = foregroundImageAlt;
        FreeText = freeText;
        FeaturedGroup = featuredGroup;
        EventCategory = eventCategory;
        MetaDescription = metaDescription;
        CampaignBanner = campaignBanner;
        CallToAction = callToAction;
        CallToActionPrimary = callToActionPrimary;
        SpotlightOnBanner = spotlightOnBanner;
    }
}