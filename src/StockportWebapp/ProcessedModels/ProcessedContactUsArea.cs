using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedContactUsArea : IProcessedContentType
    {
        public readonly string Title;
        public readonly string Slug;
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<SubItem> PrimaryItems;
        public readonly IEnumerable<Alert> Alerts;
        public readonly IEnumerable<InsetText> InsetTexts;

        public ProcessedContactUsArea(string title, string slug, IEnumerable<Crumb> breadcrumbs, IEnumerable<SubItem> primaryItems, IEnumerable<Alert> alerts, IEnumerable<InsetText> insetTexts)
        {
            Title = title;
            Slug = slug;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            PrimaryItems = primaryItems;
            InsetTexts = insetTexts;
        }
    }
}
