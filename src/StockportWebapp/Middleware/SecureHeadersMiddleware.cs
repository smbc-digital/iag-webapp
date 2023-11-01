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
            .UseReferrerPolicy()
            .UseCacheControl()
            .RemovePoweredByHeader()
            .UsePermittedCrossDomainPolicies()
            .Build();
}
