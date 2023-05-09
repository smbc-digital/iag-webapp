namespace StockportWebapp.Models;

public class DocumentPage
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Teaser { get; set; }
    public string MetaDescription { get; set; }
    public string AboutTheDocument { get; set; }
    public IEnumerable<Document> Documents { get; set; }
    public string AwsDocuments { get; set; }
    public string RequestAnAccessibleFormatContactInformation { get; set; }
    public string FurtherInformation { get; set; }
    public List<SubItem> RelatedDocuments { get; set; }
    public DateTime DatePublished { get; set; }
    public DateTime LastUpdated { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public DateTime UpdatedAt { get; set; }

    public DocumentPage(
        string title,
        string slug,
        string teaser,
        string metaDescription,
        string aboutTheDocument,
        IEnumerable<Document> documents,
        string requestAnAccessibleFormatContactInformation,
        string furtherInformation,
        List<SubItem> relatedDocuments,
        DateTime datePublished,
        DateTime lastUpdated,
        IEnumerable<Crumb> breadcrumbs,
        DateTime updatedAt)
    {
        Title = title;
        Slug = slug;
        Teaser = teaser;
        MetaDescription = metaDescription;
        AboutTheDocument = aboutTheDocument;
        Documents = documents;
        RequestAnAccessibleFormatContactInformation = requestAnAccessibleFormatContactInformation;
        FurtherInformation = furtherInformation;
        RelatedDocuments = relatedDocuments;
        DatePublished = datePublished;
        LastUpdated = lastUpdated;
        Breadcrumbs = breadcrumbs;
        UpdatedAt = updatedAt;
    }
}