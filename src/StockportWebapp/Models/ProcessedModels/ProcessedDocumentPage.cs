namespace StockportWebapp.Models.ProcessedModels
{
    public class ProcessedDocumentPage : IProcessedContentType
    {
        public string Title;
        public string Slug;
        public string Teaser;
        public string MetaDescription;
        public string AboutTheDocument;
        public IEnumerable<Document> Documents;
        public string AwsDocuments;
        public string RequestAnAccessibleFormatContactInformation;
        public string FurtherInformation;
        public List<SubItem> RelatedDocuments;
        public DateTime DatePublished;
        public DateTime LastUpdated;
        public IEnumerable<Crumb> Breadcrumbs;
        public DateTime UpdatedAt;
        public bool MultipleDocuments;

        public ProcessedDocumentPage(
            string title,
            string slug,
            string teaser,
            string metaDescription,
            string aboutTheDocument,
            IEnumerable<Document> documents,
            string awsDocuments,
            string requestAnAccessibleFormatContactInformation,
            string furtherInformation,
            List<SubItem> relatedDocuments,
            DateTime datePublished,
            DateTime lastUpdated,
            IEnumerable<Crumb> breadcrumbs,
            DateTime updatedAt,
            bool multipleDocuments)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            MetaDescription = metaDescription;
            AboutTheDocument = aboutTheDocument;
            Documents = documents;
            AwsDocuments = awsDocuments;
            RequestAnAccessibleFormatContactInformation = requestAnAccessibleFormatContactInformation;
            FurtherInformation = furtherInformation;
            RelatedDocuments = relatedDocuments;
            DatePublished = datePublished;
            LastUpdated = lastUpdated;
            Breadcrumbs = breadcrumbs;
            UpdatedAt = updatedAt;
            MultipleDocuments = multipleDocuments;
        }
    }
}
