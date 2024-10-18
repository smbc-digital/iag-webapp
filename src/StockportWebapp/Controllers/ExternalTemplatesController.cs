namespace StockportWebapp.Controllers;

[ExcludeFromCodeCoverage]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ExternalTemplatesController : Controller
{
    [Route("/ExternalTemplates/Democracy")]
    public IActionResult Democracy()
    {
        return View();
    }

    [Route("/ExternalTemplates/DemocracyExtranet")]
    public IActionResult DemocracyExtranet()
    {
        return View();
    }
}
