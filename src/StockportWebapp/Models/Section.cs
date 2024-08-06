namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class Section
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string MetaDescription { get; set; }
    public string Body { get; set; }
    public List<Profile> Profiles { get; set; }
    public List<Document> Documents { get; set; }
    public IEnumerable<Alert> AlertsInline { get; set; }
    public IEnumerable<PrivacyNotice> PrivacyNotices { get; set; }
    public List<GroupBranding> SectionBranding { get; init; }
    public string LogoAreaTitle { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Section(string title, string slug, string metaDescription, string body, List<Profile> profiles, List<Document> documents, IEnumerable<Alert> alertsInline, List<GroupBranding> sectionBranding,
    string logoAreaTitle, DateTime updatedAt)
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