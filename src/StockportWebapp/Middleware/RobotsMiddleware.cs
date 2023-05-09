namespace StockportWebapp.Middleware;

public class RobotsMiddleware
{
    private readonly RequestDelegate _next;

    public RobotsMiddleware(RequestDelegate next) => _next = next;

    public Task Invoke(HttpContext context, BusinessId businessId, IWebHostEnvironment env)
    {
        if (!env.IsEnvironment("prod"))
            context.Response.Headers.Add("X-Robots-Tag", "noindex");

        if (context.Request.Path.ToString().EndsWith("robots.txt"))
        {
            var isLive = context.Request.Host.Value.StartsWith("www.");
            var url = string.Concat("/robots-", businessId, isLive ? "-live" : "", ".txt");
            context.Request.Path = context.Request.PathBase.Add(new PathString(url));
        }

        return _next(context);
    }
}
