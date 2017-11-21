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
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Services
{
    public interface IGroupsService
    {
        Task<GroupHomepage> GetGroupHomepage();
        Task<List<GroupCategory>> GetGroupCategories();
        Task HandleArchivedGroups();
        void DoPagination(GroupResults groupResults, int currentPageNumber, int pageSize);
        string GetErrorsFromModelState(ModelStateDictionary modelState);
        bool DateNowIsNotBetweenHiddenRange(DateTime? hiddenFrom, DateTime? hiddenTo);
        bool HasGroupPermission(string email, List<GroupAdministratorItems> groupAdministrators, string permission = "E");
        string GetVolunteeringText(string volunteeringText);
        void SendEmailToGroups(IEnumerable<Group> stageOneGroups, string template, string subject, string fromAddress);
        Task<List<string>> GetAvailableGroupCategories();
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

        public void DoPagination(GroupResults groupResults, int currentPageNumber, int pageSize)
        {
            if ((groupResults != null) && groupResults.Groups.Any())
            {
                var paginatedGroups = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                    groupResults.Groups,
                    currentPageNumber,
                    "groups",
                    pageSize,
                    _configuration.GetGroupsDefaultPageSize("stockportgov"));

                groupResults.Groups = paginatedGroups.Items;
                groupResults.Pagination = paginatedGroups.Pagination;
                groupResults.Pagination.CurrentUrl = groupResults.CurrentUrl;
            }
            else
            {
                groupResults.Pagination = new Pagination();
            }
        }

        public string GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            var message = new StringBuilder();

            foreach (var state in modelState)
            {
                if (state.Value.Errors.Count > 0 && state.Key != "Email")
                {
                    message.Append($"{state.Value.Errors.First().ErrorMessage}<br />");
                }
            }
            return message.ToString();
        }

        public bool DateNowIsNotBetweenHiddenRange(DateTime? hiddenFrom, DateTime? hiddenTo)
        {
            var now = DateTime.Now;
            return hiddenFrom > now || (hiddenTo < now && hiddenTo != DateTime.MinValue) || (hiddenFrom == DateTime.MinValue && hiddenTo == DateTime.MinValue) || (hiddenFrom == null && hiddenTo == null);
        }

        public bool HasGroupPermission(string email, List<GroupAdministratorItems> groupAdministrators, string permission = "E")
        {
            var userPermission = groupAdministrators.FirstOrDefault(a => a.Email.ToUpper() == email.ToUpper())?.Permission;

            if ((userPermission == permission) || (userPermission == "A"))
            {
                return true;
            }

            return false;
        }

        public string GetVolunteeringText(string volunteeringText)
        {
            return string.IsNullOrEmpty(volunteeringText) ? "If you would like to find out more about being a volunteer with us, please e-mail with your interest and we�ll be in contact as soon as possible." : volunteeringText;
        }

        public void SendEmailToGroups(IEnumerable<Group> stageOneGroups, string template, string subject, string fromAddress)
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

        public async Task<List<string>> GetAvailableGroupCategories()
        {
            var listOfGroupCategories = await GetGroupCategories();

            return listOfGroupCategories != null ? listOfGroupCategories.Select(logc => logc.Name).OrderBy(c => c).ToList() : null;
        }

        private static IEnumerable<Group> FilterGroupsByStage(IEnumerable<Group> allGroups, int numDays)
        {
            return allGroups.Where(_ =>
                            _.DateLastModified.HasValue && _.DateLastModified.Value.AddDays(numDays) .Date == DateTime.Today);
        }
    }
}
