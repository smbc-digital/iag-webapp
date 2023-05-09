namespace StockportWebapp.ContentFactory;

public class DocumentPageFactory
{
    private readonly MarkdownWrapper _markdownWrapper;

    public DocumentPageFactory(
        MarkdownWrapper markdownWrapper)
    {
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedDocumentPage Build(DocumentPage documentPage)
    {
        var aboutTheDocument = _markdownWrapper.ConvertToHtml(documentPage.AboutTheDocument);
        var requestAnAccessibleFormatContactInformation = _markdownWrapper.ConvertToHtml(documentPage.RequestAnAccessibleFormatContactInformation);
        var furtherInformation = _markdownWrapper.ConvertToHtml(documentPage.FurtherInformation);
        var awsDocuments = _markdownWrapper.ConvertToHtml(documentPage.AwsDocuments);
        var multipleDocuments = MultipleDocuments(documentPage.Documents, documentPage.AwsDocuments);

        return new ProcessedDocumentPage(
            documentPage.Title,
            documentPage.Slug,
            documentPage.Teaser,
            documentPage.MetaDescription,
            aboutTheDocument,
            documentPage.Documents,
            awsDocuments,
            requestAnAccessibleFormatContactInformation,
            furtherInformation,
            documentPage.RelatedDocuments,
            documentPage.DatePublished,
            documentPage.LastUpdated,
            documentPage.Breadcrumbs,
            documentPage.UpdatedAt,
            multipleDocuments);
    }

    private bool MultipleDocuments(IEnumerable<Document> documents, string awsDocuments)
    {
        Regex regex = new Regex("(\\n-)");

        if (documents.Count() > 1)
            return true;

        if (documents.Count() == 1 && !string.IsNullOrEmpty(awsDocuments))
            return true;

        if (regex.Matches(awsDocuments).Count() > 0)
            return true;

        return false;
    }
}
