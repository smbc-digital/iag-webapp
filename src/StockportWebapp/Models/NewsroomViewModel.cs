using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class NewsroomViewModel
    {
        public string Title { get; }
        public string Tag { get; }
        public string EmailAlertsUrl { get; }
        public List<Crumb> Breadcrumbs { get; }
        public Newsroom Newsroom { get; }

        public NewsroomViewModel(Newsroom newsroom, string emailAlertsUrl, string title, string tag, List<Crumb> breadcrumbs)
        {
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
