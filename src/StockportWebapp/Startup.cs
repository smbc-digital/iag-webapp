﻿namespace StockportWebapp;

[ExcludeFromCodeCoverage]
public class Startup
{
    public IConfiguration Configuration { get; }
    private readonly string _appEnvironmentName;
    private readonly string _contentRootPath;
    public readonly string ConfigDir = "app-config";
    private readonly bool _useRedisSession;
    private readonly bool _sendAmazonEmails;
    private readonly Serilog.ILogger _logger;


    public Startup(IConfiguration configuration, IWebHostEnvironment env, Serilog.ILogger logger)
    {
        Configuration = configuration;
        _contentRootPath = env.ContentRootPath;
        _appEnvironmentName = env.EnvironmentName;
        _useRedisSession = Configuration["UseRedisSessions"].Equals("true");
        _sendAmazonEmails = string.IsNullOrEmpty(Configuration["SendAmazonEmails"]) || Configuration["SendAmazonEmails"].Equals("true");
        _logger = logger;

    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        _logger.Information($"WEBAPP: STARTUP : ConfigureServices : Env = {_appEnvironmentName}, UseRedisSession = {_useRedisSession}, ContentRoot = {_contentRootPath}  ");

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

        // other
        services.AddSingleton(new CurrentEnvironment(_appEnvironmentName));
        services.AddTransient(p => new HostHelper(p.GetService<CurrentEnvironment>()));
        services.AddSingleton(o => new ViewHelpers(o.GetService<ITimeProvider>()));
        services.AddScoped<BusinessId>();
        services.AddTransient(p => new UrlGenerator(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));
        services.AddTransient<IUrlGeneratorSimple>(p => new UrlGeneratorSimple(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));
        services.AddSingleton<IStaticAssets, StaticAssets>();
        services.AddTransient<IFilteredUrl>(p => new FilteredUrl(p.GetService<ITimeProvider>()));

        // custom extensions
        services.AddHttpClient<ICivicaPayGateway, CivicaPayGateway>(Configuration);
        services.AddCustomisationOfViews();
        services.AddTimeProvider();
        services.AddRedirects();
        services.AddRecapthca();
        services.AddConfiguration(Configuration);
        services.AddTagParsers();
        services.AddMarkdown();
        services.AddFactories();
        services.AddCustomHttpClients(_sendAmazonEmails);
        services.AddRepositories();
        services.AddCustomServices(_contentRootPath, _appEnvironmentName);
        services.AddBuilders();
        services.AddHelpers();
        services.AddSesEmailConfiguration(Configuration, Log.Logger);
        services.AddConfigurationOptions(Configuration);
        _logger.Information($"WEBAPP: STARTUP : ConfigureServices : Adding Cache");
        services.AddRedis(Configuration, _useRedisSession, Log.Logger);

        services.AddFeatureManagement();
    }

    public static void HandleFaviconRequests(IApplicationBuilder app)
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