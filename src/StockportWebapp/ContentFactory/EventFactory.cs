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
            var description = _simpleTagParserContainer.ParseAll(eventItem.Description, eventItem.Title);
            description = _markdownWrapper.ConvertToHtml(description ?? "");
            description = _documentTagParser.Parse(description, eventItem.Documents);

            return new ProcessedEvents(eventItem.Title, eventItem.Slug, eventItem.Teaser, eventItem.ImageUrl, 
                                       eventItem.ThumbnailImageUrl, description, eventItem.Fee, eventItem.Location, eventItem.SubmittedBy, 
                                       eventItem.EventDate, eventItem.StartTime, eventItem.EndTime, eventItem.Breadcrumbs, eventItem.Categories, eventItem.MapPosition);
        }
    }
}
