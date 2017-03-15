using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

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

        public SectionFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
            _profileTagParser = profileTagParser;
            _documentTagParser = documentTagParser;
            _alertsInlineTagParser = alertsInlineTagParser;
        }

        public ProcessedSection Build(Section section, string articleTitle = null)
        {
            
            var parsedBody = _tagParserContainer.ParseAll(section.Body, articleTitle);
            var processedBody = _markdownWrapper.ConvertToHtml(parsedBody);
            var parsedBodyWithProfiles = _profileTagParser.Parse(processedBody, section.Profiles);
            var parsedBodyWithDocuments = _documentTagParser.Parse(parsedBodyWithProfiles, section.Documents);
            var parsedBodyWithAlertsInline = _alertsInlineTagParser.Parse(parsedBodyWithDocuments, section.AlertsInline);
            

            return new ProcessedSection(
                section.Title,
                section.Slug,
                parsedBodyWithAlertsInline,
                section.Profiles,
                section.Documents,
                section.AlertsInline
                );
        }
    }
}
