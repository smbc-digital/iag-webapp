using System.Threading.Tasks;
using Quartz;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.Scheduler
{
    public class GroupArchiveJob : IJob
    {
        private readonly IContentApiRepository _repository;

        public GroupArchiveJob(IContentApiRepository repository)
        {
            _repository = repository;
        }

        public async Task Execute(IJobExecutionContext context)
        {

            var hello = "j";
        }
    }
}
