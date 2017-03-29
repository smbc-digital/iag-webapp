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
using StockportWebapp.Repositories;
using System;
using StockportWebapp.Config;

namespace StockportWebappTests.Unit.Controllers
{
    public class HomeControllerTest
    {
        private readonly HomeController _controller;
        private readonly Mock<IRepository> _repository;
        private readonly Mock<IProcessedContentRepository> _processedContentRepository;
        private readonly Mock<IApplicationConfiguration> _config;
        private const string EmailAlertsUrl = "email_alerts_url=";

        private readonly string _businessId = "aBusinessId";

        public HomeControllerTest()
        {
            _config = new Mock<IApplicationConfiguration>();
            _repository = new Mock<IRepository>();
            _processedContentRepository = new Mock<IProcessedContentRepository>();
            _controller = new HomeController(_repository.Object, _processedContentRepository.Object, new BusinessId(_businessId), _config.Object);
        }

        [Fact]
        public void GivenNavigateToIndexReturnsHomeViewWithFeaturedTopicsAndTasks()
        {
            var popularSearchTerms = new List<string> {"popular", "search", "terms"};
            var featuredTasks = new List<SubItem>
            {
                new SubItem("slug featuredTasks", "featured Tasks","teaser Fetured Tasks", "fa fa-home", "", "image", new List<SubItem>())
            };
            var alerts = new List<Alert> {new Alert("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc))};
            var carouselContents = new List<CarouselContent>
            {
                new CarouselContent("Carousel Title", "Carousel Teaser", "Carousel Image", "Carousel Url")
            };
            var featuredTopics = new List<Topic>
            {
                new Topic("Council Tax", "council-tax", "", "How to pay, discounts", "", "", "", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(), new List<Crumb>(), new List<Alert>(), true, "test-id")
            };
            var homePageContent = new ProcessedHomepage(popularSearchTerms, "heading", "summary", featuredTasks, featuredTopics,  alerts, carouselContents, "image.jpg", new List<News>(),  "homepage text");
            var sunrise = new DateTime(2015, 9, 10);
            var sunset = new DateTime(2015, 9, 20);
            var newsContent = new List<News>
            {
                new News("title", "slug", "teaser", "image", "thumbnail", "body", new List<Crumb>(), sunrise, sunset, new List<Alert>(), new List<string>(),new List<Document>())
            };

            _processedContentRepository.Setup(o => o.Get<Homepage>(It.IsAny<string>(), null)).ReturnsAsync(new HttpResponse(200, homePageContent, string.Empty));

            _repository.Setup(o => o.Get<List<News>>("2", null))
                .ReturnsAsync(new HttpResponse(200, newsContent, string.Empty));

            var eventsContent = new List<Event>
            {
                new Event { Title = "title", EventDate = new DateTime(2017, 01, 01), Featured = true}
            };

            var eventCalendar = new EventCalendar(eventsContent, null);

            _repository.Setup(o => o.GetLatestOrderByFeatured<EventCalendar>(2)).ReturnsAsync(new HttpResponse(200, eventCalendar, string.Empty));

            var indexPage = AsyncTestHelper.Resolve(_controller.Index()) as ViewResult;
            var page = indexPage.ViewData.Model as ProcessedHomepage;

            page.PopularSearchTerms.Should().NotBeNullOrEmpty();
            page.PopularSearchTerms.First().Should().Be("popular");
            page.PopularSearchTerms.Last().Should().Be("terms");

            page.FeaturedTasksHeading.Should().Be("heading");
            page.FeaturedTasksSummary.Should().Be("summary");

            page.FeaturedTasks.Should().HaveCount(1);
            page.FeaturedTasks.First().Title.Should().Be("featured Tasks");
            page.FeaturedTasks.First().NavigationLink.Should().Contain("slug featuredTasks");
            page.FeaturedTasks.First().Teaser.Should().Be("teaser Fetured Tasks");
            page.FeaturedTasks.First().Icon.Should().Be("fa fa-home");
            page.FeaturedTasks.First().Image.Should().Be("image");

            page.FeaturedTopics.First().Slug.Should().Be("council-tax");
            page.FeaturedTopics.First().Name.Should().Be("Council Tax");
            page.FeaturedTopics.First().Teaser.Should().Be("How to pay, discounts");
            page.FeaturedTopics.First().SubItems.Should().BeEmpty();

            page.Alerts.Should().HaveCount(1);
            page.Alerts.First().Title.Should().Be("title");
            page.Alerts.First().SubHeading.Should().Be("subHeading");
            page.Alerts.First().Body.Should().Contain("body");
            page.Alerts.First().Severity.Should().Be(Severity.Information);

            page.CarouselContents.Should().HaveCount(1);
            page.CarouselContents.First().Title.Should().Be("Carousel Title");
            page.CarouselContents.First().Teaser.Should().Be("Carousel Teaser");
            page.CarouselContents.First().Image.Should().Be("Carousel Image");
            page.CarouselContents.First().URL.Should().Be("Carousel Url");

            page.BackgroundImage.Should().Be("image.jpg");

            var latestNews = page.GetLatestNews();
            latestNews.Should().HaveCount(1);
            latestNews.First().Title.Should().Be("title");
            latestNews.First().Slug.Should().Be("slug");
            latestNews.First().SunriseDate.Should().Be(sunrise);
            latestNews.First().SunsetDate.Should().Be(sunset);

            page.FreeText.Should().Be("homepage text");

            var latestEvents = page.GetLatestEvents();
            latestEvents.Should().HaveCount(1);
            latestEvents[0].Title.Should().Be("title");
            latestEvents[0].EventDate.Should().Be(new DateTime(2017, 01, 01));
        }

        [Fact]
        public void ShouldReturnHomepageWhenThereAreNoEvents()
        {
            var popularSearchTerms = new List<string> { "popular", "search", "terms" };
            var featuredTasks = new List<SubItem>
            {
                new SubItem("slug featuredTasks", "featured Tasks","teaser Fetured Tasks", "fa fa-home", "", "image", new List<SubItem>())
            };
            var alerts = new List<Alert> { new Alert("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc)) };
            var carouselContents = new List<CarouselContent>
            {
                new CarouselContent("Carousel Title", "Carousel Teaser", "Carousel Image", "Carousel Url")
            };
            var featuredTopics = new List<Topic>
            {
                new Topic("Council Tax", "council-tax", "", "How to pay, discounts", "", "", "", new List<SubItem>(), new List<SubItem>(), new List<SubItem>(), new List<Crumb>(), new List<Alert>(), true, "test-id")
            };
            var homePageContent = new ProcessedHomepage(popularSearchTerms, "heading", "summary", featuredTasks, featuredTopics, alerts, carouselContents, "image.jpg", new List<News>(), "homepage text");
            var sunrise = new DateTime(2015, 9, 10);
            var sunset = new DateTime(2015, 9, 20);
            var newsContent = new List<News>
            {
                new News("title", "slug", "teaser", "image", "thumbnail", "body", new List<Crumb>(), sunrise, sunset, new List<Alert>(), new List<string>(),new List<Document>())
            };

            _processedContentRepository.Setup(o => o.Get<Homepage>(It.IsAny<string>(), null)).ReturnsAsync(new HttpResponse(200, homePageContent, string.Empty));

            _repository.Setup(o => o.Get<List<News>>("2", null))
                .ReturnsAsync(new HttpResponse(200, newsContent, string.Empty));

            _repository.Setup(o => o.GetLatestOrderByFeatured<EventCalendar>(2)).ReturnsAsync(new HttpResponse(404, null, "no events found"));

            var indexPage = AsyncTestHelper.Resolve(_controller.Index()) as ViewResult;
            var page = indexPage.ViewData.Model as ProcessedHomepage;

            page.PopularSearchTerms.Should().NotBeNullOrEmpty();
            page.PopularSearchTerms.First().Should().Be("popular");
            page.PopularSearchTerms.Last().Should().Be("terms");

            page.FeaturedTasksHeading.Should().Be("heading");
            page.FeaturedTasksSummary.Should().Be("summary");

            page.FeaturedTasks.Should().HaveCount(1);
            page.FeaturedTasks.First().Title.Should().Be("featured Tasks");
            page.FeaturedTasks.First().NavigationLink.Should().Contain("slug featuredTasks");
            page.FeaturedTasks.First().Teaser.Should().Be("teaser Fetured Tasks");
            page.FeaturedTasks.First().Icon.Should().Be("fa fa-home");
            page.FeaturedTasks.First().Image.Should().Be("image");

            page.FeaturedTopics.First().Slug.Should().Be("council-tax");
            page.FeaturedTopics.First().Name.Should().Be("Council Tax");
            page.FeaturedTopics.First().Teaser.Should().Be("How to pay, discounts");
            page.FeaturedTopics.First().SubItems.Should().BeEmpty();

            page.Alerts.Should().HaveCount(1);
            page.Alerts.First().Title.Should().Be("title");
            page.Alerts.First().SubHeading.Should().Be("subHeading");
            page.Alerts.First().Body.Should().Contain("body");
            page.Alerts.First().Severity.Should().Be(Severity.Information);

            page.CarouselContents.Should().HaveCount(1);
            page.CarouselContents.First().Title.Should().Be("Carousel Title");
            page.CarouselContents.First().Teaser.Should().Be("Carousel Teaser");
            page.CarouselContents.First().Image.Should().Be("Carousel Image");
            page.CarouselContents.First().URL.Should().Be("Carousel Url");

            page.BackgroundImage.Should().Be("image.jpg");

            var latestNews = page.GetLatestNews();
            latestNews.Should().HaveCount(1);
            latestNews.First().Title.Should().Be("title");
            latestNews.First().Slug.Should().Be("slug");
            latestNews.First().SunriseDate.Should().Be(sunrise);
            latestNews.First().SunsetDate.Should().Be(sunset);

            page.FreeText.Should().Be("homepage text");
        }

        [Fact]
        public void GetsA404NotFoundOnTheHomepage()
        {
            _processedContentRepository.Setup(o => o.Get<Homepage>(It.IsAny<string>(), null)).ReturnsAsync(new HttpResponse(404, null, "Not Found"));

            var response = AsyncTestHelper.Resolve(_controller.Index()) as HttpResponse;

            response.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
        }

        [Fact]
        public void ShouldRedirectEmailSubscribeToConfiguredUrlWithEmailAddress()
        {
            const string emailAddress = "me@email.com";
            var appSetting = AppSetting.GetAppSetting(EmailAlertsUrl);
            _config.Setup(o => o.GetEmailAlertsUrl(_businessId)).Returns(appSetting);

            var response = AsyncTestHelper.Resolve(_controller.EmailSubscribe(emailAddress));

            response.Should().BeOfType<RedirectResult>();
            var redirect = response as RedirectResult;
            _config.Verify(o => o.GetEmailAlertsUrl(_businessId), Times.Once);
            redirect.Url.Should().Be(EmailAlertsUrl + emailAddress);
        }

        [Fact]
        public void ShouldRedirectToApplicationErrorIfEmailConfigurationIsMissing()
        {
            const string emailAddress = "me@email.com";
            var appSetting = AppSetting.GetAppSetting(null);
            _config.Setup(o => o.GetEmailAlertsUrl(_businessId)).Returns(appSetting);

            var response = AsyncTestHelper.Resolve(_controller.EmailSubscribe(emailAddress)) as StatusCodeResult;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}