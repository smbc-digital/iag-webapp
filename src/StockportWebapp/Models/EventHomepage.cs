namespace StockportWebapp.Models;

public class EventHomepage(List<Alert> alerts)
{
    public List<EventHomepageRow> Rows { get; set; }
    public List<EventCategory> Categories { get; set; }
    public string MetaDescription { get; set; }
    public List<Alert> Alerts { get; set; } = alerts;
    public List<ProcessedEvents> NextEvents { get; set; } = new();
    public CallToActionBanner CallToAction { get; set; }

    public GenericFeaturedItemList GenericItemList => new()
    {
        Items = Categories.Select(cat => new GenericFeaturedItem(cat.Name, $"/events?category={cat.Slug}", cat.Icon)).ToList(),
        ButtonText = string.Empty,
        HideButton = true
    };
}