using System;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz.Impl.Triggers;
using StockportWebapp.Repositories;

namespace StockportWebapp.ContentFactory
{
    public interface ISectionFactory
    {
        ProcessedSection Build(Section section, string articleTitle);
    }

    public class SectionFactory : ISectionFactory
    {
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly IDynamicTagParser<Profile> _profileTagParser;
        private readonly IDynamicTagParser<Document> _documentTagParser;
        private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
        private readonly IDynamicTagParser<S3BucketSearch> _searchTagParser;
        private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser;
        private readonly IRepository _repository;


        public SectionFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, MarkdownWrapper markdownWrapper, 
            IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser, IDynamicTagParser<S3BucketSearch> searchTagParser, IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser, IRepository repository)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
            _profileTagParser = profileTagParser;
            _documentTagParser = documentTagParser;
            _alertsInlineTagParser = alertsInlineTagParser;
            _searchTagParser = searchTagParser;
            _privacyNoticeTagParser = privacyNoticeTagParser;
            _repository = repository;
        }

        public ProcessedSection Build(Section section, string articleTitle = null)
        {
            var parsedBody = _markdownWrapper.ConvertToHtml(section.Body);
            parsedBody = _profileTagParser.Parse(parsedBody, section.Profiles);
            parsedBody = _documentTagParser.Parse(parsedBody, section.Documents);
            parsedBody = _alertsInlineTagParser.Parse(parsedBody, section.AlertsInline);
            parsedBody = _searchTagParser.Parse(parsedBody, new List<S3BucketSearch> { section.S3Bucket });

            if (section.Body.Contains("PrivacyNotice:"))
            {
                section.PrivacyNotices = GetPrivacyNotices().Result;
                parsedBody = _privacyNoticeTagParser.Parse(parsedBody, section.PrivacyNotices);
            }

            parsedBody = _tagParserContainer.ParseAll(parsedBody, articleTitle);

            return new ProcessedSection(
                section.Title,
                section.Slug,
                parsedBody,
                section.Profiles,
                section.Documents,
                section.AlertsInline
            );
        }

        private async Task<IEnumerable<PrivacyNotice>> GetPrivacyNotices()
        {
            var response = await _repository.Get<List<PrivacyNotice>>();
            return response.Content as List<PrivacyNotice>;
        }
    }
}
