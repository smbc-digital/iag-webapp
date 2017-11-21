using System;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using Quartz;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;

namespace StockportWebapp.Scheduler
{
    public class GroupArchiveJob : IJob
    {
        private readonly IGroupsService _groupsService;
        private readonly ILogger<GroupArchiveJob> _logger;

        public GroupArchiveJob(IGroupsService groupsService, ILogger<GroupArchiveJob> logger)
        {
            _groupsService = groupsService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _groupsService.HandleStaleGroups();
            }
            catch (GroupsServiceException e)
            {
                _logger.LogError(e.Message);
            }
            
        }
    }
}
