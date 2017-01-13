using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.ArticleStartPageNewsDuration)]
    public class EventsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly ILogger<EventsController> _logger;
        private readonly FeatureToggles _featureToggles;
        private readonly BusinessId _businessId;
        private readonly IEventsRepository _eventsRepository;

        public EventsController(IRepository repository,
                                IProcessedContentRepository processedContentRepository,
                                IEventsRepository eventsRepository,
                                ILogger<EventsController> logger,
                                BusinessId businessId, 
                                FeatureToggles featureToggles)
        {
            _repository = repository;
            _processedContentRepository = processedContentRepository;
            _eventsRepository = eventsRepository;
            _logger = logger;
            _businessId = businessId;
            _featureToggles = featureToggles;
        }

        [Route("/events")]
        public async Task<IActionResult> Index([FromQuery] DateTime? datefrom = null, [FromQuery] DateTime? dateto = null)
        {
            var queries = new List<Query>();           
            if (datefrom.HasValue) queries.Add(new Query("datefrom", datefrom.Value.ToString("yyyy-MM-dd")));
            if (dateto.HasValue) queries.Add(new Query("dateto", dateto.Value.ToString("yyyy-MM-dd")));

            var httpResponse = await _repository.Get<EventCalendar>(queries: queries);
            ViewBag.VisibleFilterDrowdownClass = "filter collapsible";
            if (queries.Count != 0) ViewBag.VisibleFilterDrowdownClass = "filter";

            if (!httpResponse.IsSuccessful())
                return httpResponse;

            var eventsCalendar = httpResponse.Content as EventCalendar;
            return View(new EventsCalendarViewModel(new List<Crumb>(), eventsCalendar));
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

        [Route("/events/submitevent")]
        public IActionResult SubmitEvent()
        {
            if (!_featureToggles.EventSubmission) return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        [Route("/events/submitevent")]
        public async Task<IActionResult> SubmitEvent(EventSubmission eventSubmission)
        {
            if (!_featureToggles.EventSubmission) return RedirectToAction("Index");
            if (ModelState.IsValid)
            {
                var successCode = await _eventsRepository.SendEmailMessage(eventSubmission);
                if (successCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("ThankYouMessage");
                }
            }

            return View(eventSubmission);
        }

        [Route("/events/thankyoumessage")]
        public IActionResult ThankYouMessage()
        {
            if (!_featureToggles.EventSubmission) return RedirectToAction("Index");
            return View();
        }
    }
}
