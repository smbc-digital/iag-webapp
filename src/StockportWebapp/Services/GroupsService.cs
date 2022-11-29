using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Emails.Models;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebapp.ViewModels;


namespace StockportWebapp.Services
{
    public interface IGroupsService
    {
        Task<ProcessedGroupHomepage> GetGroupHomepage();
        Task<List<GroupCategory>> GetGroupCategories();
        Task HandleStaleGroups();
        void DoPagination(GroupResults groupResults, int currentPageNumber, int pageSize);
        string GetErrorsFromModelState(ModelStateDictionary modelState);
        bool DateNowIsNotBetweenHiddenRange(DateTime? hiddenFrom, DateTime? hiddenTo);
        bool HasGroupPermission(string email, List<GroupAdministratorItems> groupAdministrators, string permission = "E");
        string GetVolunteeringText(string volunteeringText);
        string GetDonationsText(string DonationsText);
        void SendEmailToGroups(IEnumerable<Group> stageOneGroups, string template, string subject, string fromAddress);
        Task<List<string>> GetAvailableGroupCategories();
        Task<HttpStatusCode> SendImageViaEmail(IFormFile file, string groupName, string slug);
    }

    public class GroupsService : IGroupsService
    {
        private readonly IContentApiRepository _contentApiRepository;
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IStockportApiRepository _stockportApiRepository;
        private readonly IHttpEmailClient _emailClient;
        private readonly IApplicationConfiguration _configuration;
        private readonly ILogger<GroupsService> _logger;
        private readonly BusinessId _businessId;

        public GroupsService
        (
            IContentApiRepository contentApiRepository,
            IProcessedContentRepository processedContentRepository,
            IHttpEmailClient emailClient,
            IApplicationConfiguration configuration,
            ILogger<GroupsService> logger,
            IStockportApiRepository stockportApiRepository,
            BusinessId businessId
        )
        {
            _contentApiRepository = contentApiRepository;
            _emailClient = emailClient;
            _configuration = configuration;
            _logger = logger;
            _stockportApiRepository = stockportApiRepository;
            _businessId = businessId;
            _processedContentRepository = processedContentRepository;
        }

        public async Task<ProcessedGroupHomepage> GetGroupHomepage()
        {
            var response = await _processedContentRepository.Get<GroupHomepage>();
            return response.Content as ProcessedGroupHomepage;
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
            catch (Exception)
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
            return string.IsNullOrEmpty(volunteeringText) ? "If you would like to find out more about being a volunteer with us, please email with your interest and we�ll be in contact as soon as possible." : volunteeringText;
        }

        public void SendEmailToGroups(IEnumerable<Group> stageOneGroups, string template, string subject, string fromAddress)
        {
            var handleArchivedGroups = stageOneGroups as IList<Group> ?? stageOneGroups.ToList();
            foreach (var stageOneGroup in handleArchivedGroups.ToList())
            {
                if (stageOneGroup.GroupAdministrators.Items.Any(admin => admin.Permission == "A")) _logger.LogInformation($"Sending stale group email for group: {stageOneGroup.Name}");

                stageOneGroup.GroupAdministrators.Items
                    .Where(admin => admin.Permission == "A")
                    .Select(admin => new GroupArchiveWarningEmailViewModel(admin.Name, stageOneGroup.Name, admin.Email))
                    .Select(viewModel => new EmailMessage(subject, _emailClient.GenerateEmailBodyFromHtml(viewModel, template), fromAddress, viewModel.EmailAddress, string.Empty, null))
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

        public string GetDonationsText(string DonationsText)
        {
            return string.IsNullOrEmpty(DonationsText) ? "Use the button below to find out about making a donation to support our group." : DonationsText;
        }

        public Task<HttpStatusCode> SendImageViaEmail(IFormFile file, string groupName, string slug)
        {
            return _emailClient.SendEmailToService(
                new EmailMessage(
                    $"A new image has been uploaded for the group {groupName} for approval",
                    $"A new image has been uploaded for the group {groupName} and is waiting for approval. <br /><br /> <a href='http://www.stockport.gov.uk/groups/{slug}'>Link to {groupName}</a>",
                    _configuration.GetEmailEmailFrom(_businessId.ToString()).ToString(),
                    _configuration.GetGroupSubmissionEmail(_businessId.ToString()).ToString(),
                    new List<IFormFile> { file }
                    )
            );
        }
    }
}
