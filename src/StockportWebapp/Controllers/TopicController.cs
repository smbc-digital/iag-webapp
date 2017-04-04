using System.Threading.Tasks;
using StockportWebapp.Repositories;
using StockportWebapp.Models;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class TopicController : Controller
    {
        private readonly IRepository _repository;
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;

        public TopicController(IRepository repository, IApplicationConfiguration config, BusinessId businessId)
        {
            _repository = repository;
            _config = config;
            _businessId = businessId;
        }

        [Route("/topic/{topicSlug}")]
        public async Task<IActionResult> Index(string topicSlug)
        {
            var topicHttpResponse = await _repository.Get<Topic>(topicSlug);

            if (!topicHttpResponse.IsSuccessful())
                return topicHttpResponse;
            
            var topic = topicHttpResponse.Content as Topic;

            var urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

            return View(new TopicViewModel(topic, urlSetting.ToString()));
        }
    }
}