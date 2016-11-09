using System.Collections.Generic;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Models
{
    public class NewsroomViewModel
    {
        private readonly FeatureToggles _featureToggles;
        public string Title { get; }
        public string Tag { get; }
        public string EmailAlertsUrl { get; }
        public bool EmailAlerts => _featureToggles.NewsAndTopicEmailAlerts && Newsroom.EmailAlerts;
        public List<Crumb> Breadcrumbs { get; }
        public Newsroom Newsroom { get; }

        public NewsroomViewModel(Newsroom newsroom, string emailAlertsUrl, string title, string tag, FeatureToggles featureToggles, List<Crumb> breadcrumbs)
        {
            _featureToggles = featureToggles;
            Breadcrumbs = breadcrumbs;
            Newsroom = newsroom;
            EmailAlertsUrl = SetEmailAlertsUrlWithTopicId(newsroom, emailAlertsUrl);
            Title = title;
            Tag = tag;
        }

        private static string SetEmailAlertsUrlWithTopicId(Newsroom newsroom, string url)
        {
            return !string.IsNullOrEmpty(newsroom.EmailAlertsTopicId) ? string.Concat(url, "?topic_id=", newsroom.EmailAlertsTopicId) : url;
        }
    }
}
