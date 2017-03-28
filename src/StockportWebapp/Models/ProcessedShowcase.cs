using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class ProcessedShowcase : IProcessedContentType
    {

        public readonly string Title;
        public readonly string Slug;
        public readonly string Teaser;
        public readonly string Subheading;
        public readonly string HeroImageUrl;
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<Topic> FeaturedItems;

        public ProcessedShowcase()
        { }

        public ProcessedShowcase(string title, string slug, string teaser, string subHeading, string heroImageUrl, IEnumerable<Topic> featuredItems, IEnumerable<Crumb> breadcrumbs)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Subheading = subHeading;
            HeroImageUrl = heroImageUrl;
            Breadcrumbs = breadcrumbs;
            FeaturedItems = featuredItems;
        }
    }
}
