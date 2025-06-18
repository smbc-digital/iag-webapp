namespace StockportWebapp.Models.ProcessedModels;
[ExcludeFromCodeCoverage]
public class ProcessedSection(string title,
                            string slug,
                            string metaDescription,
                            string body,
                            List<Profile> profiles,
                            List<Document> documents,
                            IEnumerable<Alert> alertsInline,
                            List<TrustedLogo> trustedLogos,
                            string logoAreaTitle,
                            DateTime updatedAt) : IProcessedContentType
{
    public readonly string Title = title;
    public string Slug = slug;
    public string MetaDescription = metaDescription;
    public string Body = body;
    public readonly List<Profile> Profiles = profiles;
    public readonly List<Document> Documents = documents;
    public readonly IEnumerable<Alert> AlertsInline = alertsInline;
    public List<TrustedLogo> TrustedLogos = trustedLogos;
    public string LogoAreaTitle = logoAreaTitle;
    public DateTime UpdatedAt = updatedAt;
}