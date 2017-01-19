using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.Config;
using Xunit;
using HttpResponse = StockportWebapp.Http.HttpResponse;
using StockportWebapp.FeatureToggling;
using StockportWebapp.ViewModels;

namespace StockportWebappTests.Unit.Controllers
{
    public class EventsControllerTest
    {
        private readonly EventsController _controller;
        private readonly Mock<IRepository> _repository = new Mock<IRepository>();
        private readonly Mock<IProcessedContentRepository> _processedContentRepository = new Mock<IProcessedContentRepository>();
        private readonly Mock<ILogger<EventsController>> _logger;
        private const string BusinessId = "businessId";
        private readonly Event _eventsItem;
        private readonly HttpResponse responseListing;
        private readonly HttpResponse _responseDetail;
        private readonly Mock<IEventsRepository> _eventRepository;
        private readonly FeatureToggles _featureToggles;

        public EventsControllerTest()
        {
            _eventsItem = new Event { Title = "title", Slug = "slug", Teaser = "teaser", ImageUrl = "image.png", ThumbnailImageUrl = "image.png", Description = "description", Fee = "fee",
                                      Location = "location", SubmittedBy = "submittedBy", EventDate = new DateTime(2016, 12, 30, 00, 00, 00), StartTime = "startTime", EndTime = "endTime", Breadcrumbs = new List<Crumb>() };
            var eventsCalendar = new EventCalendar(new List<Event> { _eventsItem });
            var eventItem = new ProcessedEvents("title", "slug", "teaser", "image.png", "image.png", "description", "fee", "location", "submittedBy", new DateTime(2016, 12, 30, 00, 00, 00), "startTime", "endTime", new List<Crumb>());

            // setup responses (with mock data)
            responseListing = new HttpResponse(200, eventsCalendar, "");
            _responseDetail = new HttpResponse(200, eventItem, "");
            var response404 = new HttpResponse(404, null, "not found");

            // setup mocks
            _repository.Setup(o => o.Get<EventCalendar>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(responseListing);

            _processedContentRepository.Setup(o => o.Get<Event>("event-of-the-century", It.Is<List<Query>>(l => l.Count == 0)))
                .ReturnsAsync(_responseDetail);

            _processedContentRepository.Setup(o => o.Get<Event>("404-event", It.Is<List<Query>>(l => l.Count == 0)))
                .ReturnsAsync(response404);

            _logger = new Mock<ILogger<EventsController>>();
            _eventRepository = new Mock<IEventsRepository>();
            _featureToggles = new FeatureToggles {EventSubmission = true};

            _controller = new EventsController(
                _repository.Object,
                _processedContentRepository.Object,
                _eventRepository.Object,
                _logger.Object,
                new BusinessId(BusinessId),
                _featureToggles);
        }

        [Fact]
        public void ShouldReturnEventsCalendar()
        {
            var actionResponse = AsyncTestHelper.Resolve(_controller.Index()) as ViewResult;

            var viewModel = actionResponse.ViewData.Model as EventsCalendarViewModel;
            var events = viewModel.EventCalendar;

            events.Events.Count.Should().Be(1);

            events.Events[0].Should().Be(_eventsItem);
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
                    Teaser = "Teaser",
                    Description = "Description",
                    EventDate = new DateTime(2017,12,01),
                    StartTime = "10:00",
                    EndTime ="12:00",
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

        [Fact]
        public void ItShouldReturnToEventsIndexIfTheFeatureToggleIsOff()
        {
            _featureToggles.EventSubmission = false;
            var eventSubmission = new EventSubmission();

            var actionResponse = AsyncTestHelper.Resolve(_controller.SubmitEvent(eventSubmission));

            actionResponse.Should().BeOfType<RedirectToActionResult>();

            var view = actionResponse as RedirectToActionResult;
            view.ActionName.Should().Be("Index");

            _eventRepository.Verify(o => o.SendEmailMessage(eventSubmission), Times.Never);
        }
    }
}
