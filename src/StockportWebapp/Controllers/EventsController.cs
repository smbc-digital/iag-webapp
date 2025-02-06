namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class EventsController(IRepository repository,
                            IProcessedContentRepository processedContentRepository,
                            IRssFeedFactory rssFeedFactory,
                            ILogger<EventsController> logger,
                            IApplicationConfiguration config,
                            BusinessId businessId,
                            IFilteredUrl filteredUrl,
                            CalendarHelper helper,
                            IDateCalculator dateCalculator,
                            IStockportApiEventsService stockportApiEventsService,
                            IFeatureManager featureManager) : Controller
{
    private readonly IRepository _repository = repository;
    private readonly IProcessedContentRepository _processedContentRepository = processedContentRepository;
    private readonly IRssFeedFactory _rssFeedFactory = rssFeedFactory;
    private readonly ILogger<EventsController> _logger = logger;
    private readonly IApplicationConfiguration _config = config;
    private readonly BusinessId _businessId = businessId;
    private readonly IFilteredUrl _filteredUrl = filteredUrl;
    private readonly CalendarHelper _helper = helper;
    private readonly IDateCalculator _dateCalculator = dateCalculator;
    private readonly IFeatureManager _featureManager = featureManager;
    private readonly IStockportApiEventsService _stockportApiEventsService = stockportApiEventsService;

    [Route("/events")]
    public async Task<IActionResult> Index(EventCalendar eventsCalendar, [FromQuery] int page, [FromQuery] int pageSize)
    {
        if (ModelState["DateTo"] is not null && ModelState["DateTo"].Errors.Count > 0)
            ModelState["DateTo"].Errors.Clear();

        if (ModelState["DateFrom"] is not null && ModelState["DateFrom"].Errors.Count > 0)
            ModelState["DateFrom"].Errors.Clear();

        if (!string.IsNullOrEmpty(eventsCalendar.Tag))
            eventsCalendar.KeepTag = eventsCalendar.Tag;

        eventsCalendar.FromSearch = eventsCalendar.IsFromSearch();
        eventsCalendar.ShouldScroll = eventsCalendar.ShouldScroll;
        
        List<Query> queries = GetEventsFilterQueries(eventsCalendar);
        HttpResponse httpResponse = await _repository.Get<EventResponse>(queries: queries);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        EventResponse eventResponse = httpResponse.Content as EventResponse;

        eventsCalendar.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(eventsCalendar.CurrentUrl);
        eventsCalendar.AddFilteredUrl(_filteredUrl);

        DoPagination(eventsCalendar, page, eventResponse, pageSize);

        if (eventResponse is not null)
        {
            eventsCalendar.AddEvents(eventResponse.Events);
            eventsCalendar.AddCategories(eventResponse.Categories);
        }

        HttpResponse httpHomeResponse = await _repository.Get<EventHomepage>();

        if (!httpHomeResponse.IsSuccessful())
            return httpHomeResponse;

        EventHomepage eventHomeResponse = httpHomeResponse.Content as EventHomepage;

        eventsCalendar.Homepage = eventHomeResponse ?? new EventHomepage(new List<Alert>());
        eventsCalendar.AddCarouselContents(eventHomeResponse?.Rows?.FirstOrDefault(row => !row.IsLatest)?.Events.Take(5).ToList());

        if (!eventsCalendar.FromSearch)
            eventsCalendar.Homepage.NextEvents = eventHomeResponse?.Rows?.FirstOrDefault(row => row.IsLatest)?.Events
                .Select(_stockportApiEventsService.BuildProcessedEvent).ToList();

        return View("Index2024", eventsCalendar);
    }

    // This is the healthy stockport filtered events homepage
    [Route("/events/category/{category}")]
    public async Task<IActionResult> IndexWithCategory(string category, [FromQuery] int page, [FromQuery] int pageSize)
    {
        List<EventCategory> categories = await _stockportApiEventsService.GetEventCategories();
        EventResultsViewModel viewModel = new() { Title = category };
        List<Event> events = await _stockportApiEventsService.GetEventsByCategory(category, false);

        if (events is null || !events.Any())
            return View("Index", viewModel);

        EventCategory eventCategory = categories.FirstOrDefault(c => c.Slug.Equals(category));

        viewModel.Title = eventCategory is not null ? eventCategory.Name : category;
        viewModel.Events = events;

        viewModel.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(viewModel.CurrentUrl);
        viewModel.AddFilteredUrl(_filteredUrl);

        DoPagination(viewModel, page, pageSize);

        return View("Index", viewModel);
    }

    [Route("/events/{slug}")]
    public async Task<IActionResult> Detail(string slug, [FromQuery] DateTime? date = null)
    {
        List<Query> queries = new();

        if (date.HasValue)
            queries.Add(new Query("date", date.Value.ToString("yyyy-MM-dd")));

        HttpResponse httpResponse = await _processedContentRepository.Get<Event>(slug, queries);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        ProcessedEvents response = httpResponse.Content as ProcessedEvents;

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();
        ViewBag.EventDate = date?.ToString("yyyy-MM-dd") ?? response?.EventDate.ToString("yyyy-MM-dd");

        // review this when we look at events as a whole
        HttpResponse httpHomeResponse = await _repository.Get<EventHomepage>();

        if (httpHomeResponse.IsSuccessful())
        {
            EventHomepage eventHomeResponse = httpHomeResponse.Content as EventHomepage;

            if (eventHomeResponse?.Alerts is not null)
                response.GlobalAlerts.AddRange(eventHomeResponse.Alerts);
        }

        return View("Detail2024", response);
    }
    
    // This is used for Healthy Stockport only
    [Route("/events/details/{slug}")]
    public async Task<IActionResult> EventDetail(string slug, [FromQuery] DateTime? date = null)
    {
        ProcessedEvents eventItem = await _stockportApiEventsService.GetProcessedEvent(slug, date);

        if (eventItem is null)
            return NotFound();

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        if (date is not null || date.Equals(DateTime.MinValue))
            ViewBag.Eventdate = date.Value.ToString("yyyy-MM-dd");
        else
            ViewBag.Eventdate = eventItem?.EventDate.ToString("yyyy-MM-dd");

        return View("Detail", eventItem);
    }

    [Route("events/rss")]
    public async Task<IActionResult> Rss()
    {
        HttpResponse httpResponse = await _repository.Get<EventResponse>();

        string host = Request is not null && Request.Host.HasValue ?
            string.Concat(Request.IsHttps ? "https://" : "http://", Request.Host.Value, "/events/") :
            string.Empty;

        if (!httpResponse.IsSuccessful())
        {
            _logger.LogDebug("Rss: Http Response not sucessful");

            return httpResponse;
        }

        EventResponse response = httpResponse.Content as EventResponse;
        AppSetting emailFromAppSetting = _config.GetRssEmail(_businessId.ToString());
        string email = emailFromAppSetting.IsValid() ? emailFromAppSetting.ToString() : string.Empty;

        _logger.LogDebug("Rss: Creating News Feed");
        
        return await Task.FromResult(Content(_rssFeedFactory.BuildRssFeed(response.Events, host, email), "application/rss+xml"));
    }

    [Route("events/add-to-calendar")]
    public IActionResult AddToCalendar(string type,
                                    string eventUrl,
                                    string slug,
                                    DateTime eventDate,
                                    string name,
                                    string location,
                                    string startTime,
                                    string endTime,
                                    string description,
                                    string summary)
    {
        if (string.IsNullOrEmpty(type))
            return NotFound();

        Event eventItem = new()
        {
            Slug = slug,
            EventDate = eventDate,
            Title = name,
            Location = location,
            StartTime = startTime,
            EndTime = endTime,
            Description = description,
            Teaser = summary
        };

        if (type.Equals("google") || type.Equals("yahoo"))
            return Redirect(_helper.GetCalendarUrl(eventItem, eventUrl, type));

        if (type.Equals("windows") || type.Equals("apple"))
            return File(Encoding.UTF8.GetBytes(_helper.GetIcsText(eventItem, eventUrl)), "text/calendar", slug + ".ics");

        return Ok();
    }

    private static List<Query> GetEventsFilterQueries(EventCalendar eventsCalendar)
    {
        List<Query> queries = new();
        string dateFormat = "yyyy-MM-dd";

        DateTime? dateFrom = null;
        DateTime? dateTo = null;

        DateTime today = DateTime.Today;
        DateTime endOfWeek = today.AddDays(DayOfWeek.Saturday - today.DayOfWeek + 1);
        DateTime endOfMonth = new(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        DateTime startOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(1);
        DateTime endOfNextMonth = new(startOfNextMonth.Year, startOfNextMonth.Month, DateTime.DaysInMonth(startOfNextMonth.Year, startOfNextMonth.Month));
        DateTime dateValue;

        if (!string.IsNullOrEmpty(eventsCalendar.DateSelection))
            switch (eventsCalendar.DateSelection)
            {
                case "today":
                    dateFrom = today;
                    dateTo = today;
                    break;
                case "thisWeek":
                    dateFrom = today;
                    dateTo = endOfWeek;
                    break;
                case "thisMonth":
                    dateFrom = today;
                    dateTo = endOfMonth;
                    break;
                case "nextMonth":
                    dateFrom = startOfNextMonth;
                    dateTo = endOfNextMonth;
                    break;
                default:
                    DateTime.TryParse(eventsCalendar.DateSelection, out dateValue);
                    dateFrom = dateValue;
                    dateTo = dateValue;
                    break;
            }

        if (eventsCalendar.DateFrom.HasValue)
            queries.Add(new Query("DateFrom", eventsCalendar.DateFrom.Value.ToString(dateFormat)));
        else if (dateFrom is not null)
            queries.Add(new Query("DateFrom", dateFrom.Value.ToString(dateFormat)));

        if (eventsCalendar.DateTo.HasValue)
            queries.Add(new Query("DateTo", eventsCalendar.DateTo.Value.ToString(dateFormat)));
        else if (dateTo is not null)
            queries.Add(new Query("DateTo", dateTo.Value.ToString(dateFormat)));

        if (!string.IsNullOrWhiteSpace(eventsCalendar.Category))
            queries.Add(new Query("Category", eventsCalendar.Category));

        if (!string.IsNullOrWhiteSpace(eventsCalendar.Tag))
            queries.Add(new Query("tag", eventsCalendar.Tag));

        if (eventsCalendar.Price is not null)
            queries.Add(new Query("price", string.Join(",", eventsCalendar.Price)));

        if (!eventsCalendar.Longitude.Equals(0))
            queries.Add(new Query("longitude", string.Join(",", eventsCalendar.Longitude)));

        if (!eventsCalendar.Latitude.Equals(0))
            queries.Add(new Query("latitude", string.Join(",", eventsCalendar.Latitude)));

        if (eventsCalendar.Free)
            queries.Add(new Query("free", "true"));

        return queries;
    }

    private void DoPagination(EventCalendar model, int currentPageNumber, EventResponse eventResponse, int pageSize)
    {
        if (eventResponse is not null && eventResponse.Events.Any())
        {
            PaginatedItems<Event> paginatedEvents = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                eventResponse.Events,
                currentPageNumber,
                "events",
                pageSize,
                _config.GetEventsDefaultPageSize(_businessId.ToString().Equals("stockroom") ? "stockroom" : "stockportgov"));

            eventResponse.Events = paginatedEvents.Items;
            model.Pagination = paginatedEvents.Pagination;
            model.Pagination.CurrentUrl = model.CurrentUrl;
        }
        else
        {
            model.Pagination = new Pagination();
        }
    }

    // This is used for Healthy Stockport only
    private void DoPagination(EventResultsViewModel model, int currentPageNumber, int pageSize)
    {
        if (model is not null && model.Events.Any())
        {
            PaginatedItems<Event> paginatedEvents = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                model.Events,
                currentPageNumber,
                "events",
                pageSize,
                _config.GetEventsDefaultPageSize("stockportgov"));

            model.Events = paginatedEvents.Items;
            model.Pagination = paginatedEvents.Pagination;
            model.Pagination.CurrentUrl = model.CurrentUrl;
        }
        else
        {
            model.Pagination = new Pagination();
        }
    }
}