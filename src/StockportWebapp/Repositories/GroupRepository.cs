using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Repositories
{
    public interface IGroupRepository
    {
    }

    public class GroupRepository : IGroupRepository
    {
        private readonly ILogger<GroupRepository> _logger;

        private readonly IApplicationConfiguration _configuration;
        private readonly BusinessId _businessId;

        public GroupRepository(ILogger<GroupRepository> logger,
            IApplicationConfiguration configuration,
            BusinessId businessId)
        {
            _logger = logger;
            _configuration = configuration;
            _businessId = businessId;
        }
    }
}