namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EventBanner(string title, string teaser, string icon, string link)
{
    public string Title { get; } = title;
    public string Teaser { get; } = teaser;
    public string Icon { get; } = icon;
    public string Link { get; } = link;
}