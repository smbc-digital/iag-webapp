using System;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Emails.Models;
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
        Task HandleStaleGroups();
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
        private readonly IContentApiRepository _contentApiRepository;
        private readonly IStockportApiRepository _stockportApiRepository;
        private readonly IHttpEmailClient _emailClient;
        private readonly IApplicationConfiguration _configuration;
        private readonly ILogger<GroupsService> _logger;

        public GroupsService(IContentApiRepository contentApiRepository, IHttpEmailClient emailClient, IApplicationConfiguration configuration, ILogger<GroupsService> logger, IStockportApiRepository stockportApiRepository)
        {
            _contentApiRepository = contentApiRepository;
            _emailClient = emailClient;
            _configuration = configuration;
            _logger = logger;
            _stockportApiRepository = stockportApiRepository;
        }

        public async Task<GroupHomepage> GetGroupHomepage()
        {
            return await _contentApiRepository.GetResponse<GroupHomepage>();
        }

        public async Task<List<GroupCategory>> GetGroupCategories()
        {
            return await _contentApiRepository.GetResponse<List<GroupCategory>>();
        }

        public async Task HandleStaleGroups()
        {
            var allGroups = await _stockportApiRepository.GetResponse<List<Group>>();

            if (allGroups == null || !allGroups.Any())
            {
                throw new GroupsServiceException("No groups were returned from content api");
            }

            _logger.LogInformation($"Returned {allGroups.Count} groups");

            var emailPeriods = _configuration.GetArchiveEmailPeriods();

            if (emailPeriods == null || !emailPeriods.Any())
            {
                throw new GroupsServiceException("No periods returned from the service");
            }

            var fromAddress = _configuration.GetEmailEmailFrom("stockportgov");

            foreach (var period in emailPeriods)
            {
                var stagedGroups = FilterGroupsByStage(allGroups, period.NumOfDays);

                if (period.NumOfDays == emailPeriods.Select(p => p.NumOfDays).Max())
                {
                    var loopExceptions = new List<Exception>();
                    foreach (var group in stagedGroups)
                    {
                        try
                        {
                            ArchiveGroup(group);
                        }
                        catch (Exception e)
                        {
                            loopExceptions.Add(e);
                        }

                    }

                    foreach (var exception in loopExceptions)
                    {
                        _logger.LogError(exception.Message);
                    }

                }

                SendEmailToGroups(stagedGroups, period.Template, period.Subject, fromAddress.ToString());
            }
        }

        private async void ArchiveGroup(Group group)
        {
            group.DateHiddenFrom = DateTime.Now;
            group.DateHiddenTo = null;

            var jsonContent = JsonConvert.SerializeObject(group);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var putResponse = await _stockportApiRepository.PutResponse<Group>(httpContent, group.Slug);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to archive group {group.Name}");
            }

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
                    .Where(admin => admin.Permission == "A")
                    .Select(admin => new GroupArchiveWarningEmailViewModel(admin.Name, stageOneGroup.Name, admin.Email))
                    .Select(viewModel => new EmailMessage(subject, _emailClient.GenerateEmailBodyFromHtml(viewModel, template), fromAddress, viewModel.EmailAddress, null))
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
                            _.DateLastModified.HasValue && _.DateLastModified.Value.AddDays(numDays).Date == DateTime.Today);
        }

    }
}
