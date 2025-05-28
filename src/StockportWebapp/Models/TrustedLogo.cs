namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class TrustedLogo(string title, string text, MediaAsset image, string link)
{
    public string Title { get; set; } = title;

    public string Text { get; set; } = text;

    public MediaAsset Image { get; set; } = image;

    public string Link { get; set; } = link;
}