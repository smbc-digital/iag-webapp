using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Services
{
    public interface INewsService
    {
        Task<List<News>> GetNewsByLimit(int limit);
        Task<News> GetLatestNewsItem();
    }

    public class NewsService : INewsService
    {
        private readonly IRepository _newsRepository;

        public NewsService(IRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<List<News>> GetNewsByLimit(int limit)
        {
            var response = await _newsRepository.GetLatest<List<News>>(limit);
            return response.Content as List<News>;
        }

        public async Task<News> GetLatestNewsItem()
        {
            var response = await _newsRepository.GetLatest<List<News>>(1);
            var newsItems = response.Content as List<News>;
            return newsItems?.First();
        }
    }
}
