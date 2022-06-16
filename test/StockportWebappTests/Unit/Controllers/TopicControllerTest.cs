using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class TopicControllerTest
    {
        private readonly TopicController _controller;
        private readonly Mock<IRepository> _repository;
        private const string BusinessId = "businessId";
        private readonly EventBanner _eventBanner;

        public TopicControllerTest()
        {
            var config = new Mock<IApplicationConfiguration>();

            config.Setup(o => o.GetEmailAlertsNewSubscriberUrl(BusinessId)).Returns(AppSetting.GetAppSetting("email-alerts-url"));

            _repository = new Mock<IRepository>();
            _controller = new TopicController(_repository.Object, config.Object, new BusinessId(BusinessId));
            _eventBanner = new EventBanner("title", "teaser", "icon", "link");
        }
        
        public SubItem CreateASubItem(int i)
        {
            return new SubItem("sub-topic" + i, "Title" + i, "Teaser", "Icon", "topic", "image", new List<SubItem>());
        }
    
        [Fact]
        public async Task GivenNavigateToTopicReturnsTopicWithExpectedProperties()
        {           
            var subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

            var topic = new Topic("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", subItems, null, null,
                new List<Crumb>(), new List<Alert>(), true, "test-id", _eventBanner, "expandingLinkText",
                new List<ExpandingLinkBox>{ new ExpandingLinkBox("title", subItems) }, string.Empty, string.Empty, true);

            const string slug = "healthy-living";
            _repository.Setup(o => o.Get<Topic>(slug, null)).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

            var indexPage = await _controller.Index(slug) as ViewResult;
            var viewModel = indexPage.ViewData.Model as TopicViewModel;
            var result = viewModel.Topic;

            Assert.Equal("Name", result.Name);
            Assert.Equal("slug", result.Slug);
            Assert.Equal("<p>Summary</p>\n", result.Summary);
            Assert.Equal("Teaser", result.Teaser);
            Assert.Equal("Icon", result.Icon);
            Assert.Equal("Image", result.BackgroundImage);
            Assert.Equal("Image", result.Image);
            Assert.True(result.DisplayContactUs);
            result.EmailAlerts.Should().Be(true);
            result.EmailAlertsTopicId.Should().Be("test-id");
            result.EventBanner.Title.Should().Be(_eventBanner.Title);
            result.EventBanner.Teaser.Should().Be(_eventBanner.Teaser);
            result.MetaDescription.Should().Be("metaDescription");
            result.EventBanner.Icon.Should().Be(_eventBanner.Icon);
            result.EventBanner.Link.Should().Be(_eventBanner.Link);
            result.ExpandingLinkTitle.Should().Be("expandingLinkText");
            result.ExpandingLinkBoxes.First().Title.Should().Be("title");
            result.ExpandingLinkBoxes.First().Links[0].Type.Should().Be("topic");
        }

        [Fact]
        public async Task GivenNavigateToTopicReturnsListOfSubItemsByTopic()
        {
            var subItems = Enumerable.Range(0, 1).Select(CreateASubItem).ToList();

            var topic = new Topic("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", subItems, null, null,
              new List<Crumb>(), new List<Alert>(), true, "test-id", _eventBanner, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true);

            const string slug = "healthy-living";
            _repository.Setup(o => o.Get<Topic>(slug, null)).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

            var indexPage = await _controller.Index("healthy-living") as ViewResult;
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
        public async Task GivesNotFoundOnRequestForNonExistentTopic()
        {
            const string nonExistentTopic = "doesnt-exist";

            _repository.Setup(o => o.Get<Topic>(nonExistentTopic, null)).ReturnsAsync(new HttpResponse(404, null, "No topic found for 'doesnt-exist'"));

            var result = await _controller.Index(nonExistentTopic) as StatusCodeResult;

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetsAlertsForTopic()
        {
            var alerts = new List<Alert>
            {
                new Alert("title", "subheading", "body", Severity.Warning, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),String.Empty, false)
            };

            var topic = new Topic("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", null, null, null,
               new List<Crumb>(), alerts, true, "test-id", _eventBanner, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true);

            const string slug = "healthy-living";
            _repository.Setup(o => o.Get<Topic>(slug, null)).ReturnsAsync(new HttpResponse(200, topic, string.Empty));

            var indexPage = await _controller.Index("healthy-living") as ViewResult;
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