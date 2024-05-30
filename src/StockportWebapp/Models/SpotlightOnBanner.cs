namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class SpotlightOnBanner
{
    public string Title { get; }
    public string Image { get; }
    public string AltText { get; }
    public string Teaser { get; }
    public string Link { get; }
    public DateTime LastUpdated { get; }

    public SpotlightOnBanner(string title, string image, string altText, string teaser, string link, DateTime lastUpdated)
    {
        Title = title;
        Image = image;
        AltText = altText;
        Teaser = teaser;
        Link = link;
        LastUpdated = lastUpdated;
    }
}