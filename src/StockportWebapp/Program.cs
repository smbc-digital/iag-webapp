[ExcludeFromCodeCoverage]
internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        if (!builder.Environment.EnvironmentName.Equals("local"))
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.File("C:\\Program Files\\Amazon\\ElasticBeanstalk\\logs\\WebAppStartupLog.log")
                    .CreateBootstrapLogger();

        Log.Logger.Information($"WEBAPP : ENVIRONMENT : {builder.Environment.EnvironmentName}");

        builder.Configuration.SetBasePath(builder.Environment.ContentRootPath + "/app-config");
        builder.Configuration
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

        bool useAwsSecretManager = bool.Parse(builder.Configuration.GetSection("UseAWSSecretManager").Value);

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
                string location = $"{builder.Configuration.GetSection("secrets-location").Value}/appsettings.{builder.Environment.EnvironmentName}.secrets.json";
                builder.Configuration.AddJsonFile(location);
                Log.Logger.Information($"WEBAPP : INITIALISE SECRETS {builder.Environment.EnvironmentName}: Load JSON Secrets from file system, {location}");
            }

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .WriteToElasticsearchAws(builder.Configuration));

            Log.Logger.Information($"WEBAPP : CONFIGURE APPLICATION START");

            ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

            Log.Logger.Information($"WEBAPP : BUILDING APPLICATION");
            WebApplication app = builder.Build();

            ConfigureMiddleware(app);

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

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        string appEnvironmentName = environment.EnvironmentName;
        string contentRootPath = environment.ContentRootPath;
        bool useRedisSession = configuration["UseRedisSessions"].Equals("true");
        bool sendAmazonEmails = string.IsNullOrEmpty(configuration["SendAmazonEmails"]) || configuration["SendAmazonEmails"].Equals("true");

        Log.Logger.Information($"WEBAPP: STARTUP : ConfigureServices : Env = {appEnvironmentName}, UseRedisSession = {useRedisSession}, ContentRoot = {contentRootPath}");

        services.AddControllersWithViews(options =>
        {
            options.ModelBinderProviders.Insert(0, new DateTimeFormatConverterModelBinderProvider());
        });

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        services.AddRazorPages();
        services.AddHttpContextAccessor();

        services.AddSingleton(new CurrentEnvironment(appEnvironmentName));
        services.AddTransient(p => new HostHelper(p.GetService<CurrentEnvironment>()));
        services.AddSingleton(o => new ViewHelpers(o.GetService<ITimeProvider>()));
        services.AddScoped<BusinessId>();
        services.AddTransient(p => new UrlGenerator(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));
        services.AddTransient<IUrlGeneratorSimple>(p => new UrlGeneratorSimple(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));
        services.AddSingleton<IStaticAssets, StaticAssets>();
        services.AddTransient<IFilteredUrl>(p => new FilteredUrl(p.GetService<ITimeProvider>()));

        services.AddHttpClient<ICivicaPayGateway, CivicaPayGateway>(configuration);
        services.AddCustomisationOfViews();
        services.AddTimeProvider();
        services.AddRedirects();
        services.AddRecapthca();
        services.AddConfiguration(configuration);
        services.AddTagParsers();
        services.AddMarkdown();
        services.AddFactories();
        services.AddCustomHttpClients(sendAmazonEmails);
        services.AddRepositories();
        services.AddCustomServices(contentRootPath, appEnvironmentName);
        services.AddBuilders();
        services.AddHelpers();
        services.AddSesEmailConfiguration(configuration, Log.Logger);
        services.AddConfigurationOptions(configuration);

        Log.Logger.Information($"WEBAPP: STARTUP : ConfigureServices : Adding Cache");
        services.AddRedis(configuration, useRedisSession, Log.Logger);
        services.AddFeatureManagement();
        services.AddHttpClient<IShedApiClient, ShedApiClient>();
        services.AddHttpClient<ITPOApiClient, TPOApiClient>();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
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
            .Map("/favicon.ico", HandleFaviconRequests)
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
    }

    private static void HandleFaviconRequests(IApplicationBuilder app)
    {
        app.Run(context =>
        {
            string defaultFaviconPath = "/assets/images/ui-images/sg/favicon.ico";
            if (context.Request.Headers.TryGetValue("BUSINESS-ID", out StringValues idFromHeader))
            {
                if (idFromHeader.Equals("healthystockport"))
                    defaultFaviconPath = "/assets/images/ui-images/Favicon.png";
            }
            context.Response.Redirect(defaultFaviconPath, true);
            return Task.CompletedTask;
        });
    }
}