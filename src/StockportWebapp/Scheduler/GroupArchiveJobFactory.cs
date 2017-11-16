using Quartz;
using Quartz.Spi;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class GroupArchiveJobFactory : IJobFactory
    {
        private readonly IContentApiRepository _repository;

        public GroupArchiveJobFactory(IContentApiRepository repository)
        {
            _repository = repository;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new GroupArchiveJob( _repository);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
