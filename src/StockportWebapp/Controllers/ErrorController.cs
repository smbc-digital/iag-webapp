namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class ErrorController(ILegacyRedirectsManager legacyRedirectsManager, ILogger<ErrorController> logger) : Controller
{
    private readonly ILegacyRedirectsManager _legacyRedirectsManager = legacyRedirectsManager;
    private readonly ILogger<ErrorController> _logger = logger;

    [Route("/error")]
    public async Task<IActionResult> Error()
    {
        int statusCode = HttpContext.Response.StatusCode;
        SetupPageMessage(statusCode);

        return await RedirectIfLegacyUrl(statusCode);
    }

    private async Task<IActionResult> RedirectIfLegacyUrl(int statusCode)
    {
        if (statusCode.Equals(404))
        {
            string path = HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath;
            string urlToRedirectLegacyRequestTo = await _legacyRedirectsManager.RedirectUrl(path);

            if (!string.IsNullOrEmpty(urlToRedirectLegacyRequestTo))
            {
                _logger.LogInformation($"A legacy redirect was found - redirecting to {urlToRedirectLegacyRequestTo}");
                
                return RedirectPermanent(urlToRedirectLegacyRequestTo);
            }

            _logger.LogInformation($"No legacy url matching current url ({path}) found");
        }

        return View();
    }

    private void SetupPageMessage(int statusCode)
    {
        if (statusCode.Equals(404))
        {
            ViewData["ErrorHeading"] = "Something's missing";
            ViewData["ErrorMessage"] = "Sorry, the page you are looking for cannot be found. " +
                                       "It may have been removed, had its name changed, or is temporarily unavailable.";
        }
        else
        {
            ViewData["ErrorHeading"] = "Something went wrong";
            ViewData["ErrorMessage"] = "An unexpected error occurred and the page you are looking for cannot be found.";
        }
    }
}