using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ContentFactory
{
    public class ShowcaseFactory
    { 
        public virtual ProcessedShowcase Build(Showcase showcase)
        {
            return new ProcessedShowcase(
                showcase.Title,
                showcase.Slug,
                showcase.Teaser,
                showcase.Subheading,
                showcase.EventCategory,
                showcase.EventSubheading,
                showcase.NewsSubheading,
                showcase.NewsCategoryTag,
                showcase.NewsCategoryOrTag,
                showcase.NewsArticle,
                showcase.HeroImageUrl,
                showcase.FeaturedItems,
                showcase.Breadcrumbs,
                showcase.Consultations,
                showcase.SocialMediaLinks,
                showcase.Events
            );
        }
    }
}
