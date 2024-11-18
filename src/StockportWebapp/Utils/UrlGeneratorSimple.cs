namespace StockportWebapp.Utils;

public interface IUrlGeneratorSimple
{
    string BaseContentApiUrl<T>();
    string StockportApiUrl<T>();
}

[ExcludeFromCodeCoverage]
public class UrlGeneratorSimple : IUrlGeneratorSimple
{
    private readonly IApplicationConfiguration _config;
    private readonly BusinessId _businessId;

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
        {typeof(EventCalendarViewModel), "events"},
        {typeof(EventHomepage), "eventhomepage"},
        {typeof(GroupHomepage), "grouphomepage"},
        {typeof(EventResponse), "events"},
        {typeof(Group), "groups/"},
        {typeof(List<Group>), "groups"},
        {typeof(Payment), "payments/"},
        {typeof(List<Payment>), "payments/"},
        {typeof(Section), "sections/"},
        {typeof(List<Section>), "sections/"},
        {typeof(Showcase), "showcases/"},
        {typeof(List<Showcase>), "showcases/"},
        {typeof(List<GroupCategory>), "group-categories/"},
        {typeof(List<EventCategory>), "event-categories"},
        {typeof(GroupResults), "group-results/"},
        {typeof(ContactUsId), "contact-us-id/"},
        {typeof(List<ArticleSiteMap>), "articleSiteMap"},
        {typeof(List<SectionSiteMap>), "sectionSiteMap"},
        {typeof(Organisation), "organisations/"},
        {typeof(GroupAdvisor), "groups/advisors/"},
        {typeof(Document), "documents/"},
        {typeof(List<Event>), "events"}
    };

    public UrlGeneratorSimple(IApplicationConfiguration config, BusinessId businessId)
    {
        _config = config;
        _businessId = businessId;
    }

    public string BaseContentApiUrl<T>()
    {
        return string.Concat(_config.GetContentApiUri(), _businessId, "/", _urls[typeof(T)], "/");
    }

    public string StockportApiUrl<T>()
    {
        return string.Concat(_config.GetStockportApiUri(), _urls[typeof(T)], "/");
    }
}