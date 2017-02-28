using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using Markdig.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.RSS;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;
using StockportWebapp.Utils;

namespace StockportWebappTests.Unit.Controllers
{
    public class NewsControllerTest
    {
        private readonly NewsController _controller;
        private readonly Mock<IRepository> _repository = new Mock<IRepository>();
        private readonly Mock<IProcessedContentRepository> _processedContentRepository = new Mock<IProcessedContentRepository>();
        private readonly Mock<IRssFeedFactory> _mockRssFeedFactory;
        private readonly Mock<ILogger<NewsController>> _logger;
        private readonly Mock<IApplicationConfiguration> _config;
        private const string BusinessId = "businessId";
        private const string EmailAlertsTopicId = "test-id";
        private const bool EmailAlertsOn = true;
        private readonly Mock<IFilteredUrl> _filteredUrl;
        private readonly FeatureToggles _featureToggles = new FeatureToggles();

        private static readonly News NewsItemWithImages = new News("Another news article",
            "another-news-article",
            "This is another news article",
            "image.jpg",
            "thumbnail.jpg",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
            new List<Crumb>(), new DateTime(2015, 9, 10), new DateTime(2015, 9, 20), new List<Alert>(), new List<string>(), new List<Document>());

        private static readonly News NewsItemWithoutImages = new News("News 26th Aug",
            "news-26th-aug",
            "test",
            "",
            "",
            "test",
            new List<Crumb>(), new DateTime(2015, 9, 10), new DateTime(2015, 9, 20), new List<Alert>(), new List<string>(), new List<Document>());

        private readonly ProcessedNews _processedNewsArticle = new ProcessedNews("Another news article",
            "another-news-article",
            "This is another news article",
            "image.jpg",
            "thumbnail.jpg",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
            new List<Crumb>(), new DateTime(2015, 9, 10), new DateTime(2015, 9, 20), new List<Alert>(), new List<string> { "Events", "Bramall Hall" });

        private readonly List<News> _listOfNewsItems = new List<News> { NewsItemWithoutImages, NewsItemWithImages };

        private readonly Newsroom _newsRoom;

        public NewsControllerTest()
        {
            _newsRoom = new Newsroom(_listOfNewsItems, new OrderedList<Alert>(), EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());

            // setup responses (with mock data)
            var responseListing = new HttpResponse(200, _newsRoom, "");
            var responseDetail = new HttpResponse(200, _processedNewsArticle, "");
            var response404 = new HttpResponse(404, null, "not found");

            // setup mocks
            _repository.Setup(o => o.Get<Newsroom>(It.IsAny<string>(), It.Is<List<Query>>(l => l.Count == 0)))
                .ReturnsAsync(responseListing);

            _repository.Setup(o => o.Get<List<News>>("7", null))
                .ReturnsAsync(HttpResponse.Successful(200, _listOfNewsItems));

            _processedContentRepository.Setup(o => o.Get<News>("another-news-article", null))
                .ReturnsAsync(responseDetail);
            _processedContentRepository.Setup(o => o.Get<News>("404-news", null))
                .ReturnsAsync(response404);

            _logger = new Mock<ILogger<NewsController>>();

            _mockRssFeedFactory = new Mock<IRssFeedFactory>();
            _mockRssFeedFactory.Setup(o => o.BuildRssFeed(It.IsAny<List<News>>(), It.IsAny<string>(), It.IsAny<string>())).Returns("rss fun");

            _config = new Mock<IApplicationConfiguration>();
            _filteredUrl = new Mock<IFilteredUrl>();

            _config.Setup(o => o.GetRssEmail(BusinessId)).Returns(AppSetting.GetAppSetting("rss-email"));
            _config.Setup(o => o.GetEmailAlertsNewSubscriberUrl(BusinessId)).Returns(AppSetting.GetAppSetting("email-alerts-url"));

            _controller = new NewsController(
                _repository.Object,
                _processedContentRepository.Object,
                _mockRssFeedFactory.Object,
                _logger.Object,
                _config.Object,
                new BusinessId(BusinessId),
                _filteredUrl.Object,
                _featureToggles
                );
        }

        [Fact]
        public void ItReturnsANewsListingPageWithTwoItems()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Index(new NewsroomViewModel(), 1)) as ViewResult;

