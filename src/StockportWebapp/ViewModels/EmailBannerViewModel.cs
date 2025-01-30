namespace StockportWebapp.ViewModels;

public class EmailBannerViewModel(string emailAlertsText, string emailAlertsTopicId)
{
    public string EmailAlertsText { get; set; } = emailAlertsText;
    public string EmailAlertsTopicId { get; set; } = emailAlertsTopicId;
}