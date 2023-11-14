namespace StockportWebapp.Models;

public class NavCard
{
    public NavCard() {}
    public NavCard(string title, string url, string teaser, string image)
    {
        Title = title;
        Url = url;
        Teaser = teaser;
        Image = image;
    }

    public string Title { get; set; }
    public string Url { get; set; }
    public string Teaser { get; set; }
    public string Image { get; set; }
}