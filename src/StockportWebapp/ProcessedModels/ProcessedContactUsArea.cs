using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedContactUsArea : IProcessedContentType
    {
        public readonly string Title;
        public readonly string Slug;
        public readonly string Teaser;
        public readonly string Body;
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<SubItem> PrimaryItems;
        public readonly IEnumerable<Alert> Alerts;

   //     public ProcessedContactUsArea()
   //     { }

        public ProcessedContactUsArea(string title, string slug, string teaser, string body, IEnumerable<Crumb> breadcrumbs, IEnumerable<SubItem> primaryItems, IEnumerable<Alert> alerts)
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
