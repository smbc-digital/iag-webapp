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
            .UseContentSecurityPolicy("script-src 'self' 'https://cdn.websitepolicies.io/lib/cookieconsent/';object-src 'self'")
            .UseReferrerPolicy()
            .UseCacheControl()
            .RemovePoweredByHeader()
            .UseCrossOriginResourcePolicy()
            .UsePermittedCrossDomainPolicies()
            .Build();
}
