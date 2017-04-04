using System.Collections.Generic;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using System.Linq;
using FluentAssertions;
using StockportWebapp.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Repositories;
using System;
using StockportWebapp.ViewModels;

namespace StockportWebappTests.Unit.Controllers
{
    public class TopicControllerTest
    {
        private readonly TopicController _controller;
        private readonly Mock<IRepository> _repository;
        private const string BusinessId = "businessId";

        public TopicControllerTest()
        {
            var config = new Mock<IApplicationConfiguration>();

            config.Setup(o => o.GetEmailAlertsNewSubscriberUrl(BusinessId)).Returns(AppSetting.GetAppSetting("email-alerts-url"));

            _repository = new Mock<IRepository>();
            _controller = new TopicController(_repository.Object, config.Object, new BusinessId(BusinessId));
        }
        
        public SubItem CreateASubItem(int i)
        {
            return new SubItem("sub-topic" + i, "Title" + i, "Teaser", "Icon", "topic", "image", new List<SubItem>());
        }
    
        [Fact]
        public void GivenNavigateToTopicReturnsTopicWithExpectedProperties()
        {           
            var subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();
            var topic = new Topic("Name", "slug", "Summary", "Teaser", "Icon", "Image", "Image", subItems, null, null,
              new List<Crumb>(), new List<Alert>(), true, "test-id");

            const string slug = "healthy-living";
            _repository.Setup(o => o.Get<Topic>(slug, null)).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

            var indexPage = AsyncTestHelper.Resolve(_controller.Index(slug)) as ViewResult;
            var viewModel = indexPage.ViewData.Model as TopicViewModel;
            var result = viewModel.Topic;

            Assert.Equal("Name", result.Name);
            Assert.Equal("slug", result.Slug);
            Assert.Equal("<p>Summary</p>\n", result.Summary);
            Assert.Equal("Teaser", result.Teaser);
            Assert.Equal("Icon", result.Icon);
            Assert.Equal("Image", result.BackgroundImage);
            Assert.Equal("Image", result.Image);
            result.EmailAlerts.Should().Be(true);
            result.EmailAlertsTopicId.Should().Be("test-id");
        }

        [Fact]
        public void GivenNavigateToTopicReturnsListOfSubItemsByTopic()
        {
            var subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();
            var topic = new Topic("Name", "slug", "Summary", "Teaser", "Icon", "Image", "Image", subItems, null, null,
              new List<Crumb>(), new List<Alert>(), true, "test-id");

            const string slug = "healthy-living";
            _repository.Setup(o => o.Get<Topic>(slug, null)).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

            var indexPage = AsyncTestHelper.Resolve(_controller.Index("healthy-living")) as ViewResult;
            var viewModel = indexPage.ViewData.Model as TopicViewModel;
            var result = viewModel.Topic;

            var subItem = result.SubItems.FirstOrDefault();

            subItem.Title.Should().Be("Title0");
            subItem.NavigationLink.Should().Be("/topic/sub-topic0");
            subItem.Teaser.Should().Be("Teaser");
            subItem.Icon.Should().Be("Icon");
            result.EmailAlerts.Should().Be(true);
            result.EmailAlertsTopicId.Should().Be("test-id");          
        }

        [Fact]
        public void GivesNotFoundOnRequestForNonExistentTopic()
        {
            const string nonExistentTopic = "doesnt-exist";

            _repository.Setup(o => o.Get<Topic>(nonExistentTopic, null)).ReturnsAsync(new HttpResponse(404, null, "No topic found for 'doesnt-exist'"));

            var result = AsyncTestHelper.Resolve(_controller.Index(nonExistentTopic)) as StatusCodeResult;

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void GetsAlertsForTopic()
        {
            var alerts = new List<Alert>
            {
                new Alert("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc))
            };
            var topic = new Topic("Name", "slug", "Summary", "Teaser", "Icon", "Image", "Image", null, null, null,
               new List<Crumb>(), alerts, true, "test-id");

            const string slug = "healthy-living";
            _repository.Setup(o => o.Get<Topic>(slug, null)).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

            var indexPage = AsyncTestHelper.Resolve(_controller.Index("healthy-living")) as ViewResult;
            var viewModel = indexPage.ViewData.Model as TopicViewModel;
            var result = viewModel.Topic;

            result.Alerts.Should().HaveCount(1);
            result.Alerts.First().Title.Should().Be("title");
            result.Alerts.First().SubHeading.Should().Be("subheading");
            result.Alerts.First().Body.Should().Be("<p>body</p>\n");
            result.Alerts.First().Severity.Should().Be(Severity.Warning);
            result.EmailAlerts.Should().Be(true);
            result.EmailAlertsTopicId.Should().Be("test-id");
        }
    }
}