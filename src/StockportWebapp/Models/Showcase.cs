using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Showcase
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Teaser { get; set; }
        public string Subheading { get; set; }
        public string HeroImageUrl { get; set; }
        public string EventCategory { get; set; }
        public string EventsCategoryOrTag { get; set; }
        public string EventSubheading { get; set; }
        public string NewsCategoryTag { get; set; }
        public string NewsCategoryOrTag { get; set; }
        public string NewsSubheading { get; set; }
        public News NewsArticle { get; set; }
        public string BodySubheading { get; set; }
        public string Body { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public IEnumerable<SubItem> FeaturedItems { get; set; }
        public IEnumerable<SubItem> PrimaryItems { get; set; }
        public IEnumerable<Consultation> Consultations { get; set; }
        public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public string EmailAlertsTopicId { get; set; }
        public string EmailAlertsText { get; set; }
        public IEnumerable<Alert> Alerts { get; }
        public string KeyFactSubheading { get; }
        public IEnumerable<KeyFact> KeyFacts { get; }
        public Profile Profile { get; }
        public FieldOrder FieldOrder { get; }

        public Showcase(string title, string slug, string teaser, string subheading, string eventCategory, string eventsCategoryOrTag, string eventSubheading, string newsSubheading, string newsCatgeoryTag, string newsCatgeoryOrTag, string bodySubheading, string body, News newsArticle, string heroImageUrl, IEnumerable<Crumb> breadcrumbs, IEnumerable<SubItem> featuredItems, IEnumerable<Consultation> consultations, IEnumerable<SocialMediaLink> socialMediaLinks, IEnumerable<Event> events, string emailAlertsTopicId, string emailAlertsText, IEnumerable<Alert> alerts, IEnumerable<SubItem> primaryItems, IEnumerable<KeyFact> keyFacts, Profile profile, FieldOrder fieldOrder, string keyFactSubheading)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Subheading = subheading;
            EventCategory = eventCategory;
            EventSubheading = eventSubheading;
            HeroImageUrl = heroImageUrl;
            Breadcrumbs = breadcrumbs;
            FeaturedItems = featuredItems;
            Consultations = consultations;
            SocialMediaLinks = socialMediaLinks;
            Events = events;
            NewsSubheading = newsSubheading;
            NewsCategoryTag = newsCatgeoryTag;
            NewsCategoryOrTag = newsCatgeoryOrTag;
            BodySubheading = bodySubheading;
            Body = body;
            NewsArticle = newsArticle;
            EventsCategoryOrTag = eventsCategoryOrTag;
            EmailAlertsTopicId = emailAlertsTopicId;
            EmailAlertsText = emailAlertsText;
            Alerts = alerts;
            KeyFacts = keyFacts;
            PrimaryItems = primaryItems;
            Profile = profile;
            FieldOrder = fieldOrder;
            KeyFactSubheading = keyFactSubheading;
        }
    }
}