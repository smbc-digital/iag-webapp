using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class TopicController : Controller
    {
        private readonly IRepository _repository;
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;
        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;

        public TopicController(IRepository repository,
                               IApplicationConfiguration config,
                               BusinessId businessId, ISimpleTagParserContainer tagParserContainer,
                               MarkdownWrapper markdownWrapper)
        {
            _repository = repository;
            _config = config;
            _businessId = businessId;
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
        }

        [Route("/topic/{topicSlug}")]
        public async Task<IActionResult> Index(string topicSlug)
        {
            var topicHttpResponse = await _repository.Get<Topic>(topicSlug);

            if (!topicHttpResponse.IsSuccessful())
                return topicHttpResponse;

            var topic = topicHttpResponse.Content as Topic;

            var urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

            var body = _markdownWrapper.ConvertToHtml(topic.Body ?? "");
            topic.Body = _tagParserContainer.ParseAll(body, topic.Title);

            return View(new TopicViewModel(topic, urlSetting.ToString()));
        }
    }
}