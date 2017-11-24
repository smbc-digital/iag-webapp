using Quartz;
using Quartz.Spi;
using StockportWebapp.FeatureToggling;
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
        private readonly FeatureToggles _featureToggles;

        public QuartzJobFactory(ShortUrlRedirects shortShortUrlRedirectses, LegacyUrlRedirects legacyUrlRedirects, IRepository repository, IGroupsService groupsService, ITimeProvider timeProvider, FeatureToggles featureToggles)
        {
            _shortShortUrlRedirectses = shortShortUrlRedirectses;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
            _groupsService = groupsService;
            _timeProvider = timeProvider;
            _featureToggles = featureToggles;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new QuartzJob(_shortShortUrlRedirectses, _legacyUrlRedirects, _repository, _groupsService, _timeProvider, _featureToggles);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
