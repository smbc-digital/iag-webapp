using System.Collections.Generic;
using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class Topic
    {
        public string Name { get; }
        public string Slug { get; }
        public string NavigationLink { get; }
        private string _summary;
        public string Summary
        {
            get { return _summary; }
            set { _summary = MarkdownWrapper.ToHtml(value); }
        }
        public string Teaser { get; }
        public string Icon { get; }
        public string BackgroundImage { get; }      
        public IEnumerable<SubItem> SubItems { get; }
        public IEnumerable<SubItem> SecondaryItems { get; }
        public IEnumerable<SubItem> TertiaryItems { get; }
        public IEnumerable<Crumb> Breadcrumbs { get; }
        public IEnumerable<Alert> Alerts { get; }
        public bool EmailAlerts { get; }
        public string EmailAlertsTopicId { get; }

        public Topic(string name, string slug, string summary, string teaser, string icon,
            string backgroundImage, IEnumerable<SubItem> subItems, IEnumerable<SubItem> secondaryItems, IEnumerable<SubItem> tertiaryItems, IEnumerable<Crumb> breadcrumbs,
            IEnumerable<Alert> alerts, bool emailAlerts, string emailAlertsTopicId)
        {
            Name = name;
            Slug = slug;
            Summary = summary;
            Teaser = teaser;
            Icon = icon;
            BackgroundImage = backgroundImage;
            SubItems = subItems;
            SecondaryItems = secondaryItems;
            TertiaryItems = tertiaryItems;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            EmailAlerts = emailAlerts;
            EmailAlertsTopicId = emailAlertsTopicId;
            NavigationLink = TypeRoutes.GetUrlFor("topic", slug);
        }
    }

    public class NullTopic : Topic
    {
        public NullTopic() : base(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, new List<SubItem>(), new List<SubItem>(), new List<SubItem>(), new List<Crumb>(), new List<Alert>(), false, string.Empty) { }
    }
}