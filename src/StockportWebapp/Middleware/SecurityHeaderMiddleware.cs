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
            httpContext.Response.Headers.Add("Content-Security-Policy", new[] { "script-src 'self' 'unsafe-inline' https://cdn.websitepolicies.io https://customer.cludo.com https://www.googletagmanager.com http://www.google-analytics.com http://siteimproveanalytics.com;media-src 'self';object-src 'self';img-src 'self' https://s3-eu-west-1.amazonaws.com/ https://images.ctfassets.net https://www.googletagmanager.com;upgrade-insecure-requests;block-all-mixed-content;" });
        }
        return _next(httpContext);
    }
}
