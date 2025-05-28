namespace StockportWebapp.Models.ProcessedModels;
public class ProcessedTopic(string name,
                            string slug,
                            string summary,
                            string teaser,
                            string metaDescription,
                            string icon,
                            string backgroundImage,
                            string image,
                            IEnumerable<SubItem> featuredTasks,
                            IEnumerable<SubItem> subItems,
                            IEnumerable<SubItem> secondaryItems,
                            IEnumerable<Crumb> breadcrumbs,
                            IEnumerable<Alert> alerts,
                            bool emailAlerts,
                            string emailAlertsTopicId,
                            EventBanner eventBanner,
                            EventCalendarBanner eventCalendarBanner,
                            bool displayContactUs,
                            CarouselContent campaignBanner,
                            string eventCategory,
                            CallToActionBanner callToAction,
                            List<TrustedLogo> trustedLogos,
                            string logoAreaTitle)
{
    public string Name { get; } = name;
    public string Slug { get; } = slug;
    public string NavigationLink { get; } = TypeRoutes.GetUrlFor("topic", slug);
    private IEnumerable<SubItem> _topSubItems = Enumerable.Empty<SubItem>();
    public string BackgroundImage { get; } = backgroundImage;
    public TriviaSection TriviaSection { get; init; }
    public string Summary { get; } = summary;
    public string Teaser { get; } = teaser;
    public string MetaDescription { get; } = metaDescription;
    public string Icon { get; } = icon;
    public Video Video { get; init; }
    public CallToActionBanner CallToAction { get; init; } = callToAction;
    public string Image { get; } = image;
    public IEnumerable<SubItem> FeaturedTasks { get; } = featuredTasks;
    public IEnumerable<SubItem> SubItems { get; } = subItems;
    public IEnumerable<SubItem> SecondaryItems { get; } = secondaryItems;
    public IEnumerable<SubItem> TopSubItems { get; }
    public IEnumerable<Crumb> Breadcrumbs { get; } = breadcrumbs;
    public IEnumerable<Alert> Alerts { get; } = alerts;
    public bool EmailAlerts { get; } = emailAlerts;
    public string EmailAlertsTopicId { get; } = emailAlertsTopicId;
    public EventBanner EventBanner { get; } = eventBanner;
    public EventCalendarBanner EventCalendarBanner { get; } = eventCalendarBanner;
    public bool DisplayContactUs { get; set; } = displayContactUs;
    public CarouselContent CampaignBanner { get; } = campaignBanner;
    public string EventCategory { get; set; } = eventCategory;
    public List<TrustedLogo> TrustedLogos { get; init; } = trustedLogos;
    public string LogoAreaTitle { get; set; } = logoAreaTitle;

    public NavCardList PrimaryItems => new()
    {
        Items = SubItems.Select(subItem => new NavCard(subItem.Title, subItem.NavigationLink, subItem.Teaser, subItem.TeaserImage, subItem.Image)).ToList()
    };
}