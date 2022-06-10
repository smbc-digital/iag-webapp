using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Services;
using StockportWebapp.ViewModels;
using StockportWebappTests_Unit.Builders;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class HomeControllerTest
    {
        private readonly HomeController _controller;
        private readonly Mock<IApplicationConfiguration> _config = new();
        private readonly Mock<INewsService> _newsService = new();
        private readonly Mock<IEventsService> _eventsService = new();
        private readonly Mock<IHomepageService> _homepageService = new();
        private readonly Mock<IStockportApiEventsService> _stockportApiService = new();
        private const string EmailAlertsUrl = "email_alerts_url=";
        private const string BusinessId = "aBusinessId";

        #region Models

        private readonly List<string> _popularSearchTerms = new() {"popular", "search", "terms"};
        private readonly List<SubItem> _featuredTasks = new()
        {
            new SubItem("slug featuredTasks", "featured Tasks","teaser Featured Tasks", "fa fa-home", "", "image", new List<SubItem>())
        };
        private readonly List<SubItem> _featuredTopics = new()
        {
            new SubItem("Council Tax", "council-tax", "How to pay, discounts", "", "", "", new List<SubItem>())
        };
        private readonly List<Alert> _alerts = new()
        {
            new Alert("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false)
        };
        private readonly List<CarouselContent> _carouselContents = new()
        {
            new CarouselContent("Carousel Title", "Carousel Teaser", "Carousel Image", "Carousel Url")
        };

        private readonly CarouselContent _campaignBanner = new CarouselContent("Campaign Title", "Campaign Teaser", "Campaign Image", "Campaign Url");

        private readonly DateTime _sunrise = new DateTime(2015, 9, 10);
        private readonly DateTime _sunset = new DateTime(2015, 9, 20);

        private readonly News _newsContent = new("title", "slug", "teaser", "purpose", "image", "thumbnail", "body",
            new List<Crumb>(), new DateTime(2015, 9, 10), new DateTime(2015, 9, 20), new List<Alert>(),
            new List<string>(), new List<Document>());

        private readonly Event _eventsContent = new()
        {
            Title = "title",
            EventDate = new DateTime(2017, 01, 01),
            Featured = true
        };

        #endregion

        public HomeControllerTest()
        {
            var homePageContent = new ProcessedHomepage(_popularSearchTerms, "heading", "summary", _featuredTasks, _featuredTopics, _alerts, _carouselContents, "image.jpg", new List<News>(), "homepage text", null, "", "meta description", _campaignBanner);

            _homepageService
                .Setup(o => o.GetHomepage())
                .ReturnsAsync(homePageContent);

            _newsService
                .Setup(o => o.GetLatestNewsItem())
                .ReturnsAsync(_newsContent);

            var appSetting = AppSetting.GetAppSetting(EmailAlertsUrl);
            _config.Setup(o => o.GetEmailAlertsUrl(BusinessId)).Returns(appSetting);

            _controller = new HomeController(new BusinessId(BusinessId), _config.Object, _newsService.Object, _eventsService.Object, _homepageService.Object, _stockportApiService.Object);
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithPopularSearchTerms()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestFeaturedEventItem())
                .ReturnsAsync(_eventsContent);

            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.NotNull(page.HomepageContent.PopularSearchTerms);
            Assert.NotEmpty(page.HomepageContent.PopularSearchTerms);
            Assert.Equal("popular", page.HomepageContent.PopularSearchTerms.First());
            Assert.Equal("terms", page.HomepageContent.PopularSearchTerms.Last());
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithFeaturedTasks()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestFeaturedEventItem())
                .ReturnsAsync(_eventsContent);

            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.Equal("heading", page.HomepageContent.FeaturedTasksHeading);
            Assert.Equal("summary", page.HomepageContent.FeaturedTasksSummary);
            page.HomepageContent.FeaturedTasks.Should().HaveCount(1);
            Assert.Single(page.HomepageContent.FeaturedTasks);
            var featuredTask = page.HomepageContent.FeaturedTasks.First();
            Assert.Equal("featured Tasks", featuredTask.Title);
            Assert.Contains("slug featuredTasks", featuredTask.NavigationLink);
            Assert.Equal("teaser Featured Tasks", featuredTask.Teaser);
            Assert.Equal("fa fa-home", featuredTask.Icon);
            Assert.Equal("image", featuredTask.Image);
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithFeaturedTopics()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestFeaturedEventItem())
                .ReturnsAsync(_eventsContent);

            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.Single(page.HomepageContent.FeaturedTopics);
            var featuredTopic = page.HomepageContent.FeaturedTopics.First();
            Assert.Equal("council-tax", featuredTopic.Title);
            Assert.Equal("How to pay, discounts", featuredTopic.Teaser);
            Assert.Empty(featuredTopic.SubItems);
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithAlerts()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestFeaturedEventItem())
                .ReturnsAsync(_eventsContent);

            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.Single(page.HomepageContent.Alerts);
            var alert = page.HomepageContent.Alerts.First();
            Assert.Equal("title", alert.Title);
            Assert.Equal("subHeading", alert.SubHeading);
            Assert.Contains("body", alert.Body);
            Assert.Equal(Severity.Information, alert.Severity);
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithCarouselContents()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestFeaturedEventItem())
                .ReturnsAsync(_eventsContent);

            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.Single(page.HomepageContent.CarouselContents);
            var carouselContent = page.HomepageContent.CarouselContents.First();
            Assert.Equal("Carousel Title", carouselContent.Title);
            Assert.Equal("Carousel Teaser", carouselContent.Teaser);
            Assert.Equal("Carousel Image", carouselContent.Image);
            Assert.Equal("Carousel Url", carouselContent.URL);
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithBackground()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestFeaturedEventItem())
                .ReturnsAsync(_eventsContent);

            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.Equal("image.jpg", page.HomepageContent.BackgroundImage);
            Assert.Equal("homepage text", page.HomepageContent.FreeText);
            Assert.Equal("meta description", page.HomepageContent.MetaDescription);
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithFeaturedNews()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestFeaturedEventItem())
                .ReturnsAsync(_eventsContent);

            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.Equal("title", page.FeaturedNews.Title);
            Assert.Equal("slug", page.FeaturedNews.Slug);
            Assert.Equal(_sunrise, page.FeaturedNews.SunriseDate);
            Assert.Equal(_sunset, page.FeaturedNews.SunsetDate);
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithFeaturedEvent()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestFeaturedEventItem())
                .ReturnsAsync(_eventsContent);

            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.Equal("title", page.FeaturedEvent.Title);
            Assert.Equal(new DateTime(2017, 01, 01), page.FeaturedEvent.EventDate);
        }

        [Fact]
        public async Task Index_Should_ReturnHomeView_WithCampaignBanner()
        {
            // Act
            var indexPage = await _controller.Index() as ViewResult;
            var page = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.Equal("Campaign Title", page.HomepageContent.CampaignBanner.Title);
            Assert.Equal("Campaign Teaser", page.HomepageContent.CampaignBanner.Teaser);
            Assert.Equal("Campaign Image", page.HomepageContent.CampaignBanner.Image);
            Assert.Equal("Campaign Url", page.HomepageContent.CampaignBanner.URL);
        }

        [Fact]
        public async void Index_Should_ReturnHomepage_WhenThereAreNoEvents()
        {
            // Arrange
            _eventsService
                .Setup(o => o.GetLatestEventsItem())
                .ReturnsAsync((Event)null);

            // Act
            var indexPage =  await _controller.Index() as ViewResult;
            var response = indexPage.ViewData.Model as HomepageViewModel;

            // Assert
            Assert.NotNull(response.HomepageContent);
            Assert.Null(response.FeaturedEvent);
        }

        [Fact]
        public async Task Index_Should_ReturnNotFound_IfHomepageIsNull()
        {
            // Arrange
            _homepageService.Setup(o => o.GetHomepage()).ReturnsAsync((ProcessedHomepage)null);

            // Act
            var response = await _controller.Index() as NotFoundResult;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Index_Should_CallApiService_IfEventsCategoryNotEmpty()
        {
            // Arrange
            var homePageContent = new ProcessedHomepage(new List<string>(), "heading", "summary", new List<SubItem>(), new List<SubItem>(), new List<Alert>(), new List<CarouselContent>(), "image.jpg", new List<News>(), "homepage text", null, "unittest", "meta description", _campaignBanner);
            
            _homepageService.Setup(o => o.GetHomepage()).ReturnsAsync(homePageContent);
            _stockportApiService.Setup(o => o.GetEventsByCategory("unittest", true)).ReturnsAsync(new List<Event> { new EventBuilder().Build() });

            // Act
            await _controller.Index();

            // Assert
            _stockportApiService.Verify(_ => _.GetEventsByCategory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task Index_ShouldNot_CallApiService_IfCategoryIsNotEmpty()
        {
            // Act
            await _controller.Index();

            // Assert
            _stockportApiService.Verify(_ => _.GetEventsByCategory(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task EmailSubscribe_Should_RedirectToConfiguredUrlWithEmailAddress()
        {
            // Arrange
            const string emailAddress = "me@email.com";

            // Act
            var result = await _controller.EmailSubscribe(emailAddress, "") as RedirectResult;

            // Assert
            Assert.IsType<RedirectResult>(result);
            _config.Verify(o => o.GetEmailAlertsUrl(BusinessId), Times.Once);
            Assert.Equal($"{EmailAlertsUrl}{emailAddress}&topic_id=", result.Url);
        }

        [Fact]
        public async Task EmailSubscribe_Should_ReturnNotFound_IfEmailConfigurationIsMissing()
        {
            // Arrange
            const string emailAddress = "me@email.com";
            var appSetting = AppSetting.GetAppSetting(null);
            _config
                .Setup(o => o.GetEmailAlertsUrl(BusinessId))
                .Returns(appSetting);

            // Act
            var response = await _controller.EmailSubscribe(emailAddress, "") as StatusCodeResult;;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
        }

        
    }
}