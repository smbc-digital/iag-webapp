﻿namespace StockportWebapp.Models.ProcessedModels
{
    public interface IContactUsMessageContainer
    {
        void AddContactUsMessage(string message, string slug = "");
    }

    public class ProcessedArticle : IProcessedContentType, IContactUsMessageContainer
    {
        public readonly string Title;
        public string Body { get; private set; }
        public readonly string Teaser;
        public readonly string MetaDescription;
        public readonly IEnumerable<ProcessedSection> Sections;
        public readonly string Icon;
        public readonly string BackgroundImage;
        public readonly string Image;
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<Alert> Alerts;
        public readonly Topic ParentTopic;
        public readonly string NavigationLink;
        public readonly IEnumerable<Alert> AlertsInline;
        public S3BucketSearch S3BucketSearch;
        public DateTime UpdatedAt;
        public bool HideLastUpdated;

        public ProcessedArticle(string title, string slug, string body, string teaser, string metaDescription,
            IEnumerable<ProcessedSection> sections, string icon, string backgroundImage, string image, IEnumerable<Crumb> breadcrumbs,
            IEnumerable<Alert> alerts, Topic topic, IEnumerable<Alert> alertsInline, S3BucketSearch s3BucketSearch, DateTime updatedAt, bool hideLastUpdated)
        {
            Title = title;
            NavigationLink = TypeRoutes.GetUrlFor("article", slug);
            Body = body;
            Teaser = teaser;
            MetaDescription = metaDescription;
            Sections = sections;
            Icon = icon;
            BackgroundImage = backgroundImage;
            Image = image;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            ParentTopic = topic;
            AlertsInline = alertsInline;
            S3BucketSearch = s3BucketSearch;
            UpdatedAt = updatedAt;
            HideLastUpdated = hideLastUpdated;
        }

        public void AddContactUsMessage(string message, string slug = "")
        {
            if (slug == "")
            {
                AddMessageToArticleBodyOrFirstSection(message);
            }
            else
            {
                AddMessageToArticleSectionWithMatchingSlug(slug, message);
            }
        }

        private void AddMessageToArticleSectionWithMatchingSlug(string slug, string htmlMessage)
        {
            var section = Sections?.ToList().Find(o => o.Slug == slug);
            if (section != null)
            {
                section.Body = ContactUsTagParser.ContactUsMessageTagRegex.Replace(section.Body, htmlMessage);
            }
        }

        private void AddMessageToArticleBodyOrFirstSection(string htmlMessage)
        {
            var matches = ContactUsTagParser.ContactUsMessageTagRegex.Matches(Body);
            if (matches.Count > 0)
            {
                Body = ContactUsTagParser.ContactUsMessageTagRegex.Replace(Body, htmlMessage);
            }
            else if (Sections != null && Sections.ToList().Count > 0)
            {
                var section = Sections.ToList().First();
                section.Body = ContactUsTagParser.ContactUsMessageTagRegex.Replace(section.Body, htmlMessage);
            }
        }
    }
}
