using System;
using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Newsroom
    {
        public List<News> News { get; }
        public List<Alert> Alerts { get; }
        public bool EmailAlerts { get; }
        public string EmailAlertsTopicId { get; }
        public List<string> Categories { get; }
        public List<DateTime> Dates { get; }

        public Newsroom(List<News> news, List<Alert> alerts, bool emailAlerts, string emailAlertsTopicId, List<string> categories, List<DateTime> dates )
        {
            News = news;
            Alerts = alerts;
            EmailAlerts = emailAlerts;
            EmailAlertsTopicId = emailAlertsTopicId;
            Categories = categories;
            Dates = dates;
        }
    }
}
