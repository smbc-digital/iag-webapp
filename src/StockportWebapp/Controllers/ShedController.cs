namespace StockportWebapp.Controllers;

[Route("[controller]")]
public class ShedController(ShedService shedService) : Controller
{
    private readonly ShedService _shedService = shedService;

    public async Task<IActionResult> Index(string ward, string listingType)
    {
        List<ShedItem> result = await _shedService.GetShedData(ward, listingType);
        
        ShedViewModel viewModel = new(result);

        return View(viewModel);
    }

    [HttpGet("by-id")]
    public async Task<IActionResult> GetShedDataById([FromQuery] string id)
    {
        List<ShedItem> result = await _shedService.GetShedDataById(id);
        return View(result);
    }

    [HttpGet("by-name")]
    public async Task<IActionResult> GetShedDataByName([FromQuery] string name)
    {
        List<ShedItem> result = await _shedService.GetShedDataByName(name);
        return View(result);
    }
}