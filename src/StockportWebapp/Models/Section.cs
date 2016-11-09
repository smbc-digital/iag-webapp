using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Section
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public List<Profile> Profiles { get; set; }
        public List<Document> Documents { get; set; }

        public Section() { }

        public Section(string title, string slug, string body, List<Profile> profiles, List<Document> documents)
        {
            Title = title;
            Slug = slug;
            Body = body;
            Profiles = profiles;
            Documents = documents;
        }
    }
}