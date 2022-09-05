using StockportWebapp.Models;
using System.Collections.Generic;

namespace StockportWebapp.ViewModels
{
    public class TopicViewModel
    {
        public Topic Topic { get; }
        public string EmailAlertsUrl { get; }

        public TopicViewModel(Topic topic, string emailAlertsUrl)
        {
            Topic = topic;
            EmailAlertsUrl = SetEmailAlertsUrlWithTopicId(topic, emailAlertsUrl);
        }

        private static string SetEmailAlertsUrlWithTopicId(Topic topic, string url)
        {
            return !string.IsNullOrEmpty(topic.EmailAlertsTopicId) ? string.Concat(url, "?topic_id=", topic.EmailAlertsTopicId) : url;
        }

        public List<Event> EventsFromApi { get; set; }
    }
}
