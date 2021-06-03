using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Extensions;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public interface ISmartResultRepository : IBaseRepository
    {
        Task<SmartResult> GetSmartResult(string slug);
    }

    public class SmartResultRepository : BaseRepository, ISmartResultRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly ILogger<BaseRepository> _logger;
        private readonly IUrlGeneratorSimple _urlGeneratorSimple;

        public SmartResultRepository(IHttpClient httpClient, IApplicationConfiguration config, ILogger<BaseRepository> logger, IUrlGeneratorSimple urlGeneratorSimple) : base(httpClient, config, logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            _urlGeneratorSimple = urlGeneratorSimple;
        }

        public async Task<SmartResult> GetSmartResult(string slug)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl<SmartResult>().AddSlug(slug);
            return await GetResponseAsync<SmartResult>(url);
        }
    }
}
