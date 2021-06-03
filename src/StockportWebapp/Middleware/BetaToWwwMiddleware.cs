using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Middleware
{
    public class BetaToWwwMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BetaToWwwMiddleware> _logger;

        public BetaToWwwMiddleware(RequestDelegate next, ILogger<BetaToWwwMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext context)
        {
            var host = context.Request.Host.Value.ToLower();

            if (host.StartsWith("beta."))
            {
                host = host.Replace("beta.", "www.");
                _logger.LogInformation(string.Concat(context.Request.Host.Value.ToLower(), " redirected to ", host, " for path: ", context.Request.Path.Value));

                var request = context.Request;
                request.Host = new HostString(host);

                context.Response.Redirect(request.GetDisplayUrl());
            }
                
            return _next(context);
        }
    }
}
