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
            var body = _tagParserContainer.ParseAll(section.Body, section.Title);
            body = _markdownWrapper.ConvertToHtml(body ?? "");
            body = _profileTagParser.Parse(body, section.Profiles);
            body = _documentTagParser.Parse(body, section.Documents);
            body = _alertsInlineTagParser.Parse(body, section.AlertsInline);

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
