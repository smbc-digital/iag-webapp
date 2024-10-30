namespace StockportWebapp.Middleware;

[ExcludeFromCodeCoverage]
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
            _logger.LogInformation($"BUSINESS-ID has been set to: {idFromHeader} from header");
        }
        else
        {
            if(context.Request.Query.TryGetValue("bid", out StringValues idFromQuery))
            {
                businessId.SetId(idFromQuery);
                _logger.LogInformation($"BUSINESS-ID has been set to: {idFromQuery} from querystring");
            }
            else
            {
                businessId.SetId(new StringValues("stockportgov"));
                //businessId.SetId(new StringValues("healthystockport"));
                //businessId.SetId(new StringValues("stockroom"));
            }

            context.Request.Headers.Add("BUSINESS-ID", businessId.ToString());
        }

        if (context.Request.Path.HasValue
                    && !context.Request.Path.Value.ToLower().Contains("/assets/")
                    && !context.Request.Path.Value.ToLower().Contains("healthcheck")) 
            _logger.LogInformation("BUSINESS-ID has not been set, setting to default");

        return _next(context);
    }
}