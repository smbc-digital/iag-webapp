using System.Collections.Generic;
using StockportWebapp.Models;
using Xunit;
using FluentAssertions;
using StockportWebapp.ViewModels;

namespace StockportWebappTests.Unit.ViewModels
{
    public class TopicViewModelTest
    {
        private const string EmailAlertsUrl = "url";

        [Fact]
        public void ShouldSetEmailAlertsUrlWithTopicId()
        {
            const bool emailAlerts = true;
            const string emailAlertsTopicId = "topic-id";

            var topic = new Topic("name", "slug", "summary", "teaser", "icon", "backgroundimage", "image", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId, null, "expandingLinkText", new List<ExpandingLinkBox>());

            var topicViewModel = new TopicViewModel(topic, EmailAlertsUrl);

            topicViewModel.EmailAlertsUrl.Should().Be(string.Concat(EmailAlertsUrl, "?topic_id=", topic.EmailAlertsTopicId));
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithoutTopicId()
        {
            const bool emailAlerts = true;
            const string emailAlertsTopicId = "";

            var topic = new Topic("name", "slug", "summary", "teaser", "icon", "backgroundimage", "image",
                new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId, null, "expandingLinkText",
                new List<ExpandingLinkBox>());

            var topicViewModel = new TopicViewModel(topic, EmailAlertsUrl);

            topicViewModel.EmailAlertsUrl.Should().Be(EmailAlertsUrl);
        }
    }
}
