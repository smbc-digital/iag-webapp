using System;

namespace StockportWebapp.Models
{
    public class Advertisement
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Teaser { get; set; }
        public DateTime SunriseDate { get; set; }
        public DateTime SunsetDate { get; set; }
        public bool Isadvertisement { get; set; }
        public string NavigationUrl { get; set; }
        public string Image { get; set; }

        public Advertisement(string title, string slug, string teaser, DateTime sunriseDate, DateTime sunsetDate, bool isadvertisement, string navigationUrl, string image)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            SunriseDate = sunriseDate;
            SunsetDate = sunsetDate;
            Isadvertisement = isadvertisement;
            NavigationUrl = navigationUrl;
            Image = image;
        }
    }
}