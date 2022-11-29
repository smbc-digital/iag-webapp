namespace StockportWebapp.Middleware
{
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
                httpContext.Response.Headers.Add("Strict-Transport-Security", new[] { "max-age=31536000" });
            }

            return _next(httpContext);
        }
    }
}
