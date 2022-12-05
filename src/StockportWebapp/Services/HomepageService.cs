using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;

namespace StockportWebapp.Services
{
    public interface IHomepageService
    {
        Task<ProcessedHomepage> GetHomepage();
    }

    public class HomepageService : IHomepageService
    {
        public IProcessedContentRepository _processedContentRepository;

        public HomepageService(IProcessedContentRepository processedContentRepository)
        {
            _processedContentRepository = processedContentRepository;
        }

        public async Task<ProcessedHomepage> GetHomepage()
        {
            var response = await _processedContentRepository.Get<Homepage>();
            return response.Content as ProcessedHomepage;
        }
    }
}
