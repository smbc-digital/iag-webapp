namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Trivia(string name, string icon, string body, string link)
{
    public string Name { get; set; } = name;
    public string Icon { get; set; } = icon;
    public string Body { get; set; } = body;
    public string Link { get; set; } = link;
}