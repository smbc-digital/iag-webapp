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
        public IEnumerable<Alert> AlertsInline { get; set; }
        public S3BucketSearch S3Bucket { get; set; }
        public IEnumerable<PrivacyNotice> PrivacyNotices { get; set; }

        public Section() { }

        public Section(string title, string slug, string body, List<Profile> profiles, List<Document> documents, IEnumerable<Alert> alertsInline)
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