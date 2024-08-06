namespace StockportWebapp.Models.ProcessedModels;
[ExcludeFromCodeCoverage]
public class ProcessedSection : IProcessedContentType
{
    public readonly string Title;
    public string Slug;
    public string MetaDescription;
    public string Body;
    public readonly List<Profile> Profiles;
    public readonly List<Document> Documents;
    public readonly IEnumerable<Alert> AlertsInline;
    public List<GroupBranding> SectionBranding;
    public string LogoAreaTitle;
    public DateTime UpdatedAt;

    public ProcessedSection(string title, string slug, string metaDescription, string body, List<Profile> profiles, List<Document> documents, IEnumerable<Alert> alertsInline, List<GroupBranding> sectionBranding, string logoAreaTitle, DateTime updatedAt)
    {
        Title = title;
        Slug = slug;
        MetaDescription = metaDescription;
        Body = body;
        Profiles = profiles;
        Documents = documents;
        AlertsInline = alertsInline;
        SectionBranding = sectionBranding;
        LogoAreaTitle = logoAreaTitle;
        UpdatedAt = updatedAt;
    }
}