namespace StockportWebapp.Controllers;

public class TPOController(ITPOService tPOService,
                            IApplicationConfiguration config,
                            IFilteredUrl filteredUrl,
                            IFeatureManager featureManager) : Controller
{
    private readonly ITPOService _tPOService = tPOService;
    private readonly IApplicationConfiguration _config = config;
    private readonly IFilteredUrl _filteredUrl = filteredUrl;
    private readonly IFeatureManager _featureManager = featureManager;

    [HttpGet("tpo")]
    public IActionResult RedirectToTPO() =>
        RedirectToActionPermanent("Index");
    

    [HttpGet("tpo/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        if (!await _featureManager.IsEnabledAsync("ShedPage"))
            return NotFound();

        TPOItem tPOItem = await _tPOService.GetTPODataByID(slug);

        return tPOItem is null
            ? NotFound()
            : View(tPOItem);
    }
}