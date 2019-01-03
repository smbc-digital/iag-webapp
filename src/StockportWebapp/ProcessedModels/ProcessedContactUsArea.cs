using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedContactUsArea : IProcessedContentType
    {
        public readonly string Title;
        public readonly string Slug;
        public readonly string CategoriesTitle;
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<SubItem> PrimaryItems;
        public readonly IEnumerable<Alert> Alerts;
        public readonly IEnumerable<InsetText> InsetTexts;
        public readonly IEnumerable<ProcessedContactUsCategory> ContactUsCategories;

        public ProcessedContactUsArea(string title, string slug, string categoriesTitle, IEnumerable<Crumb> breadcrumbs, IEnumerable<SubItem> primaryItems, IEnumerable<Alert> alerts, IEnumerable<InsetText> insetTexts, IEnumerable<ProcessedContactUsCategory> contactUsCategories)
        {
            Title = title;
            Slug = slug;
            CategoriesTitle = categoriesTitle;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            PrimaryItems = primaryItems;
            InsetTexts = insetTexts;
            ContactUsCategories = contactUsCategories;
        }
    }
}
