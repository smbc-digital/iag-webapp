namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class NavCard
{
    public NavCard() {}
    public NavCard(string title,
                string url,
                string teaser,
                string teaserImage,
                string image,
                string icon = "",
                EColourScheme colourScheme = EColourScheme.Teal,
                DateTime date = new DateTime(),
                string time = "")
    {
        Title = title;
        Url = url;
        Teaser = teaser;
        TeaserImage = teaserImage;
        Image = image;
        Icon = icon;
        ColourScheme = colourScheme;
        Date = date;
        Time = time;
    }

    public string Title { get; set; }
    public string Url { get; set; }
    public string Teaser { get; set; }
    public string TeaserImage { get; set; }
    public string Image { get; set; }
    public string Icon { get; set; }
    public EColourScheme ColourScheme { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; }
}