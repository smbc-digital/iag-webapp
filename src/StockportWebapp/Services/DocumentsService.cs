using StockportWebapp.Models;
using StockportWebapp.Repositories;
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

        public DocumentsService(IDocumentsRepository documentsRepository)
        {
            _documentsRepository = documentsRepository;
        }

        public async Task<Document> GetSecureDocument(string assetId, string groupSlug)
        {
            return await _documentsRepository.GetSecureDocument(assetId, groupSlug);
        }
    }
}
