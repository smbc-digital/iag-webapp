using OwaspHeaders.Core.Enums;
using OwaspHeaders.Core.Models;

namespace StockportWebapp.Middleware;

public class SecureHeadersMiddleware
{
    public static SecureHeadersMiddlewareConfiguration CustomConfiguration() =>
        SecureHeadersMiddlewareBuilder
            .CreateBuilder()
            .UseHsts()
            .UseXFrameOptions()
            .UseXssProtection()
            .UseContentTypeOptions()
            .UseContentSecurityPolicy()
            .UseContentDefaultSecurityPolicy()
            .UseReferrerPolicy()
            .UseCacheControl()
            .RemovePoweredByHeader()
            .UseCrossOriginResourcePolicy()
            .UsePermittedCrossDomainPolicies()
            .Build();
}
