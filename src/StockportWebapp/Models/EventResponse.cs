namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class EventResponse(List<Event> events, List<Event> featuredEvents)
{
    public List<Event> FeaturedEvents { get; set; } = featuredEvents;
    public List<Event> Events { get; set; } = events;
}