namespace StockportWebapp.Repositories;

public interface IRepository
{
    Task<HttpResponse> Get<T>(string slug = "", List<Query> queries = null);
    Task<HttpResponse> GetLatest<T>(int limit);
    Task<HttpResponse> GetLatestOrderByFeatured<T>(int limit);
    Task<HttpResponse> GetRedirects();
    Task<HttpResponse> Put<T>(HttpContent content, string slug = "");
    Task<HttpResponse> Delete<T>(string slug = "");
    Task<HttpResponse> Archive<T>(HttpContent content, string slug = "");
    Task<HttpResponse> Publish<T>(HttpContent content, string slug = "");
}