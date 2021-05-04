using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using Microsoft.Extensions.Logging;

namespace StockportWebapp.Scheduler
{
    public class QuartzScheduler
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirects;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;
        private readonly ILogger<QuartzJob> _logger;

        public QuartzScheduler(ShortUrlRedirects shortShortUrlRedirects, LegacyUrlRedirects legacyUrlRedirects, IRepository repository, ILogger<QuartzJob> logger)
        {
            _shortShortUrlRedirects = shortShortUrlRedirects;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
            _logger = logger;
        }

        public async Task Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            scheduler.JobFactory = new QuartzJobFactory(_shortShortUrlRedirects, _legacyUrlRedirects, _repository, _logger);
            var job = JobBuilder.Create<QuartzJob>().Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(RedirectTimeout.RedirectsTimeout)
                        .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
