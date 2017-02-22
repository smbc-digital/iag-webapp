using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using AngleSharp.Parser.Html;
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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using NLog.Extensions.Logging;
using StockportWebapp.ContentFactory;
using StockportWebapp.AmazonSES;
using StockportWebapp.Builders;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Services;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Middleware;
using StockportWebapp.Parsers;
using StockportWebapp.RSS;
using StockportWebapp.Scheduler;
using StockportWebapp.ModelBinders;

namespace StockportWebapp
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private readonly string _appEnvironment;
        private readonly string _contentRootPath;
        public readonly string ConfigDir = "app-config";

        public Startup(IHostingEnvironment env)
        {
            var appConfig = Path.Combine(ConfigDir, "appsettings.json");
            var envConfig = Path.Combine(ConfigDir, $"appsettings.{env.EnvironmentName}.json");
            var secretConfig = Path.Combine(ConfigDir, "injected",
                $"appsettings.{env.EnvironmentName}.secrets.json");

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(appConfig)
                .AddJsonFile(envConfig)
                .AddJsonFile(secretConfig, true)
                .AddEnvironmentVariables()
                .Build();

            _appEnvironment = env.EnvironmentName;
            _contentRootPath = env.ContentRootPath;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(p =>
            {
                var featureTogglesReader = new FeatureTogglesReader($"{_contentRootPath}/featureToggles.yml", _appEnvironment,
                    p.GetService<ILogger<FeatureTogglesReader>>());
                return featureTogglesReader.Build<FeatureToggles>();
            });

            services.AddSingleton<IApplicationConfiguration>(_ => new ApplicationConfiguration(Configuration));
            services.AddSingleton(_ => new ShortUrlRedirects(new BusinessIdRedirectDictionary()));
            services.AddSingleton(_ => new LegacyUrlRedirects(new BusinessIdRedirectDictionary()));

            services.AddSingleton<Func<System.Net.Http.HttpClient>>(
                p => () => p.GetService<System.Net.Http.HttpClient>());
            services.AddTransient<System.Net.Http.HttpClient>();

            services.AddScoped<BusinessId>();

            services.AddTransient(
                p => new UrlGenerator(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));

            services.AddSingleton<ISimpleTagParserContainer>(
                p => new SimpleTagParserContainer(p.GetService<List<ISimpleTagParser>>()));
            services.AddSingleton<IContactUsMessageTagParser, ContactUsMessageTagParser>();
            services.AddSingleton<IDynamicTagParser<Profile>, ProfileTagParser>();
            services.AddSingleton<IDynamicTagParser<Document>, DocumentTagParser>();
            services.AddSingleton<ITimeProvider>(new TimeProvider());
            services.AddSingleton(
                p =>
                    new List<ISimpleTagParser>()
                    {
                        new ButtonTagParser(),
                        new ContactUsTagParser(p.GetService<IViewRender>(), p.GetService<ILogger<ContactUsTagParser>>()),
                        new VideoTagParser(),
                        new CarouselTagParser()
                    });
            services.AddSingleton(_ => new MarkdownWrapper());
            services.AddTransient(_ => new MarkdownPipelineBuilder().UsePipeTables().Build());
            services.AddSingleton<IRssFeedFactory, RssFeedFactory>();
            services.AddTransient<ArticleFactory>();
            services.AddTransient<SectionFactory>();

            services.AddTransient<IHttpClient>(
                p => new LoggingHttpClient(new HttpClient(new System.Net.Http.HttpClient()),
                    p.GetService<ILogger<LoggingHttpClient>>()));

            services.AddTransient<IProcessedContentRepository>(
                p =>
                    new ProcessedContentRepository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>(),
                        new ContentTypeFactory(p.GetService<ISimpleTagParserContainer>(),
                            p.GetService<IDynamicTagParser<Profile>>(), p.GetService<MarkdownWrapper>(),
                            p.GetService<IDynamicTagParser<Document>>())));
            services.AddTransient<IRepository>(
                p => new Repository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>()));

            services.AddTransient<IHealthcheckService>(
                p => new HealthcheckService($"{_contentRootPath}/version.txt", $"{_contentRootPath}/sha.txt",
                    new FileWrapper(), p.GetService<FeatureToggles>(), p.GetService<Func<System.Net.Http.HttpClient>>(),
                    p.GetService<UrlGenerator>()));

            services.AddSingleton<IEmailConfigurationBuilder, EmailConfigurationBuilder>();
            services.AddTransient<IHttpEmailClient, HttpEmailClient>();
            services.AddTransient<IEmailBuilder, EmailBuilder>();
            services.AddSingleton<IHtmlUtilities, HtmlUtilities>();
            services.AddTransient<HtmlParser>();

            var amazonSesKeys = new AmazonSESKeys(Configuration["ses:accessKey"], Configuration["ses:secretKey"]);
            services.AddSingleton(amazonSesKeys);

            var credentals = new BasicAWSCredentials(amazonSesKeys.Accesskey, amazonSesKeys.SecretKey);

            services.AddTransient<IAmazonSimpleEmailService>(o => new AmazonSimpleEmailServiceClient(credentals, RegionEndpoint.EUWest1));
            services.AddSingleton<IStaticAssets, StaticAssets>();
            services.AddTransient<IFilteredUrl>(p => new FilteredUrl(p.GetService<ITimeProvider>()));
            services.AddTransient<IDateCalculator>(p => new DateCalculator(p.GetService<ITimeProvider>()));

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeFormatConverterModelBinderProvider());
            });

            services.AddSingleton<IViewRender, ViewRender>();
            services.AddScoped<ILegacyRedirectsManager, LegacyRedirectsMapper>();
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();

            services.Configure<RazorViewEngineOptions>(
            options => { options.ViewLocationExpanders.Add(new ViewLocationExpander()); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            var scheduler = new RedirectScheduler(serviceProvider.GetService<ShortUrlRedirects>(),
                serviceProvider.GetService<LegacyUrlRedirects>(), serviceProvider.GetService<IRepository>());
            await scheduler.Start();

            app.UseMiddleware<BusinessIdMiddleware>();
            app.UseMiddleware<ShortUrlRedirectsMiddleware>();
            app.UseMiddleware<RobotsTxtMiddleware>();
            app.UseMiddleware<BetaToWwwMiddleware>();
            app.UseMiddleware<SecurityHeaderMiddleware>();

            loggerFactory.AddNLog();
            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse =
                    (context) =>
                    {
                        var isLive = context.Context.Request.Host.Value.StartsWith("www.");
                        var businessId = context.Context.Request.Headers["BUSINESS-ID"];
                        var url = string.Concat("robots-", businessId, isLive ? "-live" : "", ".txt");
                        if (context.File.Name == url)
                        {
                            context.Context.Response.Headers["Cache-Control"] = "public, max-age=0";
                        }
                        else
                        {
                            context.Context.Response.Headers["Cache-Control"] = "public, max-age=" + Cache.Medium.ToString();
                        }                     
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