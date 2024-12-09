namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class CarouselContent
{
    public string Title { get; set; }
    public string Teaser { get; set; }
    public string Image { get; set; }
    public string URL { get; set; }
    public DateTime Date { get; set; }

    public CarouselContent(string title, string teaser, string image, string url, DateTime date)
    {
        Title = title;
        Teaser = teaser;
        Image = image;
        URL = url;
        Date = date;
    }
}