namespace StockportWebapp.Models
{
    public class Showcase
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string HeroImageUrl { get; set; }
        public IEnumerable<SubItem> PrimaryItems { get; set; }
        public string Teaser { get; set; }
        public string MetaDescription { get; set; }
        public string Subheading { get; set; }
        public IEnumerable<SubItem> SecondaryItems { get; set; }
        public string FeaturedItemsSubheading { get; set; }
        public IEnumerable<SubItem> FeaturedItems { get; set; }
        public string SocialMediaLinksSubheading { get; set; }
        public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }
        public string EventSubheading { get; set; }
        public string EventCategory { get; set; }
        public string EventsCategoryOrTag { get; set; }
        public string EventsReadMoreText { get; set; }
        public string NewsSubheading { get; set; }
        public string NewsCategoryTag { get; set; }
        public string NewsCategoryOrTag { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public string BodySubheading { get; set; }
        public string Body { get; set; }
        public Profile Profile { get; set; }
        public string ProfileHeading { get; set; }
        public string ProfileLink { get; set; }
        public List<Profile> Profiles { get; set; }
        public FieldOrder FieldOrder { get; set; }
        public string EmailAlertsTopicId { get; set; }
        public string EmailAlertsText { get; set; }
        public IEnumerable<Alert> Alerts { get; set; }
        public string Icon { get; set; }
        public string TriviaSubheading { get; set; }
        public List<Trivia> TriviaSection { get; set; }
        public CallToActionBanner CallToActionBanner { get; set; }
        public Video Video { get; set; }
        public string TypeformUrl { get; set; }
        public SpotlightBanner SpotlightBanner { get; set; }

        public News NewsArticle { get; set; }
        public IEnumerable<Event> Events { get; set; }

        public Showcase()
        {

        }
    }
}