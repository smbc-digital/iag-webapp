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

    [HttpGet("shed")]
    public IActionResult RedirectToHeritageAssets() =>
        RedirectToActionPermanent("Index");
    
    [HttpGet("directories/results/heritage-assets")]
    public async Task<IActionResult> Index(List<string> ward,
                                        List<string> grade,
                                        List<string> types,
                                        string searchTerm,
                                        [FromQuery] int page,
                                        [FromQuery] int pageSize)
    {
        if (!await _featureManager.IsEnabledAsync("ShedPage"))
            return NotFound();

        List<ShedItem> results = await _shedService.GetSHEDDataByNameWardsTypeAndListingTypes(searchTerm, ward, types, grade);

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
        viewModel.AppliedFilters.AddRange(types ?? Enumerable.Empty<string>());

        return View(viewModel);
    }

    [HttpGet("directories/entry/heritage-assets/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        if (!await _featureManager.IsEnabledAsync("ShedPage"))
            return NotFound();

        ShedItem shedItem = await _shedService.GetSHEDDataByHeRef(slug);

        return shedItem is null
            ? NotFound()
            : View(shedItem);
    }
    
    private void DoPagination(List<ShedItem> items, int currentPageNumber, ShedViewModel model, int pageSize)
    {
        if (items is not null && items.Any())
        {
            List<ShedEntryViewModel> entryViewModels = items.Select(item => new ShedEntryViewModel(item)).ToList();

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