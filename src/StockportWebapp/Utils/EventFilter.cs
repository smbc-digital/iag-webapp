using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Utils
{
    public class EventFilter
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string DateRange { get; set; }

        public EventFilter(string dateFrom, String dateTo, string dateRange)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            DateRange = dateRange;
        }
    }
}
