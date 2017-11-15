using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public interface IEventFactory
    {
        ProcessedEvents Build(Event eventItem);
    }

    public class EventFactory : IEventFactory
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
            var mapDetails = new MapDetails()
            {
                MapPosition = eventItem.MapPosition,
                AccessibleTransportLink = eventItem.AccessibleTransportLink
            };

            var description = _simpleTagParserContainer.ParseAll(eventItem.Description, eventItem.Title);
            description = _markdownWrapper.ConvertToHtml(description ?? "");
            description = _documentTagParser.Parse(description, eventItem.Documents);

            return new ProcessedEvents(eventItem.Title, eventItem.Slug, eventItem.Teaser, eventItem.ImageUrl,
                                       eventItem.ThumbnailImageUrl, description, eventItem.Fee, eventItem.Location, eventItem.SubmittedBy,
                                       eventItem.EventDate, eventItem.StartTime, eventItem.EndTime, eventItem.Breadcrumbs, eventItem.Categories,
                                       mapDetails, eventItem.BookingInformation, eventItem.Group, eventItem.Alerts, eventItem.AccessibleTransportLink);
        }
    }
}
