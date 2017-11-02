using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.ViewModels;
using StockportWebapp.Services;
using System.Collections.Generic;
using StockportWebapp.Models;
using System.Linq;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class HomeController : Controller
    {
        private readonly BusinessId _businessId;
        private readonly IApplicationConfiguration _config;
        private readonly INewsService _newsService;
        private readonly IEventsService _eventsService;
        private readonly IHomepageService _homepageService;
        private readonly IStockportApiEventsService _stockportApiEventsService;

        public HomeController(BusinessId businessId, IApplicationConfiguration applicationConfiguration, INewsService newsService, IEventsService eventsService, IHomepageService homepageService, IStockportApiEventsService stockportApiService)
        {
            _config = applicationConfiguration;
            _businessId = businessId;
            _newsService = newsService;
            _eventsService = eventsService;
            _homepageService = homepageService;
            _stockportApiEventsService = stockportApiService;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var homepage = await _homepageService.GetHomepage();

            if (homepage == null) return new NotFoundResult();

            var eventsFromApi = !string.IsNullOrEmpty(homepage.EventCategory) ? await _stockportApiEventsService.GetEventsByCategory(homepage.EventCategory) : new List<Event>();

            var homepageViewModel = new HomepageViewModel
            {
                HomepageContent = homepage,
                FeaturedEvent = await _eventsService.GetLatestEventsItem(),
                FeaturedNews = await _newsService.GetLatestNewsItem(),
                EventsFromApi = eventsFromApi.Take(3).ToList()
            };

            return View(homepageViewModel);
        }

        [Route("/subscribe")]
        public async Task<IActionResult> EmailSubscribe(string emailAddress, [FromQuery] string EmailAlertsTopicId)
        {
            var urlSetting = _config.GetEmailAlertsUrl(_businessId.ToString());
            if (urlSetting.IsValid())
            {
                var redirectUrl = string.Concat(urlSetting, emailAddress) + "&topic_id=" + EmailAlertsTopicId;

                return await Task.FromResult(Redirect(redirectUrl));
            }
            return NotFound();
        }
    }
}