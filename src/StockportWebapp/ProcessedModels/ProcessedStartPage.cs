using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedStartPage : IProcessedContentType
    {
        public string Slug { get; }
        public string Title { get; }
        public string Teaser { get; }
        public string Summary { get; }
        public string UpperBody { get; }
        public string FormLinkLabel { get; }
        public string FormLink { get; }
        public string LowerBody { get; }
        public IEnumerable<Crumb> Breadcrumbs { get; }
        public string BackgroundImage { get; }
        public string Icon { get; }
        public List<Alert> Alerts { get; private set; }


        public ProcessedStartPage(
            string slug,
            string title,
            string teaser,
            string summary,
            string upperBody,
            string formLinkLabel,
            string formLink,
            string lowerBody,
            IEnumerable<Crumb> breadcrumbs,
            string backgroundImage,
            string icon,
            List<Alert> alerts
            )
        {
            Slug = slug;
            Title = title;
            Teaser = teaser;
            Summary = summary;
            UpperBody = upperBody;
            FormLinkLabel = formLinkLabel;
            FormLink = formLink;
            LowerBody = lowerBody;
            Breadcrumbs = breadcrumbs;
            BackgroundImage = backgroundImage;
            Icon = icon;
            Alerts = alerts;
        }


    }
}
