using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ShowcaseFactory
    { 
        public ShowcaseFactory()
        {        
        }

        public virtual ProcessedShowcase Build(Showcase showcase)
        {
            return new ProcessedShowcase(
                showcase.Title,
                showcase.Slug,
                showcase.Teaser,
                showcase.Subheading,
                showcase.HeroImageUrl,
                showcase.FeaturedItems,
                showcase.Breadcrumbs
                );
        }
    }
}
