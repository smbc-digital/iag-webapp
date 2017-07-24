using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class EventHomepageRow
    {
        public bool IsLatest { get; set; }
        public string Tag { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }

    public class EventHomepage
    {
        public List<EventHomepageRow> Rows { get; set; }
    }
}
