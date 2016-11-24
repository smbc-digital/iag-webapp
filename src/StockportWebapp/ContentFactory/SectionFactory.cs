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

        public SectionFactory(ISimpleTagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
            _profileTagParser = profileTagParser;
            _documentTagParser = documentTagParser;
        }

        public ProcessedSection Build(Section section, string articleTitle = null)
        {
            var parsedBody = _tagParserContainer.ParseAll(section.Body, articleTitle);
            var parsedBodyWithProfiles = _profileTagParser.Parse(parsedBody, section.Profiles);
            var parsedBodyWithDocuments = _documentTagParser.Parse(parsedBodyWithProfiles, section.Documents);
            var processedBody = _markdownWrapper.ConvertToHtml(parsedBodyWithDocuments);

            return new ProcessedSection(
                section.Title,
                section.Slug,
                processedBody,
                section.Profiles,
                section.Documents
                );
        }
    }
}
