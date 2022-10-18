using System.Linq;
using StockportWebapp.ContentFactory.Trivia;
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
        private readonly ITriviaFactory _triviaFactory;

        public ShowcaseFactory(ISimpleTagParserContainer tagParserContainer,
            MarkdownWrapper markdownWrapper,
            ITriviaFactory triviaFactory)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
            _triviaFactory = triviaFactory;
        }

        public virtual ProcessedShowcase Build(Showcase showcase)
        {
            var body = _tagParserContainer.ParseAll(showcase.Body);
            showcase.Body = _markdownWrapper.ConvertToHtml(body ?? string.Empty);

            var video = showcase.Video;

            if (video != null)
            {
                video.VideoEmbedCode = _tagParserContainer.ParseAll(video.VideoEmbedCode);
            }

            var fields = showcase.FieldOrder;

            if (!fields.Items.Any())
            {
                fields.Items.Add("Primary Items");
                fields.Items.Add("Secondary Items");
                fields.Items.Add("Featured Items");
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
                showcase.MetaDescription,
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
                showcase.SocialMediaLinksSubheading,
                showcase.SocialMediaLinks,
                showcase.Events,
                showcase.EmailAlertsTopicId,
                showcase.EmailAlertsText,
                showcase.Alerts,
                showcase.PrimaryItems,
                showcase.FeaturedItemsSubheading,
                showcase.FeaturedItems,
                showcase.Profile,
                showcase.Profiles,
                showcase.CallToActionBanner,
                fields,
                showcase.Icon,
                showcase.TriviaSubheading,
                _triviaFactory.Build(showcase.TriviaSection),
                showcase.ProfileHeading,
                showcase.ProfileLink,
                showcase.EventsReadMoreText,
                video,
                showcase.SpotlightBanner
            );
        }
    }
}
