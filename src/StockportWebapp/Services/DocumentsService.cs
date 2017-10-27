using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using System;
using System.Threading.Tasks;

namespace StockportWebapp.Services
{
    public interface IDocumentsService
    {
        Task<Document> GetSecureDocument(string assetId, string groupSlug);
    }

    public class DocumentsService : IDocumentsService
    {
        private readonly IDocumentsRepository _documentsRepository;
        private readonly ILogger<DocumentsService> _logger;

        public DocumentsService(IDocumentsRepository documentsRepository, ILogger<DocumentsService> logger)
        {
            _documentsRepository = documentsRepository;
            _logger = logger;
        }

        public async Task<Document> GetSecureDocument(string assetId, string groupSlug)
        {
            try
            {
                return await _documentsRepository.GetSecureDocument(assetId, groupSlug);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, $"There was a problem getting document with assetId: {assetId} for group {groupSlug}");
                throw;
            }
        }
    }
}
