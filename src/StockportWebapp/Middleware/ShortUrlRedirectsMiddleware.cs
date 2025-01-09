namespace StockportWebapp.Middleware;

[ExcludeFromCodeCoverage]
public class ShortUrlRedirectsMiddleware(RequestDelegate next,
                                        ShortUrlRedirects shortUrlRedirects,
                                        LegacyUrlRedirects legacyUrlRedirects,
                                        ILogger<ShortUrlRedirectsMiddleware> logger,
                                        IRepository repository)
{
    private readonly RequestDelegate _next = next;
    private readonly ShortUrlRedirects _shortUrlRedirects = shortUrlRedirects;
    private readonly LegacyUrlRedirects _legacyUrlRedirects = legacyUrlRedirects;
    private readonly ILogger<ShortUrlRedirectsMiddleware> _logger = logger;
    private readonly IRepository _repository = repository;

    public async Task Invoke(HttpContext context, BusinessId businessId)
    {
        PathString path = context.Request.Path;

        if (_shortUrlRedirects.HasExpired())
        {
            HttpResponse response = await _repository.GetRedirects();
            Redirects redirects = response.Content as Redirects;

            _shortUrlRedirects.Redirects = redirects.ShortUrlRedirects;
            _shortUrlRedirects.LastUpdated = DateTime.Now;
            _legacyUrlRedirects.Redirects = redirects.LegacyUrlRedirects;
            _legacyUrlRedirects.LastUpdated = DateTime.Now;
        }

        if (_shortUrlRedirects.Redirects.ContainsKey(businessId.ToString()) && _shortUrlRedirects.Redirects[businessId.ToString()].ContainsKey(path))
        {
            string redirectTo = _shortUrlRedirects.Redirects[businessId.ToString()][path];

            _logger.LogInformation($"Short Url Redirecting from: {path}, to: {redirectTo}");

            context.Response.Redirect(redirectTo);
            context.Response.Headers["Cache-Control"] = $"public, max-age={Cache.RedirectCacheDuration}";
        }
        else
            await _next.Invoke(context);
    }
}