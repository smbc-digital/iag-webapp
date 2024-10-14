namespace StockportWebapp.Models.ProcessedModels;
public class ProcessedHomepage : IProcessedContentType
{
    public string Title;
    public readonly IEnumerable<string> PopularSearchTerms;
    public readonly string FeaturedTasksHeading;
    public readonly string FeaturedTasksSummary;
    public readonly IEnumerable<SubItem> FeaturedTasks;
    public readonly IEnumerable<SubItem> FeaturedTopics;
    public readonly IEnumerable<Alert> Alerts;
    public readonly IEnumerable<Alert> CondolenceAlerts;
    public readonly IEnumerable<CarouselContent> CarouselContents;
    public readonly string BackgroundImage;
    public readonly string ImageOverlayText;
    public readonly string ForegroundImage;
    public readonly string ForegroundImageLocation;
    public readonly string ForegroundImageLink;
    public readonly string ForegroundImageAlt;
    public readonly string FreeText;
    public readonly Group FeaturedGroupItem;
    public readonly string EventCategory;
    public readonly string MetaDescription;
    public readonly CarouselContent CampaignBanner;
    public readonly CallToActionBanner CallToAction;
    public readonly CallToActionBanner CallToActionPrimary;
    public readonly IEnumerable<SpotlightOnBanner> SpotlightOnBanner;

    public ProcessedHomepage(string title,
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
        IEnumerable<News> lastNews,
        string freeText,
        Group featuredGroup,
        string eventCategory,
        string metaDescription,
        CarouselContent campaignBanner,
        CallToActionBanner callToAction,
        CallToActionBanner callToActionPrimary,
        IEnumerable<SpotlightOnBanner> spotlightOnBanner,
        string imageOverlayText)
    {
        Title = title;
        PopularSearchTerms = popularSearchTerms;
        FeaturedTasksHeading = featuredTasksHeading;
        FeaturedTasksSummary = featuredTasksSummary;
        FeaturedTasks = featuredTasks;
        FeaturedTopics = featuredTopics;
        Alerts = alerts.Where(_ => !_.Severity.Equals(Severity.Condolence));
        CondolenceAlerts = alerts.Where(_ => _.Severity.Equals(Severity.Condolence));
        CarouselContents = carouselContents;
        BackgroundImage = backgroundImage;
        ForegroundImage = foregroundImage;
        ForegroundImageLocation = foregroundImageLocation;
        ForegroundImageLink = foregroundImageLink;
        ForegroundImageAlt = foregroundImageAlt;
        FreeText = freeText;
        FeaturedGroupItem = featuredGroup;
        EventCategory = eventCategory;
        MetaDescription = metaDescription;
        CampaignBanner = campaignBanner;
        CallToAction = callToAction;
        CallToActionPrimary = callToActionPrimary;
        SpotlightOnBanner = spotlightOnBanner;
        ImageOverlayText = imageOverlayText;
    }

    public NavCardList Services => new()
    {
        Items = FeaturedTopics.Select(topic => new NavCard(topic.Title, topic.NavigationLink, topic.Teaser, string.Empty)).ToList(),
        ButtonText = "View more services"
    };
}