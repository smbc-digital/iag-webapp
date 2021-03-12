using System;
using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedDocumentPage : IProcessedContentType
    {
        public string Title;
        public string Slug;
        public string Teaser;
        public string MetaDescription;
        public string AboutThisDocument;
        public IEnumerable<Document> Documents;
        public string RequestAnAccessibleFormatContactInformation;
        public string FurtherInformation;
        public List<SubItem> RelatedDocuments;
        public DateTime DatePublished;
        public DateTime LastUpdated;
        public IEnumerable<Crumb> Breadcrumbs;
        public DateTime UpdatedAt;

        public ProcessedDocumentPage(
            string title,
            string slug,
            string teaser,
            string metaDescription,
            string aboutThisDocument,
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
            AboutThisDocument = aboutThisDocument;
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
}
