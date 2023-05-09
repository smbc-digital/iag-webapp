namespace StockportWebapp.Models.ProcessedModels
{
    public class ProcessedContactUsArea : IProcessedContentType
    {
        public readonly string Title;
        public readonly string Slug;
        public readonly string CategoriesTitle;
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<SubItem> PrimaryItems;
        public readonly IEnumerable<Alert> Alerts;
        public readonly string InsetTextTitle;
        public readonly string InsetTextBody;
        public readonly IEnumerable<ProcessedContactUsCategory> ContactUsCategories;
        public readonly string MetaDescription;

        public ProcessedContactUsArea(string title, string slug, string categoriesTitle, IEnumerable<Crumb> breadcrumbs, IEnumerable<SubItem> primaryItems, IEnumerable<Alert> alerts, IEnumerable<ProcessedContactUsCategory> contactUsCategories, string insetTextTitle, string insetTextBody, string metaDescription)
        {
            Title = title;
            Slug = slug;
            CategoriesTitle = categoriesTitle;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            PrimaryItems = primaryItems;
            InsetTextTitle = insetTextTitle;
            InsetTextBody = insetTextBody;
            ContactUsCategories = contactUsCategories;
            MetaDescription = metaDescription;
        }
    }
}
