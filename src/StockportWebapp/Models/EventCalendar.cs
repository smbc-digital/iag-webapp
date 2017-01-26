using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class EventCalendar
    {
        public List<Event> Events { get; }
        public List<string> Categories { get; }

        public EventCalendar(List<Event> events, List<string> categories)
        {
            Events = events;
            Categories = categories;
        }

        public bool DoesCategoryExist(string category)
        {
            return Categories.Contains(category);
        }
    }
}
