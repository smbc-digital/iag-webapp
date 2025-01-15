namespace StockportWebapp.Services;

public interface IHomepageService
{
    Task<ProcessedHomepage> GetHomepage();
}

public class HomepageService(IProcessedContentRepository processedContentRepository) : IHomepageService
{
    public IProcessedContentRepository _processedContentRepository = processedContentRepository;

    public async Task<ProcessedHomepage> GetHomepage()
    {
        HttpResponse response = await _processedContentRepository.Get<Homepage>();
        
        return response.Content as ProcessedHomepage;
    }
}