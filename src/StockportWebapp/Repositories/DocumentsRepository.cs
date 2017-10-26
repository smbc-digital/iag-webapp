using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public interface IDocumentsRepository
    {
       Task<Document> GetSecureDocument(string assetId, string groupSlug);
    }

    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly IRepository _repository;

        public DocumentsRepository(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Document> GetSecureDocument(string assetId, string groupSlug)
        {
            var response = await _repository.Get<Document>($"{groupSlug}/{assetId}");
            return response.Content as Document;
        }
    }
}
