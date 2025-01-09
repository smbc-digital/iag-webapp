namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class GenericFeaturedItem(string title, string url, string icon)
{
    public string Title { get; set; } = title;
    public string Url { get; set; } = url;
    public string Icon { get; set; } = icon;
    public List<GenericFeaturedItem> SubItems { get; set; }
}