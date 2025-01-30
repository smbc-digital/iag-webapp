namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class BasicLink(string url, string text)
{
    public string Url { get; } = url;
    public string Text { get; } = text;
}