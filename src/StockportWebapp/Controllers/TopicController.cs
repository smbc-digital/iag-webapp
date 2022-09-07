using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.ContentFactory;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class TopicController : Controller
    {
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;
        private readonly ITopicRepository _topicRepository;

        public TopicController(ITopicRepository repository, IApplicationConfiguration config, BusinessId businessId)
        {
            _config = config;
            _businessId = businessId;
            _topicRepository = repository;
        }

        [Route("/topic/{topicSlug}")]
        public async Task<IActionResult> Index(string topicSlug)
        {
            var topicHttpResponse = await _topicRepository.Get<ProcessedTopic>(topicSlug);

            if (!topicHttpResponse.IsSuccessful())
                return topicHttpResponse;

            var processedTopic = topicHttpResponse.Content as ProcessedTopic;
            
            var urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

            return View(new TopicViewModel(processedTopic, urlSetting.ToString()));
        }
    }
}