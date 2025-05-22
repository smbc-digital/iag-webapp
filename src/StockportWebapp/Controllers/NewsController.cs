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
        if (model.DateFrom is null && model.DateTo is null && string.IsNullOrEmpty(model.DateRange))
        {
            if (ModelState["DateTo"] is not null && ModelState["DateTo"].Errors.Count > 0)
                ModelState["DateTo"].Errors.Clear();

            if (ModelState["DateFrom"] is not null && ModelState["DateFrom"].Errors.Count > 0)
                ModelState["DateFrom"].Errors.Clear();
        }

        List<Query> queries = new();
        if (!string.IsNullOrEmpty(model.Tag))
            queries.Add(new Query("tag", model.Tag));
        
        if (!string.IsNullOrEmpty(model.Category))
            queries.Add(new Query("Category", model.Category));
        
        if (model.DateFrom.HasValue)
            queries.Add(new Query("DateFrom", model.DateFrom.Value.ToString("yyyy-MM-dd")));
        
        if (model.DateTo.HasValue)
            queries.Add(new Query("DateTo", model.DateTo.Value.ToString("yyyy-MM-dd")));

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
    [Route("/news-articles2")]
    public async Task<IActionResult> NewsArticles2(NewsroomViewModel model, [FromQuery] int page, [FromQuery] int pageSize)
    {
        if (await _featureManager.IsEnabledAsync("NewsRedesign"))
            return RedirectToAction("Index");
        
        return RedirectToAction("NewsArticles");
    }

    [ExcludeFromCodeCoverage]
    [Route("/news-articles")]
    public async Task<IActionResult> NewsArticles(NewsroomViewModel model, [FromQuery] int page, [FromQuery] int pageSize)
    {
        if (await _featureManager.IsEnabledAsync("NewsRedesign"))
            return RedirectToAction("Index");
        
        if (model.DateFrom is null && model.DateTo is null && string.IsNullOrEmpty(model.DateRange))
        {
            if (ModelState["DateTo"] is not null && ModelState["DateTo"].Errors.Count > 0)
                ModelState["DateTo"].Errors.Clear();

            if (ModelState["DateFrom"] is not null && ModelState["DateFrom"].Errors.Count > 0)
                ModelState["DateFrom"].Errors.Clear();
        }

        List<Query> queries = new();
        if (!string.IsNullOrEmpty(model.Tag))
            queries.Add(new Query("tag", model.Tag));
        
        if (!string.IsNullOrEmpty(model.Category))
            queries.Add(new Query("Category", model.Category));
        
        if (model.DateFrom.HasValue)
            queries.Add(new Query("DateFrom", model.DateFrom.Value.ToString("yyyy-MM-dd")));
        
        if (model.DateTo.HasValue)
            queries.Add(new Query("DateTo", model.DateTo.Value.ToString("yyyy-MM-dd")));

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
            latestArticle = new() { newsRoom.FeaturedNews };

        if (newsRoom.FeaturedNews is not null)
            allNews = allNews.Where(news => !news.Slug.Equals(newsRoom.FeaturedNews.Slug)).ToList();

        List<News> latestNews = allNews.Take(3).ToList();

        newsRoom.News = allNews.Skip(3).ToList();

        DoPagination(newsRoom, model, page, pageSize);

        if (latestArticle is not null && latestArticle.Any())
            newsRoom.LatestArticle = new NavCardList() { Items = latestArticle.Select(ToNavCard).ToList() };

        if (latestNews is not null && latestNews.Any())
            newsRoom.LatestNews = new NavCardList() { Items = latestNews.Select(ToNavCard).ToList() };

        if (newsRoom.News is not null && newsRoom.News.Any())
            newsRoom.NewsItems = new NavCardList() { Items = newsRoom.News.Select(ToNavCard).ToList() };

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
        
        if (model.DateFrom is null && model.DateTo is null && string.IsNullOrEmpty(model.DateRange))
        {
            if (ModelState["DateTo"] is not null && ModelState["DateTo"].Errors.Count > 0)
                ModelState["DateTo"].Errors.Clear();

            if (ModelState["DateFrom"] is not null && ModelState["DateFrom"].Errors.Count > 0)
                ModelState["DateFrom"].Errors.Clear();
        }

        List<Query> queries = new();
        if (!string.IsNullOrEmpty(model.Tag))
            queries.Add(new Query("tag", model.Tag));
        
        if (!string.IsNullOrEmpty(model.Category))
            queries.Add(new Query("Category", model.Category));
        
        if (model.DateFrom.HasValue)
            queries.Add(new Query("DateFrom", model.DateFrom.Value.ToString("yyyy-MM-dd")));
        
        if (model.DateTo.HasValue)
            queries.Add(new Query("DateTo", model.DateTo.Value.ToString("yyyy-MM-dd")));
        
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
                "news articles",
                pageSize,
                _config.GetNewsDefaultPageSize("stockportgov"));

            newsRoom.News = paginatedNews.Items;
            model.Pagination = paginatedNews.Pagination;
            model.Pagination.CurrentUrl = model.CurrentUrl;
        }
        else
            model.Pagination = new Pagination();
    }

    private NavCard ToNavCard(News news) => new(
        news.Title,
        $"news-article/{news.Slug}",
        news.Teaser,
        news.ThumbnailImage,
        news.Image,
        string.Empty,
        EColourScheme.Teal,
        news.SunriseDate,
        string.Empty
    );
}