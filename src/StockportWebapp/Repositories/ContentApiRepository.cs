namespace StockportWebapp.Repositories;

public interface IContentApiRepository : IBaseRepository
{
    Task<T> GetResponse<T>();
    Task<T> GetResponse<T>(string extra);
    Task<T> GetResponse<T>(List<Query> queries);
    Task<T> GetResponse<T>(string extra, List<Query> queries);
    Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent);
    Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent, string extra);
}

// TODO: Test this
public class ContentApiRepository(IHttpClient httpClient,
                                IApplicationConfiguration config,
                                IUrlGeneratorSimple urlGeneratorSimple,
                                ILogger<BaseRepository> logger) : BaseRepository(httpClient, config, logger), IContentApiRepository
{
    private readonly IUrlGeneratorSimple _urlGeneratorSimple = urlGeneratorSimple;

    #region GET Methods
    public async Task<T> GetResponse<T>() =>
        await GetResponseAsync<T>(_urlGeneratorSimple.BaseContentApiUrl<T>().TrimEnd('/'));

    public async Task<T> GetResponse<T>(string extra) =>
        await GetResponseAsync<T>(_urlGeneratorSimple.BaseContentApiUrl<T>().AddSlug(extra));

    public async Task<T> GetResponse<T>(List<Query> queries) =>
        await GetResponseAsync<T>(_urlGeneratorSimple.BaseContentApiUrl<T>().AddQueryStrings(queries));

    public async Task<T> GetResponse<T>(string extra, List<Query> queries) =>
        await GetResponseAsync<T>(_urlGeneratorSimple.BaseContentApiUrl<T>().AddSlug(extra).AddQueryStrings(queries));

    #endregion

    #region PUT Methods

    public async Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent) =>
        await PutResponseAsync<T>(_urlGeneratorSimple.BaseContentApiUrl<T>(), httpContent);


    public async Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent, string extra) =>
        await PutResponseAsync<T>(_urlGeneratorSimple.BaseContentApiUrl<T>().AddSlug(extra), httpContent);

    #endregion
}