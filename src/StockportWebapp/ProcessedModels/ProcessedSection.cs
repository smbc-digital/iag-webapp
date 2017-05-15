using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedSection : IProcessedContentType
    {
        public readonly string Title;
        public string Slug;
        public string Body;
        public readonly List<Profile> Profiles;
        public readonly List<Document> Documents;
        public readonly IEnumerable<Alert> AlertsInline;


        public ProcessedSection(string title, string slug, string body, List<Profile> profiles, List<Document> documents, IEnumerable<Alert> alertsInline)
        {
            Title = title;
            Slug = slug;
            Body = body;
            Profiles = profiles;
            Documents = documents;
            AlertsInline = alertsInline;
        }
    }
}


