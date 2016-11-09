using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class ProcessedSection : IProcessedContentType
    {
        public readonly string Title;
        public string Slug;
        public string Body;
        public readonly List<Profile> Profiles;
        public readonly List<Document> Documents;


        public ProcessedSection(string title, string slug, string body, List<Profile> profiles, List<Document> documents)
        {
            Title = title;
            Slug = slug;
            Body = body;
            Profiles = profiles;
            Documents = documents;
        }
    }
}


