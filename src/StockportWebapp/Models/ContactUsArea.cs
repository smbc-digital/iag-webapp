using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class ContactUsArea
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string CategoriesTitle { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public IEnumerable<SubItem> PrimaryItems { get; set; }
        public IEnumerable<Alert> Alerts { get; }
        public string InsetTextTitle { get; set; }
        public string InsetTextBody { get; set; }
        public IEnumerable<ContactUsCategory> ContactUsCategories { get; set; }
        public string MetaDescription { get; set; }

        public ContactUsArea(string title, string slug, string categoriesTitle, IEnumerable<Crumb> breadcrumbs, IEnumerable<Alert> alerts, IEnumerable<SubItem> primaryItems, IEnumerable<ContactUsCategory> contactUsCategories, string insetTextTitle, string insetTextBody, string metaDescription)
        {
            Title = title;
            Slug = slug;
            CategoriesTitle = categoriesTitle;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            PrimaryItems = primaryItems;
            InsetTextTitle = insetTextTitle;
            InsetTextBody = MarkdownWrapper.ToHtml(insetTextBody);
            ContactUsCategories = contactUsCategories;
            MetaDescription = metaDescription;
        }
    }
}