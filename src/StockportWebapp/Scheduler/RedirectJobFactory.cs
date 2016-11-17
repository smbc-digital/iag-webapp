using Quartz;
using Quartz.Spi;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class RedirectJobFactory : IJobFactory
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirectses;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;

        public RedirectJobFactory(ShortUrlRedirects shortShortUrlRedirectses, LegacyUrlRedirects legacyUrlRedirects, IRepository repository)
        {
            _shortShortUrlRedirectses = shortShortUrlRedirectses;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new RedirectJob(_shortShortUrlRedirectses, _legacyUrlRedirects, _repository);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
