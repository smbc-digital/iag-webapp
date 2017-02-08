using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Quartz.Util;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class EventsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IEventsRepository _eventsRepository;

        public EventsController(IRepository repository,
                                IProcessedContentRepository processedContentRepository,
                                IEventsRepository eventsRepository)
        {
            _repository = repository;
            _processedContentRepository = processedContentRepository;
            _eventsRepository = eventsRepository;
        }

        [Route("/events")]
        public async Task<IActionResult> Index(EventCalendar eventsCalendar)
        {
            if (!ModelState.IsValid && !string.IsNullOrEmpty(eventsCalendar.DateRange)) return View(eventsCalendar);

            if (string.IsNullOrEmpty(eventsCalendar.category) && eventsCalendar.datefrom == null && eventsCalendar.dateto == null && string.IsNullOrEmpty(eventsCalendar.DateRange))
            {
                ModelState["dateto"].Errors.Clear();
                ModelState["datefrom"].Errors.Clear();
            }

            var queries = new List<Query>();           
            if (eventsCalendar.datefrom.HasValue) queries.Add(new Query("datefrom", eventsCalendar.datefrom.Value.ToString("yyyy-MM-dd")));
            if (eventsCalendar.dateto.HasValue) queries.Add(new Query("dateto", eventsCalendar.dateto.Value.ToString("yyyy-MM-dd")));
            if (!eventsCalendar.category.IsNullOrWhiteSpace()) queries.Add(new Query("category", eventsCalendar.category));
             
            var httpResponse = await _repository.Get<EventResponse>(queries: queries);

            if (!httpResponse.IsSuccessful()) return httpResponse;

            var eventResponse = httpResponse.Content as EventResponse;

            if (eventResponse != null)
            {
                eventsCalendar.AddEvents(eventResponse.Events);
                eventsCalendar.AddCategories(eventResponse.Categories);
            }

            return View(eventsCalendar);
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

        [Route("/events/submit-event")]
        public IActionResult SubmitEvent()
        {
            var eventSubmission = new EventSubmission();
            return View(eventSubmission);
        }

        [HttpPost]
        [Route("/events/submit-event")]
        public async Task<IActionResult> SubmitEvent(EventSubmission eventSubmission)
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
    }
}
