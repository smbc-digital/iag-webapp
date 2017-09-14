using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedShowcase : IProcessedContentType
    {
        public readonly string Title;
        public readonly string Slug;
        public readonly string Teaser;
        public readonly string Subheading;
        public readonly string HeroImageUrl;
        public readonly string EventCategory;
        public readonly string EventsCategoryOrTag;
        public readonly string EventSubheading;
        public readonly string NewsCategoryTag;
        public readonly string NewsCategoryOrTag;
        public readonly string NewsSubheading;
        public readonly string BodySubheading;
        public readonly string Body;
        public readonly News NewsArticle;
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<SubItem> FeaturedItems;
        public readonly IEnumerable<Consultation> Consultations;
        public readonly IEnumerable<SocialMediaLink> SocialMediaLinks;
        public readonly IEnumerable<Event> Events;
        public readonly string EmailAlertsTopicId;
        public readonly string EmailAlertsText;
        public readonly IEnumerable<Alert> Alerts;
        public readonly IEnumerable<KeyFact> KeyFacts;

        public ProcessedShowcase()
        { }

        public ProcessedShowcase(string title, string slug, string teaser, string subHeading, string eventCategory, string eventsCategoryOrTag, string eventSubheading, string newsSubheading, string newsCategoryTag, string newsCategoryOrTag, string bodySubheading, string body, News newsArticle, string heroImageUrl, IEnumerable<SubItem> featuredItems, IEnumerable<Crumb> breadcrumbs, IEnumerable<Consultation> consultations, IEnumerable<SocialMediaLink> socialMediaLinks, IEnumerable<Event> events, string emailAlertsTopicId, string emailAlertsText, IEnumerable<Alert> alerts, IEnumerable<KeyFact> keyFacts)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Subheading = subHeading;
            HeroImageUrl = heroImageUrl;
            EventCategory = eventCategory;
            EventSubheading = eventSubheading;
            Breadcrumbs = breadcrumbs;
            FeaturedItems = featuredItems;
            Consultations = consultations;
            SocialMediaLinks = socialMediaLinks;
            Events = events;
            NewsSubheading = newsSubheading;
            NewsCategoryTag = newsCategoryTag;
            NewsCategoryOrTag = newsCategoryOrTag;
            BodySubheading = bodySubheading;
            Body = body;
            NewsArticle = newsArticle;
            EventsCategoryOrTag = eventsCategoryOrTag;
            EmailAlertsTopicId = emailAlertsTopicId;
            EmailAlertsText = emailAlertsText;
            Alerts = alerts;
            KeyFacts = keyFacts;
        }
    }
}
