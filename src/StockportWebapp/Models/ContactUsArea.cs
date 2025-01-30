namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class ContactUsArea(string title,
                        string slug,
                        IEnumerable<Crumb> breadcrumbs,
                        IEnumerable<Alert> alerts,
                        IEnumerable<SubItem> primaryItems,
                        IEnumerable<ContactUsCategory> contactUsCategories,
                        string insetTextTitle,
                        string insetTextBody,
                        string metaDescription)
{
    public string Title { get; set; } = title;
    public string Slug { get; set; } = slug;
    public IEnumerable<Crumb> Breadcrumbs { get; set; } = breadcrumbs;
    public IEnumerable<SubItem> PrimaryItems { get; set; } = primaryItems;
    public IEnumerable<Alert> Alerts { get; } = alerts;
    public string InsetTextTitle { get; set; } = insetTextTitle;
    public string InsetTextBody { get; set; } = MarkdownWrapper.ToHtml(insetTextBody);
    public IEnumerable<ContactUsCategory> ContactUsCategories { get; set; } = contactUsCategories;
    public string MetaDescription { get; set; } = metaDescription;
}