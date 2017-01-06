using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Middleware
{   
    public class SecurityHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FeatureToggles _featureToggles;

        public SecurityHeaderMiddleware(RequestDelegate next, FeatureToggles featureToggles)
        {
            _next = next;
            _featureToggles = featureToggles;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var host = httpContext.Request.Host.Value.ToLower();
            var isRemoteHost = host.StartsWith("www") || host.StartsWith("int-") || host.StartsWith("qa-") || host.StartsWith("stage-");

            if (_featureToggles.SecurityHeaders && isRemoteHost)
            {
                httpContext.Response.Headers.Add("Strict-Transport-Security", new[] { "max-age=31536000" });
            }

            return _next(httpContext);
        }
    }
}
