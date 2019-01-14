namespace StockportWebapp.ViewModels
{
    public class EmailBannerViewModel
    {
        public string EmailAlertsText { get; set; }
        public string EmailAlertsTopicId { get; set; }

        public EmailBannerViewModel(string emailAlertsText, string emailAlertsTopicId)
        {
            EmailAlertsText = emailAlertsText;
            EmailAlertsTopicId = emailAlertsTopicId;
        }
    }
}
