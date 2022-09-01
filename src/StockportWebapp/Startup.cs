using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using StockportGovUK.NetStandard.Gateways;
using StockportWebapp.Config;
using StockportWebapp.Extensions;
using StockportWebapp.Middleware;
using StockportWebapp.ModelBinders;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using StockportWebapp.Wrappers;
using ILogger = Serilog.ILogger;

namespace StockportWebapp
{
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

        private ILogger StartupLogger { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            ConfigureSerilog();

            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeFormatConverterModelBinderProvider());
            });

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
            services.AddTransient<IHttpClientWrapper>(provider => new HttpClientWrapper(new System.Net.Http.HttpClient(), provider.GetService<ILogger<HttpClientWrapper>>()));

            // custom extensions
            services.AddCustomisedAngleSharp();
            services.AddFeatureToggles(_contentRootPath, _appEnvironmentName);
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
            services.AddGroupConfiguration(Configuration, StartupLogger);
            services.AddSesEmailConfiguration(Configuration, StartupLogger);
            services.AddRedis(Configuration, _useRedisSession, StartupLogger);
            services.AddResilientHttpClients<IGateway, Gateway>(Configuration);

            services.AddApplicationInsightsTelemetry(Configuration);
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IHostApplicationLifetime appLifetime)
        {
            loggerFactory.AddSerilog();

            if (!env.IsEnvironment("prod") && !env.IsEnvironment("stage"))
                app.UseDeveloperExceptionPage();

            app.UseMiddleware<BusinessIdMiddleware>()
                .UseMiddleware<ShortUrlRedirectsMiddleware>()
                .UseMiddleware<RobotsMiddleware>()
                .UseMiddleware<BetaToWwwMiddleware>()
                .UseMiddleware<SecurityHeaderMiddleware>()
                .UseStatusCodePagesWithReExecute("/error")
                .UseCustomStaticFiles()
                .UseCustomCulture()
                .UseRouting()
                .Map("/favicon.ico", HandleFaviconRequests)
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // Close logger
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }

        private static void HandleFaviconRequests(IApplicationBuilder app)
        {
            app.Run(context => {
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

        private void ConfigureSerilog()
        {
            var logConfig = new LoggerConfiguration()
                .ReadFrom
                .Configuration(Configuration);

            var esLogConfig = new ElasticSearchLogConfigurator(Configuration);
            esLogConfig.Configure(logConfig);

            Log.Logger = logConfig.CreateLogger();
            StartupLogger = Log.Logger;

            StartupLogger.Debug("Completed logging configuration...");
        }
    }
}