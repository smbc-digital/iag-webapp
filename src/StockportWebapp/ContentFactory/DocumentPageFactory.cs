namespace StockportWebapp.ContentFactory;

public class DocumentPageFactory
{
    private readonly MarkdownWrapper _markdownWrapper;

    public DocumentPageFactory(MarkdownWrapper markdownWrapper) => _markdownWrapper = markdownWrapper;

    public virtual DocumentPage Build(DocumentPage documentPage) 
        => new()
        {
            Title = documentPage.Title,
            Slug = documentPage.Slug,
            Teaser = documentPage.Teaser,
            MetaDescription = documentPage.MetaDescription,
            AboutTheDocument = _markdownWrapper.ConvertToHtml(documentPage.AboutTheDocument),
            Documents = documentPage.Documents,
            AwsDocuments = _markdownWrapper.ConvertToHtml(documentPage.AwsDocuments),
            RequestAnAccessibleFormatContactInformation = _markdownWrapper.ConvertToHtml(documentPage.RequestAnAccessibleFormatContactInformation),
            FurtherInformation = _markdownWrapper.ConvertToHtml(documentPage.FurtherInformation),
            RelatedDocuments = documentPage.RelatedDocuments,
            DatePublished = documentPage.DatePublished,
            LastUpdated = documentPage.LastUpdated,
            Breadcrumbs = documentPage.Breadcrumbs,
            UpdatedAt = documentPage.UpdatedAt,
            MultipleDocuments = MultipleDocuments(documentPage.Documents, documentPage.AwsDocuments)
        };

    private static bool MultipleDocuments(IEnumerable<Document> documents, string awsDocuments)
    {
        Regex regex = new("(\\n-)");
        bool hasMultipleDocuments = documents is not null && documents.Skip(1).Any();
        bool hasSingleDocumentWithAwsDocuments = documents is not null && documents.Any() && !string.IsNullOrEmpty(awsDocuments);
        bool matchesAwsDocumentsPattern = regex.IsMatch(awsDocuments);

        return hasMultipleDocuments || hasSingleDocumentWithAwsDocuments || matchesAwsDocumentsPattern;
    }
}