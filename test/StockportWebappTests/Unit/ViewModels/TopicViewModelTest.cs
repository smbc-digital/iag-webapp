using System.Collections.Generic;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using Xunit;
using FluentAssertions;

namespace StockportWebappTests.Unit.ViewModels
{
    public class TopicViewModelTest
    {
        private readonly string _emailAlertsUrl = "url";

        [Fact]
        public void ShouldNotProvideEmailAlertsIfFeatureToggleIsOff()
        {
            const bool emailAlerts = true;
            const string emailAlertsTopicId = "topic-id";

            var topic = new Topic("name", "slug", "summary", "teaser", "icon", "backgroundimage", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId);

            var topicViewModel = new TopicViewModel(topic, _emailAlertsUrl, new FeatureToggles { NewsAndTopicEmailAlerts = false });

            topicViewModel.EmailAlerts.Should().BeFalse();
        }

        [Fact]
        public void ShouldProvideEmailAlertsIfFeatureToggleIsOnAndThereAreAlerts()
        {
            const bool emailAlerts = true;
            const string emailAlertsTopicId = "topic-id";

            var topic = new Topic("name", "slug", "summary", "teaser", "icon", "backgroundimage", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId);

            var topicViewModel = new TopicViewModel(topic, _emailAlertsUrl, new FeatureToggles{NewsAndTopicEmailAlerts = true});

            topicViewModel.EmailAlerts.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotProvideEmailAlertsIfFeatureToggleIsOnButThereAreNoAlerts()
        {
            const bool emailAlerts = false;
            const string emailAlertsTopicId = "topic-id";

            var topic = new Topic("name", "slug", "summary", "teaser", "icon", "backgroundimage", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId);

            var topicViewModel = new TopicViewModel(topic, _emailAlertsUrl, new FeatureToggles { NewsAndTopicEmailAlerts = true });

            topicViewModel.EmailAlerts.Should().BeFalse();
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithTopicId()
        {
            const bool emailAlerts = true;
            const string emailAlertsTopicId = "topic-id";

            var topic = new Topic("name", "slug", "summary", "teaser", "icon", "backgroundimage", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId);

            var topicViewModel = new TopicViewModel(topic, _emailAlertsUrl, new FeatureToggles { NewsAndTopicEmailAlerts = true });

            topicViewModel.EmailAlertsUrl.Should().Be(string.Concat(_emailAlertsUrl, "?topic_id=", topic.EmailAlertsTopicId));
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithoutTopicId()
        {
            const bool emailAlerts = true;
            const string emailAlertsTopicId = "";

            var topic = new Topic("name", "slug", "summary", "teaser", "icon", "backgroundimage", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId);

            var topicViewModel = new TopicViewModel(topic, _emailAlertsUrl, new FeatureToggles { NewsAndTopicEmailAlerts = true });

            topicViewModel.EmailAlertsUrl.Should().Be(_emailAlertsUrl);
        }
    }
}
