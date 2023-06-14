var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath + "/app-config");
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddJsonFile("appversion.json", true)
    .AddJsonFile($"{builder.Configuration.GetSection("secrets-location").Value}/appsettings.{builder.Environment.EnvironmentName}.secrets.json");

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .WriteToElasticsearchAws(builder.Configuration));

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

if (!app.Environment.IsEnvironment("prod") && !app.Environment.IsEnvironment("stage"))
    app.UseDeveloperExceptionPage();

app.UseSerilogRequestLogging();

app.UseMiddleware<BusinessIdMiddleware>()
    // line 26 should be used once the CustomConfiguration has been done. Comment out line 27 to check the custom config
    // .UseSecureHeadersMiddleware(SecureHeadersMiddleware.CustomConfiguration())
    .UseSecureHeadersMiddleware()
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