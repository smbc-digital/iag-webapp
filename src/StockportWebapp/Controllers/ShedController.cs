namespace StockportWebapp.Controllers;

public class ShedController(ShedService shedService) : Controller
{
    private readonly ShedService _shedService = shedService;

    [HttpGet("shed")]
    public async Task<IActionResult> Index(string ward, string listingType, string id, string searchTerm)
    {
        List<ShedItem> result = new();
        if (!string.IsNullOrEmpty(ward) || !string.IsNullOrEmpty(listingType))
            result = await _shedService.GetShedData(ward, listingType);
        else if (!string.IsNullOrEmpty(id))
            result = await _shedService.GetShedDataById(id);
        else if (!string.IsNullOrEmpty(searchTerm))
            result = await _shedService.GetShedDataByName(searchTerm);
        
        ShedViewModel viewModel = new(result);
        viewModel.AppliedFilters = new List<string>()
        {
            !string.IsNullOrEmpty(ward) ? ward : string.Empty,
            !string.IsNullOrEmpty(listingType) ? listingType : string.Empty
        };

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
}