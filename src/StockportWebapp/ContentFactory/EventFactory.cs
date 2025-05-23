namespace StockportWebapp.ContentFactory;

public interface IEventFactory
{
    ProcessedEvents Build(Event eventItem);
}

public class EventFactory(ITagParserContainer simpleTagParserContainer,
                        MarkdownWrapper markdownWrapper) : IEventFactory
{
    private readonly ITagParserContainer _tagParserContainer = simpleTagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedEvents Build(Event eventItem)
    {
        MapDetails mapDetails = new()
        {
            MapPosition = eventItem.MapPosition
        };

        string description = _tagParserContainer.ParseAll(_markdownWrapper.ConvertToHtml(eventItem.Description ?? string.Empty),
                                                        eventItem.Title,
                                                        true,
                                                        null,
                                                        eventItem.Documents,
                                                        null,
                                                        null,
                                                        null,
                                                        eventItem.CallToActionBanners);


        return new ProcessedEvents(eventItem.Title,
                                eventItem.Slug,
                                eventItem.Teaser,
                                eventItem.ImageUrl,
                                eventItem.ThumbnailImageUrl,
                                description,
                                eventItem.Fee,
                                eventItem.Free,
                                eventItem.Location,
                                eventItem.SubmittedBy,
                                eventItem.EventDate,
                                eventItem.StartTime,
                                eventItem.EndTime,
                                eventItem.Breadcrumbs,
                                mapDetails,
                                eventItem.BookingInformation,
                                eventItem.Group,
                                eventItem.Alerts,
                                eventItem.LogoAreaTitle,
                                eventItem.EventBranding,
                                eventItem.PhoneNumber,
                                eventItem.Email,
                                eventItem.Website,
                                eventItem.Facebook,
                                eventItem.Instagram,
                                eventItem.LinkedIn,
                                eventItem.MetaDescription,
                                eventItem.Duration,
                                eventItem.Languages,
                                eventItem.RelatedEvents,
                                eventItem.CallToActionBanners);
    }
}