            var viewModel = actionResponse.ViewData.Model as NewsroomViewModel;
            var news = viewModel.Newsroom;

            news.News.Count.Should().Be(2);

            var firstNewsItem = news.News[0];
            firstNewsItem.Should().Be(NewsItemWithoutImages);

            var secondNewsItem = news.News[1];
            secondNewsItem.Should().Be(NewsItemWithImages);

            news.EmailAlerts.Should().Be(EmailAlertsOn);
            news.EmailAlertsTopicId.Should().Be(EmailAlertsTopicId);
        }

        [Fact]
        public void ItReturnsANewsPageWithImageDocumentssAndLatestNews()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Detail("another-news-article")) as ViewResult;

            var news = actionResponse.ViewData.Model as NewsViewModel;

            news.NewsItem.Title.Should().Be("Another news article");
            news.NewsItem.Slug.Should().Be("another-news-article");
            news.NewsItem.Teaser.Should().Be("This is another news article");
            news.NewsItem.Image.Should().Be("image.jpg");
            news.NewsItem.ThumbnailImage.Should().Be("thumbnail.jpg");
            news.NewsItem.Body.Should().Be("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.");
            news.NewsItem.SunriseDate.Should().Be(new DateTime(2015, 9, 10));
            news.NewsItem.SunsetDate.Should().Be(new DateTime(2015, 9, 20));
            news.NewsItem.Tags.Should().HaveCount(2);
            news.NewsItem.Tags.First().Should().Be("Events");

            news.GetLatestNews().Should().HaveCount(2);
        }

        [Fact]
        public void ItReturnsANewsPageWithNoLatestNewsItems()
        {
            _repository.Setup(o => o.Get<List<News>>("7", null)).ReturnsAsync(new HttpResponse(404, null, "not found"));
            var controller = new NewsController(
                _repository.Object,
                _processedContentRepository.Object,
                _mockRssFeedFactory.Object,
                _logger.Object, _config.Object,
                 new BusinessId(BusinessId),
                 _filteredUrl.Object,
                 _featureToggles);
            var response = AsyncTestHelper.Resolve(controller.Detail("another-news-article")) as ViewResult;

            var model = response.Model as NewsViewModel;

            model.NewsItem.Slug.Should().Be("another-news-article");
        }


        [Fact]
        public void ItReturnsAListOfNewsArticlesForATagAndACategory()
        {
            _repository.Setup(
                    o =>
                        o.Get<Newsroom>("",
                            It.Is<List<Query>>(
                                l =>
                                    l.Contains(new Query("tag", "Events")) &&
                                    l.Contains(new Query("Category", "A Category")))))
                .ReturnsAsync(HttpResponse.Successful((int) HttpStatusCode.OK, _newsRoom));
            var actionResponse = AsyncTestHelper.Resolve(_controller.Index(new NewsroomViewModel {Tag = "Events", Category = "A Category" }, 1)) as ViewResult;

            var viewModel = actionResponse.ViewData.Model as NewsroomViewModel;
            var news = viewModel.Newsroom;

            news.News.Count.Should().Be(2);

            var firstNewsItem = news.News[0];
            firstNewsItem.Should().Be(NewsItemWithoutImages);

            var secondNewsItem = news.News[1];
            secondNewsItem.Should().Be(NewsItemWithImages);

            news.EmailAlerts.Should().Be(EmailAlertsOn);
            news.EmailAlertsTopicId.Should().Be(EmailAlertsTopicId);
        }

        [Fact]
        public void ItReturns404ForNoNewsItems()
        {
            _repository.Setup(o => o.Get<Newsroom>(string.Empty, It.IsAny<List<Query>>())).ReturnsAsync(new HttpResponse(404, null, "not found"));
            var controller = new NewsController(_repository.Object, _processedContentRepository.Object, _mockRssFeedFactory.Object, _logger.Object, _config.Object, new BusinessId(BusinessId), _filteredUrl.Object, _featureToggles);
            var response = AsyncTestHelper.Resolve(controller.Index(new NewsroomViewModel(), 1)) as HttpResponse;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public void ItReturns404NotFoundForNewsArticleThatdoesNotExist()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Detail("this-news-article-does-not-exist")) as HttpResponse;

            actionResponse.StatusCode.Should().Be(404);
        }

        [Fact]
        public void CreatesRssFeedFromFactory()
        {
            _repository.Setup(o => o.Get<Newsroom>(It.IsAny<string>(), null)).ReturnsAsync(HttpResponse.Successful(200, _newsRoom));

            var response = AsyncTestHelper.Resolve(_controller.Rss()) as ContentResult;

            response.ContentType.Should().Be("application/rss+xml");
            response.Content.Should().Be("rss fun");

            _mockRssFeedFactory.Verify(o => o.BuildRssFeed(It.IsAny<List<News>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }      

        [Fact]
        public void ShouldReturnNewsItemsForADateFilter()
        {
            _repository.Setup(o =>
                o.Get<Newsroom>(
                    "",
                    It.Is<List<Query>>(l =>
                        l.Contains(new Query("DateFrom", "2016-10-01"))
                        && l.Contains(new Query("DateTo", "2016-11-01"))
                    )
                )
            ).ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _newsRoom));

            var actionResponse = AsyncTestHelper.Resolve(_controller.Index(new NewsroomViewModel { DateFrom = new DateTime(2016, 10, 01), DateTo = new DateTime(2016, 11, 01) }, 1)) as ViewResult;

            var viewModel = actionResponse.ViewData.Model as NewsroomViewModel;
            var news = viewModel.Newsroom;

            news.Should().Be(_newsRoom);
        }

        [Fact]
        public void Page2ShouldReturn5ItemsWhen20TotalItems()
        {
            List<News> longListofNewsItems = new List<News>();
            for (int i = 0; i < 20; i++)
            {
               var NewsItem = new News("News Article " + i.ToString(),
                "another-news-article",
                "This is another news article",
                "image.jpg",
                "thumbnail.jpg",
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
                new List<Crumb>(), new DateTime(2015, 9, 10), new DateTime(2015, 9, 20), new List<Alert>(), new List<string>(), new List<Document>());

                longListofNewsItems.Add(NewsItem);
            }
            var bigNewsRoom = new Newsroom(longListofNewsItems, new OrderedList<Alert>(), EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());


            _repository.Setup(o =>
                o.Get<Newsroom>(
                    "",
                    It.Is<List<Query>>(l =>
                        l.Contains(new Query("DateFrom", "2016-10-01"))
                        && l.Contains(new Query("DateTo", "2016-11-01"
                        ))
                    )
                )
            ).ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, bigNewsRoom));

           var controller = new NewsController(
               _repository.Object,
               _processedContentRepository.Object,
               _mockRssFeedFactory.Object,
               _logger.Object,
               _config.Object,
               new BusinessId(BusinessId),
               _filteredUrl.Object,
               new FeatureToggles { NewsroomPagination = true }
               );

            var actionResponse = AsyncTestHelper.Resolve(controller.Index(new NewsroomViewModel { DateFrom = new DateTime(2016, 10, 01), DateTo = new DateTime(2016, 11, 01) }, 2)) as ViewResult;

            var viewModel = actionResponse.ViewData.Model as NewsroomViewModel;
            var news = viewModel.Newsroom;

            news.News.Count.Should().Be(5);
        }

        [Fact]
        public void ItReturnsa404WhenNotReachedSunriseDate()
        {
            DateTime FutureDate = DateTime.Now.AddDays(10);

            ProcessedNews FutureNews = new ProcessedNews("Another news article",
            "another-news-article",
            "This is another news article",
            "image.jpg",
            "thumbnail.jpg",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
            new List<Crumb>(), FutureDate, FutureDate.AddDays(20), new List<Alert>(), new List<string> { "Events", "Bramall Hall" });

            _processedContentRepository.Setup(o => o.Get<Newsroom>(string.Empty, It.IsAny<List<Query>>())).ReturnsAsync(new HttpResponse(200, FutureNews, ""));
            var controller = new NewsController(_repository.Object, _processedContentRepository.Object, _mockRssFeedFactory.Object, _logger.Object, _config.Object, new BusinessId(BusinessId), _filteredUrl.Object, _featureToggles);
            var response = AsyncTestHelper.Resolve(controller.Detail("")) as HttpResponse;

            response.StatusCode.Should().Be(404);
        }
    }
}
