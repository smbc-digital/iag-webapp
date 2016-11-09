using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class RedirectScheduler
    {
        private readonly UrlRedirect _urlRedirect;
        private readonly IRepository _repository;

        public RedirectScheduler(UrlRedirect urlRedirect, IRepository repository)
        {
            _urlRedirect = urlRedirect;
            _repository = repository;
        }

        public async Task Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            scheduler.JobFactory = new RedirectJobFactory(_urlRedirect, _repository);

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
