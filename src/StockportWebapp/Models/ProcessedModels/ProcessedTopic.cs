namespace StockportWebapp.Models.ProcessedModels;
public class ProcessedTopic
{
    public string Name { get; }
    public string Title { get; }
    public string Slug { get; }
    public string NavigationLink { get; }
    private IEnumerable<SubItem> _topSubItems;
    public string BackgroundImage { get; }
    public TriviaSection TriviaSection { get; init; }
    public string Summary { get; }
    public string Teaser { get; }
    public string MetaDescription { get; }
    public string Icon { get; }
    public Video Video { get; init; }
    public CallToActionBanner CallToAction { get; init; }
    public string Image { get; }
    public IEnumerable<SubItem> FeaturedTasks { get; }
    public IEnumerable<SubItem> SubItems { get; }
    public IEnumerable<SubItem> SecondaryItems { get; }
    public IEnumerable<SubItem> TopSubItems { get; }
    public IEnumerable<Crumb> Breadcrumbs { get; }
    public IEnumerable<Alert> Alerts { get; }
    public bool EmailAlerts { get; }
    public string EmailAlertsTopicId { get; }
    public EventBanner EventBanner { get; }
    public EventCalendarBanner EventCalendarBanner { get; }
    public bool DisplayContactUs { get; set; }
    public CarouselContent CampaignBanner { get; }
    public string EventCategory { get; set; }
    public List<GroupBranding> TopicBranding { get; init; }
    public string LogoAreaTitle { get; set; }

    public ProcessedTopic(string name, string slug, string summary, string teaser, string metaDescription, string icon,
        string backgroundImage, string image, IEnumerable<SubItem> featuredTasks, IEnumerable<SubItem> subItems, IEnumerable<SubItem> secondaryItems,
        IEnumerable<Crumb> breadcrumbs, IEnumerable<Alert> alerts, bool emailAlerts, string emailAlertsTopicId, EventBanner eventBanner, EventCalendarBanner eventCalendarBanner,
        string title, bool displayContactUs, CarouselContent campaignBanner,
        string eventCategory, CallToActionBanner callToAction, List<GroupBranding> topicBranding, string logoAreaTitle)
    {
        Name = name;
        Title = title;
        Slug = slug;
        Summary = summary;
        Teaser = teaser;
        MetaDescription = metaDescription;
        Icon = icon;
        BackgroundImage = backgroundImage;
        Image = image;
        FeaturedTasks = featuredTasks;
        SubItems = subItems;
        SecondaryItems = secondaryItems;
        Breadcrumbs = breadcrumbs;
        Alerts = alerts;
        EmailAlerts = emailAlerts;
        EmailAlertsTopicId = emailAlertsTopicId;
        NavigationLink = TypeRoutes.GetUrlFor("topic", slug);
        _topSubItems = Enumerable.Empty<SubItem>();
        EventBanner = eventBanner;
        EventCalendarBanner = eventCalendarBanner;
        DisplayContactUs = displayContactUs;
        CampaignBanner = campaignBanner;
        EventCategory = eventCategory;
        CallToAction = callToAction;
        TopicBranding = topicBranding;
        LogoAreaTitle = logoAreaTitle;
    }

    public NavCardList PrimaryItems => new()
    {
        Items = SubItems.Select(subItem => new NavCard(subItem.Title, subItem.NavigationLink, subItem.Teaser, subItem.TeaserImage, subItem.Image)).ToList()
    };
}