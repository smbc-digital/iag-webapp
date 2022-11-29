using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.ProcessedModels
{
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
        public CallToAction CallToAction { get; init; }
        public string Image { get; }
        public IEnumerable<SubItem> SubItems { get; }
        public IEnumerable<SubItem> SecondaryItems { get; }
        public IEnumerable<SubItem> TertiaryItems { get; }
        public IEnumerable<SubItem> TopSubItems { get; }
        public IEnumerable<Crumb> Breadcrumbs { get; }
        public IEnumerable<Alert> Alerts { get; }
        public bool EmailAlerts { get; }
        public string EmailAlertsTopicId { get; }
        public EventBanner EventBanner { get; }
        public string ExpandingLinkTitle { get; }
        public IEnumerable<ExpandingLinkBox> ExpandingLinkBoxes { get; set; }
        public string PrimaryItemTitle { get; }
        public bool DisplayContactUs { get; set; }
        public CarouselContent CampaignBanner { get; }
        public string EventCategory { get; set; }

        public ProcessedTopic(string name, string slug, string summary, string teaser, string metaDescription, string icon,
            string backgroundImage, string image, IEnumerable<SubItem> subItems, IEnumerable<SubItem> secondaryItems, IEnumerable<SubItem> tertiaryItems,
            IEnumerable<Crumb> breadcrumbs, IEnumerable<Alert> alerts, bool emailAlerts, string emailAlertsTopicId, EventBanner eventBanner,
            string expandingLinkTitle, IEnumerable<ExpandingLinkBox> expandingLinkBoxes, string primaryItemTitle, string title, bool displayContactUs, CarouselContent campaignBanner,
            string eventCategory)
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
            SubItems = subItems;
            SecondaryItems = secondaryItems;
            TertiaryItems = tertiaryItems;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            EmailAlerts = emailAlerts;
            EmailAlertsTopicId = emailAlertsTopicId;
            NavigationLink = TypeRoutes.GetUrlFor("topic", slug);
            _topSubItems = Enumerable.Empty<SubItem>();
            EventBanner = eventBanner;
            ExpandingLinkTitle = expandingLinkTitle;
            ExpandingLinkBoxes = expandingLinkBoxes;
            PrimaryItemTitle = primaryItemTitle;
            DisplayContactUs = displayContactUs;
            CampaignBanner = campaignBanner;
            EventCategory = eventCategory;
        }
    }
}