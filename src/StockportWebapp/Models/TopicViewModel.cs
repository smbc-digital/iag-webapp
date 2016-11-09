using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Models
{
    public class TopicViewModel
    {
        public Topic Topic { get; }
        public FeatureToggles FeatureToggles { get; }
        public string EmailAlertsUrl { get; }

        public bool EmailAlerts => FeatureToggles.NewsAndTopicEmailAlerts && Topic.EmailAlerts;

        public TopicViewModel(Topic topic, string emailAlertsUrl, FeatureToggles featureToggles)
        {
            Topic = topic;
            FeatureToggles = featureToggles;
            EmailAlertsUrl = SetEmailAlertsUrlWithTopicId(topic, emailAlertsUrl);
        }

        private static string SetEmailAlertsUrlWithTopicId(Topic topic, string url)
        {
            return !string.IsNullOrEmpty(topic.EmailAlertsTopicId) ? string.Concat(url, "?topic_id=", topic.EmailAlertsTopicId) : url;
        }
    }
}
