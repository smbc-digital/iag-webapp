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
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public IEnumerable<SubItem> FeaturedItems { get; set; }

        public Showcase(string title, string slug, string teaser, string subheading, string heroImageUrl, IEnumerable<Crumb> breadcrumbs, IEnumerable<SubItem> featuredItems )
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Subheading = subheading;
            HeroImageUrl = heroImageUrl;
            Breadcrumbs = breadcrumbs;
            FeaturedItems = featuredItems;
        }
    }
}