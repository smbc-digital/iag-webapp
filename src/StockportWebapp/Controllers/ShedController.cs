namespace StockportWebapp.Controllers;

public class ShedController(ShedService shedService,
                            IApplicationConfiguration config,
                            BusinessId businessId,
                            IFilteredUrl filteredUrl) : Controller
{
    private readonly ShedService _shedService = shedService;
    private readonly IApplicationConfiguration _config = config;
    private readonly BusinessId _businessId = businessId;
    private readonly IFilteredUrl _filteredUrl = filteredUrl;

    [HttpGet("shed")]
    public async Task<IActionResult> Index(List<string> ward,
                                        List<string> listingType,
                                        string id,
                                        string searchTerm,
                                        [FromQuery] int page,
                                        [FromQuery] int pageSize)
    {
        List<ShedItem> results = await _shedService.GetSHEDDataByNameWardsAndListingTypes(searchTerm, ward, listingType);

        ShedViewModel viewModel = new(results);

        viewModel.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(viewModel.CurrentUrl);
        viewModel.AddFilteredUrl(_filteredUrl);

        DoPagination(results, page, viewModel, pageSize);

        viewModel.AppliedFilters = new List<string>();
        viewModel.SearchTerm = searchTerm;

        if (ward is not null && ward.Any())
            viewModel.AppliedFilters.AddRange(ward);

        if (listingType is not null && listingType.Any())
            viewModel.AppliedFilters.AddRange(listingType);

        return View(viewModel);
    }

    [HttpGet("shed/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        string name = slug.Replace("-", " ").ToLower();

        List<ShedItem> allSheds = await _shedService.GetShedDataByName(name);

        ShedItem shed = allSheds.FirstOrDefault(s =>
            s.Name?.ToLower() == name);

        if (shed is null)
            return NotFound();

        return View(shed);
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
                _config.GetEventsDefaultPageSize(_businessId.ToString().Equals("stockroom") ? "stockroom" : "stockportgov")
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