

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.RSS;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.ArticleStartPageNewsDuration)]
    public class NewsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRssNewsFeedFactory _rssFeedFactory;
        private readonly ILogger<NewsController> _logger;
        private readonly IApplicationConfiguration _config;
        private readonly FeatureToggles _featureToggles;
        private readonly BusinessId _businessId;

        public NewsController(IRepository repository, IProcessedContentRepository processedContentRepository, IRssNewsFeedFactory rssfeedFactory, ILogger<NewsController> logger, IApplicationConfiguration config, BusinessId businessId, FeatureToggles featureToggles)
        {
            _repository = repository;
            _processedContentRepository = processedContentRepository;
            _rssFeedFactory = rssfeedFactory;
            _logger = logger;
            _config = config;
            _businessId = businessId;
            _featureToggles = featureToggles;
        }

        [Route("/news")]
        public async Task<IActionResult> Index([FromQuery] string tag = "", [FromQuery] string category = "",
            [FromQuery] DateTime? datefrom = null, [FromQuery] DateTime? dateto = null)
        {
            var queries = new List<Query>();
            if (!string.IsNullOrEmpty(tag)) queries.Add(new Query("tag", tag));
            if (!string.IsNullOrEmpty(category)) queries.Add(new Query("category", category));
            if (datefrom.HasValue && _featureToggles.NewsDateFilter)
                queries.Add(new Query("datefrom", datefrom.Value.ToString("yyyy-MM-dd")));
            if (dateto.HasValue && _featureToggles.NewsDateFilter)
                queries.Add(new Query("dateto", dateto.Value.ToString("yyyy-MM-dd")));

            var httpResponse = await _repository.Get<Newsroom>(queries: queries);

            if (!httpResponse.IsSuccessful())
                return httpResponse;

            var newsRoom = httpResponse.Content as Newsroom;

            var titleCase = !string.IsNullOrEmpty(category) ? "news" : "News";

            var title = !string.IsNullOrEmpty(tag)
                ? $"{category} {titleCase} about {tag}".Trim()
                : $"{category} {titleCase}".Trim();

            title = datefrom.HasValue && _featureToggles.NewsDateFilter ? $"{title} from {datefrom.Value:MMMM yyyy}" : title;

            var crumbs = new List<Crumb>();
            if (!string.IsNullOrEmpty(tag) || !string.IsNullOrEmpty(category))
                crumbs.Add(new Crumb("News", "news", "news"));

            var urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

            ViewBag.Title = title;
            ViewBag.Tag = tag;

            return View(new NewsroomViewModel(newsRoom, urlSetting.ToString(), title, tag, crumbs));
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