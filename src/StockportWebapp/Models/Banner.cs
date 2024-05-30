namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public abstract class Banner
{
    public string Title { get; set; }
    public string Teaser { get; set; }
    public string Link { get; set; }
    public string Colour { get; set; }
}