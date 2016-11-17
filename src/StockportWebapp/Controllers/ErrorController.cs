using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location=ResponseCacheLocation.Any,Duration= Cache.DefaultDuration)]
    public class ErrorController : Controller
    {
        private readonly ILegacyRedirectsManager _legacyRedirectsManager;
        private readonly FeatureToggles _featureToggles;

        public ErrorController(ILegacyRedirectsManager legacyRedirectsManager, FeatureToggles featureToggles)
        {
            _legacyRedirectsManager = legacyRedirectsManager;
            _featureToggles = featureToggles;
        }

        public async Task<IActionResult> Error(string id)
        {
            SetupPageMessage(id);
            return await RedirectIfLegacyUrl(id);
        }

        private async Task<IActionResult> RedirectIfLegacyUrl(string id)
        {
            if (id.Equals("404") && _featureToggles.LegacyUrlRedirects)
            {
                var urlToRedirectLegacyRequestTo = _legacyRedirectsManager.RedirectUrl();
                if (urlToRedirectLegacyRequestTo != string.Empty)
                {
                    return await Task.FromResult(Redirect(urlToRedirectLegacyRequestTo));
                }
            }
            return await Task.FromResult(View());
        }

        private void SetupPageMessage(string id)
        {
            if (id.Equals("404"))
            {
                ViewData["ErrorHeading"] = "Something's missing";
                ViewData["ErrorMessage"] = "Sorry, the page you are looking for cannot be found. "
                                           + "It may have been removed, had its name changed, or is temporarily unavailable.";
            }
            else
            {
                ViewData["ErrorHeading"] = "Something went wrong";
                ViewData["ErrorMessage"] = "An unexpected error occurred and the page you are looking for cannot be found.";
            }
        }
    }
}