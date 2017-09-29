using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using System.Collections.Generic;

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

        public SectionFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, MarkdownWrapper markdownWrapper, 
            IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser, IDynamicTagParser<S3BucketSearch> searchTagParser)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
            _profileTagParser = profileTagParser;
            _documentTagParser = documentTagParser;
            _alertsInlineTagParser = alertsInlineTagParser;
            _searchTagParser = searchTagParser;
        }

        public ProcessedSection Build(Section section, string articleTitle = null)
        {
            
            var parsedBody = _tagParserContainer.ParseAll(section.Body, articleTitle);
            var processedBody = _markdownWrapper.ConvertToHtml(parsedBody);
            var parsedBodyWithProfiles = _profileTagParser.Parse(processedBody, section.Profiles);
            var parsedBodyWithDocuments = _documentTagParser.Parse(parsedBodyWithProfiles, section.Documents);
            var parsedBodyWithAlertsInline = _alertsInlineTagParser.Parse(parsedBodyWithDocuments, section.AlertsInline);
            var body = _searchTagParser.Parse(parsedBodyWithAlertsInline, new List<S3BucketSearch> { section.S3Bucket });

            return new ProcessedSection(
                section.Title,
                section.Slug,
                body,
                section.Profiles,
                section.Documents,
                section.AlertsInline
            );
        }
    }
}
