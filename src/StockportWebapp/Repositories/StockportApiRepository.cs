namespace StockportWebapp.Repositories;

public interface IStockportApiRepository : IBaseRepository
{
    Task<T> GetResponse<T>();
    Task<T> GetResponse<T>(string extra);
    Task<T> GetResponse<T>(List<Query> queries);
    Task<T> GetResponse<T>(string extra, List<Query> queries);
    Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent);
    Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent, string extra);
}

// TODO: Test this
public class StockportApiRepository(IHttpClient httpClient,
                                    IApplicationConfiguration config,
                                    IUrlGeneratorSimple urlGeneratorSimple,
                                    ILogger<BaseRepository> logger) : BaseRepository(httpClient, config, logger), IStockportApiRepository
{
    private readonly IUrlGeneratorSimple _urlGeneratorSimple = urlGeneratorSimple;

    public async Task<T> GetResponse<T>() =>
        await GetResponseAsync<T>(_urlGeneratorSimple.StockportApiUrl<T>().TrimEnd('/'));

    public async Task<T> GetResponse<T>(string extra) =>
        await GetResponseAsync<T>(_urlGeneratorSimple.StockportApiUrl<T>().AddSlug(extra));

    public async Task<T> GetResponse<T>(List<Query> queries) =>
        await GetResponseAsync<T>(_urlGeneratorSimple.StockportApiUrl<T>().AddQueryStrings(queries));

    public async Task<T> GetResponse<T>(string extra, List<Query> queries) =>
        await GetResponseAsync<T>(_urlGeneratorSimple.StockportApiUrl<T>().AddSlug(extra).AddQueryStrings(queries));

    #region PUT Methods

    public async Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent) =>
        await PutResponseAsync<T>(_urlGeneratorSimple.StockportApiUrl<T>().TrimEnd('/'), httpContent);

    public async Task<HttpStatusCode> PutResponse<T>(HttpContent httpContent, string extra) =>
        await PutResponseAsync<T>(_urlGeneratorSimple.StockportApiUrl<T>().AddSlug(extra), httpContent);
    
    #endregion
}