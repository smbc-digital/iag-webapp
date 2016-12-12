using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Controllers;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Middleware
{
    public class OldEventsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FeatureToggles _featureToggles;

        public OldEventsMiddleware(RequestDelegate next, FeatureToggles featureToggles)
        {
            _next = next;
            _featureToggles = featureToggles;
        }

        public Task Invoke(HttpContext context, ILegacyRedirectsManager legacyRedirectsManager)
        {
            if (context.Request.Path.HasValue 
                && (context.Request.Path.Value.Equals("/events") || context.Request.Path.Value.StartsWith("/events/")) 
                && !_featureToggles.EventCalendar)
            {
                var urlToRedirectLegacyRequestTo = legacyRedirectsManager.RedirectUrl(context.Request.Path.Value);

                if (!string.IsNullOrEmpty(urlToRedirectLegacyRequestTo))
                {
                    context.Response.Redirect(urlToRedirectLegacyRequestTo);
                }
                else
                {
                    context.Response.Redirect("/Error/404");
                }
            }

            return _next(context);
        }
    }
}
