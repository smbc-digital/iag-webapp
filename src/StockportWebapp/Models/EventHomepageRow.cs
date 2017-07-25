using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Models
{
    public class EventHomepageRow
    {
        public bool IsLatest { get; set; }
        public string Tag { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
