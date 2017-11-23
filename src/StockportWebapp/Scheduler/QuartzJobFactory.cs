using Quartz;
using Quartz.Spi;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using StockportWebapp.Utils;

namespace StockportWebapp.Scheduler
{
    public class QuartzJobFactory : IJobFactory
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirectses;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;
        private readonly IGroupsService _groupsService;
        private readonly ITimeProvider _timeProvider;

        public QuartzJobFactory(ShortUrlRedirects shortShortUrlRedirectses, LegacyUrlRedirects legacyUrlRedirects, IRepository repository, IGroupsService groupsService, ITimeProvider timeProvider)
        {
            _shortShortUrlRedirectses = shortShortUrlRedirectses;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
            _groupsService = groupsService;
            _timeProvider = timeProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new QuartzJob(_shortShortUrlRedirectses, _legacyUrlRedirects, _repository, _groupsService, _timeProvider);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
