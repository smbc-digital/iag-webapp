using Quartz;
using Quartz.Spi;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Scheduler
{
    public class QuartzJobFactory : IJobFactory
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirectses;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;
        private readonly ILogger<QuartzJob> _logger;

        public QuartzJobFactory(ShortUrlRedirects shortShortUrlRedirectses, LegacyUrlRedirects legacyUrlRedirects, IRepository repository, ILogger<QuartzJob> logger)
        {
            _shortShortUrlRedirectses = shortShortUrlRedirectses;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
            _logger = logger;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new QuartzJob(_shortShortUrlRedirectses, _legacyUrlRedirects, _repository, _logger);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
