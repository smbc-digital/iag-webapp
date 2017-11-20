using System;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Entities;
using StockportWebapp.Exceptions;
using StockportWebapp.Utils;

namespace StockportWebapp.Services
{
    public interface IGroupsService
    {
        Task<GroupHomepage> GetGroupHomepage();
        Task<List<GroupCategory>> GetGroupCategories();
        Task HandleArchivedGroups();
    }

    public class GroupsService : IGroupsService
    {
        private IContentApiRepository _contentApiRepository;
        private IHttpEmailClient _emailClient;
        private IApplicationConfiguration _configuration;

        public GroupsService(IContentApiRepository contentApiRepository, IHttpEmailClient emailClient, IApplicationConfiguration configuration)
        {
            _contentApiRepository = contentApiRepository;
            _emailClient = emailClient;
            _configuration = configuration;
        }

        public async Task<GroupHomepage> GetGroupHomepage()
        {
            return await _contentApiRepository.GetResponse<GroupHomepage>();
        }

        public async Task<List<GroupCategory>> GetGroupCategories()
        {
            return await _contentApiRepository.GetResponse<List<GroupCategory>>();
        }

        public async Task HandleArchivedGroups()
        {
            var allGroups = await _contentApiRepository.GetResponseWithBusinessId<List<Group>>("stockportgov");

            if (allGroups == null || !allGroups.Any())
            {
                throw new GroupsServiceException("No groups were returned from content api");
            }

            var emailPeriods = _configuration.GetArchiveEmailPeriods();

            if (emailPeriods == null || !emailPeriods.Any())
            {
                throw new GroupsServiceException("No periods returned from the service");
            }

            var fromAddress = _configuration.GetGroupArchiveEmail("stockportgov");

            emailPeriods.ForEach(period =>
            {
                var stagedGroups = FilterGroupsByStage(allGroups, period.NumOfDays);

                SendEmailToGroups(stagedGroups, period.Template, period.Subject, fromAddress.ToString());
            });
        }

        private void SendEmailToGroups(IEnumerable<Group> stageOneGroups, string template, string subject, string fromAddress)
        {
            var handleArchivedGroups = stageOneGroups as IList<Group> ?? stageOneGroups.ToList();
            foreach (var stageOneGroup in handleArchivedGroups.ToList())
            {
                stageOneGroup.GroupAdministrators.Items
                    .Where(_ => _.Permission == "A")
                    .Select(_ => new EmailMessage(subject, _emailClient.GenerateEmailBodyFromHtml(_, template), fromAddress, _.Email, null))
                    .ToList()
                    .ForEach(entity => _emailClient.SendEmailToService(entity));
            }
        }

        private static IEnumerable<Group> FilterGroupsByStage(IEnumerable<Group> allGroups, int numDays)
        {
            return allGroups.Where(_ =>
                            _.DateLastModified.HasValue && _.DateLastModified.Value.AddDays(numDays) .Date == DateTime.Today);
        }
    }
}
