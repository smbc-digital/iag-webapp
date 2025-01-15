namespace StockportWebapp.Models.ProcessedModels;
public class ProcessedShowcase(string title,
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
                            List<Trivia> triviaSection,
                            string profileHeading,
                            string profileLink,
                            string eventsReadMoreText,
                            Video video,
                            SpotlightOnBanner spotlightBanner) : IProcessedContentType
{
    public readonly string Title = title;
    public readonly string Slug = slug;
    public readonly string Teaser = teaser;
    public readonly string MetaDescription = metaDescription;
    public readonly string Subheading = subHeading;
    public readonly string HeroImageUrl = heroImageUrl;
    public readonly string EventCategory = eventCategory;
    public readonly string EventsCategoryOrTag = eventsCategoryOrTag;
    public readonly string EventSubheading = eventSubheading;
    public readonly string NewsCategoryTag = newsCategoryTag;
    public readonly string NewsCategoryOrTag = newsCategoryOrTag;
    public readonly string NewsSubheading = newsSubheading;
    public readonly string BodySubheading = bodySubheading;
    public readonly string Body = body;
    public readonly News NewsArticle = newsArticle;
    public readonly IEnumerable<Crumb> Breadcrumbs = breadcrumbs;
    public readonly IEnumerable<SubItem> SecondaryItems = secondaryItems;
    public readonly IEnumerable<SubItem> PrimaryItems = primaryItems;
    public readonly string FeaturedItemsSubheading = featuredItemsSubheading;
    public readonly IEnumerable<SubItem> FeaturedItems = featuredItems;
    public readonly string SocialMediaLinksSubheading = socialMediaLinksSubheading;
    public readonly IEnumerable<SocialMediaLink> SocialMediaLinks = socialMediaLinks;
    public readonly IEnumerable<Event> Events = events;
    public readonly string EmailAlertsTopicId = emailAlertsTopicId;
    public readonly string EmailAlertsText = emailAlertsText;
    public readonly IEnumerable<Alert> Alerts = alerts;
    public readonly Profile Profile = profile;
    public readonly List<Profile> Profiles = profiles;
    public string ProfileHeading = profileHeading;
    public string ProfileLink = profileLink;
    public readonly CallToActionBanner CallToActionBanner = callToActionBanner;
    public readonly FieldOrder FieldOrder = fieldOrder;
    public readonly string Icon = icon;
    public readonly List<Trivia> TriviaSection = triviaSection;
    public readonly string TriviaSubheading = triviaSubheading;
    public string EventsReadMoreText = eventsReadMoreText;
    public readonly Video Video = video;
    public readonly string TypeformUrl;
    public readonly SpotlightOnBanner SpotlightBanner = spotlightBanner;
}