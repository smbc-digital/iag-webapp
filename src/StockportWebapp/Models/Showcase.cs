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
        public IEnumerable<SubItem> SecondaryItems { get; set; }
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
        public List<Profile> Profiles { get; set; }
        public string ProfileHeading { get; set; }
        public string ProfileLink { get; set; }
        public CallToActionBanner CallToActionBanner { get; }
        public FieldOrder FieldOrder { get; }
        public string Icon { get; set; }
        public string TriviaSubheading { get; set; }
        public List<InformationItem> TriviaSection { get; set; }
        public Video Video { get; set; }

        public Showcase(string title,
            string slug,
            string teaser,
            string subheading,
            string eventCategory,
            string eventsCategoryOrTag,
            string eventSubheading,
            string newsSubheading,
            string newsCatgeoryTag,
            string newsCatgeoryOrTag,
            string bodySubheading,
            string body,
            News newsArticle,
            string heroImageUrl,
            IEnumerable<Crumb> breadcrumbs,
            IEnumerable<SubItem> secondaryItems,
            IEnumerable<Consultation> consultations,
            IEnumerable<SocialMediaLink> socialMediaLinks,
            IEnumerable<Event> events,
            string emailAlertsTopicId,
            string emailAlertsText,
            IEnumerable<Alert> alerts,
            IEnumerable<SubItem> primaryItems,
            IEnumerable<KeyFact> keyFacts,
            Profile profile,
            string profileHeading,
            string profileLink,
            List<Profile> profiles,
            FieldOrder fieldOrder,
            string keyFactSubheading,
            string icon,
            string triviaSubheading,
            List<InformationItem> triviaSection,
            CallToActionBanner callToActionBanner,
            Video video)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Subheading = subheading;
            EventCategory = eventCategory;
            EventSubheading = eventSubheading;
            HeroImageUrl = heroImageUrl;
            Breadcrumbs = breadcrumbs;
            SecondaryItems = secondaryItems;
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
            Profiles = profiles;
            FieldOrder = fieldOrder;
            KeyFactSubheading = keyFactSubheading;
            Icon = icon;
            TriviaSubheading = triviaSubheading;
            TriviaSection = triviaSection;
            CallToActionBanner = callToActionBanner;
            ProfileLink = profileLink;
            ProfileHeading = profileHeading;
            Video = video;
        }
    }
}