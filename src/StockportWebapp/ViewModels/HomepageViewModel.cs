using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class HomepageViewModel
    {
        public ProcessedModels.ProcessedHomepage HomepageContent { get; set; }
        public List<Event> EventsFromApi { get; set; }
        public Event FeaturedEvent { get; set; }
        public News FeaturedNews { get; set; }
    }
}
