namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class SpotlightOnBanner(string title,
                            MediaAsset image,
                            string imageUrl,
                            string teaser,
                            string link,
                            DateTime lastUpdated)
{
    public string Title { get; } = title;
    public MediaAsset Image { get; } = image;
    public string ImageUrl { get; } = imageUrl;
    public string Teaser { get; } = teaser;
    public string Link { get; } = link;
    public DateTime LastUpdated { get; } = lastUpdated;
}