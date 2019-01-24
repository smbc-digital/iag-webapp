using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class EventsListViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        public string Heading { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
    }
}
