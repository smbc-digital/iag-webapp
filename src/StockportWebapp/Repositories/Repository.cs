namespace StockportWebapp.Repositories;

public class Repository : IRepository
{
    private readonly UrlGenerator _urlGenerator;
    private readonly IUrlGeneratorSimple _urlGeneratorSimple;
    private readonly IHttpClient _httpClient;
    private readonly IApplicationConfiguration _config;
    private readonly Dictionary<string, string> _authenticationHeaders;

    public Repository(UrlGenerator urlGenerator, IHttpClient httpClient, IApplicationConfiguration config, IUrlGeneratorSimple urlGeneratorSimple)
    {
        _urlGenerator = urlGenerator;
        _httpClient = httpClient;
        _config = config;
        _urlGeneratorSimple = urlGeneratorSimple;
        _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
    }

    public async Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null) =>
        HttpResponse.Build<T>(await _httpClient.Get(_urlGenerator.UrlFor<T>(slug, queries), _authenticationHeaders));

    public async Task<HttpResponse> Put<T>(HttpContent content, string slug = "") =>
        await _httpClient.PutAsync(_urlGenerator.UrlFor<T>(slug), content, _authenticationHeaders);

    public async Task<HttpResponse> Delete<T>(string slug = "") =>
        await _httpClient.DeleteAsync(_urlGenerator.UrlFor<T>(slug), _authenticationHeaders);

    public async Task<HttpResponse> Archive<T>(HttpContent content, string slug = "") =>
        await _httpClient.PutAsync(_urlGenerator.UrlFor<T>(slug), content, _authenticationHeaders);

    public async Task<HttpResponse> Publish<T>(HttpContent content, string slug = "") =>
        await _httpClient.PutAsync(_urlGenerator.UrlFor<T>(slug), content, _authenticationHeaders);

    public async Task<HttpResponse> GetLatest<T>(int limit) =>
        HttpResponse.Build<T>(await _httpClient.Get(_urlGenerator.UrlForLimit<T>(limit), _authenticationHeaders));

    public async Task<HttpResponse> RemoveAdministrator(string slug, string email) =>
        await _httpClient.DeleteAsync($"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}", _authenticationHeaders);

    public async Task<HttpResponse> UpdateAdministrator(HttpContent user, string slug, string email) =>
        await _httpClient.PutAsync($"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}", user, _authenticationHeaders);

    public async Task<HttpResponse> AddAdministrator(HttpContent user, string slug, string email) =>
        await _httpClient.PostAsync($"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}", user, _authenticationHeaders);

    public async Task<HttpResponse> GetLatestOrderByFeatured<T>(int limit)
    {
        string url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddExtraToUrl($"latest/{limit}").AddQueryStrings(new Query("featured", "true"));        
        return HttpResponse.Build<T>(await _httpClient.Get(url, _authenticationHeaders));
    }

    public async Task<HttpResponse> GetRedirects() =>
        HttpResponse.Build<Redirects>(await _httpClient.Get(_urlGenerator.RedirectUrl(), _authenticationHeaders));

    public async Task<HttpResponse> GetAdministratorsGroups(string email) =>
        HttpResponse.Build<List<Group>>(await _httpClient.Get(_urlGenerator.AdministratorsGroups(email), _authenticationHeaders));
}