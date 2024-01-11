namespace StockportWebapp.Middleware;

public class SecurityHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        var host = httpContext.Request.Host.Value.ToLower();
        var isRemoteHost = host.StartsWith("www") || host.StartsWith("int-") || host.StartsWith("qa-") || host.StartsWith("stage-");

        if (isRemoteHost)
        {
            var cspBuilder = new ContentSecurityPolicyBuilder();
            var allowedContent = cspBuilder.BuildPolicy();

            httpContext.Response.Headers.Add("Content-Security-Policy", new[] { allowedContent });
        }

        return _next(httpContext);
    }
}
