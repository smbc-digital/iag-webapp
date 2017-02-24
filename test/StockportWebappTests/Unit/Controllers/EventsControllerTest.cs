using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.RSS;
using StockportWebapp.Utils;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;


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
        private readonly Mock<IEventsRepository> _eventRepository;
        private readonly Mock<IRssFeedFactory> _mockRssFeedFactory;
        private readonly Mock<ILogger<EventsController>> _logger;
        private readonly Mock<IApplicationConfiguration> _config;
        private const string BusinessId = "businessId";
        private readonly Mock<IFilteredUrl> _filteredUrl;


        public EventsControllerTest()
        {
            _eventsItem = new Event { Title = "title", Slug = "slug", Teaser = "teaser", ImageUrl = "image.png", ThumbnailImageUrl = "image.png", Description = "description", Fee = "fee",
                                      Location = "location", SubmittedBy = "submittedBy", EventDate = new DateTime(2016, 12, 30, 00, 00, 00), StartTime = "startTime", EndTime = "endTime", Breadcrumbs = new List<Crumb>() };
            _categories = new List<string> {"Category 1", "Category 2"};

            var eventsCalendar = new EventResponse(new List<Event> { _eventsItem }, _categories);
            var eventItem = new ProcessedEvents("title", "slug", "teaser", "image.png", "image.png", "description", 
                "fee", "location", "submittedBy", new DateTime(2016, 12, 30, 00, 00, 00), "startTime", "endTime", 
                new List<Crumb>(), _categories, new MapPosition(), "booking information");

            // setup responses (with mock data)
            responseListing = new HttpResponse(200, eventsCalendar, "");
            _responseDetail = new HttpResponse(200, eventItem, "");
            var response404 = new HttpResponse(404, null, "not found");

            // setup mocks
            _repository.Setup(o => o.Get<EventResponse>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(responseListing);

            _processedContentRepository.Setup(o => o.Get<Event>("event-of-the-century", It.Is<List<Query>>(l => l.Count == 0)))
                .ReturnsAsync(_responseDetail);

            _processedContentRepository.Setup(o => o.Get<Event>("404-event", It.Is<List<Query>>(l => l.Count == 0)))
                .ReturnsAsync(response404);

            _eventRepository = new Mock<IEventsRepository>();

            _mockRssFeedFactory = new Mock<IRssFeedFactory>();
            _logger = new Mock<ILogger<EventsController>>();
            _config = new Mock<IApplicationConfiguration>();
            _config = new Mock<IApplicationConfiguration>();
            _filteredUrl = new Mock<IFilteredUrl>();

            _config.Setup(o => o.GetRssEmail(BusinessId)).Returns(AppSetting.GetAppSetting("rss-email"));
            _config.Setup(o => o.GetEmailAlertsNewSubscriberUrl(BusinessId)).Returns(AppSetting.GetAppSetting("email-alerts-url"));

            _controller = new EventsController(
                _repository.Object,
                _processedContentRepository.Object,
                _eventRepository.Object, 
                _mockRssFeedFactory.Object,
                _logger.Object,
                _config.Object,
                new BusinessId(BusinessId),
                _filteredUrl.Object
                );
        }

        [Fact]
        public void ShouldReturnEventsCalendar()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Index(new EventCalendar(), 1)) as ViewResult;

            var events = actionResponse.ViewData.Model as EventCalendar;
            events.Events.Count.Should().Be(1);

            events.Events[0].Should().Be(_eventsItem);
        }

        [Fact]
        public void ShouldReturnEventsCalendarWhenQueryStringIsPassed()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Index(new EventCalendar {Category = "test", DateFrom = new DateTime(2017, 01, 20), DateTo = new DateTime(2017, 01, 25), DateRange = "customdate"}, 1)) as ViewResult;

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

        [Fact]
        public void ItShouldGetARedirectResultForAValidEventSubmission()
        {
            var eventSubmission =  new EventSubmission()
                {
                    Title = "Title",
                    Description = "Description",
                    EventDate = new DateTime(2017, 12, 01),
                    StartTime = new DateTime(2017, 01, 01, 10, 00, 00),
                    EndTime = new DateTime(2017, 01, 01, 12, 00, 00),
                    Fee = "£5.00",
                    Frequency = "Frequency",
                    Image = null,
                    Attachment = null
            };

            _eventRepository.Setup(o => o.SendEmailMessage(It.IsAny<EventSubmission>())).ReturnsAsync(HttpStatusCode.OK);

            var actionResponse = AsyncTestHelper.Resolve(_controller.SubmitEvent(eventSubmission)) as RedirectToActionResult;
            actionResponse.ActionName.Should().Be("ThankYouMessage");
        }

        [Fact]
        public void ItShouldReturnBackToTheViewForAnInvalidEmailSubmission()
        {
            var eventSubmission = new EventSubmission();

            _eventRepository.Setup(o => o.SendEmailMessage(It.IsAny<EventSubmission>())).ReturnsAsync(HttpStatusCode.BadRequest);

            var actionResponse = AsyncTestHelper.Resolve(_controller.SubmitEvent(eventSubmission));

            actionResponse.Should().BeOfType<ViewResult>();
            _eventRepository.Verify(o => o.SendEmailMessage(eventSubmission), Times.Once);
        }

        [Fact]
        public void ItShouldNotSendAnEmailForAnInvalidFormSumbission()
        {
            var eventSubmission = new EventSubmission();

            _controller.ModelState.AddModelError("Title", "an invalid title was provided");

            var actionResponse = AsyncTestHelper.Resolve(_controller.SubmitEvent(eventSubmission));

            actionResponse.Should().BeOfType<ViewResult>();
            _eventRepository.Verify(o => o.SendEmailMessage(eventSubmission), Times.Never);
        }
    }
}
