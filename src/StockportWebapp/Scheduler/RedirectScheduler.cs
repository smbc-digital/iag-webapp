using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class RedirectScheduler
    {
        private readonly ShortUrlRedirects _shortShortUrlRedirects;
        private readonly LegacyUrlRedirects _legacyUrlRedirects;
        private readonly IRepository _repository;

        public RedirectScheduler(ShortUrlRedirects shortShortUrlRedirects, LegacyUrlRedirects legacyUrlRedirects, IRepository repository)
        {
            _shortShortUrlRedirects = shortShortUrlRedirects;
            _legacyUrlRedirects = legacyUrlRedirects;
            _repository = repository;
        }

        public async Task Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            scheduler.JobFactory = new RedirectJobFactory(_shortShortUrlRedirects, _legacyUrlRedirects, _repository);

            var job = JobBuilder.Create<RedirectJob>().Build();

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
