using System;
using System.Collections.Generic;
using System.Globalization;
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
using StockportWebapp.DataProtection;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Filters;
using StockportWebapp.Validation;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using StockportWebapp.Helpers;
using StockportWebapp.QuestionBuilder;

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
            services.AddSingleton(p =>
            {
                var featureTogglesReader = new FeatureTogglesReader($"{_contentRootPath}/featureToggles.yml", _appEnvironment,
                    p.GetService<ILogger<FeatureTogglesReader>>());
                return featureTogglesReader.Build<FeatureToggles>();
            });

            services.AddSingleton<CurrentEnvironment>(new CurrentEnvironment(_appEnvironment));
            services.AddSingleton<IApplicationConfiguration>(_ => new ApplicationConfiguration(Configuration));
            services.AddSingleton(_ => new ShortUrlRedirects(new BusinessIdRedirectDictionary()));
            services.AddSingleton(_ => new LegacyUrlRedirects(new BusinessIdRedirectDictionary()));
            services.AddSingleton(_ => new ValidateReCaptchaAttribute(_.GetService<IApplicationConfiguration>(), _.GetService<IHttpClient>()));

            services.AddSingleton<Func<System.Net.Http.HttpClient>>(
                p => () => p.GetService<System.Net.Http.HttpClient>());
            services.AddTransient<System.Net.Http.HttpClient>();

            services.AddScoped<BusinessId>();

            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<ValidateReCaptchaAttribute>();

            services.AddTransient(
                p => new UrlGenerator(p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));

            services.AddSingleton<ISimpleTagParserContainer>(
                p => new SimpleTagParserContainer(p.GetService<List<ISimpleTagParser>>()));
            services.AddSingleton<IContactUsMessageTagParser, ContactUsMessageTagParser>();
            services.AddSingleton<IDynamicTagParser<Profile>, ProfileTagParser>();
            services.AddSingleton<IDynamicTagParser<Document>, DocumentTagParser>();
            services.AddSingleton<IDynamicTagParser<Alert>, AlertsInlineTagParser>();
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
                            p.GetService<IDynamicTagParser<Document>>(), p.GetService<IDynamicTagParser<Alert>>())));
            services.AddTransient<IRepository>(
                p => new Repository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>()));

            services.AddTransient<IHealthcheckService>(
                p => new HealthcheckService($"{_contentRootPath}/version.txt", $"{_contentRootPath}/sha.txt",
                    new FileWrapper(), p.GetService<FeatureToggles>(), p.GetService<Func<System.Net.Http.HttpClient>>(),
                    p.GetService<UrlGenerator>(), _appEnvironment));

            services.AddSingleton<IEmailConfigurationBuilder, EmailConfigurationBuilder>();
            services.AddTransient<IHttpEmailClient, HttpEmailClient>();
            services.AddTransient<IEmailBuilder, Builders.EmailBuilder>();
            services.AddTransient<HtmlParser>();
            services.AddSingleton<IHtmlUtilities, HtmlUtilities>();
            services.AddSingleton<ParisHashHelper>();

            var loggerFactory = new LoggerFactory().AddNLog();
            ILogger logger = loggerFactory.CreateLogger<Startup>();

            if (!string.IsNullOrEmpty(Configuration["paris:preSalt"]) &&
                !string.IsNullOrEmpty(Configuration["paris:postSalt"]) &&
                !string.IsNullOrEmpty(Configuration["paris:privateSalt"]))
            {
                var parisKeys = new ParisKeys()
                {
                    PreSalt = Configuration["paris:preSalt"],
                    PostSalt = Configuration["paris:postSalt"],
                    PrivateSalt = Configuration["paris:privateSalt"]
                };

                services.AddSingleton(parisKeys);
            }
            else
            {
                logger.LogInformation("Paris secrets not found.");
            }

            if (!string.IsNullOrEmpty(Configuration["group:authenticationKey"]))
            {
                var groupKeys = new GroupAuthenticationKeys { Key = Configuration["group:authenticationKey"] };
                services.AddSingleton(groupKeys);

                services.AddScoped<GroupAuthorisation>(p => new GroupAuthorisation(p.GetService<IApplicationConfiguration>(), p.GetService<IJwtDecoder>(), p.GetService<CurrentEnvironment>(), p.GetService<ILogger<GroupAuthorisation>>()));

                services.AddSingleton<IJwtDecoder>(p => new JwtDecoder(p.GetService<GroupAuthenticationKeys>(), p.GetService<ILogger<JwtDecoder>>()));
            }

            if (!string.IsNullOrEmpty(Configuration["ses:accessKey"]) &&
                !string.IsNullOrEmpty(Configuration["ses:secretKey"]))
            {
                var amazonSesKeys = new AmazonSESKeys(Configuration["ses:accessKey"], Configuration["ses:secretKey"]);
                services.AddSingleton(amazonSesKeys);
                var credentals = new BasicAWSCredentials(amazonSesKeys.Accesskey, amazonSesKeys.SecretKey);
                services.AddTransient<IAmazonSimpleEmailService>(
                    o => new AmazonSimpleEmailServiceClient(credentals, RegionEndpoint.EUWest1));
            }
            else
            {
                logger.LogInformation("Secrets not found.");
            }

            services.AddSingleton<IStaticAssets, StaticAssets>();
            services.AddTransient<IFilteredUrl>(p => new FilteredUrl(p.GetService<ITimeProvider>()));
            services.AddTransient<IDateCalculator>(p => new DateCalculator(p.GetService<ITimeProvider>()));

            ConfigureDataProtection(services, logger);

            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddNodeServices();
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeFormatConverterModelBinderProvider());
                if (_useRedisSession) options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddSingleton<IViewRender, ViewRender>();
            services.AddScoped<ILegacyRedirectsManager, LegacyRedirectsMapper>();
            services.AddTransient<IEventsRepository, EventsRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();

            services.AddTransient(p => new GroupEmailBuilder(p.GetService<ILogger<GroupEmailBuilder>>(), 
                                                            p.GetService<IHttpEmailClient>(),
                                                            p.GetService<IApplicationConfiguration>(),
                                                            p.GetService<BusinessId>()));

            services.Configure<RazorViewEngineOptions>(
            options => { options.ViewLocationExpanders.Add(new ViewLocationExpander()); });

            services.AddScoped<IBuildingRegsQuestions, BuildingRegsQuestions>(provider =>
            {
                return QuestionLoader.LoadQuestions<BuildingRegsQuestions>("BuildingRegs.json");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsEnvironment("int") || env.IsEnvironment("local") || env.IsEnvironment("qa"))
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

            var ci = new CultureInfo("en-GB") { DateTimeFormat = { ShortDatePattern = "dd/MM/yyyy" } };

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-GB"),
            },
                SupportedUICultures = new List<CultureInfo>
            {
                new CultureInfo("en-GB"),
            }
            });

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
            app.UseMvc(routes => { routes.MapRoute("thankyou", "{controller=ContactUs}/{action=ThankYou}/"); });
            app.UseMvc(routes => { routes.MapRoute("search", "{controller=Search}/{action=Index}"); });
            app.UseMvc(routes => { routes.MapRoute("rss", "{controller=Rss}/{action=Index}"); });
        }

        private void ConfigureDataProtection(IServiceCollection services, ILogger logger)
        {
            if (_useRedisSession)
            {
                var redisUrl = Configuration["TokenStoreUrl"];
                var redisIp = GetHostEntryForUrl(redisUrl, logger);
                logger.LogInformation($"Using redis for session management - url {redisUrl}, ip {redisIp}");
                services.AddDataProtection().PersistKeysToRedis(redisIp);
            }
            else
            {
                services.AddAntiforgery();
                logger.LogInformation("Not using redis for session management!");
            }
        }

        private static string GetHostEntryForUrl(string host, ILogger logger)
        {

            var addresses = Dns.GetHostEntryAsync(host).Result.AddressList;

            if (!addresses.Any())
            {
                logger.LogError($"Could not resolve IP address for redis instance : {host}");
                throw new Exception($"No redis instance could be found for host {host}");
            }

            if (addresses.Length > 1)
            {
                logger.LogWarning($"Multple IP address for redis instance : {host} attempting to use first");
            }

            return addresses.First().ToString();
        }
    }
}