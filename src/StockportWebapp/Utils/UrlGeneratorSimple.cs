namespace StockportWebapp.Utils;

public interface IUrlGeneratorSimple
{
    string BaseContentApiUrl<T>();
    string StockportApiUrl<T>();
}

[ExcludeFromCodeCoverage]
public class UrlGeneratorSimple(IApplicationConfiguration config, BusinessId businessId) : IUrlGeneratorSimple
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
        {typeof(List<News>), "news/latest/"},
        {typeof(List<AtoZ>), "atoz/"},
        {typeof(Footer), "footer"},
        {typeof(Event), "events"},
        {typeof(EventCalendar), "events"},
        {typeof(EventHomepage), "eventhomepage"},
        {typeof(EventResponse), "events"},
        {typeof(Payment), "payments/"},
        {typeof(List<Payment>), "payments/"},
        {typeof(Section), "sections/"},
        {typeof(List<Section>), "sections/"},
        {typeof(List<EventCategory>), "event-categories"},
        {typeof(ContactUsId), "contact-us-id/"},
        {typeof(List<ArticleSiteMap>), "articleSiteMap"},
        {typeof(List<SectionSiteMap>), "sectionSiteMap"},
        {typeof(Document), "documents/"},
        {typeof(List<Event>), "events"}
    };

    public string BaseContentApiUrl<T>() =>
        string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/");

    public string StockportApiUrl<T>() =>
        string.Concat(_config.GetStockportApiUri(), _urls[typeof(T)], "/");
}