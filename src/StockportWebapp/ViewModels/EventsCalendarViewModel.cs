using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class EventsCalendarViewModel
    {
        public List<Crumb> Breadcrumbs { get; }
        public EventCalendar EventCalendar { get; }

        public EventsCalendarViewModel(List<Crumb> breadcrumbs, EventCalendar eventCalendar)
        {
            Breadcrumbs = breadcrumbs;
            EventCalendar = eventCalendar;
        }
    }
}
