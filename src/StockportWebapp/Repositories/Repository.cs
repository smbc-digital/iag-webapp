namespace StockportWebapp.Repositories;

public class Repository : IRepository
{
    private readonly UrlGenerator _urlGenerator;
    private readonly IUrlGeneratorSimple _urlGeneratorSimple;
    private readonly IHttpClient _httpClient;
    private readonly IApplicationConfiguration _config;
    private Dictionary<string, string> _authenticationHeaders;

    public Repository(UrlGenerator urlGenerator, IHttpClient httpClient, IApplicationConfiguration config, IUrlGeneratorSimple urlGeneratorSimple)
    {
        _urlGenerator = urlGenerator;
        _httpClient = httpClient;
        _config = config;
        _urlGeneratorSimple = urlGeneratorSimple;
        _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
    }

    public async Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null)
    {
        var url = _urlGenerator.UrlFor<T>(slug, queries);
        var httpResponse = await _httpClient.Get(url, _authenticationHeaders);
        return HttpResponse.Build<T>(httpResponse);
    }

    public async Task<HttpResponse> Put<T>(HttpContent content, string slug = "")
    {
        var url = $"{_urlGenerator.UrlFor<T>(slug)}";
        return await _httpClient.PutAsync(url, content, _authenticationHeaders);
    }

    public async Task<HttpResponse> Delete<T>(string slug = "")
    {
        var url = $"{_urlGenerator.UrlFor<T>(slug)}";
        return await _httpClient.DeleteAsync(url, _authenticationHeaders);
    }

    public async Task<HttpResponse> Archive<T>(HttpContent content, string slug = "")
    {
        var url = $"{_urlGenerator.UrlFor<T>(slug)}";
        return await _httpClient.PutAsync(url, content, _authenticationHeaders);
    }

    public async Task<HttpResponse> Publish<T>(HttpContent content, string slug = "")
    {
        var url = $"{_urlGenerator.UrlFor<T>(slug)}";
        return await _httpClient.PutAsync(url, content, _authenticationHeaders);
    }

    public async Task<HttpResponse> GetLatest<T>(int limit)
    {
        var url = _urlGenerator.UrlForLimit<T>(limit);
        var httpResponse = await _httpClient.Get(url, _authenticationHeaders);
        return HttpResponse.Build<T>(httpResponse);
    }

    public async Task<HttpResponse> RemoveAdministrator(string slug, string email)
    {
        var url = $"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}";
        return await _httpClient.DeleteAsync(url, _authenticationHeaders);
    }

    public async Task<HttpResponse> UpdateAdministrator(HttpContent user, string slug, string email)
    {
        var url = $"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}";
        return await _httpClient.PutAsync(url, user, _authenticationHeaders);
    }

    public async Task<HttpResponse> AddAdministrator(HttpContent user, string slug, string email)
    {
        var url = $"{_urlGenerator.UrlFor<Group>(slug)}/administrators/{email}";
        return await _httpClient.PostAsync(url, user, _authenticationHeaders);
    }

    public async Task<HttpResponse> GetLatestOrderByFeatured<T>(int limit)
    {
        var url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddExtraToUrl($"latest/{limit}").AddQueryStrings(new Query("featured", "true"));
        var httpResponse = await _httpClient.Get(url, _authenticationHeaders);
        return HttpResponse.Build<T>(httpResponse);
    }

    public async Task<HttpResponse> GetRedirects()
    {
        var url = _urlGenerator.RedirectUrl();
        var httpResponse = await _httpClient.Get(url, _authenticationHeaders);
        return HttpResponse.Build<Redirects>(httpResponse);
    }

    public async Task<HttpResponse> GetAdministratorsGroups(string email)
    {
        var url = _urlGenerator.AdministratorsGroups(email);
        var httpResponse = await _httpClient.Get(url, _authenticationHeaders);
        return HttpResponse.Build<List<Group>>(httpResponse);
    }
}