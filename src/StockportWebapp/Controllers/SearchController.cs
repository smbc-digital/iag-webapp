namespace StockportWebapp.Controllers;

public class SearchController(IApplicationConfiguration config,
                            BusinessId businessId) : Controller
{
    private readonly IApplicationConfiguration _config = config;
    private readonly BusinessId _businessId = businessId;

    [Route("/postcode")]
    public async Task<IActionResult> Postcode(string query)
    {
        AppSetting urlSetting = _config.GetPostcodeSearchUrl(_businessId.ToString());

        if (urlSetting.IsValid())
            return await Task.FromResult(Redirect(string.Concat(urlSetting, query)));

        return NotFound();
    }

    [Route("/searchResults")]
    public IActionResult SearchResults(string query)
    {
        ViewData["Title"] = "Search results";
        
        return View();
    }
}