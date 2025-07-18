namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class NewsController(IRepository repository,
                            IProcessedContentRepository processedContentRepository,
                            IRssFeedFactory rssfeedFactory,
                            ILogger<NewsController> logger,
                            IApplicationConfiguration config,
                            BusinessId businessId,
                            IFilteredUrl filteredUrl,
                            IFeatureManager featureManager) : Controller
{
    private readonly IRepository _repository = repository;
    private readonly IProcessedContentRepository _processedContentRepository = processedContentRepository;
    private readonly IRssFeedFactory _rssFeedFactory = rssfeedFactory;
    private readonly ILogger<NewsController> _logger = logger;
    private readonly IApplicationConfiguration _config = config;
    private readonly BusinessId _businessId = businessId;
    private readonly IFilteredUrl _filteredUrl = filteredUrl;
    private readonly IFeatureManager _featureManager = featureManager;

    [Route("/news")]
    public async Task<IActionResult> Index(NewsroomViewModel model, [FromQuery] int page, [FromQuery] int pageSize)
    {
        ClearDateErrorsIfNoDates(model);

        List<Query> queries = BuildQueries(model);

        HttpResponse httpResponse = await _repository.Get<Newsroom>(queries: queries);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        Newsroom newsRoom = httpResponse.Content as Newsroom;

        AppSetting urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

        model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(model.CurrentUrl);
        model.AddFilteredUrl(_filteredUrl);

        DoPagination(newsRoom, model, page, pageSize);

        model.AddNews(newsRoom);
        model.AddUrlSetting(urlSetting, model.Newsroom.EmailAlertsTopicId);

        return View(model);
    }

    [ExcludeFromCodeCoverage]
    [Route("/news-articles")]
    public async Task<IActionResult> NewsArticles(NewsroomViewModel model, [FromQuery] int page, [FromQuery] int pageSize)
    {
        if (await _featureManager.IsEnabledAsync("NewsRedesign"))
            return RedirectToAction("Index");

        ClearDateErrorsIfNoDates(model);

        List<Query> queries = BuildQueries(model);

        HttpResponse httpResponse = await _repository.Get<Newsroom>(queries: queries);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        Newsroom newsRoom = httpResponse.Content as Newsroom;

        AppSetting urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

        model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(model.CurrentUrl);
        model.AddFilteredUrl(_filteredUrl);

        List<News> allNews = newsRoom.News?.OrderByDescending(n => n.SunriseDate).ToList() ?? new List<News>();
        List<News> latestArticle = null;

        if (newsRoom.FeaturedNews is not null)
        {
            latestArticle = new() { newsRoom.FeaturedNews };
            allNews = allNews.Where(news => !news.Slug.Equals(newsRoom.FeaturedNews.Slug)).ToList();
        }

        List<News> latestNews = allNews.Take(3).ToList();

        newsRoom.News = newsRoom.CallToAction is null
            ? allNews
            : allNews.Skip(3).ToList();
        
        DoPagination(newsRoom, model, page, pageSize);

        newsRoom.LatestArticle = latestArticle?.Any() is true
            ? CreateNavCardList(latestArticle)
            : null;

        newsRoom.LatestNews = latestNews?.Any() is true
            ? CreateNavCardList(latestNews)
            : null;

        newsRoom.NewsItems = newsRoom.News?.Any() is true
            ? CreateNavCardList(newsRoom.News)
            : null;

        model.AddNews(newsRoom);
        model.AddUrlSetting(urlSetting, model.Newsroom.EmailAlertsTopicId);

        return View(model);
    }

    [ExcludeFromCodeCoverage]
    [Route("/news-archive")]
    public async Task<IActionResult> NewsArchive(NewsroomViewModel model, [FromQuery] int page, [FromQuery] int pageSize)
    {
        if (await _featureManager.IsEnabledAsync("NewsRedesign"))
            return RedirectToAction("Index");

        List<Query> queries = BuildQueries(model);

        HttpResponse httpResponse = await _repository.Get<Newsroom>(slug: "/archive", queries: queries);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        Newsroom newsRoom = httpResponse.Content as Newsroom;

        AppSetting urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

        model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(model.CurrentUrl);
        model.AddFilteredUrl(_filteredUrl);

        DoPagination(newsRoom, model, page, pageSize);

        model.AddNews(newsRoom);
        model.AddUrlSetting(urlSetting, model.Newsroom.EmailAlertsTopicId);

        return View(model);
    }

    [Route("/news/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        HttpResponse initialResponse = await _processedContentRepository.Get<News>(slug);
        IActionResult finalResult = initialResponse;

        if (initialResponse.IsSuccessful())
        {
            ProcessedNews response = initialResponse.Content as ProcessedNews;
            HttpResponse latestNewsResponse = await _repository.GetLatest<List<News>>(7);
            List<News> latestNews = latestNewsResponse.Content as List<News>;
            NewsViewModel newsViewModel = new(response, latestNews);

            ViewBag.CurrentUrl = Request?.GetDisplayUrl();

            finalResult = View(newsViewModel);
        }

        return finalResult;
    }

    [ExcludeFromCodeCoverage]
    [Route("/news-article/{slug}")]
    public async Task<IActionResult> NewsArticle(string slug)
    {
        if (await _featureManager.IsEnabledAsync("NewsRedesign"))
            return RedirectToAction("Detail", new { slug });

        HttpResponse initialResponse = await _processedContentRepository.Get<News>(slug);
        IActionResult finalResult = initialResponse;

        if (initialResponse.IsSuccessful())
        {
            ProcessedNews response = initialResponse.Content as ProcessedNews;
            HttpResponse latestNewsResponse = await _repository.GetLatest<List<News>>(7);
            List<News> latestNews = latestNewsResponse.Content as List<News>;
            NewsViewModel newsViewModel = new(response, latestNews);

            ViewBag.CurrentUrl = Request?.GetDisplayUrl();

            finalResult = View(newsViewModel);
        }

        return finalResult;
    }

    [Route("news/rss")]
    public async Task<IActionResult> Rss()
    {
        HttpResponse httpResponse = await _repository.Get<Newsroom>();
        string host = Request is not null && Request.Host.HasValue ? string.Concat(Request.IsHttps ? "https://" : "http://", Request.Host.Value, "/news/") : string.Empty;

        if (!httpResponse.IsSuccessful())
        {
            _logger.LogDebug("Rss: Http Response not sucessful");

            return httpResponse;
        }

        Newsroom response = httpResponse.Content as Newsroom;
        AppSetting emailFromAppSetting = _config.GetRssEmail(_businessId.ToString());

        string email = emailFromAppSetting.IsValid()
            ? emailFromAppSetting.ToString()
            : string.Empty;

        _logger.LogDebug("Rss: Creating News Feed");

        return await Task.FromResult(Content(_rssFeedFactory.BuildRssFeed(response.News, host, email), "application/rss+xml"));
    }

    private void DoPagination(Newsroom newsRoom, NewsroomViewModel model, int currentPageNumber, int pageSize)
    {
        if (newsRoom is not null && newsRoom.News.Any())
        {
            PaginatedItems<News> paginatedNews = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                newsRoom.News,
                currentPageNumber,
                "articles",
                pageSize,
                _config.GetNewsDefaultPageSize("stockportgov"));

            newsRoom.News = paginatedNews.Items;
            model.Pagination = paginatedNews.Pagination;
            model.Pagination.CurrentUrl = model.CurrentUrl;
        }
        else
            model.Pagination = new Pagination();
    }

    private NavCard ToNavCard(News news)
    {
        DateTime.TryParse(news.PublishingDate, out DateTime sunriseDate);

        return new(
            news.Title,
            $"news-article/{news.Slug}",
            news.Teaser,
            news.ThumbnailImage,
            news.Image,
            string.Empty,
            EColourScheme.Teal,
            string.IsNullOrEmpty(news.PublishingDate) || news.PublishingDate.Equals(DateTime.MinValue.ToString("yyyy-MM-dd"))
                ? news.SunriseDate
                : sunriseDate,
            string.Empty);
    }

    private NavCardList CreateNavCardList(IEnumerable<News> newsItems) =>
        new() { Items = newsItems.Select(ToNavCard).ToList() };

    private void ClearDateErrorsIfNoDates(NewsroomViewModel model)
    {
        if (model.DateFrom is null && model.DateTo is null && string.IsNullOrEmpty(model.DateRange))
        {
            ClearModelStateError("DateFrom");
            ClearModelStateError("DateTo");
        }
    }

    private void ClearModelStateError(string fieldName)
    {
        if (ModelState.TryGetValue(fieldName, out ModelStateEntry state) && state.Errors.Count > 0)
            state.Errors.Clear();
    }
    
    private static List<Query> BuildQueries(NewsroomViewModel model)
    {
        List<Query> queries = new();
        
        if (!string.IsNullOrEmpty(model.Tag))
            queries.Add(new Query("tag", model.Tag));
        
        if (!string.IsNullOrEmpty(model.Category))
            queries.Add(new Query("Category", model.Category));
        
        if (model.DateFrom.HasValue)
            queries.Add(new Query("DateFrom", model.DateFrom.Value.ToString("yyyy-MM-dd")));
        
        if (model.DateTo.HasValue)
            queries.Add(new Query("DateTo", model.DateTo.Value.ToString("yyyy-MM-dd")));
        
        return queries;
    }
}