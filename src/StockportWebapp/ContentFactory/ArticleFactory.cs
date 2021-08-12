using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ArticleFactory
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

        public ArticleFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, ISectionFactory sectionFactory, MarkdownWrapper markdownWrapper,
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

        public virtual ProcessedArticle Build(Article article)
        {
            var processedSections = new List<ProcessedSection>();
            foreach (var section in article.Sections)
            {
                section.S3Bucket = article.S3Bucket;
                processedSections.Add(_sectionFactory.Build(section, article.Title));
            }

            var body = _markdownWrapper.ConvertToHtml(article.Body ?? "");
            body = _profileTagParser.Parse(body, article.Profiles);
            body = _documentTagParser.Parse(body, article.Documents);
            body = _alertsInlineTagParser.Parse(body, article.AlertsInline);
            body = _searchTagParser.Parse(body, new List<S3BucketSearch> { article.S3Bucket });
            if (body.Contains("PrivacyNotice:"))
            {
                article.PrivacyNotices = GetPrivacyNotices().Result;
                body = _privacyNoticeTagParser.Parse(body, article.PrivacyNotices);
            }

            body = _tagParserContainer.ParseAll(body, article.Title);

            return new ProcessedArticle(article.Title, article.Slug, body, article.Teaser, article.MetaDescription,
                processedSections, article.Icon, article.BackgroundImage, article.Image, article.Breadcrumbs, article.Alerts, article.ParentTopic, article.AlertsInline, article.S3Bucket, article.UpdatedAt);
        }

        private async Task<IEnumerable<PrivacyNotice>> GetPrivacyNotices()
        {
            var response = await _repository.Get<List<PrivacyNotice>>();
            return response.Content as List<PrivacyNotice>;
        }
    }
}
