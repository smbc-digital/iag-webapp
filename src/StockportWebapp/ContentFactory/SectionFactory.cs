using System;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using System.Collections.Generic;
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
            
            if (section.Body.Contains("PrivacyNotice:"))
            {
                var test = GetPrivacyNoticesMatchingTitleAsync();
                section.PrivacyNotices = test.Result;
            }
            var parsedBody = _privacyNoticeTagParser.Parse(section.Body, section.PrivacyNotices);

            string body = string.Empty;

            if (!section.Body.Contains("PrivacyNotice:"))
            {
                parsedBody = _tagParserContainer.ParseAll(section.Body, articleTitle);
                var processedBody = _markdownWrapper.ConvertToHtml(parsedBody);
                var parsedBodyWithProfiles = _profileTagParser.Parse(processedBody, section.Profiles);
                var parsedBodyWithDocuments = _documentTagParser.Parse(parsedBodyWithProfiles, section.Documents);
                var parsedBodyWithAlertsInline = _alertsInlineTagParser.Parse(parsedBodyWithDocuments, section.AlertsInline);
                body = _searchTagParser.Parse(parsedBodyWithAlertsInline, new List<S3BucketSearch> { section.S3Bucket });
            }
            if (body == String.Empty)
            {
                body = parsedBody;
            }
            return new ProcessedSection(
                section.Title,
                section.Slug,
                body,
                section.Profiles,
                section.Documents,
                section.AlertsInline
            );
        }

        private async System.Threading.Tasks.Task<IEnumerable<PrivacyNotice>> GetPrivacyNoticesMatchingTitleAsync()
        {
            var response = await _repository.Get<List<PrivacyNotice>>();
            return response.Content as List<PrivacyNotice>;
        }
    }
}
