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

        public GenericFeaturedItemList GenericItemList
        {
            get
            {
                var result = new GenericFeaturedItemList();
                result.Items = new List<GenericFeaturedItem>();
                foreach (var featuredItem in FeaturedItems)
                {
                    var item = new GenericFeaturedItem
                    {
                        Icon = featuredItem.Icon,
                        Title = featuredItem.Title,
                        Url = featuredItem.NavigationLink,
                        SubItems = new List<GenericFeaturedItem>()
                    };

                    foreach (var subItem in featuredItem.SubItems)
                    {
                        item.SubItems.Add(new GenericFeaturedItem { Title = subItem.Title, Url = subItem.NavigationLink, Icon = subItem.Icon });
                    }

                    result.Items.Add(item);
                }

                result.ButtonText = string.Empty;
                result.ButtonCssClass = string.Empty;

                return result;
            }
        }

        public ProcessedShowcase()
        { }

        public ProcessedShowcase(string title, string slug, string teaser, string subHeading, string eventCategory, string eventsCategoryOrTag, string eventSubheading, string newsSubheading, string newsCategoryTag, string newsCategoryOrTag, string bodySubheading, string body, News newsArticle, string heroImageUrl, IEnumerable<SubItem> featuredItems, IEnumerable<Crumb> breadcrumbs, IEnumerable<Consultation> consultations, IEnumerable<SocialMediaLink> socialMediaLinks, IEnumerable<Event> events)
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
        }
    }
}
