using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Services
{
    public interface IDocumentsService
    {
        Document GetSecureDocument(string businessId, string assetId, string groupSlug);
    }

    public class DocumentsService : IDocumentsService
    {
        private readonly IDocumentsRepository _documentsRepository;

        public DocumentsService(IDocumentsRepository documentsRepository)
        {
            _documentsRepository = documentsRepository;
        }
        public Document GetSecureDocument(string businessId, string assetId, string groupSlug)
        {
            _documentsRepository.GetSecureDocument(businessId, assetId, groupSlug);
            return null;
        }
    }
}
