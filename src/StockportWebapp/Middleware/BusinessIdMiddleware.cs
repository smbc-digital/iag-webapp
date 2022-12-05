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
            if (context.Request.Headers.TryGetValue("BUSINESS-ID", out StringValues idFromHeader))
            {
                businessId.SetId(idFromHeader);
                _logger.LogInformation($"BUSINESS-ID has been set to: {idFromHeader}");
            }
            else
            {
                // default to stockportgov if no businessid
                businessId.SetId(new StringValues("stockportgov"));
                // to run healthystockport locally, comment out the above line and uncomment line below
                //businessId.SetId(new StringValues("healthystockport"));
                context.Request.Headers.Add("BUSINESS-ID", businessId.ToString());

                if (context.Request.Path.HasValue
                    && !context.Request.Path.Value.ToLower().Contains("/assets/")
                    && !context.Request.Path.Value.ToLower().Contains("healthcheck")) _logger.LogInformation("BUSINESS-ID has not been set, setting to default");
            }

            return _next(context);
        }
    }
}
