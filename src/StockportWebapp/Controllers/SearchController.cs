namespace StockportWebapp.Controllers;

public class SearchController : Controller
{
    private readonly IApplicationConfiguration _config;
    private readonly BusinessId _businessId;

    public SearchController(IApplicationConfiguration config, BusinessId businessId)
    {
        _businessId = businessId;
        _config = config;
    }

    [Route("/postcode")]
    public async Task<IActionResult> Postcode(string query)
    {
        var urlSetting = _config.GetPostcodeSearchUrl(_businessId.ToString());
        if (urlSetting.IsValid())
        {
            var url = string.Concat(urlSetting, query);
            return await Task.FromResult(Redirect(url));
        }
        return NotFound();
    }

    [Route("/searchResults")]
    public async Task<IActionResult> SearchResults(string query)
    {
        ViewData["Title"] = "Search results";
        return View();
    }
}