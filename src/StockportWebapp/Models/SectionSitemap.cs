namespace StockportWebapp.Models;

public class SectionSiteMap
{
    public string Slug { get; set; }
    public DateTime SunriseDate { get; }
    public DateTime SunsetDate { get; }

    public SectionSiteMap(string slug, DateTime sunriseDate, DateTime sunsetDate)
    {
        Slug = slug;
        SunriseDate = sunriseDate;
        SunsetDate = sunsetDate;
    }
}