using HtmlAgilityPack;

namespace StockportWebapp.Controllers;

[ExcludeFromCodeCoverage]
[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class ExternalTemplatesController(IFeatureManager featureManager) : Controller
{
    private readonly IFeatureManager _featureManager = featureManager;

    [Route("/ExternalTemplates/Democracy")]
    public async Task<IActionResult> Democracy() =>
        await _featureManager.IsEnabledAsync("ExternalTemplates")
            ? View("Democracy2025")
            : View();
    
    [Route("/ExternalTemplates/DemocracyExtranet")]
    public async Task<IActionResult> DemocracyExtranet() =>
        await _featureManager.IsEnabledAsync("ExtranetExternalTemplates")
            ? View("DemocracyExtranet2025")
            : View();
}