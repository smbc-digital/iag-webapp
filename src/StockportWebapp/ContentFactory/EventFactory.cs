﻿namespace StockportWebapp.ContentFactory;

public interface IEventFactory
{
    ProcessedEvents Build(Event eventItem);
}

public class EventFactory : IEventFactory
{
    private readonly ITagParserContainer _tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper;

    public EventFactory(ITagParserContainer simpleTagParserContainer, MarkdownWrapper markdownWrapper)
    {
        _tagParserContainer = simpleTagParserContainer;
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedEvents Build(Event eventItem)
    {
        MapDetails mapDetails = new()
        {
            MapPosition = eventItem.MapPosition,
            AccessibleTransportLink = eventItem.AccessibleTransportLink
        };

        string description = _tagParserContainer.ParseAll(eventItem.Description, eventItem.Title, true, null, eventItem.Documents, null, null, null);
        description = _markdownWrapper.ConvertToHtml(description ?? string.Empty);

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
                                eventItem.Categories,
                                mapDetails,
                                eventItem.BookingInformation,
                                eventItem.Group,
                                eventItem.Alerts,
                                eventItem.AccessibleTransportLink,
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
                                eventItem.RelatedEvents);
    }
}