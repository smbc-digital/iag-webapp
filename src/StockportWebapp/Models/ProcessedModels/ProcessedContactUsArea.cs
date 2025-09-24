namespace StockportWebapp.Models.ProcessedModels;

[ExcludeFromCodeCoverage]
public class ProcessedContactUsArea(string title,
                                    string slug,
                                    IEnumerable<Crumb> breadcrumbs,
                                    IEnumerable<SubItem> primaryItems,
                                    IEnumerable<Alert> alerts,
                                    IEnumerable<ProcessedContactUsCategory> contactUsCategories,
                                    string insetTextTitle,
                                    string insetTextBody,
                                    string metaDescription) : IProcessedContentType
{
    public readonly string Title = title;
    public readonly string Slug = slug;
    public readonly IEnumerable<Crumb> Breadcrumbs = breadcrumbs;
    public readonly IEnumerable<SubItem> PrimaryItems = primaryItems;
    public readonly IEnumerable<Alert> Alerts = alerts;
    public readonly string InsetTextTitle = insetTextTitle;
    public readonly string InsetTextBody = insetTextBody;
    public readonly IEnumerable<ProcessedContactUsCategory> ContactUsCategories = contactUsCategories;
    public readonly string MetaDescription = metaDescription;
}