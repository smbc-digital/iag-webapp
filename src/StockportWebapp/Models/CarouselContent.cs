namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class CarouselContent(string title, string teaser, string image, string url, DateTime date)
{
    public string Title { get; set; } = title;
    public string Teaser { get; set; } = teaser;
    public string Image { get; set; } = image;
    public string URL { get; set; } = url;
    public DateTime Date { get; set; } = date;
}