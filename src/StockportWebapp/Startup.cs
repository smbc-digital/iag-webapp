using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.Compression;
using Elasticsearch.Net;
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
using ILogger = Microsoft.Extensions.Logging.ILogger;
using StockportWebapp.QuestionBuilder;
using StockportWebapp.Extensions;
using Microsoft.AspNetCore.Http;
using Quartz;
using Quartz.Impl;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using StockportWebapp.Configuration;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Services;
using StockportWebapp.Wrappers;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore1;

namespace StockportWebapp
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private readonly string _appEnvironment;
        private readonly string _contentRootPath;
        public readonly string ConfigDir = "app-config";
        private readonly bool _useRedisSession;
        private readonly bool _sendAmazonEmails;

        public Startup(IHostingEnvironment env)
        {
            _contentRootPath = env.ContentRootPath;

            var configBuilder = new ConfigurationBuilder();
            var configLoader = new ConfigurationLoader(configBuilder, ConfigDir);

            Configuration = configLoader.LoadConfiguration(env, _contentRootPath);
            _appEnvironment = configLoader.EnvironmentName(env);

            _useRedisSession = Configuration["UseRedisSessions"] == "true";
            _sendAmazonEmails = string.IsNullOrEmpty(Configuration["SendAmazonEmails"]) || Configuration["SendAmazonEmails"] == "true";

            var loggerConfig = new LoggerConfiguration();

            // when this "feature toggle" has been removed, this can be deleted
            var esConfig = new ElasticSearch();
            Configuration.GetSection("ElasticSearch").Bind(esConfig);

            if (esConfig.Enabled)
            {
                // elastic search and logging to the console in one
                loggerConfig.Enrich.FromLogContext().ReadFrom.Configuration(Configuration);
            }
            else
            {
                loggerConfig.Enrich.FromLogContext().WriteTo.Console();
            }

            Log.Logger = loggerConfig.CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            // logging
            var loggerFactory = new LoggerFactory().AddSerilog();
            ILogger logger = loggerFactory.CreateLogger<Startup>();

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
            services.AddParisConfiguration(Configuration, logger);
            services.AddGroupConfiguration(Configuration, logger);
            services.AddSesEmailConfiguration(Configuration, logger);
            services.AddRedis(Configuration, _useRedisSession, logger);

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

            //Quartz stuff

            var scheduler = new QuartzScheduler(serviceProvider.GetService<ShortUrlRedirects>(),
                serviceProvider.GetService<LegacyUrlRedirects>(), serviceProvider.GetService<IRepository>(), serviceProvider.GetService<ITimeProvider>(), serviceProvider.GetService<IGroupsService>(), serviceProvider.GetService<FeatureToggles>());
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

            // close logger
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}