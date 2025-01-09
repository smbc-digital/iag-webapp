using System.Linq;

namespace StockportWebapp.Services;

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

public class GroupsService(IContentApiRepository contentApiRepository,
                        IProcessedContentRepository processedContentRepository,
                        IHttpEmailClient emailClient,
                        IApplicationConfiguration configuration,
                        ILogger<GroupsService> logger,
                        IStockportApiRepository stockportApiRepository,
                        BusinessId businessId
                        ) : IGroupsService
{
    private readonly IContentApiRepository _contentApiRepository = contentApiRepository;
    private readonly IProcessedContentRepository _processedContentRepository = processedContentRepository;
    private readonly IStockportApiRepository _stockportApiRepository = stockportApiRepository;
    private readonly IHttpEmailClient _emailClient = emailClient;
    private readonly IApplicationConfiguration _configuration = configuration;
    private readonly ILogger<GroupsService> _logger = logger;
    private readonly BusinessId _businessId = businessId;

    public async Task<ProcessedGroupHomepage> GetGroupHomepage()
    {
        HttpResponse response = await _processedContentRepository.Get<GroupHomepage>();

        return response.Content as ProcessedGroupHomepage;
    }

    public async Task<List<GroupCategory>> GetGroupCategories() =>
        await _contentApiRepository.GetResponse<List<GroupCategory>>();

    public async Task HandleStaleGroups()
    {
        List<Group> allGroups = await _stockportApiRepository.GetResponse<List<Group>>();

        if (allGroups is null || !allGroups.Any())
            throw new GroupsServiceException("No groups were returned from content api");

        _logger.LogInformation($"Returned {allGroups.Count} groups");

        List<ArchiveEmailPeriod> emailPeriods = _configuration.GetArchiveEmailPeriods();

        if (emailPeriods is null || !emailPeriods.Any())
            throw new GroupsServiceException("No periods returned from the service");

        AppSetting fromAddress = _configuration.GetEmailEmailFrom("stockportgov");

        foreach (ArchiveEmailPeriod period in emailPeriods)
        {
            IEnumerable<Group> stagedGroups = FilterGroupsByStage(allGroups, period.NumOfDays);

            if (period.NumOfDays.Equals(emailPeriods.Select(p => p.NumOfDays).Max()))
            {
                List<Exception> loopExceptions = new();
                foreach (Group group in stagedGroups)
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

                foreach (Exception exception in loopExceptions)
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

        string jsonContent = JsonConvert.SerializeObject(group);
        StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            HttpStatusCode putResponse = await _stockportApiRepository.PutResponse<Group>(httpContent, group.Slug);
        }
        catch (Exception)
        {
            throw new Exception($"Failed to archive group {group.Name}");
        }
    }

    public void DoPagination(GroupResults groupResults, int currentPageNumber, int pageSize)
    {
        if ((groupResults is not null) && groupResults.Groups.Any())
        {
            PaginatedItems<Group> paginatedGroups = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
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
            groupResults.Pagination = new Pagination();
    }

    public string GetErrorsFromModelState(ModelStateDictionary modelState)
    {
        StringBuilder message = new();
        foreach (KeyValuePair<string, ModelStateEntry> state in modelState.Where(state => state.Value.Errors.Count > 0 && !state.Key.Equals("Email")))
        {
            message.Append($"{state.Value.Errors.First().ErrorMessage}<br />");
        }

        return message.ToString();
    }

    public bool DateNowIsNotBetweenHiddenRange(DateTime? hiddenFrom, DateTime? hiddenTo)
    {
        DateTime now = DateTime.Now;

        return hiddenFrom > now
            || (hiddenTo < now && hiddenTo != DateTime.MinValue)
            || (hiddenFrom.Equals(DateTime.MinValue) && hiddenTo.Equals(DateTime.MinValue))
            || (hiddenFrom is null && hiddenTo is null);
    }

    public bool HasGroupPermission(string email, List<GroupAdministratorItems> groupAdministrators, string permission = "E")
    {
        string userPermission = groupAdministrators.FirstOrDefault(a => a.Email.ToUpper().Equals(email.ToUpper()))?.Permission;

        if (userPermission.Equals(permission) || userPermission.Equals("A"))
            return true;

        return false;
    }

    public string GetVolunteeringText(string volunteeringText) =>
        string.IsNullOrEmpty(volunteeringText)
            ? "If you would like to find out more about being a volunteer with us, please email with your interest and we�ll be in contact as soon as possible."
            : volunteeringText;

    public void SendEmailToGroups(IEnumerable<Group> stageOneGroups, string template, string subject, string fromAddress)
    {
        IList<Group> handleArchivedGroups = stageOneGroups as IList<Group> ?? stageOneGroups.ToList();

        foreach (Group stageOneGroup in handleArchivedGroups.ToList())
        {
            if (stageOneGroup.GroupAdministrators.Items.Any(admin => admin.Permission.Equals("A")))
                _logger.LogInformation($"Sending stale group email for group: {stageOneGroup.Name}");

            stageOneGroup.GroupAdministrators.Items
                .Where(admin => admin.Permission.Equals("A"))
                .Select(admin => new GroupArchiveWarningEmailViewModel(admin.Name, stageOneGroup.Name, admin.Email))
                .Select(viewModel => new EmailMessage(subject,
                                                    _emailClient.GenerateEmailBodyFromHtml(viewModel, template),
                                                    fromAddress,
                                                    viewModel.EmailAddress,
                                                    string.Empty,
                                                    null))
                .ToList()
                .ForEach(entity => _emailClient.SendEmailToService(entity));
        }
    }

    public async Task<List<string>> GetAvailableGroupCategories()
    {
        List<GroupCategory> listOfGroupCategories = await GetGroupCategories();

        return listOfGroupCategories is not null
            ? listOfGroupCategories.Select(logc => logc.Name).OrderBy(c => c).ToList()
            : null;
    }

    private static IEnumerable<Group> FilterGroupsByStage(IEnumerable<Group> allGroups, int numDays) =>
        allGroups.Where(_ => _.DateLastModified.HasValue && _.DateLastModified.Value.AddDays(numDays).Date.Equals(DateTime.Today));

    public string GetDonationsText(string donationsText) =>
        string.IsNullOrEmpty(donationsText)
            ? "Use the button below to find out about making a donation to support our group."
            : donationsText;

    public Task<HttpStatusCode> SendImageViaEmail(IFormFile file, string groupName, string slug) =>
        _emailClient.SendEmailToService(
            new EmailMessage(
                $"A new image has been uploaded for the group {groupName} for approval",
                $"A new image has been uploaded for the group {groupName} and is waiting for approval. <br /><br /> <a href='http://www.stockport.gov.uk/groups/{slug}'>Link to {groupName}</a>",
                _configuration.GetEmailEmailFrom(_businessId.ToString()).ToString(),
                _configuration.GetGroupSubmissionEmail(_businessId.ToString()).ToString(),
                new List<IFormFile> { file }));
}