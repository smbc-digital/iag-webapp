using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Profile
    {
        public Profile() { }

        public string Type { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Subtitle { get; set; }
        public string Teaser { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }
        public string BackgroundImage { get; set; }
        public string Icon { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public List<Alert> Alerts { get; set; }

        public Profile(string type, string title, string slug, string subtitle, string teaser, string image, 
            string body, string backgroundImage, string icon, IEnumerable<Crumb> breadcrumbs, List<Alert> alerts)
        {
            Type = type;
            Title = title;
            Slug = slug;
            Subtitle = subtitle;
            Teaser = teaser;
            Image = image;
            Body = body;
            BackgroundImage = backgroundImage;
            Icon = icon;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
        }
    }
}