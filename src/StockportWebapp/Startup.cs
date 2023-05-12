namespace StockportWebapp;

public class Startup
{
    public IConfiguration Configuration { get; }
    private readonly string _appEnvironmentName;
    private readonly string _contentRootPath;
    public readonly string ConfigDir = "app-config";
    private readonly bool _useRedisSession;
    private readonly bool _sendAmazonEmails;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        _contentRootPath = env.ContentRootPath;
        _appEnvironmentName = env.EnvironmentName;
        _useRedisSession = Configuration["UseRedisSessions"] == "true";
        _sendAmazonEmails = string.IsNullOrEmpty(Configuration["SendAmazonEmails"]) || Configuration["SendAmazonEmails"] == "true";
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
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
        services.AddGroupConfiguration(Configuration, Log.Logger);
        services.AddSesEmailConfiguration(Configuration, Log.Logger);
        services.AddRedis(Configuration, _useRedisSession, Log.Logger);
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