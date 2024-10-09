namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class SiteHeader
{
    public string Title { get; set; }
    public IEnumerable<SubItem> Items { get; set; }
    public string Logo { get; set; }

    public SiteHeader(string title, List<SubItem> items, string logo)
    {
        Title = title;
        Items = items;
        Logo = logo;
    }
}