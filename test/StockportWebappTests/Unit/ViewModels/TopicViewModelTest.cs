﻿namespace StockportWebappTests_Unit.Unit.ViewModels;

public class TopicViewModelTest
{
    private const string EmailAlertsUrl = "url";

    [Fact]
    public void ShouldSetEmailAlertsUrlWithTopicId()
    {
        const bool emailAlerts = true;
        const string emailAlertsTopicId = "topic-id";

        var topic = new ProcessedTopic("name", "slug", "metaDescription", "summary", "teaser", "icon", "backgroundimage", "image", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
            new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId, null, null, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true,
             new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, new CallToActionBanner(), null, string.Empty);

        var topicViewModel = new TopicViewModel(topic, EmailAlertsUrl);

        topicViewModel.EmailAlertsUrl.Should().Be(string.Concat(EmailAlertsUrl, "?topic_id=", topic.EmailAlertsTopicId));
    }

    [Fact]
    public void ShouldSetEmailAlertsUrlWithoutTopicId()
    {
        const bool emailAlerts = true;
        const string emailAlertsTopicId = "";

        var topic = new ProcessedTopic("name", "slug", "metaDescription", "summary", "teaser", "icon", "backgroundimage", "image",
            new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
            new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId, null, null, "expandingLinkText",
            new List<ExpandingLinkBox>(), string.Empty, string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, new CallToActionBanner(), null, string.Empty);

        var topicViewModel = new TopicViewModel(topic, EmailAlertsUrl);

        topicViewModel.EmailAlertsUrl.Should().Be(EmailAlertsUrl);
    }
}
