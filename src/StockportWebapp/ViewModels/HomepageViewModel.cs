namespace StockportWebapp.ViewModels;

public class HomepageViewModel
{
    public ProcessedHomepage HomepageContent { get; set; }
    public List<Event> EventsFromApi { get; set; }
    public Event FeaturedEvent { get; set; }
    public List<Event> FeaturedEvents { get; set; }
    public News FeaturedNews { get; set; }

    public List<Event> GetFeaturedEventsToDisplay()
    {
        if (FeaturedEvents is null || FeaturedEvents.Count < 3)
            return FeaturedEvents;

        int numberItemsToDisplay = FeaturedEvents.Count / 3 * 3;

        return FeaturedEvents.Take(numberItemsToDisplay).ToList();
    }
}