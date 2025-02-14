namespace StockportWebapp.Controllers;

public class SearchController(IApplicationConfiguration config,
                            BusinessId businessId,
                            IFeatureManager featureManager) : Controller
{
    private readonly IApplicationConfiguration _config = config;
    private readonly BusinessId _businessId = businessId;
    private readonly IFeatureManager _featureManager = featureManager;

    [Route("/postcode")]
    public async Task<IActionResult> Postcode(string query)
    {
        AppSetting urlSetting = _config.GetPostcodeSearchUrl(_businessId.ToString());

        if (urlSetting.IsValid())
            return await Task.FromResult(Redirect(string.Concat(urlSetting, query)));

        return NotFound();
    }

    [Route("/searchResults")]
    public async Task<IActionResult> SearchResults(string query)
    {
        ViewData["Title"] = "Search results";

        return await _featureManager.IsEnabledAsync("SearchPages") && _businessId.ToString().Equals("stockportgov")
            ? View("SearchResults2025")
            : View();
    }
}