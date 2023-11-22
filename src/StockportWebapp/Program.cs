using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath + "/app-config");
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddJsonFile("appversion.json", true);

var useAwsSecretManager = bool.Parse(builder.Configuration.GetSection("UseAWSSecretManager").Value);
Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();


if (useAwsSecretManager)
{
    builder.AddSecrets();
    Log.Logger.Information($"INITIALISE SECRETS: AWS Secrets Manager");
}
else
{
    builder.Configuration.AddJsonFile($"{builder.Configuration.GetSection("secrets-location").Value}/appsettings.{builder.Environment.EnvironmentName}.secrets.json");
    Log.Logger.Information($"INITIALISE SECRETS: Load JSON Secrets from file system");
}

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .WriteToElasticsearchAws(builder.Configuration));

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.UseSerilogRequestLogging();

if (!app.Environment.IsEnvironment("prod") && !app.Environment.IsEnvironment("stage"))
    app.UseDeveloperExceptionPage();

app.UseMiddleware<BusinessIdMiddleware>()
    .UseSecureHeadersMiddleware(SecureHeadersMiddleware.CustomConfiguration())
    .UseMiddleware<ShortUrlRedirectsMiddleware>()
    .UseMiddleware<RobotsMiddleware>()
    .UseMiddleware<SecurityHeaderMiddleware>()
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