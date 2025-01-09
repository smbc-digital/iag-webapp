namespace StockportWebapp.Middleware;

[ExcludeFromCodeCoverage]
public class RobotsMiddleware
{
    private readonly RequestDelegate _next;

    public RobotsMiddleware(RequestDelegate next) => _next = next;

    public Task Invoke(HttpContext context, BusinessId businessId, IWebHostEnvironment env)
    {
        if (!env.IsEnvironment("prod"))
            context.Response.Headers.Append("X-Robots-Tag", "noindex");

        if (context.Request.Path.ToString().EndsWith("robots.txt"))
        {
            bool isLive = context.Request.Host.Value.StartsWith("www.") || context.Request.Host.Value.StartsWith("prod-");
            string url = string.Concat("/robots-",
                                    businessId,
                                    isLive
                                        ? "-live"
                                        : string.Empty,
                                    ".txt");

            context.Request.Path = context.Request.PathBase.Add(new PathString(url));
        }

        return _next(context);
    }
}