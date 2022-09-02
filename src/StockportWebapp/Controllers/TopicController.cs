using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class TopicController : Controller
    {
        private readonly IRepository _repository;
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;
        private readonly IStockportApiEventsService _stockportApiEventsService;

        public TopicController(IRepository repository, IApplicationConfiguration config, BusinessId businessId, IStockportApiEventsService stockportApiService)
        {
            _repository = repository;
            _config = config;
            _businessId = businessId;
            _stockportApiEventsService = stockportApiService;
        }

        [Route("/topic/{topicSlug}")]
        public async Task<IActionResult> Index(string topicSlug)
        {
            var topicHttpResponse = await _repository.Get<Topic>(topicSlug);

            if (!topicHttpResponse.IsSuccessful())
                return topicHttpResponse;
            
            var topic = topicHttpResponse.Content as Topic;

            var urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

            var eventsFromApi = !string.IsNullOrEmpty(topic.EventCategory) ? await _stockportApiEventsService.GetEventsByCategory(topic.EventCategory) : new List<Event>();

            var topicViewModel = new TopicViewModel(topic, urlSetting.ToString());

            topicViewModel.EventsFromApi = eventsFromApi?.Take(3).ToList();

            return View(topicViewModel);
        }
    }
}