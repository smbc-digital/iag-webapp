using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Extensions;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Repositories
{
    public interface INewsRepository : IBaseRepository
    {

    }

    public class NewsRepository : BaseRepository, INewsRepository
    {
        private readonly IHttpClient _httpClient;
        private readonly IApplicationConfiguration _config;
        private readonly IUrlGeneratorSimple _urlGeneratorSimple;
        private readonly ILoggedInHelper _loggedInHelper;
        private readonly ILogger<BaseRepository> _logger;

        public NewsRepository(IHttpClient httpClient, IApplicationConfiguration config, IUrlGeneratorSimple urlGeneratorSimple, ILoggedInHelper loggedInHelper, ILogger<BaseRepository> logger) : base(httpClient, config, logger)
        {
            _httpClient = httpClient;
            _config = config;
            _urlGeneratorSimple = urlGeneratorSimple;
            _loggedInHelper = loggedInHelper;
            _logger = logger;
        }

        
    }
}
