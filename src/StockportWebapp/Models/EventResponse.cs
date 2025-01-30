namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class EventResponse(List<Event> events, List<string> categories, List<Event> featuredEvents)
{
    public List<Event> FeaturedEvents { get; set; } = featuredEvents;
    public List<Event> Events { get; set; } = events;
    public List<string> Categories { get; } = categories;
}