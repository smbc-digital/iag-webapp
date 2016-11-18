using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using StockportWebapp.Config;

namespace StockportWebapp.Middleware
{
    public class BusinessIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BusinessIdMiddleware> _logger;

        public BusinessIdMiddleware(RequestDelegate next, ILogger<BusinessIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext context, BusinessId businessId)
        {
            StringValues idFromHeader;

            if (context.Request.Headers.TryGetValue("BUSINESS-ID", out idFromHeader))
            {
                businessId.SetId(idFromHeader);
                _logger.LogInformation($"BUSINESS-ID has been set to: {idFromHeader}");
            }
            else
            {
                _logger.LogError("BUSINESS-ID has not been set");
            }

            return _next(context);
        }
    }
}
