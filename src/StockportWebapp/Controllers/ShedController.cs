namespace StockportWebapp.Controllers;

public class ShedController(IShedService shedService,
                            IApplicationConfiguration config,
                            IFilteredUrl filteredUrl,
                            IFeatureManager featureManager) : Controller
{
    private readonly IShedService _shedService = shedService;
    private readonly IApplicationConfiguration _config = config;
    private readonly IFilteredUrl _filteredUrl = filteredUrl;
    private readonly IFeatureManager _featureManager = featureManager;

    [HttpGet("heritage-assets")]
    public async Task<IActionResult> Index(List<string> ward,
                                        List<string> grade,
                                        string searchTerm,
                                        [FromQuery] int page,
                                        [FromQuery] int pageSize)
    {
        if (!await _featureManager.IsEnabledAsync("ShedPage"))
            return NotFound();

        List<ShedItem> results = await _shedService.GetSHEDDataByNameWardsAndListingTypes(searchTerm, ward, grade);

        ShedViewModel viewModel = new(results)
        {
            SearchTerm = searchTerm,
            AppliedFilters = new List<string>()
        };

        viewModel.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(viewModel.CurrentUrl);
        viewModel.AddFilteredUrl(_filteredUrl);

        DoPagination(results, page, viewModel, pageSize);

        viewModel.AppliedFilters.AddRange(ward ?? Enumerable.Empty<string>());
        viewModel.AppliedFilters.AddRange(grade ?? Enumerable.Empty<string>());

        return View(viewModel);
    }

    [HttpGet("heritage-assets/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        if (!await _featureManager.IsEnabledAsync("ShedPage"))
            return NotFound();

        string name = slug.Replace("-", " ").ToLowerInvariant();

        List<ShedItem> results = await _shedService.GetShedDataByName(name); // I think we can change this to return just one item

        ShedItem shed = results.FirstOrDefault(shedItem => string.Equals(shedItem.Name, name, StringComparison.OrdinalIgnoreCase));

        return shed is null ? NotFound() : View(shed);
    }
    
    private void DoPagination(List<ShedItem> items, int currentPageNumber, ShedViewModel model, int pageSize)
    {
        if (items != null && items.Any())
        {
            var entryViewModels = items.Select(item => new ShedEntryViewModel(item)).ToList();

            PaginatedItems<ShedEntryViewModel> paginatedItems = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                entryViewModels,
                currentPageNumber,
                "results",
                pageSize,
                _config.GetEventsDefaultPageSize("stockportgov")
            );

            model.ShedItems = paginatedItems.Items;
            model.Pagination = paginatedItems.Pagination;
            model.Pagination.CurrentUrl = model.CurrentUrl;
        }
        else
        {
            model.Pagination = new Pagination();
            model.ShedItems = new List<ShedEntryViewModel>();
        }
    }
}