namespace StockportWebapp.Utils.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomStaticFiles(this IApplicationBuilder app)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = context =>
            {
                var isLive = context.Context.Request.Host.Value.StartsWith("www.");
                var businessId = context.Context.Request.Headers["BUSINESS-ID"];
                var url = string.Concat("robots-", businessId, isLive ? "-live" : "", ".txt");
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
        var ci = new CultureInfo("en-GB") { DateTimeFormat = { ShortDatePattern = "dd/MM/yyyy" } };

        // Configure the Localization middleware .
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(ci),
            SupportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-GB"),
            },
            SupportedUICultures = new List<CultureInfo>
            {
                new CultureInfo("en-GB"),
            }
        });

        return app;
    }
}
