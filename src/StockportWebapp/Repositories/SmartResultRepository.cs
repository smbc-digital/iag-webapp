using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.Http;
using StockportWebapp.Config;
using Microsoft.Extensions.Logging;
using StockportWebapp.Extensions;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public interface ISmartResultRepository : IGenericRepository<SmartResult>
    {
        Task<SmartResult> GetSmartResult(string slug);
    }

    public class SmartResultRepository : GenericRepository<SmartResult>, ISmartResultRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly ILogger<GenericRepository<SmartResult>> _logger;
        private readonly IUrlGeneratorSimple<SmartResult> _urlGeneratorSimple;

        public SmartResultRepository(IHttpClient httpClient, IApplicationConfiguration config, ILogger<GenericRepository<SmartResult>> logger, IUrlGeneratorSimple<SmartResult> urlGeneratorSimple) : base(httpClient, config, logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            _urlGeneratorSimple = urlGeneratorSimple;
        }

        public async Task<SmartResult> GetSmartResult(string slug)
        {
            var url = _urlGeneratorSimple.BaseContentApiUrl().AddSlug(slug);
            return await GetResponseAsync(url);
        }
    }
}
