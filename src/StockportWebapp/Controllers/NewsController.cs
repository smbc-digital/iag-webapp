namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class NewsController : Controller
{
    private readonly IRepository _repository;
    private readonly IProcessedContentRepository _processedContentRepository;
    private readonly IRssFeedFactory _rssFeedFactory;
    private readonly ILogger<NewsController> _logger;
    private readonly IApplicationConfiguration _config;
    private readonly BusinessId _businessId;
    private readonly IFilteredUrl _filteredUrl;

    public NewsController(
        IRepository repository,
        IProcessedContentRepository processedContentRepository,
        IRssFeedFactory rssfeedFactory,
        ILogger<NewsController> logger,
        IApplicationConfiguration config,
        BusinessId businessId,
        IFilteredUrl filteredUrl)
    {
        _repository = repository;
        _processedContentRepository = processedContentRepository;
        _rssFeedFactory = rssfeedFactory;
        _logger = logger;
        _config = config;
        _businessId = businessId;
        _filteredUrl = filteredUrl;
    }

    [Route("/news")]
    public async Task<IActionResult> Index(NewsroomViewModel model, [FromQuery] int page, [FromQuery] int pageSize)
    {
        if (model.DateFrom == null && model.DateTo == null && string.IsNullOrEmpty(model.DateRange))
        {
            if (ModelState["DateTo"] != null && ModelState["DateTo"].Errors.Count > 0) ModelState["DateTo"].Errors.Clear();
            if (ModelState["DateFrom"] != null && ModelState["DateFrom"].Errors.Count > 0) ModelState["DateFrom"].Errors.Clear();
        }

        var queries = new List<Query>();
        if (!string.IsNullOrEmpty(model.Tag)) queries.Add(new Query("tag", model.Tag));
        if (!string.IsNullOrEmpty(model.Category)) queries.Add(new Query("Category", model.Category));
        if (model.DateFrom.HasValue) queries.Add(new Query("DateFrom", model.DateFrom.Value.ToString("yyyy-MM-dd")));
        if (model.DateTo.HasValue) queries.Add(new Query("DateTo", model.DateTo.Value.ToString("yyyy-MM-dd")));

        var httpResponse = await _repository.Get<Newsroom>(queries: queries);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        var newsRoom = httpResponse.Content as Newsroom;

        var urlSetting = _config.GetEmailAlertsNewSubscriberUrl(_businessId.ToString());

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
        IActionResult finalResult = (IActionResult)initialResponse;

        if (initialResponse.IsSuccessful())
        {
            var response = initialResponse.Content as ProcessedNews;

            var latestNewsResponse = await _repository.GetLatest<List<News>>(7);
            var latestNews = latestNewsResponse.Content as List<News>;
            var newsViewModel = new NewsViewModel(response, latestNews);

            ViewBag.CurrentUrl = Request?.GetDisplayUrl();

            finalResult = View(newsViewModel);
        }

        return finalResult;
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

    private void DoPagination(Newsroom newsRoom, NewsroomViewModel model, int currentPageNumber, int pageSize)
    {
        if (newsRoom != null && newsRoom.News.Any())
        {
            var paginatedNews = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
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
        {
            model.Pagination = new Pagination();
        }
    }
}