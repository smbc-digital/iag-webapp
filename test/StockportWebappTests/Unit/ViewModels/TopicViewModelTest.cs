namespace StockportWebappTests_Unit.Unit.ViewModels;

public class TopicViewModelTest
{
    private const string EmailAlertsUrl = "url";

    [Fact]
    public void ShouldSetEmailAlertsUrlWithTopicId()
    {
        // Arrange
        ProcessedTopic topic = new("name",
                                    "slug",
                                    "metaDescription",
                                    "summary",
                                    "teaser",
                                    "icon",
                                    "backgroundimage",
                                    "image",
                                    new List<SubItem>(),
                                    new List<SubItem>(),
                                    new List<SubItem>(),
                                    new List<Crumb>(),
                                    new List<Alert>(),
                                    true,
                                    "topic-id",
                                    null,
                                    null,
                                    true,
                                    new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                                    string.Empty,
                                    new CallToActionBanner(),
                                    null,
                                    string.Empty);

        // Act
        TopicViewModel topicViewModel = new(topic, EmailAlertsUrl);

        // Assert
        Assert.Equal(string.Concat(EmailAlertsUrl, "?topic_id=", topic.EmailAlertsTopicId), topicViewModel.EmailAlertsUrl);
    }

    [Fact]
    public void ShouldSetEmailAlertsUrlWithoutTopicId()
    {
        // Arrange
        ProcessedTopic topic = new("name",
                                    "slug",
                                    "metaDescription",
                                    "summary",
                                    "teaser",
                                    "icon",
                                    "backgroundimage",
                                    "image",
                                    new List<SubItem>(),
                                    new List<SubItem>(),
                                    new List<SubItem>(),
                                    new List<Crumb>(),
                                    new List<Alert>(),
                                    true,
                                    string.Empty,
                                    null,
                                    null,
                                    true,
                                    new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                                    string.Empty,
                                    new CallToActionBanner(),
                                    null,
                                    string.Empty);

        // Act
        TopicViewModel topicViewModel = new(topic, EmailAlertsUrl);

        // Assert
        Assert.Equal(EmailAlertsUrl, topicViewModel.EmailAlertsUrl);
    }
}