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
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<SubItem> FeaturedItems;
        public readonly IEnumerable<Consultation> Consultations;

        public ProcessedShowcase()
        { }

        public ProcessedShowcase(string title, string slug, string teaser, string subHeading, string heroImageUrl, IEnumerable<SubItem> featuredItems, IEnumerable<Crumb> breadcrumbs, IEnumerable<Consultation> consultations)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Subheading = subHeading;
            HeroImageUrl = heroImageUrl;
            Breadcrumbs = breadcrumbs;
            FeaturedItems = featuredItems;
            Consultations = consultations;
        }
    }
}
