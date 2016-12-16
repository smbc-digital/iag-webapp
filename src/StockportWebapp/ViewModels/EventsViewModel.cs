using StockportWebapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.ViewModels
{
    public class EventsViewModel
    {
        public ProcessedEvents EventsItem { get; }

        public EventsViewModel(ProcessedEvents eventsItem)
        {
            EventsItem = eventsItem;         
        }      
    }
}
