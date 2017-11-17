using Quartz;
using Quartz.Spi;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;

namespace StockportWebapp.Scheduler
{
    public class GroupArchiveJobFactory : IJobFactory
    {
        private readonly IGroupsService _groupsService;

        public GroupArchiveJobFactory(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new GroupArchiveJob(_groupsService);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
