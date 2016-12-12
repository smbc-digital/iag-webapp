using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class EventFactory
    {
        private readonly ISimpleTagParserContainer _simpleTagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Document> _documentTagParser;

        public EventFactory(ISimpleTagParserContainer simpleTagParserContainer, MarkdownWrapper markdownWrapper, IDynamicTagParser<Document> documentTagParser)
        {
            _simpleTagParserContainer = simpleTagParserContainer;
            _markdownWrapper = markdownWrapper;
            _documentTagParser = documentTagParser;
        }

        public virtual ProcessedEvents Build(Event eventItem)
        {
            var body = _simpleTagParserContainer.ParseAll(eventItem.Description, eventItem.Title);
            body = _markdownWrapper.ConvertToHtml(body ?? "");

            return new ProcessedEvents(eventItem.Title, eventItem.Slug, eventItem.Teaser, eventItem.Image, eventItem.ThumbnailImage, body);
        }
    }
}
