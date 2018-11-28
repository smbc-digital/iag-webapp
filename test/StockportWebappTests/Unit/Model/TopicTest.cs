using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StockportWebapp.Models;
using FluentAssertions;
using StockportWebapp.FeatureToggling;
using Xunit;

namespace StockportWebappTests.Unit.Model
{
    public class TopicTest : TestingBaseClass
    {
        [Fact]
        public void ConvertsMarkdownToHtml()
        {
            const string summary = "# This is a heading for a topic";

            var advertisement = new Advertisement(string.Empty, string.Empty, string.Empty, DateTime.MinValue,
                DateTime.MinValue, false, string.Empty, string.Empty);


            var topic = new Topic("Name", "slug", summary, "Teaser", "Icon", "Image", "Image", null, null, null,
              new List<Crumb>(), null, true, "test-id", null, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, advertisement);

            Assert.Equal("<h1>This is a heading for a topic</h1>\n", topic.Summary);
        }

        [Fact]
        public void ShouldDeserializeATopicWithOnePrimarySubItems()
        {
            var content = GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.TopicWithAlerts.json");
            var topic = JsonConvert.DeserializeObject<Topic>(content);

            topic.Name.Should().Be("Healthy Living");
            topic.SubItems.Count().Should().Be(1);
            topic.EmailAlerts.Should().Be(true);
            topic.EmailAlertsTopicId.Should().Be("test-id");
        }

        [Fact]
        public void ShouldDeserializeATopicWithoutSubItems()
        {
            var content = GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Topic.json");
            var topic = JsonConvert.DeserializeObject<Topic>(content);

            topic.SubItems.Should().BeEmpty();
            topic.EmailAlerts.Should().Be(true);
            topic.EmailAlertsTopicId.Should().Be("test-id");
        }

        [Fact]
        public void ShouldDeserializeATopicWithSecondaryItems()
        {
            var content = GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.TopicWithSecondaryItems.json");
            var topic = JsonConvert.DeserializeObject<Topic>(content);

            topic.SecondaryItems.Count().Should().Be(1);
            topic.SubItems.Should().BeEmpty();
            topic.EmailAlerts.Should().Be(true);
            topic.EmailAlertsTopicId.Should().Be("test-id");

        }

        [Fact]
        public void ShouldDeserializeATopicWithTertiaryItems()
        {
            var content = GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.TopicWithTertiaryItems.json");
            var topic = JsonConvert.DeserializeObject<Topic>(content);

            topic.TertiaryItems.Count().Should().Be(1);
            topic.EmailAlerts.Should().Be(true);
            topic.EmailAlertsTopicId.Should().Be("test-id");
        }

        [Fact]
        public void ShouldUseSecondaryAndTertiaryItemsAsInTopItems()
        {
            var content = GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.TopicWithAllItems.json");
            var topic = JsonConvert.DeserializeObject<Topic>(content);

            topic.SubItems.Count().Should().Be(3);
            topic.SecondaryItems.Count().Should().Be(2);
            topic.TertiaryItems.Count().Should().Be(2);
            topic.TopSubItems.Count().Should().Be(6);
            topic.TopSubItems.ToList()[0].Title.Should().Be("Getting Support");
            topic.TopSubItems.ToList()[5].Title.Should().Be("Title 5");
        }
    }
}
