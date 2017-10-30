using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.Http;
using StockportWebapp.Config;
using StockportWebapp.Utils;
using StockportWebapp.Extensions;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Repositories
{
    public interface IDocumentsRepository : IGenericRepository<Document>
    {
       Task<Document> GetSecureDocument(string assetId, string groupSlug);
    }

    public class DocumentsRepository : GenericRepository<Document>, IDocumentsRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly IUrlGeneratorSimple<Document> _urlGeneratorSimple;
        private readonly ILoggedInHelper _loggedInHelper;
        private readonly ILogger<GenericRepository<Document>> _logger;

        public DocumentsRepository(IHttpClient httpClient, IApplicationConfiguration config, IUrlGeneratorSimple<Document> urlGeneratorSimple, ILoggedInHelper loggedInHelper, ILogger<GenericRepository<Document>> logger) : base (httpClient, config, logger)
        {
            _httpClient = httpClient;
            _config = config;
            _urlGeneratorSimple = urlGeneratorSimple;
            _loggedInHelper = loggedInHelper;
            _logger = logger;
        }

        public async Task<Document> GetSecureDocument(string assetId, string groupSlug)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl().AddSlug($"{assetId}/{groupSlug}");

            var loggedInPerson = _loggedInHelper.GetLoggedInPerson();

            if (string.IsNullOrEmpty(loggedInPerson.Email))
            {
                _logger.LogWarning($"Document {assetId} was requested, but the user wasn't logged in");
                return null;
            }
            
            AddHeader("jwtCookie", loggedInPerson.rawCookie);
            return await GetResponseAsync(url);
        }
    }
}
