using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockportWebapp.Repositories;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using StockportWebapp.Config;
using StockportWebapp.Middleware;
using StockportWebapp.Scheduler;
using StockportWebapp.ModelBinders;
using ILogger = Serilog.ILogger;
using StockportWebapp.QuestionBuilder;
using StockportWebapp.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Services;
using StockportWebapp.Wrappers;

namespace StockportWebapp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string _appEnvironment;
        private readonly string _contentRootPath;
        public readonly string ConfigDir = "app-config";
        private readonly bool _useRedisSession;
        private readonly bool _sendAmazonEmails;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //var configBuilder = new ConfigurationBuilder();
            //var configLoader = new ConfigurationLoader(configBuilder, ConfigDir);
            ////Configuration = configLoader.LoadConfiguration(env, _contentRootPath);
            Configuration = configuration;
            _contentRootPath = env.ContentRootPath;
            _appEnvironment = env.EnvironmentName;
            _useRedisSession = Configuration["UseRedisSessions"] == "true";
            _sendAmazonEmails = string.IsNullOrEmpty(Configuration["SendAmazonEmails"]) || Configuration["SendAmazonEmails"] == "true";
        }

        private ILogger StartupLogger { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            // logging
            ConfigureSerilog();

            // other
            services.AddSingleton(new CurrentEnvironment(_appEnvironment));
            services.AddTransient(p => new HostHelper(p.GetService<CurrentEnvironment>()));
            services.AddSingleton(o => new ViewHelpers(o.GetService<ITimeProvider>()));
            services.AddScoped<BusinessId>();
            services.AddTransient(p => new UrlGenerator(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));
            services.AddTransient<IUrlGeneratorSimple>(p => new UrlGeneratorSimple(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));
            services.AddSingleton<IStaticAssets, StaticAssets>();
            services.AddTransient<IFilteredUrl>(p => new FilteredUrl(p.GetService<ITimeProvider>()));
            services.AddTransient(p => new QuestionLoader(p.GetService<IRepository>()));
            services.AddTransient<IHttpClientWrapper>(provider => new HttpClientWrapper(new System.Net.Http.HttpClient(), provider.GetService<ILogger<HttpClientWrapper>>()));

            // custom extensions
            services.AddCustomisedAngleSharp();
            services.AddFeatureToggles(_contentRootPath, _appEnvironment);
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
            services.AddCustomServices(_contentRootPath, _appEnvironment);
            services.AddBuilders();
            services.AddHelpers();
            services.AddParisConfiguration(Configuration, StartupLogger);
            services.AddGroupConfiguration(Configuration, StartupLogger);
            services.AddSesEmailConfiguration(Configuration, StartupLogger);
            services.AddRedis(Configuration, _useRedisSession, StartupLogger);

            // sdk
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddNodeServices();
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeFormatConverterModelBinderProvider());
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, IApplicationLifetime appLifetime)
        {
            // add logging
            loggerFactory.AddSerilog();

            if (_appEnvironment == "int" || _appEnvironment == "local" || _appEnvironment == "qa")
            {
                app.UseDeveloperExceptionPage();
            }

            // Quartz stuff

            var scheduler = new QuartzScheduler(serviceProvider.GetService<ShortUrlRedirects>(),
                serviceProvider.GetService<LegacyUrlRedirects>(), serviceProvider.GetService<IRepository>(), serviceProvider.GetService<ITimeProvider>(), serviceProvider.GetService<IGroupsService>(), serviceProvider.GetService<FeatureToggles>());
            await scheduler.Start();

            app.UseMiddleware<BusinessIdMiddleware>();
            app.UseMiddleware<ShortUrlRedirectsMiddleware>();
            app.UseMiddleware<RobotsTxtMiddleware>();
            app.UseMiddleware<BetaToWwwMiddleware>();
            app.UseMiddleware<SecurityHeaderMiddleware>();
            app.UseStatusCodePagesWithReExecute("/Error/Error/{0}");

            // custom extenstions
            app.UseCustomStaticFiles();
            app.UseCustomCulture();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
            app.UseMvc(routes => { routes.MapRoute("thankyou", "{controller=ContactUs}/{action=ThankYou}/"); });
            app.UseMvc(routes => { routes.MapRoute("search", "{controller=Search}/{action=Index}"); });
            app.UseMvc(routes => { routes.MapRoute("rss", "{controller=Rss}/{action=Index}"); });

            // Close logger
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
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

            StartupLogger.Warning("Completed logging configuration...");
        }
    }
}