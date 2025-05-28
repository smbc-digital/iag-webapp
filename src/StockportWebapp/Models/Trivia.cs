namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Trivia(string title, string icon, string body, string link)
{
    public string Title { get; set; } = title;
    public string Icon { get; set; } = icon;
    public string BodyText { get; set; } = body;
    public string Link { get; set; } = link;
}