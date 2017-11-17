using System;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Entities;
using StockportWebapp.Exceptions;
using StockportWebapp.Utils;

namespace StockportWebapp.Services
{
    public interface IGroupsService
    {
        Task<GroupHomepage> GetGroupHomepage();
        Task<List<GroupCategory>> GetGroupCategories();
        Task<IEnumerable<Group>> HandleArchivedGroups();
    }

    public class GroupsService : IGroupsService
    {
        private IContentApiRepository _contentApiRepository;
        private IEmailHandler _emailHandler;

        public GroupsService(IContentApiRepository contentApiRepository, IEmailHandler emailHandler)
        {
            _contentApiRepository = contentApiRepository;
            _emailHandler = emailHandler;
        }

        public async Task<GroupHomepage> GetGroupHomepage()
        {
            return await _contentApiRepository.GetResponse<GroupHomepage>();
        }

        public async Task<List<GroupCategory>> GetGroupCategories()
        {
            return await _contentApiRepository.GetResponse<List<GroupCategory>>();
        }

        public async Task<IEnumerable<Group>> HandleArchivedGroups()
        {
            var allGroups = await _contentApiRepository.GetResponseWithBusinessId<List<Group>>("stockportgov");

            if (allGroups == null || !allGroups.Any())
            {
                throw new GroupsServiceException("No groups were returned from content api");
            }

            var stageOneGroups = FilterGroupsByStage(allGroups, 180);

            foreach (var stageOneGroup in stageOneGroups.ToList())
            {
                stageOneGroup.GroupAdministrators.Items
                    .Where(_ => _.Permission == "A")
                    .Select(_ => new EmailEntity
                        {
                            Recipient = _.Email,
                            Body = _emailHandler.GenerateEmailBodyFromHtml(_, "ArchiveGroupScheduler")
                        })
                    .ToList()
                    .ForEach(entity => _emailHandler.SendEmail(entity));
            }

            return stageOneGroups;
        }

        private static IEnumerable<Group> FilterGroupsByStage(IEnumerable<Group> allGroups, int numDays)
        {
            return allGroups.Where(_ =>
                            _.DateLastModified.HasValue && _.DateLastModified.Value.AddDays(numDays) .Date == DateTime.Today);
        }
    }
}
