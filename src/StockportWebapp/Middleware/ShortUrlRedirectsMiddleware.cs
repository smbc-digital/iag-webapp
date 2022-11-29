using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Middleware
{
    public class ShortUrlRedirectsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ShortUrlRedirects _shortUrlRedirects;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly ILogger<ShortUrlRedirectsMiddleware> _logger;
        private readonly IRepository _repository;

        public ShortUrlRedirectsMiddleware(RequestDelegate next,
            ShortUrlRedirects shortUrlRedirects,
            LegacyUrlRedirects legacyUrlRedirects,
            ILogger<ShortUrlRedirectsMiddleware> logger,
            IRepository repository)
        {
            _next = next;
            _shortUrlRedirects = shortUrlRedirects;
            _legacyUrlRedirects = legacyUrlRedirects;
            _logger = logger;
            _repository = repository;
        }

        public async Task Invoke(HttpContext context, BusinessId businessId)
        {
            var path = context.Request.Path;
            if (_shortUrlRedirects.HasExpired())
            {
                var response = await _repository.GetRedirects();
                var redirects = response.Content as Redirects;
                _shortUrlRedirects.Redirects = redirects.ShortUrlRedirects;
                _shortUrlRedirects.LastUpdated = System.DateTime.Now;
                _legacyUrlRedirects.Redirects = redirects.LegacyUrlRedirects;
                _legacyUrlRedirects.LastUpdated = System.DateTime.Now;
            }

            if (_shortUrlRedirects.Redirects.ContainsKey(businessId.ToString()) && _shortUrlRedirects.Redirects[businessId.ToString()].ContainsKey(path))
            {
                var redirectTo = _shortUrlRedirects.Redirects[businessId.ToString()][path];
                _logger.LogInformation($"Short Url Redirecting from: {path}, to: {redirectTo}");
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
