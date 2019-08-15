using System;
using System.Collections.Generic;
using StockportWebapp.Models;
using Xunit;
using FluentAssertions;
using StockportWebapp.ViewModels;

namespace StockportWebappTests_Unit.Unit.ViewModels
{
    public class TopicViewModelTest
    {
        private const string EmailAlertsUrl = "url";

        [Fact]
        public void ShouldSetEmailAlertsUrlWithTopicId()
        {
            const bool emailAlerts = true;
            const string emailAlertsTopicId = "topic-id";

            var advertisement = new Advertisement("ad-title", "ad-slug", "ad-teaser", DateTime.MinValue,
                DateTime.MinValue, true, "image-url", string.Empty);

            var topic = new Topic("name", "slug", "summary", "teaser", "metaDescription", "icon", "backgroundimage", "image", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId, null, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, advertisement);

            var topicViewModel = new TopicViewModel(topic, EmailAlertsUrl);

            topicViewModel.EmailAlertsUrl.Should().Be(string.Concat(EmailAlertsUrl, "?topic_id=", topic.EmailAlertsTopicId));
        }

        [Fact]
        public void ShouldSetEmailAlertsUrlWithoutTopicId()
        {
            const bool emailAlerts = true;
            const string emailAlertsTopicId = "";

            var advertisement = new Advertisement(string.Empty, string.Empty, string.Empty, DateTime.MinValue,
                DateTime.MinValue, false, string.Empty, string.Empty);

            var topic = new Topic("name", "slug", "summary", "teaser", "metaDescription", "icon", "backgroundimage", "image",
                new List<SubItem>(), new List<SubItem>(), new List<SubItem>(),
                new List<Crumb>(), new List<Alert>(), emailAlerts, emailAlertsTopicId, null, "expandingLinkText",
                new List<ExpandingLinkBox>(), string.Empty, string.Empty, advertisement);

            var topicViewModel = new TopicViewModel(topic, EmailAlertsUrl);

            topicViewModel.EmailAlertsUrl.Should().Be(EmailAlertsUrl);
        }
    }
}
