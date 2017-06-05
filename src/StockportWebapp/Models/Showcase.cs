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
        public string EventSubheading { get; set; }
        public string NewsCategoryTag { get; set; }
        public string NewsCategoryOrTag { get; set; }
        public string NewsSubheading { get; set; }
        public News NewsArticle { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public IEnumerable<SubItem> FeaturedItems { get; set; }
        public IEnumerable<Consultation> Consultations { get; set; }
        public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }
        public IEnumerable<Event> Events { get; set; }

        public Showcase(string title, string slug, string teaser, string subheading, string eventCategory, string eventSubheading, string newsSubheading, string newsCatgeoryTag, string newsCatgeoryOrTag, News newsArticle, string heroImageUrl, IEnumerable<Crumb> breadcrumbs, IEnumerable<SubItem> featuredItems, IEnumerable<Consultation> consultations, IEnumerable<SocialMediaLink> socialMediaLinks, IEnumerable<Event> events)
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
            NewsArticle = newsArticle;
        }
    }
}