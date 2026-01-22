namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class InlineQuote(string image, string imageAltText, string quote, string author, string slug, EColourScheme theme)
{
    public string Image { get; set; } = image;
    public string ImageAltText { get; set; } = imageAltText;
    public string Quote { get; set; } = MarkdownWrapper.ToHtml(quote);
    public string Author { get; set; } = author;
    public string Slug { get; set; } = slug;
    public EColourScheme Theme { get; set; } = theme;
}