using System;
using System.Collections.Generic;
using Markdig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using NLog.Extensions.Logging;
using StockportWebapp.ContentFactory;
using StockportWebapp.AmazonSES;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Services;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Middleware;
using StockportWebapp.Parsers;
using StockportWebapp.RSS;
using StockportWebapp.Scheduler;

namespace StockportWebapp
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private readonly string _appEnvironment;
        private readonly string _contentRootPath;
        
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            _appEnvironment = env.EnvironmentName;
            _contentRootPath = env.ContentRootPath;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            var featureToggleYaml = $"{_contentRootPath}/featureToggles.yml";

            services.AddSingleton(p =>
            {
                var featureTogglesReader = new FeatureTogglesReader(featureToggleYaml, _appEnvironment,
                    p.GetService<ILogger<FeatureTogglesReader>>());
                return featureTogglesReader.Build<FeatureToggles>();
            });

            services.AddSingleton<IApplicationConfiguration>(_ => new ApplicationConfiguration(Configuration));
            var urlRedirect = new UrlRedirect(new BusinessIdRedirectDictionary());
            services.AddSingleton(_ => urlRedirect);

            services.AddSingleton<Func<System.Net.Http.HttpClient>>(p => () => p.GetService<System.Net.Http.HttpClient>());
            services.AddTransient<System.Net.Http.HttpClient>();

            var featureTogglesReaderTemp = new FeatureTogglesReader(featureToggleYaml, _appEnvironment, null);
            var featureToggles = featureTogglesReaderTemp.Build<FeatureToggles>();

            var businessId = new BusinessId(Environment.GetEnvironmentVariable("BUSINESS_ID"));

            if (featureToggles.BusinessIdFromRequest)
            {
                services.AddScoped<BusinessId>();
            }
            else
            {
                services.AddSingleton(businessId);
            }

            services.AddTransient(p => new UrlGenerator(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));

            services.AddSingleton<ISimpleTagParserContainer>(p => new SimpleTagParserContainer(p.GetService<List<ISimpleTagParser>>()));
            services.AddSingleton<IContactUsMessageTagParser, ContactUsMessageTagParser>();
            services.AddSingleton<IDynamicTagParser<Profile>, ProfileTagParser>();
            services.AddSingleton<IDynamicTagParser<Document>, DocumentTagParser>();
            services.AddSingleton(p => new List<ISimpleTagParser>() { new ButtonTagParser(), new ContactUsTagParser(p.GetService<IViewRender>(), p.GetService<ILogger<ContactUsTagParser>>()), new VideoTagParser() });
            services.AddSingleton(_ => new MarkdownWrapper());
            services.AddTransient(_ => new MarkdownPipelineBuilder().UsePipeTables().Build());
            services.AddSingleton<IRssNewsFeedFactory,RssNewsFeedFactory>();
            services.AddTransient<ArticleFactory>();
            services.AddTransient<SectionFactory>();

            services.AddTransient<IHttpClient>(
                p => new LoggingHttpClient(new HttpClient(new System.Net.Http.HttpClient()),
                        p.GetService<ILogger<LoggingHttpClient>>()));
            
            services.AddTransient<IProcessedContentRepository>(
                p => new ProcessedContentRepository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>(), new ContentTypeFactory(p.GetService<ISimpleTagParserContainer>(), p.GetService<IDynamicTagParser<Profile>>(), p.GetService<MarkdownWrapper>(), p.GetService<IDynamicTagParser<Document>>())));
            services.AddTransient<IRepository>(
                p => new Repository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>()));

            services.AddTransient<IHealthcheckService>(
                p => new HealthcheckService($"{_contentRootPath}/version.txt", $"{_contentRootPath}/sha.txt",
                     new FileWrapper(), p.GetService<FeatureToggles>(), p.GetService<Func<System.Net.Http.HttpClient>>(),
                     p.GetService<UrlGenerator>()));

            services.AddSingleton<IEmailConfigurationBuilder, EmailConfigurationBuilder>();
            services.AddTransient<AmazonAuthorizationHeader>();
            services.AddTransient<IHttpEmailClient, HttpEmailClient>();
            services.AddSingleton(new AmazonSESKeys(
                        Environment.GetEnvironmentVariable("SES_ACCESS_KEY"),
                        Environment.GetEnvironmentVariable("SES_SECRET_KEY")));

            services.AddSingleton<IStaticAssets, StaticAssets>();

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
            services.AddSingleton<IViewRender, ViewRender>();
            services.AddScoped<ILegacyRedirects, LegacyRedirects>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander(businessId.ToString(), featureToggles));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            var scheduler = new RedirectScheduler(serviceProvider.GetService<UrlRedirect>(), serviceProvider.GetService<IRepository>());
            await scheduler.Start();

            app.UseMiddleware<BusinessIdMiddleware>();
            app.UseMiddleware<RedirectMiddleware>();
            app.UseMiddleware<RobotsTxtMiddleware>();

            loggerFactory.AddNLog();
            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = (context) =>
                {
                    context.Context.Response.Headers["Cache-Control"] = "public, max-age="+ Cache.DefaultDuration.ToString();
                }
            });
            app.UseStatusCodePagesWithReExecute("/Error/Error/{0}");

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
            app.UseMvc(routes => { routes.MapRoute("thankyou", "{controller=ContactUs}/{action=ThankYou}/"); });
            app.UseMvc(routes => { routes.MapRoute("search", "{controller=Search}/{action=Index}"); });
            app.UseMvc(routes => { routes.MapRoute("rss", "{controller=Rss}/{action=Index}"); });
        }
    }
}