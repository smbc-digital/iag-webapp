using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedProfile : IProcessedContentType
    {
        public readonly string Type;
        public readonly string Title;
        public readonly string Slug;
        public readonly string LeadParagraph;
        public readonly string Teaser;
        public readonly string Quote;
        public readonly string Image;
        public readonly string Body;
        public readonly string BackgroundImage;
        public readonly string Icon;
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public List<Alert> Alerts { get; set; }

        public ProcessedProfile(string type,
            string title,
            string slug,
            string leadParagraph,
            string teaser,
            string quote,
            string image,
            string body,
            string backgroundImage,
            string icon,
            IEnumerable<Crumb> breadcrumbs,
            List<Alert> alerts)
        {
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            Type = type;
            Title = title;
            Slug = slug;
            LeadParagraph = leadParagraph;
            Teaser = teaser;
            Quote = quote;
            Image = image;
            Body = body;
            BackgroundImage = backgroundImage;
            Icon = icon;
        }
    }
}