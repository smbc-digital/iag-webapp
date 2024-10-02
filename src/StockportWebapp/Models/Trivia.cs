namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Trivia
{
    public string Name { get; set; }

    public string Icon { get; set; }

    public string Body { get; set; }

    public string Link { get; set; }

    public Trivia(string name, string icon, string body, string link)
    {
        Name = name;
        Icon = icon;
        Body = body;
        Link = link;
    }
}