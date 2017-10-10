using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.ProcessedModels;
using StockportWebapp.RSS;
using StockportWebapp.Utils;
using StockportWebapp.ViewModels;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;
using StockportWebapp.AmazonSES;
using StockportWebappTests.Builders;

namespace StockportWebappTests.Unit.Controllers
{
    public class EventsControllerTest
    {
        private readonly EventsController _controller;
        private readonly Mock<IRepository> _repository = new Mock<IRepository>();
        private readonly Mock<IProcessedContentRepository> _processedContentRepository = new Mock<IProcessedContentRepository>();
        private readonly Event _eventsItem;
        private readonly List<string> _categories;
        private readonly HttpResponse responseListing;
        private readonly HttpResponse _responseDetail;
        private readonly EventEmailBuilder _eventEmailBuilder;
        private readonly Mock<IHttpEmailClient> _emailClient;
        private readonly Mock<IApplicationConfiguration> _applicationConfiguration;
        private readonly Mock<IRssFeedFactory> _mockRssFeedFactory;
        private readonly Mock<ILogger<EventEmailBuilder>> _emaillogger;
        private readonly Mock<ILogger<EventsController>> _logger;
        private readonly Mock<IApplicationConfiguration> _config;
        private const string BusinessId = "businessId";
        private readonly Mock<IFilteredUrl> _filteredUrl;
        private readonly DateCalculator _datetimeCalculator;
        private readonly Group _group = new GroupBuilder().Build();

        private readonly List<Alert> _alerts = new List<Alert> { new Alert("title", "subHeading", "body",
                                                                 "severity", new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty) };

        public const int MaxNumberOfItemsPerPage = 15;

        public EventsControllerTest()
        {
            _eventsItem = new Event { Title = "title", Slug = "slug", Teaser = "teaser", ImageUrl = "image.png", ThumbnailImageUrl = "image.png", Description = "description", Fee = "fee",
                                      Location = "location", SubmittedBy = "submittedBy", EventDate = new DateTime(2016, 12, 30, 00, 00, 00), StartTime = "startTime", EndTime = "endTime", Breadcrumbs = new List<Crumb>(),Group = _group, Alerts = _alerts};
            _categories = new List<string> {"Category 1", "Category 2"};

            var mockTime = new Mock<ITimeProvider>();
            _datetimeCalculator = new DateCalculator(mockTime.Object);

            var eventsCalendar = new EventResponse(new List<Event> { _eventsItem }, _categories);
            var eventItem = new ProcessedEvents("title", "slug", "teaser", "image.png", "image.png", "description", 
                "fee", "location", "submittedBy", new DateTime(2016, 12, 30, 00, 00, 00), "startTime", "endTime", 
                new List<Crumb>(), _categories, new MapDetails(), "booking information",_group, _alerts,string.Empty);

            var eventHomepage = new EventHomepage { Categories = new List<EventCategory>(), Rows = new List<EventHomepageRow>() };

            // setup responses (with mock data)
            responseListing = new HttpResponse(200, eventsCalendar, "");
            _responseDetail = new HttpResponse(200, eventItem, "");
            var responseHomepage = new HttpResponse(200, eventHomepage, "");
            var response404 = new HttpResponse(404, null, "not found");

            // setup mocks
            _repository.Setup(o => o.Get<EventHomepage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(responseHomepage);

            _repository.Setup(o => o.Get<EventResponse>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(responseListing);

            _processedContentRepository.Setup(o => o.Get<Event>("event-of-the-century", It.Is<List<Query>>(l => l.Count == 0)))
                .ReturnsAsync(_responseDetail);

            _processedContentRepository.Setup(o => o.Get<Event>("404-event", It.Is<List<Query>>(l => l.Count == 0)))
                .ReturnsAsync(response404);

            _emailClient = new Mock<IHttpEmailClient>();
            _applicationConfiguration = new Mock<IApplicationConfiguration>();
            _logger = new Mock<ILogger<EventsController>>();
            _emaillogger = new Mock<ILogger<EventEmailBuilder>>();

            _applicationConfiguration.Setup(a => a.GetEmailEmailFrom(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));

            _eventEmailBuilder = new EventEmailBuilder(_emaillogger.Object, _emailClient.Object, _applicationConfiguration.Object, new BusinessId("businessId"));

            _mockRssFeedFactory = new Mock<IRssFeedFactory>();
            _config = new Mock<IApplicationConfiguration>();
            _filteredUrl = new Mock<IFilteredUrl>();

            _config.Setup(o => o.GetRssEmail(BusinessId)).Returns(AppSetting.GetAppSetting("rss-email"));
            _config.Setup(o => o.GetEmailAlertsNewSubscriberUrl(BusinessId)).Returns(AppSetting.GetAppSetting("email-alerts-url"));

            _controller = new EventsController(
                _repository.Object,
                _processedContentRepository.Object,
                _eventEmailBuilder, 
                _mockRssFeedFactory.Object,
                _logger.Object,
                _config.Object,
                new BusinessId(BusinessId),
                _filteredUrl.Object,
                null,
                null,
                _datetimeCalculator
                );
        }

        [Fact]
        public void ShouldReturnEventsCalendar()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Index(new EventCalendar() { FromSearch = true }, 1, 12)) as ViewResult;

            var events = actionResponse.ViewData.Model as EventCalendar;
            events.Events.Count.Should().Be(1);

            events.Events[0].Should().Be(_eventsItem);
        }

