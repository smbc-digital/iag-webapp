namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EventResponse(List<Event> events)
{
    public List<Event> Events { get; set; } = events;
}