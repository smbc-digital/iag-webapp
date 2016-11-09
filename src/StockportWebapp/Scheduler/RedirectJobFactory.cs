using Quartz;
using Quartz.Spi;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class RedirectJobFactory : IJobFactory
    {
        private readonly UrlRedirect _redirect;
        private readonly IRepository _repository;

        public RedirectJobFactory(UrlRedirect redirect, IRepository repository)
        {
            _redirect = redirect;
            _repository = repository;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new RedirectJob(_redirect, _repository);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
