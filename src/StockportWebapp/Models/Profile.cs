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
        public string Quote { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }
        public string BackgroundImage { get; set; }
        public string Icon { get; set; }
        public string Subject { get; set; }
        public string Author { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public List<Alert> Alerts { get; set; }

        public Profile(string type,
            string title,
            string slug,
            string subtitle,
            string teaser,
            string quote,
            string image, 
            string body,
            string backgroundImage,
            string icon,
            IEnumerable<Crumb> breadcrumbs,
            List<Alert> alerts,
            string subject,
            string author)
        {
            Type = type;
            Title = title;
            Slug = slug;
            Subtitle = subtitle;
            Teaser = teaser;
            Quote = quote;
            Image = image;
            Body = body;
            BackgroundImage = backgroundImage;
            Icon = icon;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            Subject = subject;
            Author = author;
        }
    }
}