        [Fact]
        public void ShouldReturnEventsCalendarWhenQueryStringIsPassed()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Index(new EventCalendar { FromSearch = true, Category = "test", DateFrom = new DateTime(2017, 01, 20), DateTo = new DateTime(2017, 01, 25), DateRange = "customdate"}, 1, 12)) as ViewResult;

            var events = actionResponse.ViewData.Model as EventCalendar;
            events.Events.Count.Should().Be(1);

            events.Events[0].Should().Be(_eventsItem);

            events.Category.Should().Be("test");
            events.DateFrom.Should().Be(new DateTime(2017, 01, 20));
            events.DateTo.Should().Be(new DateTime(2017, 01, 25));
            events.DateRange.Should().Be("customdate");            
        }

        [Fact]
        public void ShouldReturnEvent()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Detail("event-of-the-century")) as ViewResult;

            var model = actionResponse.ViewData.Model as ProcessedEvents;

            model.Title.Should().Be("title");
            model.Slug.Should().Be("slug");
            model.Teaser.Should().Be("teaser");
            model.Fee.Should().Be("fee");
            model.Location.Should().Be("location");
            model.SubmittedBy.Should().Be("submittedBy");
            model.EventDate.Should().Be(new DateTime(2016, 12, 30, 00, 00, 00));
            model.StartTime.Should().Be("startTime");
            model.EndTime.Should().Be("endTime");
            model.BookingInformation.Should().Be("booking information");
            model.Group.Name.Should().Be(_group.Name);
            model.Alerts[0].Title.Should().Be(_alerts[0].Title);
            model.Alerts[0].Body.Should().Be(_alerts[0].Body);
            model.Alerts[0].Severity.Should().Be(_alerts[0].Severity);
            model.Alerts[0].SubHeading.Should().Be(_alerts[0].SubHeading);
            model.Alerts[0].SunriseDate.Should().Be(_alerts[0].SunriseDate);
            model.Alerts[0].SunsetDate.Should().Be(_alerts[0].SunsetDate);
        }

        [Fact]
        public void ShouldReturnEventWhenDateQueryPassedIn()
        {
            var date = new DateTime();
            const string slug = "event-of-the-century";
            _processedContentRepository.Setup(o => o.Get<Event>(slug, It.Is<List<Query>>(q => q.Contains(new Query("date", date.ToString("yyyy-MM-dd")))))).ReturnsAsync(_responseDetail);

            AsyncTestHelper.Resolve(_controller.Detail(slug, date));

            _processedContentRepository.Verify(o => o.Get<Event>(slug, It.Is<List<Query>>(q => q.Contains(new Query("date", date.ToString("yyyy-MM-dd"))))));
        }

        [Fact]
        public void ItReturns404NotFoundForEvent()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Detail("404-event")) as HttpResponse;

            actionResponse.StatusCode.Should().Be(404);
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(2, 1, 2, 1)]
        [InlineData(MaxNumberOfItemsPerPage, 1, MaxNumberOfItemsPerPage, 1)]
        [InlineData(MaxNumberOfItemsPerPage * 3, 1, MaxNumberOfItemsPerPage, 3)]
        [InlineData(MaxNumberOfItemsPerPage + 1, 2, 1, 2)]
        public void PaginationShouldResultInCorrectNumItemsOnPageAndCorrectNumPages(
            int totalNumItems,
            int requestedPageNumber,
            int expectedNumItemsOnPage,
            int expectedNumPages)
        {
            // Arrange
            var controller = SetUpController(totalNumItems);
            var model = new EventCalendar() { FromSearch = true };

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Index(model, requestedPageNumber, MaxNumberOfItemsPerPage)) as ViewResult;

            // Assert
            var viewModel = actionResponse.ViewData.Model as EventCalendar;
            viewModel.Events.Count.Should().Be(expectedNumItemsOnPage);
            model.Pagination.TotalPages.Should().Be(expectedNumPages);
        }

        [Theory]
        [InlineData(0, 50, 1)]
        [InlineData(5, MaxNumberOfItemsPerPage * 3, 3)]
        public void IfSpecifiedPageNumIsImpossibleThenActualPageNumWillBeAdjustedAccordingly(
            int specifiedPageNumber,
            int numItems,
            int expectedPageNumber)
        {
            // Arrange
            var controller = SetUpController(numItems);
            var model = new EventCalendar() { FromSearch = true };

            // Act
            AsyncTestHelper.Resolve(controller.Index(model, specifiedPageNumber, MaxNumberOfItemsPerPage));

            // Assert
            model.Pagination.CurrentPageNumber.Should().Be(expectedPageNumber);
        }

        [Fact]
        public void ShouldReturnEmptyPaginationObjectIfNoEventsExist()
        {
            // Arrange
            const int zeroItems = 0;
            var controller = SetUpController(zeroItems);
            var model = new EventCalendar() { FromSearch = true };

            // Act
            AsyncTestHelper.Resolve(controller.Index(model, 0, 12));

            // Assert
            model.Pagination.Should().NotBeNull();
        }

        [Fact]
        public void ShouldReturnCurrentURLForPagination()
        {
            // Arrange
            int numItems = 10;
            var controller = SetUpController(numItems);
            var model = new EventCalendar() { FromSearch = true };

            // Act
            AsyncTestHelper.Resolve(controller.Index(model, 0, 12));

            // Assert
            model.Pagination.CurrentUrl.Should().NotBeNull();
        }

        private EventsController SetUpController(int numItems)
        {
            List<Event> listOfEvents = BuildEventList(numItems);

            var categories = new List<string> { "Category 1", "Category 2" };
            var eventsCalendar = new EventResponse(listOfEvents, categories);
            var eventListResponse = new HttpResponse(200, eventsCalendar, "");

            _repository.Setup(o => 
                o.Get<EventResponse>(
                    It.IsAny<string>(), 
                    It.IsAny<List<Query>>()))
                .ReturnsAsync(eventListResponse);

            var controller = new EventsController(
                _repository.Object,
                _processedContentRepository.Object,
                _eventEmailBuilder,
                _mockRssFeedFactory.Object,
                _logger.Object,
                _config.Object,
                new BusinessId(BusinessId),
                _filteredUrl.Object,
                null,
                null,
                _datetimeCalculator
            );

            return controller;
        }

        private List<Event> BuildEventList(int numberOfItems)
        {
            List<Event> listOfEvents = new List<Event>();

            for (int i = 0; i < numberOfItems; i++)
            {
                var eventItem = new Event
                {
                    Title = "title",
                    Slug = "slug",
                    Teaser = "teaser",
                    ImageUrl = "image.png",
                    ThumbnailImageUrl = "image.png",
                    Description = "description",
                    Fee = "fee",
                    Location = "location",
                    SubmittedBy = "submittedBy",
                    EventDate = new DateTime(2016, 12, 30, 00, 00, 00),
                    StartTime = "startTime",
                    EndTime = "endTime",
                    Breadcrumbs = new List<Crumb>(),
                    Group = _group,
                    Alerts = _alerts
                };

                listOfEvents.Add(eventItem);
            }

            return listOfEvents;
        }
    }
}
