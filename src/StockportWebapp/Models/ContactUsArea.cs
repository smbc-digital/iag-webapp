using System.Collections.Generic;

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
        public IEnumerable<InsetText> InsetTexts { get; set; }
        public IEnumerable<ContactUsCategory> ContactUsCategories { get; set; }

        public ContactUsArea(string title, string slug, string categoriesTitle, IEnumerable<Crumb> breadcrumbs, IEnumerable<Alert> alerts, IEnumerable<SubItem> primaryItems, IEnumerable<InsetText> insetTexts, IEnumerable<ContactUsCategory> contactUsCategories)
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