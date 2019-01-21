using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class ProfileNew
    {
        public string Title { get; }
        public string Slug { get; }
        public string LeadParagraph { get; }
        public string Quote { get; }
        public string Image { get; }
        public string Body { get; }
        public IEnumerable<Crumb> Breadcrumbs { get; }
        public List<Alert> Alerts { get; }
        public List<InformationList> DidYouKnowSection { get; }
        public List<InformationList> KeyFactsSection { get; }
        public FieldOrder FieldOrder { get; }

        public ProfileNew()
        {

        }

        public ProfileNew(string title,
            string slug,
            string leadParagraph,
            string quote,
            string image,
            string body,
            IEnumerable<Crumb> breadcrumbs,
            List<Alert> alerts,
            List<InformationList> didYouKnowSection,
            List<InformationList> keyFactsSection,
            FieldOrder fieldOrder)
        {
            Title = title;
            Slug = slug;
            LeadParagraph = leadParagraph;
            Quote = quote;
            Image = image;
            Body = body;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            DidYouKnowSection = didYouKnowSection;
            KeyFactsSection = keyFactsSection;
            FieldOrder = fieldOrder;
        }

    }
}
