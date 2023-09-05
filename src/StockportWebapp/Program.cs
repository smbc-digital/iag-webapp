var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath + "/app-config");
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddJsonFile("appversion.json", true);

builder.AddSecrets();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();

Log.Logger.Information($"STARTING APPLICATION Environment:{builder.Environment.EnvironmentName}, Custom Value: { builder.Configuration.GetValue<string>("MyCustomValue") }, TOKEN STORE SECRET { builder.Configuration.GetValue<string>("TokenStoreUrl") }");

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