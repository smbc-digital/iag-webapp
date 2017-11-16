using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class GroupArchiveScheduler
    {
        private readonly IContentApiRepository _rcontentApiRpository;

        public GroupArchiveScheduler(IContentApiRepository rcontentApiRpository)
        {
            _rcontentApiRpository = rcontentApiRpository;
        }

        public async Task Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            scheduler.JobFactory = new GroupArchiveJobFactory(_rcontentApiRpository);

            var job = JobBuilder.Create<GroupArchiveJob>().Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10)
                        .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
