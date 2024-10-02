namespace StockportWebapp.Repositories;

public interface ITopicRepository
{
    Task<HttpResponse> Get<T>(string slug = "");
}

public class TopicRepository : ITopicRepository
{
    private readonly TopicFactory _topicFactory;
    private readonly UrlGenerator _urlGenerator;
    private readonly IHttpClient _httpClient;
    private readonly IApplicationConfiguration _config;
    private Dictionary<string, string> _authenticationHeaders;

    public TopicRepository(TopicFactory topicFactory, UrlGenerator urlGenerator, IHttpClient httpClient, IApplicationConfiguration config)
    {
        _topicFactory = topicFactory;
        _urlGenerator = urlGenerator;
        _httpClient = httpClient;
        _config = config;
        _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
    }

    public async Task<HttpResponse> Get<T>(string slug = "")
    {
        string url = _urlGenerator.UrlFor<Topic>(slug);
        HttpResponse httpResponse = await _httpClient.Get(url, _authenticationHeaders);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        HttpResponse model = HttpResponse.Build<Topic>(httpResponse);
        Topic topic = (Topic)model.Content;

        ProcessedTopic processedModel = _topicFactory.Build(topic);
        return HttpResponse.Successful(200, processedModel);
    }
}
