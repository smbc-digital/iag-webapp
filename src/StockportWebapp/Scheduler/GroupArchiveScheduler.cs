using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;

namespace StockportWebapp.Scheduler
{
    public class GroupArchiveScheduler
    {
        private readonly IGroupsService _groupsService;

        public GroupArchiveScheduler(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        public async Task Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            scheduler.JobFactory = new GroupArchiveJobFactory(_groupsService);

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
