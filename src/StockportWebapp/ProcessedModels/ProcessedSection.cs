using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedSection : IProcessedContentType
    {
        public readonly string Title;
        public string Slug;
        public string MetaDescription;
        public string Body;
        public readonly List<Profile> Profiles;
        public readonly List<Document> Documents;
        public readonly IEnumerable<Alert> AlertsInline;
        public bool HideLastUpdated;


        public ProcessedSection(string title, string slug, string metaDescription, string body, List<Profile> profiles, List<Document> documents, IEnumerable<Alert> alertsInline, bool hideLastUpdated)
        {
            Title = title;
            Slug = slug;
            MetaDescription = metaDescription;
            Body = body;
            Profiles = profiles;
            Documents = documents;
            AlertsInline = alertsInline;
            HideLastUpdated = hideLastUpdated;
        }
    }
}


