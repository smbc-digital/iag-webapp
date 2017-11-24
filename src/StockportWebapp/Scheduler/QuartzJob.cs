using System;
using System.Threading.Tasks;
using Quartz;
using StockportWebapp.Exceptions;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using StockportWebapp.Utils;

namespace StockportWebapp.Scheduler
{
    public class QuartzJob : IJob
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirectses;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;
        private readonly IGroupsService _groupsService;
        private readonly ITimeProvider _timeProvider;
        private readonly FeatureToggles _featureToggles;

        public QuartzJob(ShortUrlRedirects shortShortUrlRedirectses, LegacyUrlRedirects legacyUrlRedirects, IRepository repository, IGroupsService groupsService, ITimeProvider timeProvider, FeatureToggles featureToggles)
        {
            _shortShortUrlRedirectses = shortShortUrlRedirectses;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
            _groupsService = groupsService;
            _timeProvider = timeProvider;
            _featureToggles = featureToggles;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var response = await _repository.GetRedirects();

            var redirects = response.Content as Redirects;

            _shortShortUrlRedirectses.Redirects = redirects.ShortUrlRedirects;
            _legacyUrlRedirects.Redirects = redirects.LegacyUrlRedirects;

            if (_featureToggles.GroupArchiveEmails)
            {
                var handleStaleGroupsFirstPossibleStartTime = new TimeSpan(0, 12, 0, 0);
                var handleStaleGroupsLastPossibleStartTime = new TimeSpan(0, 12, 30, RedirectTimeout.RedirectsTimeout);


                if (_timeProvider.Now().TimeOfDay >= handleStaleGroupsFirstPossibleStartTime &&
                    _timeProvider.Now().TimeOfDay <= handleStaleGroupsLastPossibleStartTime)
                {
                    try
                    {
                        await _groupsService.HandleStaleGroups();
                    }
                    catch (GroupsServiceException)
                    {
                        // TODO: Add logger
                    }
                }
            }
        }
    }
}
