using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Models;

namespace StockportWebapp.Scheduler
{
    public class RedirectMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ShortUrlRedirects _shortUrlRedirects;
        private readonly ILogger<RedirectMiddleware> _logger;

        public RedirectMiddleware(RequestDelegate next, ShortUrlRedirects shortUrlRedirects, ILogger<RedirectMiddleware> logger)
        {
            _next = next;
            _shortUrlRedirects = shortUrlRedirects;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, BusinessId businessId)
        {
            var path = context.Request.Path;
            if (_shortUrlRedirects.Redirects.ContainsKey(businessId.ToString()) && _shortUrlRedirects.Redirects[businessId.ToString()].ContainsKey(path))
            {
                var redirectTo = _shortUrlRedirects.Redirects[businessId.ToString()][path];
                _logger.LogInformation($"Redirecting from: {path}, to: {redirectTo}");
                context.Response.Redirect(redirectTo);
                context.Response.Headers["Cache-Control"] = "public, max-age=" + Cache.RedirectCacheDuration;
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
