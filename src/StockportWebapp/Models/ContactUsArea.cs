using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class ContactUsArea
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Teaser { get; set; }
        public string Body { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public IEnumerable<SubItem> PrimaryItems { get; set; }
        public IEnumerable<Alert> Alerts { get; }

        public ContactUsArea(string title, string slug, string teaser, string body, IEnumerable<Crumb> breadcrumbs, IEnumerable<Alert> alerts, IEnumerable<SubItem> primaryItems)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Breadcrumbs = breadcrumbs;
            Body = body;
            Alerts = alerts;
            PrimaryItems = primaryItems;
        }
    }
}