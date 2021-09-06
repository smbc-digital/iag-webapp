using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location=ResponseCacheLocation.Any, Duration=Cache.Medium)]
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

        [Route("/error")]
        public IActionResult Error(string id = "404")
        {
            SetupPageMessage(id);
            return RedirectIfLegacyUrl(id);
        }

        private IActionResult RedirectIfLegacyUrl(string id)
        {
            if (id.Equals("404"))
            {
                //var path = GetCurrentPath(_httpContextAccessor);
                var curpath = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                var path = curpath.OriginalPath;
                //var path = System.Web.HttpUtility.UrlDecode(currentPath);
                var urlToRedirectLegacyRequestTo = _legacyRedirectsManager.RedirectUrl(path);
                if (!string.IsNullOrEmpty(urlToRedirectLegacyRequestTo))
                {
                    _logger.LogInformation($"A legacy redirect was found - redirecting to {urlToRedirectLegacyRequestTo}");
                    return RedirectPermanent(urlToRedirectLegacyRequestTo);
                }

                _logger.LogInformation($"No legacy url matching current url ({path}) found");
            }
            return View();
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