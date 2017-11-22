using System.Threading.Tasks;
using Quartz;
using StockportWebapp.Exceptions;
using StockportWebapp.Services;

namespace StockportWebapp.Scheduler
{
    public class GroupArchiveJob : IJob
    {
        private readonly IGroupsService _groupsService;

        public GroupArchiveJob(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        public async Task Execute(IJobExecutionContext context)
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
