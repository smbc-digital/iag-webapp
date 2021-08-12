using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedShowcase : IProcessedContentType
    {
        public readonly string Title;
        public readonly string Slug;
        public readonly string Teaser;
        public readonly string MetaDescription;
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
        public readonly IEnumerable<SubItem> SecondaryItems;
        public readonly IEnumerable<SubItem> PrimaryItems;
        public readonly string FeaturedItemsSubheading;
        public readonly IEnumerable<SubItem> FeaturedItems;
        public readonly string SocialMediaLinksSubheading;
        public readonly IEnumerable<SocialMediaLink> SocialMediaLinks;
        public readonly IEnumerable<Event> Events;
        public readonly string EmailAlertsTopicId;
        public readonly string EmailAlertsText;
        public readonly IEnumerable<Alert> Alerts;
        public readonly Profile Profile;
        public readonly List<Profile> Profiles;
        public string ProfileHeading;
        public string ProfileLink;
        public readonly CallToActionBanner CallToActionBanner;
        public readonly FieldOrder FieldOrder;
        public readonly string Icon;
        public readonly List<ProcessedInformationItem> TriviaSection;
        public readonly string TriviaSubheading;
        public string EventsReadMoreText;
        public readonly Video Video;
        public readonly string TypeformUrl;
        public readonly SpotlightBanner SpotlightBanner;

        public ProcessedShowcase()
        { }

        public ProcessedShowcase(string title,
            string slug,
            string teaser,
            string metaDescription,
            string subHeading,
            string eventCategory,
            string eventsCategoryOrTag,
            string eventSubheading,
            string newsSubheading,
            string newsCategoryTag,
            string newsCategoryOrTag,
            string bodySubheading,
            string body,
            News newsArticle,
            string heroImageUrl,
            IEnumerable<SubItem> secondaryItems,
            IEnumerable<Crumb> breadcrumbs,
            string socialMediaLinksSubheading,
            IEnumerable<SocialMediaLink> socialMediaLinks,
            IEnumerable<Event> events,
            string emailAlertsTopicId,
            string emailAlertsText,
            IEnumerable<Alert> alerts,
            IEnumerable<SubItem> primaryItems,
            string featuredItemsSubheading,
            IEnumerable<SubItem> featuredItems,
            Profile profile,
            List<Profile> profiles,
            CallToActionBanner callToActionBanner,
            FieldOrder fieldOrder,
            string icon,
            string triviaSubheading,
            List<ProcessedInformationItem> triviaSection,
            string profileHeading,
            string profileLink,
            string eventsReadMoreText,
            Video video,
            string typeformUrl,
            SpotlightBanner spotlightBanner)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            MetaDescription = metaDescription;
            Subheading = subHeading;
            HeroImageUrl = heroImageUrl;
            EventCategory = eventCategory;
            EventSubheading = eventSubheading;
            Breadcrumbs = breadcrumbs;
            SecondaryItems = secondaryItems;
            SocialMediaLinksSubheading = socialMediaLinksSubheading;
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
            PrimaryItems = primaryItems;
            FeaturedItemsSubheading = featuredItemsSubheading;
            FeaturedItems = featuredItems;
            Profile = profile;
            Profiles = profiles;
            FieldOrder = fieldOrder;
            Icon = icon;
            TriviaSubheading = triviaSubheading;
            TriviaSection = triviaSection;
            CallToActionBanner = callToActionBanner;
            ProfileHeading = profileHeading;
            ProfileLink = profileLink;
            EventsReadMoreText = eventsReadMoreText;
            Video = video;
            TypeformUrl = typeformUrl;
            SpotlightBanner = spotlightBanner;
        }
    }
}
