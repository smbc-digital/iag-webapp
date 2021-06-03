using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Extensions;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public interface IDocumentsRepository : IBaseRepository
    {
       Task<Document> GetSecureDocument(string assetId, string groupSlug);
    }

    public class DocumentsRepository : BaseRepository, IDocumentsRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly IUrlGeneratorSimple _urlGeneratorSimple;
        private readonly ILoggedInHelper _loggedInHelper;
        private readonly ILogger<BaseRepository> _logger;

        public DocumentsRepository(IHttpClient httpClient, IApplicationConfiguration config, IUrlGeneratorSimple urlGeneratorSimple, ILoggedInHelper loggedInHelper, ILogger<BaseRepository> logger) : base (httpClient, config, logger)
        {
            _httpClient = httpClient;
            _config = config;
            _urlGeneratorSimple = urlGeneratorSimple;
            _loggedInHelper = loggedInHelper;
            _logger = logger;
        }

        public async Task<Document> GetSecureDocument(string assetId, string groupSlug)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<Document>().AddSlug($"{groupSlug}/{assetId}");

            var loggedInPerson = _loggedInHelper.GetLoggedInPerson();

            if (string.IsNullOrEmpty(loggedInPerson.Email))
            {
                _logger.LogWarning($"Document {assetId} was requested, but the user wasn't logged in");
                return null;
            }
            
            AddHeader("jwtCookie", loggedInPerson.rawCookie);
            return await GetResponseAsync<Document>(url);
        }
    }
}
