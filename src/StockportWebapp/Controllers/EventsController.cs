using StockportWebapp.Models;

namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class EventsController : Controller
{
    private readonly IRepository _repository;
    private readonly IProcessedContentRepository _processedContentRepository;
    private readonly IRssFeedFactory _rssFeedFactory;
    private readonly ILogger<EventsController> _logger;
    private readonly IApplicationConfiguration _config;
    private readonly BusinessId _businessId;
    private readonly IFilteredUrl _filteredUrl;
    private readonly CalendarHelper _helper;
    private readonly IDateCalculator _dateCalculator;
    private readonly IFeatureManager _featureManager;
    private readonly IStockportApiEventsService _stockportApiEventsService;

    public EventsController(
        IRepository repository,
        IProcessedContentRepository processedContentRepository,
        IRssFeedFactory rssFeedFactory,
        ILogger<EventsController> logger,
        IApplicationConfiguration config,
        BusinessId businessId,
        IFilteredUrl filteredUrl,
        CalendarHelper helper,
        IDateCalculator dateCalculator,
        IStockportApiEventsService stockportApiEventsService,
        IFeatureManager featureManager)
    {
        _repository = repository;
        _processedContentRepository = processedContentRepository;
        _rssFeedFactory = rssFeedFactory;
        _logger = logger;
        _config = config;
        _businessId = businessId;
        _filteredUrl = filteredUrl;
        _helper = helper;
        _dateCalculator = dateCalculator;
        _stockportApiEventsService = stockportApiEventsService;
        _featureManager = featureManager;
    }

    [Route("/events")]
    public async Task<IActionResult> Index(EventCalendar eventsCalendar, [FromQuery] int Page, [FromQuery] int pageSize)
    {
        if (ModelState["DateTo"] is not null && ModelState["DateTo"].Errors.Count > 0)
            ModelState["DateTo"].Errors.Clear();

        if (ModelState["DateFrom"] is not null && ModelState["DateFrom"].Errors.Count > 0)
            ModelState["DateFrom"].Errors.Clear();

        if (!string.IsNullOrEmpty(eventsCalendar.Tag)) 
            eventsCalendar.KeepTag = eventsCalendar.Tag;

        eventsCalendar.FromSearch = eventsCalendar.FromSearch 
            || !string.IsNullOrWhiteSpace(eventsCalendar.Category) 
            || !string.IsNullOrWhiteSpace(eventsCalendar.Tag)
            || eventsCalendar.DateFrom != null 
            || eventsCalendar.DateTo != null;

        List<Query> queries = new();
        string dateFormat = "yyyy-MM-dd";

        if (eventsCalendar.DateFrom.HasValue)
            queries.Add(new Query("DateFrom", eventsCalendar.DateFrom.Value.ToString(dateFormat)));
        
        if (eventsCalendar.DateTo.HasValue)
            queries.Add(new Query("DateTo", eventsCalendar.DateTo.Value.ToString(dateFormat)));
        
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

        HttpResponse httpResponse = await _repository.Get<EventResponse>(queries: queries);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        EventResponse eventResponse = httpResponse.Content as EventResponse;

        eventsCalendar.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(eventsCalendar.CurrentUrl);
        eventsCalendar.AddFilteredUrl(_filteredUrl);

        DoPagination(eventsCalendar, Page, eventResponse, pageSize);

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
        eventsCalendar.AddHeroCarouselItems(eventHomeResponse?.Rows?.FirstOrDefault(row => !row.IsLatest)?.Events.Take(5).ToList());

        eventsCalendar.Homepage.NextEvents = eventHomeResponse?.Rows?.FirstOrDefault(row => row.IsLatest)?.Events
            .Select(baseEvent => _stockportApiEventsService.BuildProcessedEvent(baseEvent)).ToList();

        return View(eventsCalendar);
    }

    [Route("/events/free")]
    public async Task<IActionResult> IndexWithFreeEvents(EventCalendar eventsCalendar, [FromQuery] int page, [FromQuery] int pageSize)
    {
        HttpResponse httpFreeEventsResponse = await _repository.Get<EventResponse>("/free");

        if (!httpFreeEventsResponse.IsSuccessful())
            return httpFreeEventsResponse;

        EventResponse freeEvents = (EventResponse)httpFreeEventsResponse.Content;

        eventsCalendar.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(eventsCalendar.CurrentUrl);
        eventsCalendar.AddFilteredUrl(_filteredUrl);

        DoPagination(eventsCalendar, page, freeEvents, pageSize);

        if (freeEvents is not null)
            eventsCalendar.AddEvents(freeEvents.Events);

        HttpResponse httpHomeResponse = await _repository.Get<EventHomepage>();

        if (!httpHomeResponse.IsSuccessful())
            return httpHomeResponse;

        EventHomepage eventHomeResponse = httpHomeResponse.Content as EventHomepage;

        eventsCalendar.Homepage = eventHomeResponse ?? new EventHomepage(new List<Alert>());
        eventsCalendar.AddHeroCarouselItems(eventHomeResponse?.Rows?.FirstOrDefault(row => !row.IsLatest)?.Events.Take(5).ToList());
        eventsCalendar.Homepage.NextEvents = new List<ProcessedEvents>();

        return View("Index", eventsCalendar);
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

    [Route("/events/{slug}")]
    public async Task<IActionResult> Detail(string slug, [FromQuery] DateTime? date = null)
    {
        List<Query> queries = new();

        if (date.HasValue)
            queries.Add(new Query("date", date.Value.ToString("yyyy-MM-dd")));

        HttpResponse httpResponse = await _processedContentRepository.Get<Event>(slug, queries);

        if (httpResponse.IsSuccessful() is not true)
            return httpResponse;

        ProcessedEvents response = httpResponse.Content as ProcessedEvents;

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();
        ViewBag.EventDate = date?.ToString("yyyy-MM-dd") ?? response?.EventDate.ToString("yyyy-MM-dd");

        HttpResponse httpHomeResponse = await _repository.Get<EventHomepage>();

        if (httpHomeResponse.IsSuccessful())
        {
            EventHomepage eventHomeResponse = httpHomeResponse.Content as EventHomepage;

            if (eventHomeResponse?.Alerts is not null)
                response.GlobalAlerts.AddRange(eventHomeResponse.Alerts);
        }

        return View(await _featureManager.IsEnabledAsync("Events") ? "Detail2024" : "Detail", response);
    }
    
    // this is not used!
    [Route("/events/details/{slug}")]
    public async Task<IActionResult> EventDetail(string slug, [FromQuery] DateTime? date = null)
    {
        ProcessedEvents eventItem = await _stockportApiEventsService.GetProcessedEvent(slug, date);

        if (eventItem is null) return NotFound();

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
}