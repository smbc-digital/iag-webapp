namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class Section(string title,
                    string slug,
                    string metaDescription,
                    string body,
                    List<Profile> profiles,
                    List<Document> documents,
                    IEnumerable<Alert> alertsInline,
                    List<GroupBranding> sectionBranding,
                    string logoAreaTitle,
                    DateTime updatedAt)
{
    public string Title { get; set; } = title;
    public string Slug { get; set; } = slug;
    public string MetaDescription { get; set; } = metaDescription;
    public string Body { get; set; } = body;
    public List<Profile> Profiles { get; set; } = profiles;
    public List<Document> Documents { get; set; } = documents;
    public IEnumerable<Alert> AlertsInline { get; set; } = alertsInline;
    public IEnumerable<PrivacyNotice> PrivacyNotices { get; set; }
    public List<GroupBranding> SectionBranding { get; init; } = sectionBranding;
    public string LogoAreaTitle { get; set; } = logoAreaTitle;
    public DateTime UpdatedAt { get; set; } = updatedAt;
}