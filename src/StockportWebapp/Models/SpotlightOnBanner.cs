namespace StockportWebapp.Models;

public class SpotlightOnBanner
{
    public string Title { get; }
    public string Image { get; }
    public string AltText { get; }
    public string Teaser { get; }
    public string Link { get; }

    public SpotlightOnBanner(string title, string image, string altText, string teaser, string link)
    {
        Title = title;
        Image = image;
        AltText = altText;
        Teaser = teaser;
        Link = link;
    }
}


public class NullSpotlightOnBanner : SpotlightOnBanner
{
    public NullSpotlightOnBanner() : base(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty) { }
}
