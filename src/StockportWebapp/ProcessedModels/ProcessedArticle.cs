using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ProcessedModels
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
        public readonly IEnumerable<ProcessedSection> Sections;
        public readonly string Icon;
        public readonly string BackgroundImage;
        public readonly string Image;
        public readonly IEnumerable<Crumb> Breadcrumbs;
        public readonly IEnumerable<Alert> Alerts;
        public readonly Topic ParentTopic;
        public readonly string NavigationLink;
        public bool LiveChatVisible { get; set; }
        public LiveChat LiveChat { get; set; }
        public readonly IEnumerable<Alert> AlertsInline;
        public Advertisement Advertisement;
        
        public ProcessedArticle(string title, string slug, string body, string teaser,
            IEnumerable<ProcessedSection> sections, string icon, string backgroundImage, string image, IEnumerable<Crumb> breadcrumbs, 
            IEnumerable<Alert> alerts, Topic topic, bool liveChatVisible, LiveChat liveChat, IEnumerable<Alert> alertsInline, Advertisement advertisement)
        {
            Title = title;
            NavigationLink = TypeRoutes.GetUrlFor("article", slug);
            Body = body;
            Teaser = teaser;
            Sections = sections;
            Icon = icon;
            BackgroundImage = backgroundImage;
            Image = image;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            ParentTopic = topic;
            LiveChatVisible = liveChatVisible;
            LiveChat = liveChat;
            AlertsInline = alertsInline;
            Advertisement = advertisement;
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
