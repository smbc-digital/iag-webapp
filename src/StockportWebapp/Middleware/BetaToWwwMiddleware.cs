using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using StockportWebapp.FeatureToggling;

namespace StockportWebapp.Middleware
{
    public class BetaToWwwMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BetaToWwwMiddleware> _logger;
        private readonly FeatureToggles _featureToggles;

        public BetaToWwwMiddleware(RequestDelegate next, ILogger<BetaToWwwMiddleware> logger, FeatureToggles featureToggles)
        {
            _next = next;
            _logger = logger;
            _featureToggles = featureToggles;
        }

        public Task Invoke(HttpContext context)
        {
            var host = context.Request.Host.Value.ToLower();

            if (_featureToggles.BetaToWwwRedirect && host.StartsWith("beta."))
            {
                host = host.Replace("beta.", "www.");
                _logger.LogInformation(string.Concat(context.Request.Host.Value.ToLower(), " redirected to ", host, " for path: ", context.Request.Path.Value));

                var request = context.Request;
                request.Host = new HostString(host);

                context.Response.Redirect(request.GetDisplayUrl());
            }

            _logger.LogInformation("Host: " + context.Request.Host);
                
            return _next(context);
        }
    }
}
