namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class SpotlightOnBanner(string title,
                            string image,
                            string altText,
                            string teaser,
                            string link,
                            DateTime lastUpdated)
{
    public string Title { get; } = title;
    public string Image { get; } = image;
    public string AltText { get; } = altText;
    public string Teaser { get; } = teaser;
    public string Link { get; } = link;
    public DateTime LastUpdated { get; } = lastUpdated;
}