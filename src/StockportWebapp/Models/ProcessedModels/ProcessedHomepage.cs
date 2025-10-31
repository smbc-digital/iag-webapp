namespace StockportWebapp.Models.ProcessedModels;

public class ProcessedHomepage(string title,
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
                            string eventCategory,
                            string metaDescription,
                            CarouselContent campaignBanner,
                            CallToActionBanner callToAction,
                            CallToActionBanner callToActionPrimary,
                            IEnumerable<SpotlightOnBanner> spotlightOnBanner) : IProcessedContentType
{
    public string Title = title;
    public readonly string FeaturedTasksHeading = featuredTasksHeading;
    public readonly string FeaturedTasksSummary = featuredTasksSummary;
    public readonly IEnumerable<SubItem> FeaturedTasks = featuredTasks;
    public readonly IEnumerable<SubItem> FeaturedTopics = featuredTopics;
    public readonly IEnumerable<Alert> Alerts = alerts.Where(_ => !_.Severity.Equals(Severity.Condolence));
    public readonly IEnumerable<Alert> CondolenceAlerts = alerts.Where(_ => _.Severity.Equals(Severity.Condolence));
    public readonly IEnumerable<CarouselContent> CarouselContents = carouselContents;
    public readonly string BackgroundImage = backgroundImage;
    public readonly string ForegroundImage = foregroundImage;
    public readonly string ForegroundImageLocation = foregroundImageLocation;
    public readonly string ForegroundImageLink = foregroundImageLink;
    public readonly string ForegroundImageAlt = foregroundImageAlt;
    public readonly IEnumerable<News> LastNews = lastNews;
    public readonly string FreeText = freeText;
    public readonly string EventCategory = eventCategory;
    public readonly string MetaDescription = metaDescription;
    public readonly CarouselContent CampaignBanner = campaignBanner;
    public readonly CallToActionBanner CallToAction = callToAction;
    public readonly CallToActionBanner CallToActionPrimary = callToActionPrimary;
    public readonly IEnumerable<SpotlightOnBanner> SpotlightOnBanner = spotlightOnBanner;

    public NavCardList Services => new()
    {
        Items = FeaturedTopics.Select(topic => new NavCard(topic.Title, topic.NavigationLink, topic.Teaser, topic.TeaserImage, string.Empty)).ToList(),
        ButtonText = "View more services"
    };
}