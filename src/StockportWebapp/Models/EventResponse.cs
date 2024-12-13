namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class EventResponse
{
    public List<Event> FeaturedEvents { get; set; }
    public List<Event> Events { get; set; }
    public List<string> Categories { get; }

    public EventResponse(List<Event> events, List<string> categories, List<Event> featuredEvents)
    {
        Events = events;
        Categories = categories;
        FeaturedEvents = featuredEvents;
    }
}