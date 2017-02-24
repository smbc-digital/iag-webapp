using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class EventResponse
    {
        public List<Event> Events { get; set; }
        public List<string> Categories { get; }

        public EventResponse(List<Event> events, List<string> categories)
        {
            Events = events;
            Categories = categories;
        }
    }
}
