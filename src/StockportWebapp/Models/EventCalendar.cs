using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class EventCalendar
    {
        public List<Event> Events { get; }

        public EventCalendar(List<Event> events)
        {
            Events = events;
        }
    }
}
