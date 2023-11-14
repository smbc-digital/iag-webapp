namespace StockportWebapp.Models;

public class EventHomepage
{
    public List<EventHomepageRow> Rows { get; set; }
    public List<EventCategory> Categories { get; set; }
    public string MetaDescription { get; set; }
    public List<Alert> Alerts { get; set; }


    public EventHomepage(List<Alert> alerts)
    {
        Alerts = alerts;
    }

    public GenericFeaturedItemList GenericItemList => new()
    {
        Items = Categories.Select(cat => new GenericFeaturedItem(cat.Name, $"/events?category={cat.Slug}", cat.Icon)).ToList(),
        ButtonText = string.Empty,
        HideButton = true
    };
}
