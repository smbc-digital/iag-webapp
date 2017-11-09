using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using StockportWebapp.Config;
using StockportWebapp.ProcessedModels;
using StockportWebapp.ViewModels;
using StockportWebapp.Services;
using StockportWebappTests.Builders;

namespace StockportWebappTests.Unit.Controllers
{
    public class HomeControllerTest
    {
        private readonly HomeController _controller;
        private readonly Mock<IApplicationConfiguration> _config = new Mock<IApplicationConfiguration>();
        private readonly Mock<INewsService> _newsService = new Mock<INewsService>();
        private readonly Mock<IEventsService> _eventsService = new Mock<IEventsService>();
        private readonly Mock<IHomepageService> _homepageService = new Mock<IHomepageService>();
        private readonly Mock<IStockportApiEventsService> _stockportApiService = new Mock<IStockportApiEventsService>();
        private const string EmailAlertsUrl = "email_alerts_url=";
        private const string _businessId = "aBusinessId";

        public HomeControllerTest()
        {
            _controller = new HomeController(new BusinessId(_businessId), _config.Object, _newsService.Object, _eventsService.Object, _homepageService.Object, _stockportApiService.Object);
        }

        [Fact]
        public void GivenNavigateToIndexReturnsHomeViewWithFeaturedTopicsAndTasks()
        {
            // TODO: Tidy up...
            // Arrange
            var popularSearchTerms = new List<string> {"popular", "search", "terms"};
            var featuredTasks = new List<SubItem>
            {
                new SubItem("slug featuredTasks", "featured Tasks","teaser Fetured Tasks", "fa fa-home", "", "image", new List<SubItem>())
            };
            var alerts = new List<Alert> {new Alert("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty)};
            var carouselContents = new List<CarouselContent>
            {
                new CarouselContent("Carousel Title", "Carousel Teaser", "Carousel Image", "Carousel Url")
            };
            var featuredTopics = new List<SubItem>
            {
                new SubItem("Council Tax", "council-tax", "How to pay, discounts", "", "", "", new List<SubItem>())
            };
            var homePageContent = new ProcessedHomepage(popularSearchTerms, "heading", "summary", featuredTasks, featuredTopics,  alerts, carouselContents, "image.jpg", new List<News>(),  "homepage text", null, "");
            var sunrise = new DateTime(2015, 9, 10);
            var sunset = new DateTime(2015, 9, 20);
            var newsContent = new News("title", "slug", "teaser", "image", "thumbnail", "body", new List<Crumb>(), sunrise, sunset, new List<Alert>(), new List<string>(), new List<Document>());
            var eventsContent = new Event { Title = "title", EventDate = new DateTime(2017, 01, 01), Featured = true };

            // Mock
            _homepageService.Setup(o => o.GetHomepage()).ReturnsAsync(homePageContent);
            _newsService.Setup(o => o.GetLatestNewsItem()).ReturnsAsync(newsContent);
            _eventsService.Setup(o => o.GetLatestEventsItem()).ReturnsAsync(eventsContent);

            // Act
            var indexPage = AsyncTestHelper.Resolve(_controller.Index()) as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            page.HomepageContent.PopularSearchTerms.Should().NotBeNullOrEmpty();
            page.HomepageContent.PopularSearchTerms.First().Should().Be("popular");
            page.HomepageContent.PopularSearchTerms.Last().Should().Be("terms");
            page.HomepageContent.FeaturedTasksHeading.Should().Be("heading");
            page.HomepageContent.FeaturedTasksSummary.Should().Be("summary");
            page.HomepageContent.FeaturedTasks.Should().HaveCount(1);
            page.HomepageContent.FeaturedTasks.First().Title.Should().Be("featured Tasks");
            page.HomepageContent.FeaturedTasks.First().NavigationLink.Should().Contain("slug featuredTasks");
            page.HomepageContent.FeaturedTasks.First().Teaser.Should().Be("teaser Fetured Tasks");
            page.HomepageContent.FeaturedTasks.First().Icon.Should().Be("fa fa-home");
            page.HomepageContent.FeaturedTasks.First().Image.Should().Be("image");
            page.HomepageContent.FeaturedTopics.First().Title.Should().Be("council-tax");
            page.HomepageContent.FeaturedTopics.First().Teaser.Should().Be("How to pay, discounts");
            page.HomepageContent.FeaturedTopics.First().SubItems.Should().BeEmpty();
            page.HomepageContent.Alerts.Should().HaveCount(1);
            page.HomepageContent.Alerts.First().Title.Should().Be("title");
            page.HomepageContent.Alerts.First().SubHeading.Should().Be("subHeading");
            page.HomepageContent.Alerts.First().Body.Should().Contain("body");
            page.HomepageContent.Alerts.First().Severity.Should().Be(Severity.Information);
            page.HomepageContent.CarouselContents.Should().HaveCount(1);
            page.HomepageContent.CarouselContents.First().Title.Should().Be("Carousel Title");
            page.HomepageContent.CarouselContents.First().Teaser.Should().Be("Carousel Teaser");
            page.HomepageContent.CarouselContents.First().Image.Should().Be("Carousel Image");
            page.HomepageContent.CarouselContents.First().URL.Should().Be("Carousel Url");
            page.HomepageContent.BackgroundImage.Should().Be("image.jpg");
            page.HomepageContent.FreeText.Should().Be("homepage text");

            page.FeaturedNews.Title.Should().Be("title");
            page.FeaturedNews.Slug.Should().Be("slug");
            page.FeaturedNews.SunriseDate.Should().Be(sunrise);
            page.FeaturedNews.SunsetDate.Should().Be(sunset);

            page.FeaturedEvent.Title.Should().Be("title");
            page.FeaturedEvent.EventDate.Should().Be(new DateTime(2017, 01, 01));
        }

        [Fact]
        public async void ShouldReturnHomepageWhenThereAreNoEvents()
        {
            // Arrange
            var popularSearchTerms = new List<string> { "popular", "search", "terms" };
            var featuredTasks = new List<SubItem>
            {
                new SubItem("slug featuredTasks", "featured Tasks","teaser Fetured Tasks", "fa fa-home", "", "image", new List<SubItem>())
            };
            var alerts = new List<Alert> { new Alert("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty) };
            var carouselContents = new List<CarouselContent>
            {
                new CarouselContent("Carousel Title", "Carousel Teaser", "Carousel Image", "Carousel Url")
            };
            var featuredTopics = new List<SubItem>
            {
                new SubItem("Council Tax", "council-tax", "", "How to pay, discounts", "", "", new List<SubItem>())
            };
            var homePageContent = new ProcessedHomepage(popularSearchTerms, "heading", "summary", featuredTasks, featuredTopics, alerts, carouselContents, "image.jpg", new List<News>(), "homepage text", null, "");
            var sunrise = new DateTime(2015, 9, 10);
            var sunset = new DateTime(2015, 9, 20);
            var newsContent = new News("title", "slug", "teaser", "image", "thumbnail", "body", new List<Crumb>(), sunrise, sunset, new List<Alert>(), new List<string>(), new List<Document>());

            // Mock
            _homepageService.Setup(o => o.GetHomepage()).ReturnsAsync(homePageContent);
            _newsService.Setup(o => o.GetLatestNewsItem()).ReturnsAsync(newsContent);
            _eventsService.Setup(o => o.GetLatestEventsItem()).ReturnsAsync((Event)null);

            // Act
            var indexPage =  await _controller.Index() as ViewResult;
            var response = indexPage.ViewData.Model as HomepageViewModel;

            // aSSERT
            response.HomepageContent.Should().NotBeNull();
            response.FeaturedEvent.Should().BeNull();
        }

        [Fact]
        public void GetsA404NotFoundOnTheHomepage()
        {
            _homepageService.Setup(o => o.GetHomepage()).ReturnsAsync((ProcessedHomepage)null);

            var response = AsyncTestHelper.Resolve(_controller.Index()) as NotFoundResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public void ShouldRedirectEmailSubscribeToConfiguredUrlWithEmailAddress()
        {
            const string emailAddress = "me@email.com";
            var appSetting = AppSetting.GetAppSetting(EmailAlertsUrl);
            _config.Setup(o => o.GetEmailAlertsUrl(_businessId)).Returns(appSetting);

            var response = AsyncTestHelper.Resolve(_controller.EmailSubscribe(emailAddress, ""));

            response.Should().BeOfType<RedirectResult>();
            var redirect = response as RedirectResult;
            _config.Verify(o => o.GetEmailAlertsUrl(_businessId), Times.Once);
            redirect.Url.Should().Be(EmailAlertsUrl + emailAddress + "&topic_id=");
        }

        [Fact]
        public void ShouldRedirectToApplicationErrorIfEmailConfigurationIsMissing()
        {
            const string emailAddress = "me@email.com";
            var appSetting = AppSetting.GetAppSetting(null);
            _config.Setup(o => o.GetEmailAlertsUrl(_businessId)).Returns(appSetting);

            var response = AsyncTestHelper.Resolve(_controller.EmailSubscribe(emailAddress, "")) as StatusCodeResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public void ShouldReturnEventsFromTheApi()
        {
            // Arrange
            var homePageContent = new ProcessedHomepage(new List<string>(), "heading", "summary", new List<SubItem>(), new List<SubItem>(), new List<Alert>(), new List<CarouselContent>(), "image.jpg", new List<News>(), "homepage text", null, "unittest");

            // Mock
            _homepageService.Setup(o => o.GetHomepage()).ReturnsAsync(homePageContent);
            _stockportApiService.Setup(o => o.GetEventsByCategory("unittest")).ReturnsAsync(new List<Event> { new EventBuilder().Build() });

            // Act
            var indexPage = AsyncTestHelper.Resolve(_controller.Index()) as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            page.EventsFromApi.Should().NotBeNull();
            page.EventsFromApi.Count.Should().Be(1);
        }

        [Fact]
        public void ShouldReturnEmptyEventsIfCategoryIsNotSet()
        {
            // Arrange
            var homePageContent = new ProcessedHomepage(new List<string>(), "heading", "summary", new List<SubItem>(), new List<SubItem>(), new List<Alert>(), new List<CarouselContent>(), "image.jpg", new List<News>(), "homepage text", null, "");

            // Mock
            _homepageService.Setup(o => o.GetHomepage()).ReturnsAsync(homePageContent);

            // Act
            var indexPage = AsyncTestHelper.Resolve(_controller.Index()) as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            page.EventsFromApi.Should().NotBeNull();
            page.EventsFromApi.Count.Should().Be(0);
        }
    }
}