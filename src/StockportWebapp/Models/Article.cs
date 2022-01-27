using System;
using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public string Teaser { get; set; }
        public string MetaDescription { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public string Icon { get; set; }
        public string BackgroundImage { get; set; }
        public string Image { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public IEnumerable<Alert> Alerts { get; set; }
        public IEnumerable<Profile> Profiles { get; set; }
        public Topic ParentTopic { get; set; }
        public IEnumerable<Document> Documents { get; set; }
        public IEnumerable<Alert> AlertsInline { get; set; }
        public S3BucketSearch S3Bucket { get; set; }
        public IEnumerable<PrivacyNotice> PrivacyNotices { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool HideLastUpdated { get; set; }

        public Article(string title, string slug, string body, string teaser, string metaDescription, IEnumerable<Section> sections, string icon, string backgroundImage, string image,
            IEnumerable<Crumb> breadcrumbs, IEnumerable<Profile> profiles, IEnumerable<Document> documents, IEnumerable<Alert> alertsInline, DateTime updatedAt, bool hideLastUpdated)
        {
            Title = title;
            Slug = slug;
            Body = body;
            Teaser = teaser;
            MetaDescription = metaDescription;
            Sections = sections;
            Icon = icon;
            BackgroundImage = backgroundImage;
            Image = image;
            Breadcrumbs = breadcrumbs;
            Profiles = profiles;
            Documents = documents;
            AlertsInline = alertsInline;
            UpdatedAt = updatedAt;
            HideLastUpdated = hideLastUpdated;
        }
    }
}