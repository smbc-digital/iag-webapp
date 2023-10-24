namespace StockportWebapp.Models;

public class GenericFeaturedItem
{
    public GenericFeaturedItem(string title, string url, string icon){
        Title = title;
        Url = url;
        Icon = icon;
    }

    public string Title { get; set; }
    public string Url { get; set; }
    public string Icon { get; set; }
    public List<GenericFeaturedItem> SubItems { get; set; }
}
