using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class Topic
    {
        public string Name { get; }
        public string Title { get; }
        public string Slug { get; }
        public string NavigationLink { get; }

        private IEnumerable<SubItem> _topSubItems;
        public string Summary {get; }
        public string Teaser { get; }
        public string MetaDescription { get; }
        public string Icon { get; }
        public string BackgroundImage { get; }
        public Video Video { get; init; }
        public CallToAction CallToAction { get; init; }
        public string Image { get; }
        public IEnumerable<SubItem> SubItems { get; }
        public IEnumerable<SubItem> SecondaryItems { get; }
        public IEnumerable<SubItem> TertiaryItems { get; }
        public IEnumerable<SubItem> TopSubItems
        {
            get
            {
                const int take = 6;
                if (_topSubItems.Any()) return _topSubItems;

                _topSubItems = ConcatSubItems(_topSubItems, SubItems, take);
                _topSubItems = ConcatSubItems(_topSubItems, SecondaryItems, take);
                _topSubItems = ConcatSubItems(_topSubItems, TertiaryItems, take);
                _topSubItems = _topSubItems.Take(take);

                return _topSubItems;
            }
        }

        private static IEnumerable<SubItem> ConcatSubItems(IEnumerable<SubItem> primary, IEnumerable<SubItem> secondary, int take)
        {
            return secondary != null ? primary.Concat(secondary.Take(take)) : primary;
        }

        public IEnumerable<Crumb> Breadcrumbs { get; }
        public IEnumerable<Alert> Alerts { get; }
        public bool EmailAlerts { get; }
        public string EmailAlertsTopicId { get; }
        public EventBanner EventBanner { get; }
        public string ExpandingLinkTitle { get; }
        public IEnumerable<ExpandingLinkBox> ExpandingLinkBoxes { get; set;  }
        public string PrimaryItemTitle { get; }
        public bool DisplayContactUs { get; set; }
        public CarouselContent CampaignBanner { get;}
        public string EventCategory { get; set; }

        public Topic(string name, string slug, string summary, string teaser, string metaDescription, string icon,
            string backgroundImage, string image, IEnumerable<SubItem> subItems, IEnumerable<SubItem> secondaryItems, IEnumerable<SubItem> tertiaryItems, 
            IEnumerable<Crumb> breadcrumbs, IEnumerable<Alert> alerts, bool emailAlerts, string emailAlertsTopicId, EventBanner eventBanner,
            string expandingLinkTitle, IEnumerable<ExpandingLinkBox> expandingLinkBoxes, string primaryItemTitle, string title, bool displayContactUs, CarouselContent campaignBanner, string eventCategory)
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

    public class NullTopic : Topic
    {
        public NullTopic() :base(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty ,string.Empty,
            string.Empty, string.Empty, new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
            new List<Crumb>(), new List<Alert>(), false, string.Empty, null, string.Empty, new List<ExpandingLinkBox>(),
            string.Empty, string.Empty, true, new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty)
         { }
    }
}