using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.ProcessedModels;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly BusinessId _businessId;
        private readonly IApplicationConfiguration _config;

        public HomeController(IRepository repository, IProcessedContentRepository processedContentRepository,
            BusinessId businessId, IApplicationConfiguration applicationConfiguration)
        {
            _config = applicationConfiguration;
            _businessId = businessId;
            _repository = repository;
            _processedContentRepository = processedContentRepository;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var httpResponse = await _processedContentRepository.Get<Homepage>();

            if (!httpResponse.IsSuccessful())
                return httpResponse;

            var homepage = httpResponse.Content as ProcessedHomepage;

            var latestNewsResponse = await _repository.Get<List<News>>("2");
            var latestNews = latestNewsResponse.Content as List<News>;

            if (homepage != null && latestNews != null) homepage.SetLatestNews(latestNews);

            var latestEventsResponse = await _repository.GetLatestOrderByFeatured<EventCalendar>(2);
            var latestEvents = latestEventsResponse.Content as EventCalendar;

            if (homepage != null && latestEvents != null) homepage.SetLatestEvents(latestEvents.Events);

            return View(homepage);
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