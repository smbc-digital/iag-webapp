﻿namespace StockportWebapp.ViewModels;

public class TopicViewModel
{
    public ProcessedTopic Topic { get; }
    public string EmailAlertsUrl { get; }

    public TopicViewModel(ProcessedTopic topic, string emailAlertsUrl)
    {
        Topic = topic;
        EmailAlertsUrl = SetEmailAlertsUrlWithTopicId(topic, emailAlertsUrl);
    }

    private static string SetEmailAlertsUrlWithTopicId(ProcessedTopic topic, string url) =>
        !string.IsNullOrEmpty(topic.EmailAlertsTopicId) ? string.Concat(url, "?topic_id=", topic.EmailAlertsTopicId) : url;

    public bool TopicHasImage =>
        !string.IsNullOrEmpty(Topic.Image);

    public List<Event> EventsFromApi { get; set; }
}
