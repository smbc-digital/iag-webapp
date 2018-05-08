using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public string Teaser { get; set; }
        public IEnumerable<Section> Sections { get; set; }
        public string Icon { get; set; }
        public string BackgroundImage { get; set; }
        public string Image { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public IEnumerable<Alert> Alerts { get; set; }
        public IEnumerable<Profile> Profiles { get; set; }
        public Topic ParentTopic { get; set; }
        public IEnumerable<Document> Documents { get; set; }
        public bool LiveChatVisible { get; set; }
        public LiveChat LiveChat { get; set; }
        public IEnumerable<Alert> AlertsInline { get; set; }
        public Advertisement Advertisement { get; set; }
        public S3BucketSearch S3Bucket { get; set; }
        public IEnumerable<PrivacyNotice> PrivacyNotices { get; set; }

        public Article(string title, string slug, string body, string teaser, IEnumerable<Section> sections, string icon, string backgroundImage, string image,
            IEnumerable<Crumb> breadcrumbs, IEnumerable<Profile> profiles, IEnumerable<Document> documents, bool liveChatVisible, LiveChat liveChat, IEnumerable<Alert> alertsInline, Advertisement advertisement)
        {
            Title = title;
            Slug = slug;
            Body = body;
            Teaser = teaser;
            Sections = sections;
            Icon = icon;
            BackgroundImage = backgroundImage;
            Image = image;
            Breadcrumbs = breadcrumbs;
            Profiles = profiles;
            Documents = documents;
            LiveChatVisible = liveChatVisible;
            LiveChat = liveChat;
            AlertsInline = alertsInline;
            Advertisement = advertisement;
        }
    }
}