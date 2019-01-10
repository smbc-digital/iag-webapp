using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using System.Linq;

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
            showcase.Body = _markdownWrapper.ConvertToHtml(body ?? string.Empty);

            var fields = showcase.FieldOrder;

            if (!fields.Items.Any())
            {
                fields.Items.Add("Primary Items");
                fields.Items.Add("Featured Items");
                fields.Items.Add("Consultations");
                fields.Items.Add("Key Facts");
                fields.Items.Add("News");
                fields.Items.Add("Events");
                fields.Items.Add("Profile");
                fields.Items.Add("Social Media");
                fields.Items.Add("Body");
            }

            return new ProcessedShowcase(
                showcase.Title,
                showcase.Slug,
                showcase.Teaser,
                showcase.Subheading,
                showcase.EventCategory,
                showcase.EventsCategoryOrTag,
                showcase.EventSubheading,
                showcase.NewsSubheading,
                showcase.NewsCategoryTag,
                showcase.NewsCategoryOrTag,
                showcase.BodySubheading,
                showcase.Body,
                showcase.NewsArticle,
                showcase.HeroImageUrl,
                showcase.FeaturedItems,
                showcase.Breadcrumbs,
                showcase.Consultations,
                showcase.SocialMediaLinks,
                showcase.Events,
                showcase.EmailAlertsTopicId,
                showcase.EmailAlertsText,
                showcase.Alerts,
                showcase.PrimaryItems,
                showcase.KeyFacts,
                showcase.Profile,
                fields,
                showcase.KeyFactSubheading,
                showcase.Icon,
                showcase.DidYouKnowSection
            );
        }
    }
}
