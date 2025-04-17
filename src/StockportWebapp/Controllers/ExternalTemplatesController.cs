namespace StockportWebapp.Controllers;

[ExcludeFromCodeCoverage]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ExternalTemplatesController(IFeatureManager featureManager) : Controller
{
    private readonly IFeatureManager _featureManager = featureManager;

    [Route("/ExternalTemplates/Democracy")]
    public async Task<IActionResult> Democracy() =>
        View("Democracy");
    
    [Route("/ExternalTemplates/DemocracyExtranet")]
    public async Task<IActionResult> DemocracyExtranet() =>
        View("DemocracyExtranet");
}