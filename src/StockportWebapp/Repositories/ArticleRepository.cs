namespace StockportWebapp.Repositories;

public interface IArticleRepository
{
    Task<HttpResponse> Get(string slug = "");
}

public class ArticleRepository : IArticleRepository
{
    private readonly ArticleFactory _articleFactory;
    private readonly IHttpClient _httpClient;
    private readonly IStubToUrlConverter _urlGenerator;
    private readonly Dictionary<string, string> authenticationHeaders;
    private readonly IApplicationConfiguration _config;

    public ArticleRepository(IStubToUrlConverter urlGenerator, IHttpClient httpClient, ArticleFactory articleFactory, IApplicationConfiguration config)
    {
        _urlGenerator = urlGenerator;
        _httpClient = httpClient;
        _articleFactory = articleFactory;
        _config = config;
        authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
    }

    public async Task<HttpResponse> Get(string slug = "")
    {
        var url = _urlGenerator.UrlFor<Article>(slug);
        var httpResponse = await _httpClient.Get(url, authenticationHeaders);

        if (!httpResponse.IsSuccessful())
            return httpResponse;

        var model = HttpResponse.Build<Article>(httpResponse);
        var article = (Article)model.Content;

        var processedModel = _articleFactory.Build(article);

        return HttpResponse.Successful(200, processedModel);
    }
}