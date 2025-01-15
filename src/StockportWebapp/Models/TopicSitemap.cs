namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class TopicSitemap(string slug, DateTime sunriseDate, DateTime sunsetDate)
{
    public string Slug { get; set; } = slug;
    public DateTime SunriseDate { get; } = sunriseDate;
    public DateTime SunsetDate { get; } = sunsetDate;
}