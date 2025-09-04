namespace StockportWebapp.Middleware;

[ExcludeFromCodeCoverage]
public class SecureHeadersMiddleware
{
    public static SecureHeadersMiddlewareConfiguration CustomConfiguration() =>
        SecureHeadersMiddlewareBuilder
            .CreateBuilder()
            .UseHsts()
            .UseXssProtection()
            .UseContentTypeOptions()
            .UseReferrerPolicy(OwaspHeaders.Core.Enums.ReferrerPolicyOptions.strictWhenCrossOrigin)
            .UseCacheControl()
            .RemovePoweredByHeader()
            .UsePermittedCrossDomainPolicies()
            .Build();
}
