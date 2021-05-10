using System;
using System.Threading.Tasks;
using Quartz;
using StockportWebapp.Exceptions;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace StockportWebapp.Scheduler
{
    public class QuartzJob : IJob
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirectses;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;
        private readonly ILogger<QuartzJob> _logger;

        public QuartzJob(ShortUrlRedirects shortShortUrlRedirectses, LegacyUrlRedirects legacyUrlRedirects, IRepository repository, ILogger<QuartzJob> logger)
        {
            _shortShortUrlRedirectses = shortShortUrlRedirectses;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var response = await _repository.GetRedirects();

                var redirects = response.Content as Redirects;

                _logger.LogWarning(
                    $"QuartzJob:Execute, Performed redirects update. New redirects contains {redirects.ShortUrlRedirects?.Sum(_ => _.Value.Count())} Short Url and {redirects.LegacyUrlRedirects?.Sum(_ => _.Value.Count())} Legacy Url entires");

                _shortShortUrlRedirectses.Redirects = redirects.ShortUrlRedirects;
                _legacyUrlRedirects.Redirects = redirects.LegacyUrlRedirects;
            }
            catch (Exception ex)
            {
                _logger.LogError($"QuartzJob: Execute:: Failed - {ex.InnerException}");
            }
        }
    }
}
