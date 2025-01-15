namespace StockportWebapp.Utils.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomStaticFiles(this IApplicationBuilder app)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = context =>
            {
                bool isLive = context.Context.Request.Host.Value.StartsWith("www.");
                StringValues businessId = context.Context.Request.Headers["BUSINESS-ID"];
                string url = string.Concat("robots-", businessId, isLive ? "-live" : string.Empty, ".txt");
                if (context.File.Name.Equals(url))
                {
                    context.Context.Response.Headers
                        .Append("Cache-Control", "max-age=0");
                }
                else
                {
                    context.Context.Response.Headers
                        .Append("Cache-Control", "public, max-age=31536000, immutable");
                }
            }
        });

        return app;
    }

    public static IApplicationBuilder UseCustomCulture(this IApplicationBuilder app)
    {
        CultureInfo ci = new CultureInfo("en-GB") { DateTimeFormat = { ShortDatePattern = "dd/MM/yyyy" } };

        // Configure the Localization middleware .
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(ci),
            SupportedCultures = new List<CultureInfo>
            {
                new("en-GB"),
            },
            SupportedUICultures = new List<CultureInfo>
            {
                new("en-GB"),
            }
        });

        return app;
    }
}