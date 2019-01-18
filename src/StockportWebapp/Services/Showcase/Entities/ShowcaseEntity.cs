using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Services.Showcase.Entities
{
    public class ShowcaseEntity
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
        public string BodySubheading { get; set; }
        public string Body { get; set; }
        public News NewsArticle { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public IEnumerable<SubItem> SecondaryItems { get; set; }
        public IEnumerable<SubItem> PrimaryItems { get; set; }
        public IEnumerable<Consultation> Consultations { get; set; }
        public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public string EmailAlertsTopicId { get; set; }
        public string EmailAlertsText { get; set; }
        public IEnumerable<Alert> Alerts { get; set; }
        public string KeyFactSubheading { get; set; }
        public IEnumerable<KeyFact> KeyFacts { get; set; }
        public Models.Profile Profile { get; set; }
        public List<Models.Profile> Profiles { get; set; }
        public CallToActionBanner CallToActionBanner { get; set; }
        public FieldOrder FieldOrder { get; set; }
        public string Icon { get; set; }
        public List<ProcessedInformationItem> DidYouKnowSection { get; set; }
        public List<InformationItem> KeyFactsSection { get; set; }
    }
}
