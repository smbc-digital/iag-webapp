using StockportWebapp.Configuration;
using ILogger = Serilog.ILogger;
using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomisationOfViews(this IServiceCollection services)
        {
            services.AddSingleton<IViewRender, ViewRender>();
            services.Configure<RazorViewEngineOptions>(options => { options.ViewLocationExpanders.Add(new ViewLocationExpander()); });

            return services;
        }

        public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CivicaPayConfiguration>(configuration.GetSection(CivicaPayConfiguration.ConfigValue));

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

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IApplicationConfiguration>(_ => new ApplicationConfiguration(configuration));
            services.AddSingleton(configuration);
            services.AddSingleton<IAnalyticsConfiguration>(_ => new Models.Config.AnalyticsConfiguration(_.GetService<IApplicationConfiguration>()));

            return services;
        }

        public static IServiceCollection AddTagParsers(this IServiceCollection services)
        {
            services.AddSingleton<ISimpleTagParserContainer>(p => new SimpleTagParserContainer(p.GetService<List<ISimpleTagParser>>()));
            services.AddSingleton<IContactUsMessageTagParser, ContactUsMessageTagParser>();
            services.AddSingleton<IDynamicTagParser<Profile>, ProfileTagParser>();
            services.AddSingleton<IDynamicTagParser<InlineQuote>, InlineQuoteTagParser>();
            services.AddSingleton<IDynamicTagParser<Document>, DocumentTagParser>();
            services.AddSingleton<IDynamicTagParser<Alert>, AlertsInlineTagParser>();
            services.AddSingleton<IDynamicTagParser<S3BucketSearch>, S3BucketSearchTagParser>();
            services.AddSingleton<IDynamicTagParser<PrivacyNotice>, PrivacyNoticeTagParser>();
            services.AddSingleton(
                p =>
                    new List<ISimpleTagParser>()
                    {
                        new ButtonTagParser(),
                        new ContactUsTagParser(p.GetService<IViewRender>(), p.GetService<ILogger<ContactUsTagParser>>()),
                        new VideoTagParser(),
                        new CarouselTagParser(),
                        new IFrameTagParser(),
                        new FormBuilderTagParser(),
                        new MapTagParser()
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
            services.AddTransient<ITriviaFactory>(p => new TriviaFactory(p.GetService<MarkdownWrapper>()));
            services.AddTransient<SectionFactory>();
            services.AddTransient(p => new ArticleFactory(
                p.GetService<ISimpleTagParserContainer>(),
                p.GetService<IDynamicTagParser<Profile>>(),
                p.GetService<SectionFactory>(),
                p.GetService<MarkdownWrapper>(),
                p.GetService<IDynamicTagParser<Document>>(),
                p.GetService<IDynamicTagParser<Alert>>(),
                p.GetService<IDynamicTagParser<S3BucketSearch>>(),
                p.GetService<IDynamicTagParser<PrivacyNotice>>(),
                p.GetService<IRepository>()));
            services.AddTransient(p => new DocumentPageFactory(
                p.GetService<MarkdownWrapper>()));

            services.AddTransient<TopicFactory>();
            services.AddTransient(p => new TopicFactory(
                p.GetService<ISimpleTagParserContainer>(),
                p.GetService<MarkdownWrapper>()));

            return services;
        }

        public static IServiceCollection AddCustomHttpClients(this IServiceCollection services, bool sendAmazonEmails)
        {
            services.AddSingleton<Func<System.Net.Http.HttpClient>>(p => () => p.GetService<System.Net.Http.HttpClient>());
            services.AddTransient<System.Net.Http.HttpClient>();
            services.AddTransient<IHttpClient>(
                p => new LoggingHttpClient(new Client.HttpClient(new System.Net.Http.HttpClient()),
                    p.GetService<ILogger<LoggingHttpClient>>()));
            services.AddTransient<IHttpEmailClient>(p => new HttpEmailClient(p.GetService<ILogger<HttpEmailClient>>(), p.GetService<IEmailBuilder>(), p.GetService<IAmazonSimpleEmailService>(), sendAmazonEmails));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IProcessedContentRepository>(
                p =>
                    new ProcessedContentRepository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>(),
                        new ContentTypeFactory(
                                        p.GetService<ISimpleTagParserContainer>(),
                                            p.GetService<IDynamicTagParser<Profile>>(),
                                            p.GetService<MarkdownWrapper>(),
                                            p.GetService<IDynamicTagParser<Document>>(),
                                            p.GetService<IDynamicTagParser<Alert>>(),
                                            p.GetService<IHttpContextAccessor>(),
                                            p.GetService<IDynamicTagParser<S3BucketSearch>>(),
                                            p.GetService<IDynamicTagParser<PrivacyNotice>>()),
                        p.GetService<IApplicationConfiguration>()));
            services.AddTransient<IRepository>(p => new Repository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>(), p.GetService<IApplicationConfiguration>(), p.GetService<IUrlGeneratorSimple>(), p.GetService<ILogger<Repository>>()));
            services.AddTransient<IStockportApiRepository>(p => new StockportApiRepository(p.GetService<IHttpClient>(), p.GetService<IApplicationConfiguration>(), p.GetService<IUrlGeneratorSimple>(), p.GetService<ILogger<BaseRepository>>()));
            services.AddTransient<IContentApiRepository>(p => new ContentApiRepository(p.GetService<IHttpClient>(), p.GetService<IApplicationConfiguration>(), p.GetService<IUrlGeneratorSimple>(), p.GetService<ILogger<BaseRepository>>()));
           
            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services, string contentRootPath, string appEnvironment)
        {
            services.AddTransient<IHealthcheckService>(
                p => new HealthcheckService($"{contentRootPath}/version.txt", $"{contentRootPath}/sha.txt",
                    new FileWrapper(), p.GetService<IHttpClient>(),
                    p.GetService<UrlGenerator>(), appEnvironment, p.GetService<IApplicationConfiguration>(), p.GetService<BusinessId>()));

            services.AddTransient<INewsService>(p => new NewsService(p.GetService<IRepository>()));
            services.AddTransient<IEventsService>(p => new EventsService(p.GetService<IRepository>()));
            services.AddTransient<IHomepageService>(p => new HomepageService(p.GetService<IProcessedContentRepository>()));
            services.AddTransient<IStockportApiEventsService>(p => new StockportApiEventsService(p.GetService<IStockportApiRepository>(), p.GetService<IUrlGeneratorSimple>(), p.GetService<IEventFactory>()));
            services.AddTransient<IGroupsService>(p => new GroupsService(p.GetService<IContentApiRepository>(), p.GetService<IProcessedContentRepository>(), p.GetService<IHttpEmailClient>(), p.GetService<IApplicationConfiguration>(), p.GetService<ILogger<GroupsService>>(), p.GetService<IStockportApiRepository>(), p.GetService<BusinessId>()));
            services.AddTransient<IProfileService>(p => new
                ProfileService(
                    p.GetService<IRepository>(),
                    p.GetService<ISimpleTagParserContainer>(),
                    p.GetService<MarkdownWrapper>(),
                    p.GetService<IDynamicTagParser<Alert>>(),
                    p.GetService<ITriviaFactory>(),
                    p.GetService<IDynamicTagParser<InlineQuote>>()));

            services.AddTransient<IDirectoryService>(p => new DirectoryService(p.GetService<IApplicationConfiguration>(), p.GetService<MarkdownWrapper>(), p.GetService<IRepository>()));

            return services;
        }

        public static IServiceCollection AddBuilders(this IServiceCollection services)
        {
            services.AddSingleton<IEmailConfigurationBuilder, EmailConfigurationBuilder>();
            services.AddTransient<IEmailBuilder, EmailBuilder>();
            services.AddTransient(p => new GroupEmailBuilder(p.GetService<ILogger<GroupEmailBuilder>>(),
                p.GetService<IHttpEmailClient>(),
                p.GetService<IApplicationConfiguration>(),
                p.GetService<BusinessId>()));

            return services;
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            services.AddSingleton(p => new CalendarHelper());
            services.AddTransient<ICookiesHelper, CookiesHelper>();
            services.AddSingleton(p => new CookiesHelper(p.GetService<IHttpContextAccessor>(), p.GetService<ILogger<CookiesHelper>>()));
            services.AddTransient<IArticleRepository>(
                p =>
                    new ArticleRepository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>(),
                        p.GetService<ArticleFactory>(), p.GetService<IApplicationConfiguration>()));

            services.AddTransient<ITopicRepository>(
              p =>
                  new TopicRepository(p.GetService<TopicFactory>(), p.GetService<UrlGenerator>(), p.GetService<IHttpClient>(),
                      p.GetService<IApplicationConfiguration>(), p.GetService<UrlGeneratorSimple>()));
            services.AddTransient<IDocumentPageRepository>(
                p =>
                    new DocumentPageRepository(p.GetService<UrlGenerator>(), p.GetService<IHttpClient>(),
                        p.GetService<DocumentPageFactory>(), p.GetService<IApplicationConfiguration>()));
            services.AddSingleton<IEventFactory>(p => new EventFactory(p.GetService<ISimpleTagParserContainer>(), p.GetService<MarkdownWrapper>(), p.GetService<IDynamicTagParser<Document>>()));
            services.AddTransient<ILoggedInHelper>(p => new LoggedInHelper(p.GetService<IHttpContextAccessor>(), p.GetService<CurrentEnvironment>(), p.GetService<IJwtDecoder>(), p.GetService<ILogger<LoggedInHelper>>()));
            
            return services;
        }

        public static IServiceCollection AddGroupConfiguration(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            if (!string.IsNullOrEmpty(configuration["group:authenticationKey"]))
            {
                var groupKeys = new GroupAuthenticationKeys { Key = configuration["group:authenticationKey"] };
                services.AddSingleton(groupKeys);

                services.AddScoped(p => new GroupAuthorisation(p.GetService<IApplicationConfiguration>(), p.GetService<ILoggedInHelper>()));

                services.AddSingleton<IJwtDecoder>(p => new JwtDecoder(p.GetService<GroupAuthenticationKeys>(), p.GetService<ILogger<JwtDecoder>>()));
            }
            else
            {
                logger.Information("Group authenticationKey not found.");
            }

            return services;
        }

        public static IServiceCollection AddSesEmailConfiguration(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            services.Configure<SesConfiguration>(configuration.GetSection(SesConfiguration.ConfigValue));
            var sesConfiguration = new SesConfiguration();
            configuration.GetSection(SesConfiguration.ConfigValue).Bind(sesConfiguration);

            if (sesConfiguration is not null 
                && !string.IsNullOrEmpty(sesConfiguration.AccessKey) 
                && !string.IsNullOrEmpty(sesConfiguration.SecretKey))
            {
                logger.Information("WEBAPP : ServiceCollectionsExtensions : AddSesEmailConfiguration : Using SES configuration from secrets.");


                var amazonSesKeys = new AmazonSESKeys(sesConfiguration.AccessKey, sesConfiguration.SecretKey);
                services.AddSingleton(amazonSesKeys);
                services.AddTransient<IAmazonSimpleEmailService>(
                    implementation => new AmazonSimpleEmailServiceClient(new BasicAWSCredentials(sesConfiguration.AccessKey, sesConfiguration.SecretKey), RegionEndpoint.EUWest1));
            }
            else
            {
                logger.Information("WEBAPP : ServiceCollectionsExtensions : AddSesEmailConfiguration : Secrets not found.");
            }

            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration, bool useRedisSession, ILogger logger)
        {
            logger.Information($"WEBAPP : ServiceCollectionsExtensions : AddRedis : Configure redis for session management - TokenStoreUrl: {configuration["TokenStoreUrl"]} Enabled: {useRedisSession}");

            if (useRedisSession)
            {
                var redisUrl = configuration.GetValue<string>("TokenStoreUrl");
                logger.Information($"WEBAPP : ServiceCollectionsExtensions : AddRedis : Using Redis URL {redisUrl}");

                try
                {
                    var redisIp = GetHostEntryForUrl(redisUrl, logger);
                    logger.Information($"WEBAPP : ServiceCollectionExtensions : AddRedis : Using Redis for session management - url {redisUrl}, ip {redisIp}");
                    services.AddDataProtection().PersistKeysToRedis(redisIp);
                }
                catch(Exception ex)
                {
                    logger.Error(ex, $"WEBAPP : ServiceCollectionExtensions : AddRedis : Unable to setup Using redis for session management - url {redisUrl}");
                }
            }
            else
            {
                logger.Information("WEBAPP : ServiceCollectionExtensions : AddRedis : Not using redis for session management, falling back to memory cache");
            }

            return services;
        }

        private static string GetHostEntryForUrl(string host, ILogger logger)
        {
            if(string.IsNullOrEmpty(host))
                throw new ArgumentNullException("WEBAPP : ServiceCollectionExtensions : GetHostEntryForUrl: host can not be null");

            logger.Information($"WEBAPP : ServiceCollectionExtensions : GetHostEntryForUrl: Attempting to resolve {host}");

            var addresses = Dns.GetHostEntryAsync(host).Result.AddressList;

            if (!addresses.Any())
                throw new Exception($"WEBAPP : ServiceCollectionExtensions : GetHostEntryForUrl: No redis instance could be found for host {host}");
            
            if (addresses.Length > 1)
                logger.Warning($"WEBAPP : ServiceCollectionExtensions : GetHostEntryForUrl: Multple IP address for redis instance : {host} attempting to use first");

            return addresses.First().ToString();
        }
    }
}
