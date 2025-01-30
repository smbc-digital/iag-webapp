namespace StockportWebapp.ViewModels;

public class HomepageViewModel
{
    public ProcessedHomepage HomepageContent { get; set; }
    public List<Event> EventsFromApi { get; set; }
    public Event FeaturedEvent { get; set; }
    public List<Event> FeaturedEvents { get; set; }
    public News FeaturedNews { get; set; }

    public NavCardList PrimaryItems()
    {
        if (FeaturedEvents is null || FeaturedEvents.Count < 3)
        {
            return new NavCardList()
            {
                Items = FeaturedEvents.Select(subItem => new NavCard()).ToList()
            };
        }

        int numberItemsToDisplay = FeaturedEvents.Count / 3 * 3;

        List<NavCard> items = FeaturedEvents.Take(numberItemsToDisplay).ToList().Select(subItem => new NavCard(
            subItem.Title, 
            GenerateEventDetailUrl(subItem.Slug, subItem.EventDate),
            subItem.Teaser, 
            subItem.ThumbnailImageUrl,
            subItem.ImageUrl,
            string.Empty,
            EColourScheme.Teal,
            subItem.EventDate,
            subItem.StartTime
        )).ToList();

        return new NavCardList() { Items = items };
    }

    private string GenerateEventDetailUrl(string slug, DateTime eventDate) =>
        $"/events/{slug}?date={eventDate:yyyy-MM-dd}";
}