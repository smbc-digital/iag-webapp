using System;

namespace StockportWebapp.Models
{
    public class TopicSitemap
    {
        public string Slug { get; set; }
        public DateTime SunriseDate { get; }
        public DateTime SunsetDate { get; }

        public TopicSitemap(string slug, DateTime sunriseDate, DateTime sunsetDate)
        {
            Slug = slug;
            SunriseDate = sunriseDate;
            SunsetDate = sunsetDate;
        }
    }
}