using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class TopicFactory
    {

        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;

        public TopicFactory(ISimpleTagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
        }

        public virtual ProcessedTopic Build(Topic topic)
        {
            var summary = _markdownWrapper.ConvertToHtml(topic.Summary ?? "");
            summary = _tagParserContainer.ParseAll(summary, topic.Title);

            return new ProcessedTopic(topic.Name, topic.Slug, summary, topic.Teaser, topic.MetaDescription, topic.Icon, topic.BackgroundImage,
                topic.Image, topic.SubItems, topic.SecondaryItems, topic.TertiaryItems, topic.Breadcrumbs, topic.Alerts, topic.EmailAlerts,
                topic.EmailAlertsTopicId, topic.EventBanner, topic.ExpandingLinkTitle, topic.ExpandingLinkBoxes, topic.PrimaryItemTitle,
                topic.Title, topic.DisplayContactUs, topic.CampaignBanner, topic.EventCategory)
            {
                TriviaSection = topic.TriviaSection,
                Video = !string.IsNullOrEmpty(topic.Video.VideoEmbedCode) ?
                    new Video(topic.Video.Heading, topic.Video.Text, _tagParserContainer.ParseAll(topic.Video.VideoEmbedCode)) : null,
                CallToAction = topic.CallToAction
            };
        }
    }
}
