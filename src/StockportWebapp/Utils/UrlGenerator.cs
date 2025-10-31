using Directory = StockportWebapp.Models.Directory;

namespace StockportWebapp.Utils;

public interface IStubToUrlConverter
{
    string UrlFor<T>(string slug = "", List<Query> queries = null);
    string HealthcheckUrl();
}

public class UrlGenerator(IApplicationConfiguration config, BusinessId businessId) : IStubToUrlConverter
{
    private readonly IApplicationConfiguration _config = config;
    private readonly BusinessId _businessId = businessId;

    private readonly Dictionary<Type, string> _urls = new()
    {
        {typeof(Topic), "topics/"},
        {typeof(List<TopicSitemap>), "topics/"},
        {typeof(Article), "articles/"},
        {typeof(Models.Profile), "profiles/"},
        {typeof(List<Models.Profile>), "profiles/"},
        {typeof(StartPage), "start-page/"},
        {typeof(List<StartPage>), "start-page/"},
        {typeof(Homepage), "homepage"},
        {typeof(News), "news/"},
        {typeof(Newsroom), "news"},
        {typeof(List<News>), "news"},
        {typeof(List<AtoZ>), "atoz/"},
        {typeof(SiteHeader), "header"},
        {typeof(Footer), "footer"},
        {typeof(Event), "events/"},
        {typeof(EventCalendar), "events"},
        {typeof(List<Event>), "events"},
        {typeof(EventHomepage), "eventhomepage"},
        {typeof(EventResponse), "events"},
        {typeof(Payment), "payments/"},
        {typeof(List<Payment>), "payments/"},
        {typeof(ServicePayPayment), "service-pay-payments/"},
        {typeof(Section), "sections/"},
        {typeof(List<Section>), "sections/"},
        {typeof(List<EventCategory>), "event-categories/"},
        {typeof(ContactUsId), "contact-us-id/"},
        {typeof(List<ArticleSiteMap>), "articleSiteMap"},
        {typeof(List<SectionSiteMap>), "sectionSiteMap"},
        {typeof(Document), "documents/"},
        {typeof(PrivacyNotice), "privacy-notices/"},
        {typeof(List<PrivacyNotice>), "privacy-notices/"},
        {typeof(ContactUsArea), "contactusarea"},
        {typeof(DocumentPage), "document-page/"},
        {typeof(Directory), "directory/"},
        {typeof(DirectoryEntry), "directory-entry/"},
        {typeof(LandingPage), "landing/"},
    };

    public string UrlFor<T>(string slug = "", List<Query> queries = null)
    {
        var url = string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], slug,
            CreateQueryString(queries));
        return url;
    }

    public string UrlForLimit<T>(int limit) =>
        string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/latest/",
            limit.ToString());

    public string UrlForLimitAndFeatured<T>(int limit, bool featured) =>
        string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/latest/",
            limit.ToString(), $"?featured={featured.ToString().ToLower()}");

    private static string CreateQueryString(List<Query> queries) =>
        queries is null || queries.Count < 1
            ? string.Empty
            : "?" + string.Join("&", queries);

    public string RedirectUrl() =>
        string.Concat(_config.GetContentApiUri(), "redirects");

    public string ArticlesForSiteMap(string slug = "", List<Query> queries = null) =>
        string.Concat(_config.GetContentApiUri(), _businessId, "/", "articleSiteMap", slug, CreateQueryString(queries));

    public string HealthcheckUrl()
    {
        Uri baseUrl = _config.GetContentApiUrlRoot();
        
        return new UriBuilder
        {
            Host = baseUrl.Host,
            Port = baseUrl.Port,
            Scheme = baseUrl.Scheme,
            Path = baseUrl.LocalPath + "_healthcheck"
        }.Uri.ToString();
    }
}