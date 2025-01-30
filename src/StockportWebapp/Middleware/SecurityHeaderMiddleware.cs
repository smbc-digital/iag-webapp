namespace StockportWebapp.Middleware;

[ExcludeFromCodeCoverage]
public class SecurityHeaderMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public Task Invoke(HttpContext httpContext)
    {
        string host = httpContext.Request.Host.Value.ToLower();
        bool isRemoteHost = host.StartsWith("www") || host.StartsWith("int-") || host.StartsWith("qa-") || host.StartsWith("stage-");

        if (isRemoteHost)
        {
            ContentSecurityPolicyBuilder cspBuilder = new();
            string allowedContent = cspBuilder.BuildPolicy();

            httpContext.Response.Headers.Append("Content-Security-Policy", new[] { allowedContent });
        }

        return _next(httpContext);
    }
}