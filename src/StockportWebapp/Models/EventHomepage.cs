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

    public GenericFeaturedItemList GenericItemList
    {
        get
        {
            var result = new GenericFeaturedItemList();
            result.Items = new List<GenericFeaturedItem>();
            foreach (var cat in Categories)
            {
                result.Items.Add(new GenericFeaturedItem { Icon = cat.Icon, Title = cat.Name, Url = $"/events?category={cat.Slug}" });
            }

            result.ButtonText = string.Empty;
            result.HideButton = true;
            return result;
        }
    }
}
