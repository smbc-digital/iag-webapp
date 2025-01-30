namespace StockportWebapp.ViewModels;

public class TopicViewModel(ProcessedTopic topic, string emailAlertsUrl)
{
    public ProcessedTopic Topic { get; } = topic;
    public string EmailAlertsUrl { get; } = SetEmailAlertsUrlWithTopicId(topic, emailAlertsUrl);

    private static string SetEmailAlertsUrlWithTopicId(ProcessedTopic topic, string url) =>
        !string.IsNullOrEmpty(topic.EmailAlertsTopicId)
            ? string.Concat(url, "?topic_id=", topic.EmailAlertsTopicId)
            : url;

    public bool TopicHasImage =>
        !string.IsNullOrEmpty(Topic.Image);

    public List<Event> EventsFromApi { get; set; }
}