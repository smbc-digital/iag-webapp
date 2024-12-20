﻿namespace StockportWebapp.Models;

public class Topic
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
        secondary is not null ? primary.Concat(secondary.Take(take)) : primary;

    public IEnumerable<Crumb> Breadcrumbs { get; }
    public IEnumerable<Alert> Alerts { get; }
    public bool EmailAlerts { get; }
    public string EmailAlertsTopicId { get; }
    public EventCalendarBanner EventBanner { get; }
    public bool DisplayContactUs { get; set; }
    public CarouselContent CampaignBanner { get; }
    public string EventCategory { get; set; }
    public List<GroupBranding> TopicBranding { get; init; }
    public string LogoAreaTitle { get; }

    public Topic(string name,
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
        Name = name;
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
        DisplayContactUs = displayContactUs;
        CampaignBanner = campaignBanner;
        EventCategory = eventCategory;
        TopicBranding = topicBranding;
        LogoAreaTitle = logoAreaTitle;
    }
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