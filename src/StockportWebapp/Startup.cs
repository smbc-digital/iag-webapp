using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockportWebapp.Repositories;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using NLog.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Middleware;
using StockportWebapp.Scheduler;
using StockportWebapp.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using StockportWebapp.AmazonSES;
using StockportWebapp.Controllers;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using StockportWebapp.QuestionBuilder;
using StockportWebapp.Extensions;

namespace StockportWebapp
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private readonly string _appEnvironment;
        private readonly string _contentRootPath;
        public readonly string ConfigDir = "app-config";
        private readonly bool _useRedisSession;

        public Startup(IHostingEnvironment env)
        {
            _contentRootPath = env.ContentRootPath;

            var configBuilder = new ConfigurationBuilder();
            var configLoader = new ConfigurationLoader(configBuilder, ConfigDir);

            Configuration = configLoader.LoadConfiguration(env, _contentRootPath);
            _appEnvironment = configLoader.EnvironmentName(env);

            _useRedisSession = Configuration["UseRedisSessions"] == "true";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            // logging
            var loggerFactory = new LoggerFactory().AddNLog();
            ILogger logger = loggerFactory.CreateLogger<Startup>();

            // other
            services.AddSingleton<CurrentEnvironment>(new CurrentEnvironment(_appEnvironment));
            services.AddSingleton(o => new ViewHelpers(o.GetService<ITimeProvider>()));
            services.AddScoped<BusinessId>();
            services.AddTransient(p => new UrlGenerator(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));
            services.AddSingleton<IStaticAssets, StaticAssets>();
            services.AddTransient<IFilteredUrl>(p => new FilteredUrl(p.GetService<ITimeProvider>()));
            services.AddTransient(p => new QuestionLoader(p.GetService<IRepository>()));

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
            services.AddCustomHttpClients();
            services.AddRepositories();
            services.AddCustomServices(_contentRootPath, _appEnvironment);
            services.AddBuilders();
            services.AddHelpers();
            services.AddParisConfiguration(Configuration, logger);
            services.AddGroupConfiguration(Configuration, logger);
            services.AddSesEmailConfiguration(Configuration, logger);
            services.AddRedis(Configuration, _useRedisSession, logger);

            // sdk
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddNodeServices();
            services.AddAntiforgery(p => { p.CookieName = "SK-ANTI-FORGERY"; });
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeFormatConverterModelBinderProvider());
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (_appEnvironment == "int" || _appEnvironment == "local" || _appEnvironment == "qa")
            {
                app.UseDeveloperExceptionPage();
            }

            var scheduler = new RedirectScheduler(serviceProvider.GetService<ShortUrlRedirects>(),
                serviceProvider.GetService<LegacyUrlRedirects>(), serviceProvider.GetService<IRepository>());
            await scheduler.Start();

            app.UseMiddleware<BusinessIdMiddleware>();
            app.UseMiddleware<ShortUrlRedirectsMiddleware>();
            app.UseMiddleware<RobotsTxtMiddleware>();
            app.UseMiddleware<BetaToWwwMiddleware>();
            app.UseMiddleware<SecurityHeaderMiddleware>();

            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();
            app.UseStatusCodePagesWithReExecute("/Error/Error/{0}");

            // custom extenstions
            app.UseCustomStaticFiles();
            app.UseCustomCulture();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
            app.UseMvc(routes => { routes.MapRoute("thankyou", "{controller=ContactUs}/{action=ThankYou}/"); });
            app.UseMvc(routes => { routes.MapRoute("search", "{controller=Search}/{action=Index}"); });
            app.UseMvc(routes => { routes.MapRoute("rss", "{controller=Rss}/{action=Index}"); });
        }
    }
}