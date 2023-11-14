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
            .UseReferrerPolicy(OwaspHeaders.Core.Enums.ReferrerPolicyOptions.strictWhenCrossOrigin)
            .UseCacheControl()
            .RemovePoweredByHeader()
            .UsePermittedCrossDomainPolicies()
            .Build();
}
