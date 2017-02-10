using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.RSS;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class NewsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRssFeedFactory _rssFeedFactory;
        private readonly ILogger<NewsController> _logger;
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;

        public NewsController(IRepository repository, IProcessedContentRepository processedContentRepository, IRssFeedFactory rssfeedFactory, ILogger<NewsController> logger, IApplicationConfiguration config, BusinessId businessId)
        {
            _repository = repository;
            _processedContentRepository = processedContentRepository;
            _rssFeedFactory = rssfeedFactory;
            _logger = logger;
            _config = config;
            _businessId = businessId;
        }

        [Route("/news")]
        public async Task<IActionResult> Index([FromQuery] string tag = "", [FromQuery] string category = "",
            [FromQuery] DateTime? datefrom = null, [FromQuery] DateTime? dateto = null)
        {
            var queries = new List<Query>();
            if (!string.IsNullOrEmpty(tag)) queries.Add(new Query("tag", tag)); 
            if (!string.IsNullOrEmpty(category)) queries.Add(new Query("category", category)); 
            if (datefrom.HasValue) queries.Add(new Query("datefrom", datefrom.Value.ToString("yyyy-MM-dd")));
            if (dateto.HasValue)  queries.Add(new Query("dateto", dateto.Value.ToString("yyyy-MM-dd")));

            var httpResponse = await _repository.Get<Newsroom>(queries: queries);

            if (!httpResponse.IsSuccessful())
                return httpResponse;

            var newsRoom = httpResponse.Content as Newsroom;                          
            var urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

            return View(new NewsroomViewModel(newsRoom, urlSetting.ToString()));
        }


        [Route("/news/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var httpResponse = await _processedContentRepository.Get<News>(slug);

            if (!httpResponse.IsSuccessful()) return httpResponse;

            var response = httpResponse.Content as ProcessedNews;
            var latestNewsResponse = await _repository.Get<List<News>>("7");
            var latestNews = latestNewsResponse.Content as List<News>;
            var newsViewModel = new NewsViewModel(response, latestNews);

            ViewBag.CurrentUrl = Request?.GetUri();

            return View(newsViewModel);
        }

        [Route("news/rss")]
        public async Task<IActionResult> Rss()
        {
            var httpResponse = await _repository.Get<Newsroom>();

            var host = Request != null && Request.Host.HasValue ? string.Concat(Request.IsHttps ? "https://" : "http://", Request.Host.Value, "/news/") : string.Empty;

            if (!httpResponse.IsSuccessful())
            {
                _logger.LogDebug("Rss: Http Response not sucessful");
                return httpResponse;
            }

            var response = httpResponse.Content as Newsroom;
            var emailFromAppSetting = _config.GetRssEmail(_businessId.ToString());
            var email = emailFromAppSetting.IsValid() ? emailFromAppSetting.ToString() : string.Empty;

            _logger.LogDebug("Rss: Creating News Feed");
            return await Task.FromResult(Content(_rssFeedFactory.BuildRssFeed(response.News, host, email), "application/rss+xml"));
        }
    }
}