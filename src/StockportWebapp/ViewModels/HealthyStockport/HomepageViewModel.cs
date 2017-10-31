using StockportWebapp.Models;
using System.Collections.Generic;

namespace StockportWebapp.ViewModels.HealthyStockport
{
    public class HomepageViewModel
    {
        public ProcessedModels.ProcessedHomepage Homepage { get; set; }
        public List<Event> Events { get; set; }
    }
}
