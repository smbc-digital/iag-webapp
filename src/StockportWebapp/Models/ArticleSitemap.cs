using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StockportWebapp.Models
{
  public class ArticleSiteMap
    {
        public string Slug { get; set; }
        public DateTime SunriseDate { get; }
        public DateTime SunsetDate { get; }

        public ArticleSiteMap(string slug, DateTime sunriseDate, DateTime sunsetDate)
        {
            Slug = slug;
            SunriseDate = sunriseDate;
            SunsetDate = sunsetDate;
        }
    }
}