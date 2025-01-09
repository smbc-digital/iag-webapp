namespace StockportWebapp.Models;

public class Topic(string name,
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
                EventCalendarBanner eventBanner,
                bool displayContactUs,
                CarouselContent campaignBanner,
                string eventCategory,
                List<GroupBranding> topicBranding,
                string logoAreaTitle)
{
    public string Name { get; } = name;
    public string Title { get; }
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
    public CallToActionBanner CallToAction { get; init; }
    public string Image { get; } = image;
    public IEnumerable<SubItem> FeaturedTasks { get; } = featuredTasks;
    public IEnumerable<SubItem> SubItems { get; } = subItems;
    public IEnumerable<SubItem> SecondaryItems { get; } = secondaryItems;
    public IEnumerable<SubItem> TopSubItems
    {
        get
        {
            const int take = 6;
            if (_topSubItems.Any()) return _topSubItems;

            _topSubItems = ConcatSubItems(_topSubItems, SubItems, take);
            _topSubItems = ConcatSubItems(_topSubItems, SecondaryItems, take);
            _topSubItems = _topSubItems.Take(take);

            return _topSubItems;
        }
    }

    private static IEnumerable<SubItem> ConcatSubItems(IEnumerable<SubItem> primary, IEnumerable<SubItem> secondary, int take) => 
        secondary is not null
            ? primary.Concat(secondary.Take(take))
            : primary;

    public IEnumerable<Crumb> Breadcrumbs { get; } = breadcrumbs;
    public IEnumerable<Alert> Alerts { get; } = alerts;
    public bool EmailAlerts { get; } = emailAlerts;
    public string EmailAlertsTopicId { get; } = emailAlertsTopicId;
    public EventCalendarBanner EventBanner { get; } = eventBanner;
    public bool DisplayContactUs { get; set; } = displayContactUs;
    public CarouselContent CampaignBanner { get; } = campaignBanner;
    public string EventCategory { get; set; } = eventCategory;
    public List<GroupBranding> TopicBranding { get; init; } = topicBranding;
    public string LogoAreaTitle { get; } = logoAreaTitle;
}

public class NullTopic : Topic
{
    public NullTopic() : base(string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            new List<SubItem>(),
                            new List<SubItem>(),
                            new List<SubItem>(),
                            new List<Crumb>(),
                            new List<Alert>(),
                            false,
                            string.Empty,
                            null,
                            true,
                            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                            string.Empty,
                            null,
                            string.Empty)
    { }
}