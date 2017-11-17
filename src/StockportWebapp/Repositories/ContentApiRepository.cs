using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Extensions;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockportWebapp.Repositories
{
    public interface IContentApiRepository : IBaseRepository
    {
        Task<T> GetResponse<T>();
        Task<T> GetResponse<T>(string extra);
        Task<T> GetResponse<T>(List<Query> queries);
        Task<T> GetResponse<T>(string extra, List<Query> queries);
        Task<T> GetResponseWithBusinessId<T>(string businessId);
    }

    // TODO: Test this
    public class ContentApiRepository : BaseRepository, IContentApiRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly IUrlGeneratorSimple _urlGeneratorSimple;
        private readonly ILogger<BaseRepository> _logger;

        public ContentApiRepository(IHttpClient httpClient, IApplicationConfiguration config, IUrlGeneratorSimple urlGeneratorSimple, ILogger<BaseRepository> logger) : base(httpClient, config, logger)
        {
            _httpClient = httpClient;
            _config = config;
            _urlGeneratorSimple = urlGeneratorSimple;
            _logger = logger;
        }

        #region GET Methods
        public async Task<T> GetResponse<T>()
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().TrimEnd('/');

            return await GetResponseAsync<T>(url);
        }

        public async Task<T> GetResponse<T>(string extra)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddSlug(extra);

            return await GetResponseAsync<T>(url);
        }

        public async Task<T> GetResponse<T>(List<Query> queries)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddQueryStrings(queries);

            return await GetResponseAsync<T>(url);
        }

        public async Task<T> GetResponse<T>(string extra, List<Query> queries)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>().AddSlug(extra).AddQueryStrings(queries);

            return await GetResponseAsync<T>(url);
        }

        public async Task<T> GetResponseWithBusinessId<T>(string businessId)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<T>(businessId);

            return await GetResponseAsync<T>(url);
        }

        #endregion
    }
}
