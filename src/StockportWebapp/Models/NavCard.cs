namespace StockportWebapp.Models;

public class NavCard
{
    public NavCard() {}
    public NavCard(string title, string url, string teaser, string image, string icon = "", string colourScheme = "")
    {
        Title = title;
        Url = url;
        Teaser = teaser;
        Image = image;
        Icon = icon;
        ColourScheme = colourScheme;
    }

    public string Title { get; set; }
    public string Url { get; set; }
    public string Teaser { get; set; }
    public string Image { get; set; }
    public string Icon { get; set; }
    public string ColourScheme { get; set; }
}