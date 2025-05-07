namespace StockportWebapp.Controllers;

[ExcludeFromCodeCoverage]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ExternalTemplatesController() : Controller
{

    [Route("/ExternalTemplates/Democracy")]
    public IActionResult Democracy() =>
        View("Democracy");

    [Route("/ExternalTemplates/DemocracyExtranet")]
    public IActionResult DemocracyExtranet() =>
        View("DemocracyExtranet");
}