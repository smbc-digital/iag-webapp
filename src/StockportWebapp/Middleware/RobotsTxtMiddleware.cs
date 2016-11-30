using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Config;

namespace StockportWebapp.Middleware
{
    public class RobotsTxtMiddleware
    {
        private readonly RequestDelegate _next;

        public RobotsTxtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context, BusinessId businessId)
        {
            if (context.Request.Path.ToString().EndsWith("robots.txt"))
            {
                var isLive = context.Request.Host.Value.StartsWith("www.");
                var url = string.Concat("/robots-", businessId, isLive ? "-live" : "", ".txt");
                context.Request.Path = context.Request.PathBase.Add(new PathString(url));
            }

            return _next(context);
        }
    }
}
