using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ShowcaseFactory
    { 
        public ShowcaseFactory()
        {        
        }

        public virtual ProcessedShowcase Build(Showcase Showcase)
        {
            return new ProcessedShowcase(
                Showcase.Title,
                Showcase.Slug,
                Showcase.Teaser,
                Showcase.Subheading,
                Showcase.HeroImageUrl,
                Showcase.FeaturedItems,
                Showcase.Breadcrumbs
                );
        }
    }
}
