using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using System.Linq;
using StockportWebapp.ContentFactory.InformationFactory;

namespace StockportWebapp.ContentFactory
{
    public class ShowcaseFactory
    {
        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IInformationFactory _informationFactory;

        public ShowcaseFactory(ISimpleTagParserContainer tagParserContainer,
            MarkdownWrapper markdownWrapper,
            IInformationFactory informationFactory)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
            _informationFactory = informationFactory;
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
                fields.Items.Add("Profiles");
                fields.Items.Add("Social Media");
                fields.Items.Add("Body");
                fields.Items.Add("Video");
                fields.Items.Add("Trivia");
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
                showcase.SecondaryItems,
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
                showcase.Profiles,
                showcase.CallToActionBanner,
                fields,
                showcase.KeyFactSubheading,
                showcase.Icon,
                showcase.TriviaSubheading,
                _informationFactory.Build(showcase.TriviaSection),
                showcase.ProfileHeading,
                showcase.ProfileLink,
                showcase.Video
            );
        }
    }
}
