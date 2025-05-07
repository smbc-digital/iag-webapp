namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class Image(string cssclass, string imageUrl)
{
    public string CssClass { get; } = cssclass;
    public string Url { get; } = imageUrl;
}