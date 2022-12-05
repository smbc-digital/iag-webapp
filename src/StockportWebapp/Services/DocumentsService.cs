using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Wrappers;

namespace StockportWebapp.Services
{
    public interface IDocumentsService
    {
        Task<DocumentToDownload> GetSecureDocument(string assetId, string groupSlug);
    }

    public class DocumentsService : IDocumentsService
    {
        private readonly IDocumentsRepository _documentsRepository;
        private readonly ILogger<DocumentsService> _logger;
        private readonly IHttpClientWrapper _httpClient;

        public DocumentsService(IDocumentsRepository documentsRepository, IHttpClientWrapper httpClient, ILogger<DocumentsService> logger)
        {
            _documentsRepository = documentsRepository;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<DocumentToDownload> GetSecureDocument(string assetId, string groupSlug)
        {
            try
            {
                var document = await _documentsRepository.GetSecureDocument(assetId, groupSlug);

                if (document == null)
                {
                    _logger.LogWarning($"No document was returned for assetId: {assetId}");
                    return null;
                }

                var result = await _httpClient.GetAsync($"https:{document.Url}");

                return new DocumentToDownload
                {
                    FileData = await result.Content.ReadAsByteArrayAsync(),
                    MediaType = document.MediaType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, $"There was a problem getting document with assetId: {assetId} for group {groupSlug}");
                return null;
            }
        }
    }
}
