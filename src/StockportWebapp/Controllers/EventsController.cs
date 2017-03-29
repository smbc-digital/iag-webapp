using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz.Util;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.RSS;
using StockportWebapp.Utils;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class EventsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRssFeedFactory _rssFeedFactory;
        private readonly ILogger<EventsController> _logger;
        private readonly IEventsRepository _eventsRepository;
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;
        private readonly IFilteredUrl _filteredUrl;

        public EventsController(
            IRepository repository,
            IProcessedContentRepository processedContentRepository,
            IEventsRepository eventsRepository, 
            IRssFeedFactory rssFeedFactory,
            ILogger<EventsController> logger, 
            IApplicationConfiguration config, 
            BusinessId businessId, 
            IFilteredUrl filteredUrl)
        {
            _repository = repository;
            _processedContentRepository = processedContentRepository;
            _eventsRepository = eventsRepository;
            _rssFeedFactory = rssFeedFactory;
            _logger = logger;
            _config = config;
            _businessId = businessId;
            _filteredUrl = filteredUrl;
        }

        [Route("/events")]
        public async Task<IActionResult> Index(EventCalendar eventsCalendar, [FromQuery]int Page)
        {           
            if (eventsCalendar.DateFrom == null && eventsCalendar.DateTo == null && string.IsNullOrEmpty(eventsCalendar.DateRange))
            {
                if(ModelState["DateTo"] != null && ModelState["DateTo"].Errors.Count > 0)
                    ModelState["DateTo"].Errors.Clear();
                if (ModelState["DateFrom"] != null && ModelState["DateFrom"].Errors.Count > 0)
                    ModelState["DateFrom"].Errors.Clear();
            }

            var queries = new List<Query>();           

            if (eventsCalendar.DateFrom.HasValue) queries.Add(new Query("DateFrom", eventsCalendar.DateFrom.Value.ToString("yyyy-MM-dd")));
            if (eventsCalendar.DateTo.HasValue) queries.Add(new Query("DateTo", eventsCalendar.DateTo.Value.ToString("yyyy-MM-dd")));
            if (!eventsCalendar.Category.IsNullOrWhiteSpace()) queries.Add(new Query("Category", eventsCalendar.Category));
            if (!eventsCalendar.Tag.IsNullOrWhiteSpace()) queries.Add(new Query("tag", eventsCalendar.Tag));
             
            var httpResponse = await _repository.Get<EventResponse>(queries: queries);

            if (!httpResponse.IsSuccessful()) return httpResponse;

            var eventResponse = httpResponse.Content as EventResponse;

            eventsCalendar.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
            _filteredUrl.SetQueryUrl(eventsCalendar.CurrentUrl);
            eventsCalendar.AddFilteredUrl(_filteredUrl);

            DoPagination(eventsCalendar, Page, eventResponse);

            if (eventResponse != null)
            {
                eventsCalendar.AddEvents(eventResponse.Events);
                eventsCalendar.AddCategories(eventResponse.Categories);
            }

            return View(eventsCalendar);
        }

        private void DoPagination(EventCalendar model, int currentPageNumber, EventResponse eventResponse)
        {
            model.Pagination = new Pagination();

            if (eventResponse != null && eventResponse.Events.Any())
            {
                var paginatedEvents = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                    eventResponse.Events, 
                    currentPageNumber, 
                    "Events");

                eventResponse.Events = paginatedEvents.Items;
                model.Pagination = paginatedEvents.Pagination;
                model.Pagination.CurrentUrl = model.CurrentUrl;
            }
        }

        [Route("/events/{slug}")]
        public async Task<IActionResult> Detail(string slug, [FromQuery] DateTime? date = null)
        {
            var queries = new List<Query>();
            if (date.HasValue) queries.Add(new Query("date", date.Value.ToString("yyyy-MM-dd")));

            var httpResponse = await _processedContentRepository.Get<Event>(slug, queries);

            if (!httpResponse.IsSuccessful()) return httpResponse;

            var response = httpResponse.Content as ProcessedEvents;

            ViewBag.CurrentUrl = Request?.GetUri();
           
            return View(response);
        }

        [Route("/events/add-your-event")]
        public async Task<IActionResult> AddYourEvent()
        {         
            var eventSubmission = new EventSubmission();
            return View(eventSubmission);
        }

        [HttpPost]
        [Route("/events/add-your-event")]
        public async Task<IActionResult> AddYourEvent(EventSubmission eventSubmission)
        {
            if (!ModelState.IsValid) return View(eventSubmission);

            var successCode = await _eventsRepository.SendEmailMessage(eventSubmission);
            if (successCode == HttpStatusCode.OK) return RedirectToAction("ThankYouMessage");
               
            ViewBag.SubmissionError = "There was a problem submitting the event, please try again.";

            return View(eventSubmission);
        }

        [Route("/events/thank-you-message")]
        public IActionResult ThankYouMessage()
        {
            return View();
        }


        [Route("events/rss")]
        public async Task<IActionResult> Rss()
        {
            var httpResponse = await _repository.Get<EventResponse>();

            var host = Request != null && Request.Host.HasValue ? string.Concat(Request.IsHttps ? "https://" : "http://", Request.Host.Value, "/events/") : string.Empty;

            if (!httpResponse.IsSuccessful())
            {
                _logger.LogDebug("Rss: Http Response not sucessful");
                return httpResponse;
            }

            var response = httpResponse.Content as EventResponse;
            var emailFromAppSetting = _config.GetRssEmail(_businessId.ToString());
            var email = emailFromAppSetting.IsValid() ? emailFromAppSetting.ToString() : string.Empty;

            _logger.LogDebug("Rss: Creating News Feed");
            return await Task.FromResult(Content(_rssFeedFactory.BuildRssFeed(response.Events, host, email), "application/rss+xml"));
        }
    }
}
