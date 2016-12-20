using System.Collections.Generic;
using System.Reflection;
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
