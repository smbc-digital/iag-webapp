using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using AngleSharp.Parser.Html;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockportWebapp.AmazonSES;
using StockportWebapp.Builders;
using StockportWebapp.Config;
using StockportWebapp.ContentFactory;
using StockportWebapp.Controllers;
using StockportWebapp.DataProtection;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Filters;
using StockportWebapp.Helpers;
using StockportWebapp.Http;
using StockportWebapp.Middleware;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.RSS;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using StockportWebapp.Validation;

namespace StockportWebapp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFeatureToggles(this IServiceCollection services, string contentRootPath, string appEnvironment)
        {
            services.AddSingleton(p =>
            {
                var featureTogglesReader = new FeatureTogglesReader($"{contentRootPath}/featureToggles.yml", appEnvironment,
                    p.GetService<ILogger<FeatureTogglesReader>>());
                return featureTogglesReader.Build<FeatureToggles>();
            });

            return services;
        }

        public static IServiceCollection AddCustomisationOfViews(this IServiceCollection services)
        {
            services.AddSingleton<IViewRender, ViewRender>();
            services.Configure<RazorViewEngineOptions>(options => { options.ViewLocationExpanders.Add(new ViewLocationExpander()); });

            return services;
        }

        public static IServiceCollection AddRedirects(this IServiceCollection services)
        {
            services.AddSingleton(_ => new ShortUrlRedirects(new BusinessIdRedirectDictionary()));
            services.AddSingleton(_ => new LegacyUrlRedirects(new BusinessIdRedirectDictionary()));
            services.AddScoped<ILegacyRedirectsManager, LegacyRedirectsMapper>();

            return services;
        }

        public static IServiceCollection AddTimeProvider(this IServiceCollection services)
        {
            services.AddSingleton<ITimeProvider>(new TimeProvider());
            services.AddTransient<IDateCalculator>(p => new DateCalculator(p.GetService<ITimeProvider>()));

            return services;
        }

        public static IServiceCollection AddRecapthca(this IServiceCollection services)
        {
            services.AddSingleton(_ => new ValidateReCaptchaAttribute(_.GetService<IApplicationConfiguration>(), _.GetService<IHttpClient>()));
            services.AddScoped<ValidateReCaptchaAttribute>();

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<IApplicationConfiguration>(_ => new ApplicationConfiguration(configuration));
            services.AddSingleton<IConfigurationRoot>(configuration);
            services.AddSingleton<IConfiguration>(configuration);

            return services;
        }

        public static IServiceCollection AddTagParsers(this IServiceCollection services)
        {
            services.AddSingleton<ISimpleTagParserContainer>(p => new SimpleTagParserContainer(p.GetService<List<ISimpleTagParser>>()));
            services.AddSingleton<IContactUsMessageTagParser, ContactUsMessageTagParser>();
            services.AddSingleton<IDynamicTagParser<Profile>, ProfileTagParser>();
            services.AddSingleton<IDynamicTagParser<Document>, DocumentTagParser>();
            services.AddSingleton<IDynamicTagParser<Alert>, AlertsInlineTagParser>();
            services.AddSingleton(
                p =>
                    new List<ISimpleTagParser>()
                    {
                        new ButtonTagParser(),
                        new ContactUsTagParser(p.GetService<IViewRender>(), p.GetService<ILogger<ContactUsTagParser>>()),
                        new VideoTagParser(),
                        new CarouselTagParser(),
                        new IShareTagParser()
                    });

            return services;
        }

        public static IServiceCollection AddMarkdown(this IServiceCollection services)
        {
            services.AddSingleton(_ => new MarkdownWrapper());
            services.AddTransient(_ => new MarkdownPipelineBuilder().UsePipeTables().Build());

            return services;
        }

        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<IRssFeedFactory, RssFeedFactory>();
            services.AddTransient<ArticleFactory>();
            services.AddTransient<SectionFactory>();

            return services;
        }

        public static IServiceCollection AddCustomHttpClients(this IServiceCollection services)
        {
            services.AddSingleton<Func<System.Net.Http.HttpClient>>(p => () => p.GetService<System.Net.Http.HttpClient>());
            services.AddTransient<System.Net.Http.HttpClient>();
            services.AddTransient<IHttpClient>(
                p => new LoggingHttpClient(new HttpClient(new System.Net.Http.HttpClient()),
                    p.GetService<ILogger<LoggingHttpClient>>()));
            services.AddTransient<IHttpEmailClient, HttpEmailClient>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IProcessedContentRepository>(
                p =>
                    new ProcessedContentRepository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>(),
                        new ContentTypeFactory(p.GetService<ISimpleTagParserContainer>(),
                            p.GetService<IDynamicTagParser<Profile>>(), p.GetService<MarkdownWrapper>(),
                            p.GetService<IDynamicTagParser<Document>>(), p.GetService<IDynamicTagParser<Alert>>())));
            services.AddTransient<IRepository>(p => new Repository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>()));
            services.AddTransient<IPaymentRepository, PaymentRepository>();

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services, string contentRootPath, string appEnvironment)
        {
            services.AddTransient<IHealthcheckService>(
                p => new HealthcheckService($"{contentRootPath}/version.txt", $"{contentRootPath}/sha.txt",
                    new FileWrapper(), p.GetService<FeatureToggles>(), p.GetService<Func<System.Net.Http.HttpClient>>(),
                    p.GetService<UrlGenerator>(), appEnvironment));

            return services;
        }

        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            services.AddSingleton<IEmailConfigurationBuilder, EmailConfigurationBuilder>();
            services.AddTransient<IEmailBuilder, Builders.EmailBuilder>();
            services.AddTransient(p => new GroupEmailBuilder(p.GetService<ILogger<GroupEmailBuilder>>(),
                p.GetService<IHttpEmailClient>(),
                p.GetService<IApplicationConfiguration>(),
                p.GetService<BusinessId>()));
            services.AddTransient(p => new EventEmailBuilder(p.GetService<ILogger<EventEmailBuilder>>(),
                p.GetService<IHttpEmailClient>(),
                p.GetService<IApplicationConfiguration>(),
                p.GetService<BusinessId>()));

            return services;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddSingleton(p => new CalendarHelper(p.GetService<ITimeProvider>()));
            services.AddSingleton<ParisHashHelper>();

            return services;
        }

        public static IServiceCollection AddParisConfiguration(this IServiceCollection services, IConfigurationRoot configuration, ILogger logger)
        {
            if (!string.IsNullOrEmpty(configuration["paris:preSalt"]) &&
                !string.IsNullOrEmpty(configuration["paris:postSalt"]) &&
                !string.IsNullOrEmpty(configuration["paris:privateSalt"]))
            {
                var parisKeys = new ParisKeys()
                {
                    PreSalt = configuration["paris:preSalt"],
                    PostSalt = configuration["paris:postSalt"],
                    PrivateSalt = configuration["paris:privateSalt"]
                };

                services.AddSingleton(parisKeys);
            }
            else
            {
                logger.LogInformation("Paris secrets not found.");
            }

            return services;
        }

        public static IServiceCollection AddGroupConfiguration(this IServiceCollection services, IConfigurationRoot configuration, ILogger logger)
        {
            if (!string.IsNullOrEmpty(configuration["group:authenticationKey"]))
            {
                var groupKeys = new GroupAuthenticationKeys { Key = configuration["group:authenticationKey"] };
                services.AddSingleton(groupKeys);

                services.AddScoped<GroupAuthorisation>(p => new GroupAuthorisation(p.GetService<IApplicationConfiguration>(), p.GetService<IJwtDecoder>(), p.GetService<CurrentEnvironment>(), p.GetService<ILogger<GroupAuthorisation>>()));

                services.AddSingleton<IJwtDecoder>(p => new JwtDecoder(p.GetService<GroupAuthenticationKeys>(), p.GetService<ILogger<JwtDecoder>>()));
            }
            else
            {
                logger.LogInformation("Group authenticationKey not found.");
            }

            return services;
        }

        public static IServiceCollection AddSesEmailConfiguration(this IServiceCollection services, IConfigurationRoot configuration, ILogger logger)
        {
            if (!string.IsNullOrEmpty(configuration["ses:accessKey"]) &&
                !string.IsNullOrEmpty(configuration["ses:secretKey"]))
            {
                var amazonSesKeys = new AmazonSESKeys(configuration["ses:accessKey"], configuration["ses:secretKey"]);
                services.AddSingleton(amazonSesKeys);
                var credentals = new BasicAWSCredentials(amazonSesKeys.Accesskey, amazonSesKeys.SecretKey);
                services.AddTransient<IAmazonSimpleEmailService>(
                    o => new AmazonSimpleEmailServiceClient(credentals, RegionEndpoint.EUWest1));
            }
            else
            {
                logger.LogInformation("Secrets not found.");
            }

            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfigurationRoot configuration, bool useRedisSession, ILogger logger)
        {
            if (useRedisSession)
            {
                var redisUrl = configuration["TokenStoreUrl"];
                var redisIp = GetHostEntryForUrl(redisUrl, logger);
                logger.LogInformation($"Using redis for session management - url {redisUrl}, ip {redisIp}");
                services.AddDataProtection().PersistKeysToRedis(redisIp);
            }
            else
            {
                logger.LogInformation("Not using redis for session management!");
            }

            return services;
        }

        public static IServiceCollection AddCustomisedAngleSharp(this IServiceCollection services)
        {
            services.AddTransient<HtmlParser>();
            services.AddSingleton<IHtmlUtilities, HtmlUtilities>();

            return services;
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
