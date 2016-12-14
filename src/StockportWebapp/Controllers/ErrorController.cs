using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location=ResponseCacheLocation.Any,Duration= Cache.DefaultDuration)]
    public class ErrorController : Controller
    {
        private readonly ILegacyRedirectsManager _legacyRedirectsManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILegacyRedirectsManager legacyRedirectsManager, IHttpContextAccessor httpContextAccessor, ILogger<ErrorController> logger)
        {
            _legacyRedirectsManager = legacyRedirectsManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> Error(string id)
        {
            SetupPageMessage(id);
            return await RedirectIfLegacyUrl(id);
        }

        private async Task<IActionResult> RedirectIfLegacyUrl(string id)
        {
            if (id.Equals("404"))
            {
                var currentPath = GetCurrentPath(_httpContextAccessor);
                var urlToRedirectLegacyRequestTo = _legacyRedirectsManager.RedirectUrl(currentPath);
                if (!string.IsNullOrEmpty(urlToRedirectLegacyRequestTo))
                {
                    _logger.LogInformation($"A legacy redirect was found - redirecting to {urlToRedirectLegacyRequestTo}");
                    return await Task.FromResult(RedirectPermanent(urlToRedirectLegacyRequestTo));
                }

                _logger.LogInformation($"No legacy url matching current url ({currentPath}) found");
            }
            return await Task.FromResult(View());
        }

        private void SetupPageMessage(string id)
        {
            if (id.Equals("404"))
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

        private static string GetCurrentPath(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Features.Get<IStatusCodeReExecuteFeature>().OriginalPath;
        }
    }
}