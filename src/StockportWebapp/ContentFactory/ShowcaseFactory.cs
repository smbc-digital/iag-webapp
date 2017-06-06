using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ShowcaseFactory
    {
        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;

        public ShowcaseFactory(ISimpleTagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
        }

        public virtual ProcessedShowcase Build(Showcase showcase)
        {
            var body = _tagParserContainer.ParseAll(showcase.Body);
            body = _markdownWrapper.ConvertToHtml(body ?? string.Empty);

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
                showcase.BodySubheading,
                body,
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
