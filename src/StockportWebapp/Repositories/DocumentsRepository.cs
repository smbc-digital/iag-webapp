using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;

namespace StockportWebapp.Repositories
{
    public interface IDocumentsRepository
    {
       Task<Document> GetSecureDocument(string businessId, string assetId, string groupSlug);
    }

    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly Dictionary<string, string> _authenticationHeaders;


        public DocumentsRepository(IHttpClient httpClient, IApplicationConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
        }

        public async Task<Document> GetSecureDocument(string businessId, string assetId, string groupSlug)
        {
           var response =  await _httpClient.Get($"{_config.GetContentApiUri()}/{businessId}/documents/{groupSlug}/{assetId}", _authenticationHeaders);

            return response.Content as Document;
        }
    }
}
