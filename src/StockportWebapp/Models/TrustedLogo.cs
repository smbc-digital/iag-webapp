namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class TrustedLogo(string title, string text, MediaAsset file, string url)
{
    public string Title { get; set; } = title;

    public string Text { get; set; } = text;

    public MediaAsset File { get; set; } = file;

    public string Url { get; set; } = url;
}