using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.RSS;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using StockportWebapp.Validation;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class EventsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRssFeedFactory _rssFeedFactory;
        private readonly ILogger<EventsController> _logger;
        private readonly EventEmailBuilder _emailBuilder;
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;
        private readonly IFilteredUrl _filteredUrl;
        private readonly CalendarHelper _helper;
        private readonly IDateCalculator _dateCalculator;
        private readonly IStockportApiEventsService _stockportApiEventsService;

        public EventsController(
            IRepository repository,
            IProcessedContentRepository processedContentRepository,
            EventEmailBuilder emailBuilder,
            IRssFeedFactory rssFeedFactory,
            ILogger<EventsController> logger,
            IApplicationConfiguration config,
            BusinessId businessId,
            IFilteredUrl filteredUrl,
            CalendarHelper helper,
            ITimeProvider timeProvider,
            IDateCalculator dateCalculator,
            IStockportApiEventsService stockportApiEventsService)
        {
            _repository = repository;
            _processedContentRepository = processedContentRepository;
            _emailBuilder = emailBuilder;
            _rssFeedFactory = rssFeedFactory;
            _logger = logger;
            _config = config;
            _businessId = businessId;
            _filteredUrl = filteredUrl;
            _helper = helper;
            _dateCalculator = dateCalculator;
            _stockportApiEventsService = stockportApiEventsService;
        }

        [Route("/events")]
        public async Task<IActionResult> Index(EventCalendar eventsCalendar, [FromQuery]int Page, [FromQuery]int pageSize)
        {
            if (ModelState["DateTo"] != null && ModelState["DateTo"].Errors.Count > 0)
            {
                ModelState["DateTo"].Errors.Clear();
            }

            if (ModelState["DateFrom"] != null && ModelState["DateFrom"].Errors.Count > 0)
            {
                ModelState["DateFrom"].Errors.Clear();
            }

            if (!string.IsNullOrEmpty(eventsCalendar.Tag)) { eventsCalendar.KeepTag = eventsCalendar.Tag; }

            eventsCalendar.FromSearch = eventsCalendar.FromSearch || !string.IsNullOrWhiteSpace(eventsCalendar.Category) || !string.IsNullOrWhiteSpace(eventsCalendar.Tag)
                                                                    || eventsCalendar.DateFrom != null || eventsCalendar.DateTo != null;

            var queries = new List<Query>();

            if (eventsCalendar.DateFrom.HasValue) queries.Add(new Query("DateFrom", eventsCalendar.DateFrom.Value.ToString("yyyy-MM-dd")));
            if (eventsCalendar.DateTo.HasValue) queries.Add(new Query("DateTo", eventsCalendar.DateTo.Value.ToString("yyyy-MM-dd")));
            if (!string.IsNullOrWhiteSpace(eventsCalendar.Category)) queries.Add(new Query("Category", eventsCalendar.Category));
            if (!string.IsNullOrWhiteSpace(eventsCalendar.Tag)) queries.Add(new Query("tag", eventsCalendar.Tag));
            if (eventsCalendar.Price != null) queries.Add(new Query("price", string.Join(",", eventsCalendar.Price)));
            if (eventsCalendar.Longitude != 0) queries.Add(new Query("longitude", string.Join(",", eventsCalendar.Longitude)));
            if (eventsCalendar.Latitude != 0) queries.Add(new Query("latitude", string.Join(",", eventsCalendar.Latitude)));

            var httpResponse = await _repository.Get<EventResponse>(queries: queries);

            if (!httpResponse.IsSuccessful()) return httpResponse;

            var eventResponse = httpResponse.Content as EventResponse;

            eventsCalendar.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
            _filteredUrl.SetQueryUrl(eventsCalendar.CurrentUrl);
            eventsCalendar.AddFilteredUrl(_filteredUrl);

            DoPagination(eventsCalendar, Page, eventResponse, pageSize);

            if (eventResponse != null)
            {
                eventsCalendar.AddEvents(eventResponse.Events);
                eventsCalendar.AddCategories(eventResponse.Categories);
            }

            var httpHomeResponse = await _repository.Get<EventHomepage>();

            if (!httpHomeResponse.IsSuccessful()) return httpHomeResponse;

            var eventHomeResponse = httpHomeResponse.Content as EventHomepage;

            eventsCalendar.Homepage = eventHomeResponse;

            return View(eventsCalendar);
        }

        [Route("/events/category/{category}")]
        public async Task<IActionResult> IndexWithCategory(string category, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var categories = await _stockportApiEventsService.GetEventCategories();

            var viewModel = new EventResultsVIewModel()
            {
                Title = category
            };

            var events = await _stockportApiEventsService.GetEventsByCategory(category, false);

            if (events == null || !events.Any()) return View("Index", viewModel);

            var eventCategory = categories.FirstOrDefault(c => c.Slug == category);

            viewModel.Title = eventCategory != null ? eventCategory.Name : category;
            viewModel.Events = events;

            viewModel.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
            _filteredUrl.SetQueryUrl(viewModel.CurrentUrl);
            viewModel.AddFilteredUrl(_filteredUrl);

            DoPagination(viewModel, page, pageSize);

            return View("Index", viewModel);
        }

        private void DoPagination(EventCalendar model, int currentPageNumber, EventResponse eventResponse, int pageSize)
        {
            if (eventResponse != null && eventResponse.Events.Any())
            {
                var paginatedEvents = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                    eventResponse.Events,
                    currentPageNumber,
                    "events",
                    pageSize,
                    _config.GetEventsDefaultPageSize("stockportgov"));

                eventResponse.Events = paginatedEvents.Items;
                model.Pagination = paginatedEvents.Pagination;
                model.Pagination.CurrentUrl = model.CurrentUrl;
            }
            else
            {
                model.Pagination = new Pagination();
            }
        }

        private void DoPagination(EventResultsVIewModel model, int currentPageNumber, int pageSize)
        {
            if (model != null && model.Events.Any())
            {
                var paginatedEvents = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                    model.Events,
                    currentPageNumber,
                    "events",
                    pageSize,
                    _config.GetEventsDefaultPageSize("stockportgov"));

                model.Events = paginatedEvents.Items;
                model.Pagination = paginatedEvents.Pagination;
                model.Pagination.CurrentUrl = model.CurrentUrl;
            }
            else
            {
                model.Pagination = new Pagination();
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

            if (date != null || date == DateTime.MinValue)
            {
                ViewBag.Eventdate = date.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.Eventdate = response?.EventDate.ToString("yyyy-MM-dd");
            }

            var httpHomeResponse = await _repository.Get<EventHomepage>();

            if (httpHomeResponse.IsSuccessful())
            {
                var eventHomeResponse = httpHomeResponse.Content as EventHomepage;

                if(eventHomeResponse.Alerts != null)
                {
                    foreach (var item in eventHomeResponse.Alerts)
                        response.GlobalAlerts.Add(item);
                }                
            }

            return View(response);
        }

        [Route("/events/details/{slug}")]
        public async Task<IActionResult> EventDetail(string slug, [FromQuery] DateTime? date = null)
        {
            var eventItem = await _stockportApiEventsService.GetProcessedEvent(slug, date);

            if (eventItem == null) return NotFound();

            ViewBag.CurrentUrl = Request?.GetUri();

            if (date != null || date == DateTime.MinValue)
            {
                ViewBag.Eventdate = date.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.Eventdate = eventItem?.EventDate.ToString("yyyy-MM-dd");
            }

            return View("Detail", eventItem);
        }

        [Route("/events/add-your-event")]
        [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
        public IActionResult AddYourEvent()
        {
            var eventSubmission = new EventSubmission();
            return View("Add-Your-Event", eventSubmission);
        }

        [HttpPost]
        [Route("/events/add-your-event")]
        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
        public async Task<IActionResult> AddYourEvent(EventSubmission eventSubmission)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubmissionError = GetErrorsFromModelState(ModelState);
                return View("Add-Your-Event", eventSubmission);
            }

            if (eventSubmission.IsRecurring)
            {
                Enum.TryParse(eventSubmission.Frequency, out EventFrequency frequency);
                if (frequency != EventFrequency.None)
                {
                    eventSubmission.Occurrences = _dateCalculator.GetEventOccurences(frequency, (DateTime)eventSubmission.EventDate, (DateTime)eventSubmission.EndDate);
                }
            }

            var successCode = await _emailBuilder.SendEmailAddNew(eventSubmission);
            if (successCode == HttpStatusCode.OK) return RedirectToAction("ThankYouMessage");

            ViewBag.SubmissionError = "There was a problem submitting the event, please try again.";

            return View("Add-Your-Event", eventSubmission);
        }

        private string GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            var message = new StringBuilder();

            foreach (var state in modelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    message.Append(state.Value.Errors.First().ErrorMessage + Environment.NewLine + "<br />");
                }
            }
            return message.ToString();
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

        [Route("events/add-to-calendar")]
        public IActionResult AddToCalendar(string type, string eventUrl, string slug, DateTime eventDate, string name, string location, string startTime, string endTime, string description, string summary)
        {
            var eventItem = new Event()
            {
                Slug = slug,
                EventDate = eventDate,
                Title = name,
                Location = location,
                StartTime = startTime,
                EndTime = endTime,
                Description = description,
                Teaser = summary
            };

            if (type == "google" || type == "yahoo")
            {
                var url = _helper.GetCalendarUrl(eventItem, eventUrl, type);
                return Redirect(url);
            }

            if (type == "windows" || type == "apple")
            {
                byte[] calendarBytes = System.Text.Encoding.UTF8.GetBytes(_helper.GetIcsText(eventItem, eventUrl));
                return File(calendarBytes, "text/calendar", slug + ".ics");
            }

            return null;
        }
    }
}
