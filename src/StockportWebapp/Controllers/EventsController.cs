using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.RSS;
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

        public EventsController(IRepository repository, IProcessedContentRepository processedContentRepository, ILogger<EventsController> logger, BusinessId businessId, FeatureToggles featureToggles)
        {
            _repository = repository;
            _processedContentRepository = processedContentRepository;
            _logger = logger;
            _businessId = businessId;
            _featureToggles = featureToggles;
        }

        [Route("/events")]
        public async Task<IActionResult> Index()
        {       
            var httpResponse = await _repository.Get<EventCalendar>();

            if (!httpResponse.IsSuccessful())
                return httpResponse;

            var eventsCalendar = httpResponse.Content as EventCalendar;

            var crumbs = new List<Crumb> {new Crumb("Events", "events", "events")};

            return View(new EventsCalendarViewModel(crumbs, eventsCalendar));
        }


        [Route("/events/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var httpResponse = await _processedContentRepository.Get<Event>(slug);

            if (!httpResponse.IsSuccessful()) return httpResponse;

            var response = httpResponse.Content as ProcessedEvents;

            return View(response);
        }
    }
}
