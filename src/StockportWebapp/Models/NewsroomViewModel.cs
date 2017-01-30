using System.Collections.Generic;
using System.Linq;

namespace StockportWebapp.Models
{
    public class NewsroomViewModel
    {
        public string EmailAlertsUrl { get; }
        public Newsroom Newsroom { get; }

        public List<string> Categories
        {
            get { return Newsroom.Categories.OrderBy(c => c).ToList(); }
        }

        public NewsroomViewModel(Newsroom newsroom, string emailAlertsUrl)
        {
            Newsroom = newsroom;
            EmailAlertsUrl = SetEmailAlertsUrlWithTopicId(newsroom, emailAlertsUrl);
        }

        private static string SetEmailAlertsUrlWithTopicId(Newsroom newsroom, string url)
        {
            return !string.IsNullOrEmpty(newsroom.EmailAlertsTopicId) ? string.Concat(url, "?topic_id=", newsroom.EmailAlertsTopicId) : url;
        }
    }
}
