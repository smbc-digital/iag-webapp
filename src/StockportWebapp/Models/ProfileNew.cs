using System.Collections.Generic;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Models
{
    public class ProfileNew
    {
        public string Title { get; }
        public string Slug { get; }
        public string Subtitle { get; }
        public string Quote { get; }
        public string Image { get; }
        public string Body { get; }
        public IEnumerable<Crumb> Breadcrumbs { get; }
        public List<Alert> Alerts { get; }
        public string DidYouKnowSubheading { get; }
        public List<ProcessedInformationItem> DidYouKnowSection { get; }
        public FieldOrder FieldOrder { get; }

        public ProfileNew()
        {

        }

        public ProfileNew(string title,
            string slug,
            string subtitle,
            string quote,
            string image,
            string body,
            IEnumerable<Crumb> breadcrumbs,
            List<Alert> alerts,
            string didYouKnowSubheading,
            List<ProcessedInformationItem> didYouKnowSection,
            FieldOrder fieldOrder)
        {
            Title = title;
            Slug = slug;
            Subtitle = subtitle;
            Quote = quote;
            Image = image;
            Body = body;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            DidYouKnowSubheading = didYouKnowSubheading;
            DidYouKnowSection = didYouKnowSection;
            FieldOrder = fieldOrder;
        }

    }
}
