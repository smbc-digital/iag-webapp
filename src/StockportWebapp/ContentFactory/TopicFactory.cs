using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class TopicFactory
    {

        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly IDynamicTagParser<Profile> _profileTagParser;
        private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
        private readonly ISectionFactory _sectionFactory;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Document> _documentTagParser;
        private readonly IDynamicTagParser<S3BucketSearch> _searchTagParser;
        private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser;
        private readonly IRepository _repository;

        public TopicFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, ISectionFactory sectionFactory, MarkdownWrapper markdownWrapper,
            IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser, IDynamicTagParser<S3BucketSearch> searchTagParser, IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser, IRepository repository)
        {
            _tagParserContainer = tagParserContainer;
            _sectionFactory = sectionFactory;
            _markdownWrapper = markdownWrapper;
            _profileTagParser = profileTagParser;
            _documentTagParser = documentTagParser;
            _alertsInlineTagParser = alertsInlineTagParser;
            _searchTagParser = searchTagParser;
            _privacyNoticeTagParser = privacyNoticeTagParser;
            _repository = repository;
        }

        public virtual ProcessedTopic Build(Topic topic)
        {

            var summary = _markdownWrapper.ConvertToHtml(topic.Summary ?? "");
          
            summary = _tagParserContainer.ParseAll(summary, topic.Title);

            return new ProcessedTopic(topic.Name, topic.Slug, summary, topic.Teaser, topic.MetaDescription, topic.Icon, topic.BackgroundImage,
                topic.Image, topic.SubItems, topic.SecondaryItems, topic.TertiaryItems, topic.Breadcrumbs, topic.Alerts, topic.EmailAlerts,
                topic.EmailAlertsTopicId, topic.EventBanner, topic.ExpandingLinkTitle, topic.ExpandingLinkBoxes, topic.PrimaryItemTitle,
                topic.Title,topic.DisplayContactUs, topic.CampaignBanner);
                       }

    }
}
