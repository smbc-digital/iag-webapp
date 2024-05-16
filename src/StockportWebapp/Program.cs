[ExcludeFromCodeCoverage]
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (!builder.Environment.EnvironmentName.Equals("local"))
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.File("C:\\Program Files\\Amazon\\ElasticBeanstalk\\logs\\WebAppStartupLog.log")
                    .CreateBootstrapLogger();

        Log.Logger.Information($"WEBAPP : ENVIRONMENT : {builder.Environment.EnvironmentName}");

        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath + "/app-config");
        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

        var useAwsSecretManager = bool.Parse(builder.Configuration.GetSection("UseAWSSecretManager").Value);

        Log.Logger.Information($"WEBAPP : ENVIRONMENT : {builder.Environment.EnvironmentName}");

        try
        {
            if (useAwsSecretManager)
            {
                builder.AddSecrets();
                Log.Logger.Information($"WEBPAPP : INITIALISE SECRETS  {builder.Environment.EnvironmentName} : AWS Secrets Manager");
            }
            else
            {
                var location = $"{builder.Configuration.GetSection("secrets-location").Value}/appsettings.{builder.Environment.EnvironmentName}.secrets.json";
                builder.Configuration.AddJsonFile(location);
                Log.Logger.Information($"WEBAPP : INITIALISE SECRETS {builder.Environment.EnvironmentName}: Load JSON Secrets from file system, {location}");
            }

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteToElasticsearchAws(builder.Configuration));

            Log.Logger.Information($"WEBAPP : CONFIGURE APPLICATION START");
            var startup = new Startup(builder.Configuration, builder.Environment, Log.Logger);
            startup.ConfigureServices(builder.Services);

            Log.Logger.Information($"WEBAPP : BUILDING APPLICATION");
            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (!app.Environment.IsEnvironment("prod") && !app.Environment.IsEnvironment("stage"))
                app.UseDeveloperExceptionPage();

            app.UseMiddleware<BusinessIdMiddleware>()
                .UseSecureHeadersMiddleware(SecureHeadersMiddleware.CustomConfiguration())
                .UseMiddleware<ShortUrlRedirectsMiddleware>()
                .UseMiddleware<RobotsMiddleware>()
                .UseMiddleware<SecurityHeaderMiddleware>()
                .UseMiddleware<CookiesComplianceMiddleware>()
                .UseStatusCodePagesWithReExecute("/error")
                .UseCustomStaticFiles()
                .UseCustomCulture()
                .UseRouting()
                .Map("/favicon.ico", Startup.HandleFaviconRequests)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "WEBAPP : FAILURE : Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}