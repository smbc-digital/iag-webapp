namespace StockportWebapp.Services;

public interface INewsService
{
    Task<List<News>> GetNewsByLimit(int limit);
    Task<News> GetLatestNewsItem();
}

public class NewsService(IRepository newsRepository) : INewsService
{
    private readonly IRepository _newsRepository = newsRepository;

    public async Task<List<News>> GetNewsByLimit(int limit)
    {
        HttpResponse response = await _newsRepository.GetLatest<List<News>>(limit);

        return response.Content as List<News>;
    }

    public async Task<News> GetLatestNewsItem()
    {
        HttpResponse response = await _newsRepository.GetLatest<List<News>>(1);
        List<News> newsItems = response.Content as List<News>;

        return newsItems?.FirstOrDefault();
    }
}