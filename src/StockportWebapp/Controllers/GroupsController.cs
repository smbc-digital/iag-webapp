namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
[Obsolete("Groups is being replaced by directories/directory entries")]
[ExcludeFromCodeCoverage(Justification = "Obsolete")]
public class GroupsController : Controller
{
    private readonly IProcessedContentRepository _processedContentRepository;
    private readonly IRepository _repository;
    private readonly GroupEmailBuilder _emailBuilder;
    private readonly IFilteredUrl _filteredUrl;
    private readonly ILogger<GroupsController> _logger;
    private readonly List<Query> _managementQuery;
    private readonly IApplicationConfiguration _configuration;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly ViewHelpers _viewHelpers;
    private readonly IDateCalculator _dateCalculator;
    private readonly ICookiesHelper _cookiesHelper;
    private readonly ILoggedInHelper _loggedInHelper;
    private readonly IGroupsService _groupsService;

    public GroupsController(
        IProcessedContentRepository processedContentRepository,
        IRepository repository,
        GroupEmailBuilder emailBuilder,
        IFilteredUrl filteredUrl,
        ILogger<GroupsController> logger,
        IApplicationConfiguration configuration,
        MarkdownWrapper markdownWrapper,
        ViewHelpers viewHelpers,
        IDateCalculator dateCalculator,
        ILoggedInHelper loggedInHelper,
        IGroupsService groupsService,
        ICookiesHelper cookiesHelper)
    {
        _processedContentRepository = processedContentRepository;
        _repository = repository;
        _filteredUrl = filteredUrl;
        _logger = logger;
        _configuration = configuration;
        _emailBuilder = emailBuilder;
        _managementQuery = new List<Query> { new("onlyActive", "false") };
        _markdownWrapper = markdownWrapper;
        _viewHelpers = viewHelpers;
        _dateCalculator = dateCalculator;
        _cookiesHelper = cookiesHelper;
        _loggedInHelper = loggedInHelper;
        _groupsService = groupsService;
    }

    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    [Route("/groups")]
    public async Task<IActionResult> Index()
    {
        List<GroupCategory> listOfGroupCategories = await _groupsService.GetGroupCategories();
        ProcessedGroupHomepage homepage = await _groupsService.GetGroupHomepage();

        GroupStartPage model = new()
        {
            PrimaryFilter = new PrimaryFilter
            {
                Location = Groups.Location,
                Latitude = Groups.StockportLatitude,
                Longitude = Groups.StockportLongitude
            }
        };

        if (listOfGroupCategories is not null)
        {
            model.Categories = listOfGroupCategories;
            model.PrimaryFilter.Categories = listOfGroupCategories.OrderBy(c => c.Name).ToList();
        }

        model.BackgroundImage = homepage.BackgroundImage;
        model.FeaturedGroupsHeading = homepage.FeaturedGroupsHeading;
        model.FeaturedGroups = homepage.FeaturedGroups;
        model.FeaturedGroupsCategory = homepage.FeaturedGroupsCategory;
        model.FeaturedGroupsSubCategory = homepage.FeaturedGroupsSubCategory;
        model.Alerts = homepage.Alerts;
        model.BodyHeading = homepage.BodyHeading;
        model.Body = homepage.Body;
        model.SecondaryBodyHeading = homepage.SecondaryBodyHeading;
        model.SecondaryBody = homepage.SecondaryBody;
        model.EventBanner = homepage.EventBanner;
        model.MetaDescription = homepage.MetaDescription;

        return View(model);
    }

    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    [Route("/groups/{slug}")]
    public async Task<IActionResult> Detail(string slug, bool confirmedUpToDate = false)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(slug);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        bool userIsAdministrator = false;
        bool isLoggedIn = false;
        LoggedInPerson loggedInPerson = _loggedInHelper.GetLoggedInPerson();

        if (!string.IsNullOrEmpty(loggedInPerson.Email))
        {
            userIsAdministrator = group.GroupAdministrators.Items.Any(admin => admin.Email.Equals(loggedInPerson.Email));
            isLoggedIn = true;
        }

        int daysTillStale = _configuration.GetArchiveEmailPeriods().First().NumOfDays;

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        GroupDetailsViewModel viewModel = new()
        {
            Group = group,
            MyAccountUrl = _configuration.GetMyAccountUrl() + "?returnUrl=" + Request?.GetDisplayUrl(),
            ShouldShowAdminOptions = userIsAdministrator,
            ConfirmedUpToDate = confirmedUpToDate,
            IsLoggedIn = isLoggedIn,
            DaysTillStale = daysTillStale
        };

        return View(viewModel);
    }

    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    [Route("groups/results")]
    public async Task<IActionResult> Results([FromQuery] int page, [FromQuery] int pageSize, GroupSearch groupSearch)
    {
        GroupResults model = new();
        List<Query> queries = new();

        if (!string.IsNullOrEmpty(groupSearch.Tag))
            groupSearch.KeepTag = groupSearch.Tag;

        if (!groupSearch.Latitude.Equals(0))
            queries.Add(new Query("latitude", groupSearch.Latitude.ToString()));
        
        if (!groupSearch.Longitude.Equals(0))
            queries.Add(new Query("longitude", groupSearch.Longitude.ToString()));
        
        if (!string.IsNullOrEmpty(groupSearch.Category))
            queries.Add(new Query("Category", groupSearch.Category.Equals("all") ? string.Empty : groupSearch.Category));

        if (!string.IsNullOrEmpty(groupSearch.Order))
            queries.Add(new Query("Order", groupSearch.Order));

        if (!string.IsNullOrEmpty(groupSearch.Location))
            queries.Add(new Query("location", groupSearch.Location));

        if (!string.IsNullOrEmpty(groupSearch.GetInvolved))
            queries.Add(new Query("getinvolved", groupSearch.GetInvolved));

        if (!string.IsNullOrEmpty(groupSearch.Tag))
            queries.Add(new Query("organisation", groupSearch.Tag));

        if (groupSearch.SubCategories.Any())
            queries.Add(new Query("subcategories", string.Join(",", groupSearch.SubCategories)));

        if (!string.IsNullOrEmpty(groupSearch.Tags))
            queries.Add(new Query("Tags", groupSearch.Tags));

        HttpResponse response = await _repository.Get<GroupResults>(queries: queries);

        if (response.IsNotFound())
            return NotFound();

        model = response.Content as GroupResults;

        ViewBag.SelectedCategory = string.IsNullOrEmpty(groupSearch.Category)
            ? "All"
            : (char.ToUpper(groupSearch.Category[0]) + groupSearch.Category.Substring(1)).Replace("-", " ");
        
        model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(model.CurrentUrl);
        model.AddFilteredUrl(_filteredUrl);

        _groupsService.DoPagination(model, page, pageSize);

        if ((model.Categories is not null) && model.Categories.Any())
        {
            ViewBag.Category = model.Categories.FirstOrDefault(c => c.Slug.Equals(groupSearch.Category));
            model.PrimaryFilter.Categories = model.Categories.OrderBy(c => c.Name).ToList();
        }

        model.PrimaryFilter.Order = groupSearch.Order;
        model.PrimaryFilter.Location = groupSearch.Location;
        model.PrimaryFilter.Latitude = !groupSearch.Latitude.Equals(0)
            ? groupSearch.Latitude
            : Groups.StockportLatitude;

        model.PrimaryFilter.Longitude = !groupSearch.Longitude.Equals(0)
            ? groupSearch.Longitude
            : Groups.StockportLongitude;

        model.GetInvolved = groupSearch.GetInvolved.Equals("yes");
        model.SubCategories = groupSearch.SubCategories;
        model.Tag = groupSearch.Tag; // organisation filter
        model.KeepTag = groupSearch.KeepTag; // get first found organisation with Tag


        if (!string.IsNullOrEmpty(groupSearch.Tag) && model.Groups.Any(g => g.Organisation?.Slug == groupSearch.Tag))
        {
            Group firstGroup = model.Groups.First(g => g.Organisation?.Slug == groupSearch.Tag);
            model.OrganisationName = firstGroup?.Organisation is null
                ? string.Empty
                : firstGroup.Organisation.Title;
        }
        else if (!string.IsNullOrEmpty(groupSearch.KeepTag))
        {
            HttpResponse organisationFilterResponse = await _repository.Get<Organisation>(groupSearch.KeepTag);
            Organisation organisationFilter = organisationFilterResponse.Content as Organisation;

            if (organisationFilter is not null)
                model.OrganisationName = organisationFilter.Title;
        }

        try
        {
            ViewBag.AbsoluteUri = Request?.GetDisplayUrl();
        }
        catch
        {
            // TODO: Find out a better way todo this
            //This is for unit tests
            ViewBag.AbsoluteUri = string.Empty;
        }

        return View(model);
    }

    [Route("/groups/add-a-group")]
    public async Task<IActionResult> AddAGroup()
    {
        return NotFound();

        GroupSubmission groupSubmission = new();

        groupSubmission.AvailableCategories = await _groupsService.GetAvailableGroupCategories();

        return View(groupSubmission);
    }

    [HttpPost]
    [Route("/groups/add-a-group")]
    [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
    public async Task<IActionResult> AddAGroup(GroupSubmission groupSubmission)
    {
        return NotFound();

        groupSubmission.AvailableCategories = await _groupsService.GetAvailableGroupCategories();

        if (!ModelState.IsValid)
        {
            ViewBag.SubmissionError = _groupsService.GetErrorsFromModelState(ModelState);
            return View(groupSubmission);
        }

        HttpStatusCode successCode = await _emailBuilder.SendEmailAddNew(groupSubmission);
        if (successCode.Equals(HttpStatusCode.OK))
            return RedirectToAction("ThankYouMessage");

        ViewBag.SubmissionError = "There was a problem submitting the group, please try again.";

        return View(groupSubmission);
    }

    // TODO: Move into events service
    private async Task<List<string>> GetAvailableEventCategories()
    {
        HttpResponse response = await _repository.Get<List<EventCategory>>();
        List<EventCategory> listOfEventCategories = response.Content as List<EventCategory>;

        if (listOfEventCategories is not null)
            return listOfEventCategories.Select(logc => logc.Name).OrderBy(c => c).ToList();

        return null;
    }

    [HttpGet]
    [Route("/groups/{slug}/change-group-info")]
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public ActionResult ChangeGroupInfo(string slug, string groupname) =>
        View(new ChangeGroupInfoViewModel()
        {
            GroupName = groupname,
            Slug = slug
        });

    [HttpPost]
    [Route("/groups/{slug}/change-group-info")]
    [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public IActionResult ChangeGroupInfo(string slug, ChangeGroupInfoViewModel submission)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.SubmissionError = _groupsService.GetErrorsFromModelState(ModelState);

            return View(submission);
        }

        HttpStatusCode successCode = _emailBuilder.SendEmailChangeGroupInfo(submission).Result;
        if (successCode.Equals(HttpStatusCode.OK))
            return RedirectToAction("ChangeGroupInfoConfirmation", new { slug, groupName = submission.GroupName });

        ViewBag.SubmissionError = "There was a problem submitting the group, please try again.";

        return View(submission);
    }

    [HttpGet]
    [Route("/groups/{slug}/report-group-info")]
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public ActionResult ReportGroupInfo(string slug, string groupname) =>
        View(new ReportGroupViewModel
        {
            GroupName = groupname,
            Slug = slug
        });

    [HttpPost]
    [Route("/groups/{slug}/report-group-info")]
    [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public IActionResult ReportGroupInfo(string slug, ReportGroupViewModel submission)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.SubmissionError = _groupsService.GetErrorsFromModelState(ModelState);

            return View(submission);
        }

        HttpStatusCode successCode = _emailBuilder.SendEmailReportGroup(submission).Result;
        if (successCode.Equals(HttpStatusCode.OK))
            return RedirectToAction("ReportGroupInfoConfirmation", new { slug, groupName = submission.GroupName });
        
        ViewBag.SubmissionError = "There was a problem submitting the report, please try again.";

        return View(submission);
    }

    [Route("/groups/{slug}/report-group-info-confirmation")]
    public IActionResult ReportGroupInfoConfirmation(string slug, string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            return NotFound();

        ViewBag.Slug = slug;
        ViewBag.GroupName = groupName;

        ConfirmationViewModel viewmodel = new()
        {
            Title = "Report this page as inappropriate",
            SubTitle = $"You've successfully submitted a report for {groupName}",
            ConfirmationText = "We will take a look at the report you have submitted in line with our <a target='_blank' href = "
                + Url.Content("https://www.stockport.gov.uk/terms-and-conditions") + ">Terms and Conditions</a> and reply to you within 10 working days",
            ButtonText = $"Go back to Stockport Local {groupName}",
            ButtonLink = "/groups/" + slug,
            Icon = "check",
            IconColour = "green",
            Crumbs = new List<Crumb>
                {
                    new("Stockport Local", "groups", "Group"),
                    new(ViewBag.GroupName, ViewBag.Slug, "groups")
                }
        };

        return View("Confirmation", viewmodel);
    }

    [Route("/groups/{slug}/change-group-info-confirmation")]
    public IActionResult ChangeGroupInfoConfirmation(string slug, string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            return NotFound();

        ViewBag.Slug = slug;
        ViewBag.GroupName = groupName;

        ConfirmationViewModel viewmodel = new()
        {
            Title = "Changes to a group's information",
            SubTitle = $"You've successfully submitted a change for {groupName}",
            ConfirmationText = "We will take a look at the changes you have suggested so that we can make sure that they are correct.",
            ButtonText = $"Go back to Stockport Local {groupName}",
            ButtonLink = "/groups/" + slug,
            Icon = "check",
            IconColour = "green",
            Crumbs = new List<Crumb>
                {
                    new("Stockport Local", "groups", "Group"),
                    new(ViewBag.GroupName, ViewBag.Slug, "groups")
                }
        };

        return View("Confirmation", viewmodel);
    }

    [Route("/groups/manage/{slug}/users")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> Users(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        return View(group);
    }

    [HttpGet]
    [Route("/groups/manage/{slug}/newuser")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> NewUser(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        AddEditUserViewModel model = new()
        {
            Slug = slug,
            Name = @group.Name
        };

        return View(model);
    }

    [HttpPost]
    [Route("/groups/manage/{slug}/newuser")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> NewUser(AddEditUserViewModel model, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(model.Slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();
        else if (!ModelState.IsValid)
            ViewBag.SubmissionError = _groupsService.GetErrorsFromModelState(ModelState);
        else if (group.GroupAdministrators.Items.Any(u => u.Email.ToUpper().Equals(model.GroupAdministratorItem.Email.ToUpper())))
            ViewBag.SubmissionError = "Sorry, this email already exists for this group. You can only assign an email to a group once.";
        else
        {
            string jsonContent = JsonConvert.SerializeObject(model.GroupAdministratorItem);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            response = await _repository.AddAdministrator(httpContent, model.Slug, model.GroupAdministratorItem.Email);
            if (!response.IsSuccessful())
                return response;
            
            await _emailBuilder.SendEmailNewUser(model);

            ConfirmationViewModel viewmodel = new()
            {
                Title = $"Add a new user",
                SubTitle = $"You've successfully added {model.GroupAdministratorItem.Name} to {group.Name}",
                ConfirmationText = "p>The change you've made will happen shortly so you won't have to do anything.</p>" +
                                   "<p> This user will now be able to manage your group's information.</p>",
                ButtonText = "Go back to add or remove users",
                ButtonLink = @Url.Action("Users", "Groups", new { slug = model.Slug }),
                Icon = "check",
                IconColour = "green",
                Crumbs = new List<Crumb>
                    {
                        new("Stockport Local", "groups", "Group"),
                        new("Manage your groups", "manage", "groups"),
                        new(group.Name, "manage/" + model.Slug, "groups"),
                        new("Users", "manage/" + model.Slug + "/users", "groups")
                    }
            };

            return View("Confirmation", viewmodel);
        }

        return View(model);
    }

    [HttpGet]
    [Route("/groups/manage/{slug}/edituser")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> EditUser(string slug, string email, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        GroupAdministratorItems groupAdministrator = group.GroupAdministrators.Items.FirstOrDefault(i => i.Email.Equals(email));
        if (groupAdministrator is null)
            return NotFound();

        AddEditUserViewModel model = new()
        {
            Slug = slug,
            Name = group.Name,
            GroupAdministratorItem = groupAdministrator,
            Previousrole = groupAdministrator.Permission
        };

        return View(model);
    }

    [HttpPost]
    [Route("/groups/manage/{slug}/edituser")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> EditUser(AddEditUserViewModel model, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(model.Slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        IEnumerable<GroupAdministratorItems> administratorItem = group.GroupAdministrators.Items.Where(i => i.Email.Equals(model.GroupAdministratorItem.Email));

        if (!administratorItem.Any() || !_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();
        else if (!ModelState.IsValid)
            ViewBag.SubmissionError = _groupsService.GetErrorsFromModelState(ModelState);
        else
        {
            string jsonContent = JsonConvert.SerializeObject(model.GroupAdministratorItem);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            response = await _repository.UpdateAdministrator(httpContent, model.Slug, model.GroupAdministratorItem.Email);
            if (!response.IsSuccessful())
                return response;
            
            await _emailBuilder.SendEmailEditUser(model);
            return RedirectToAction("EditUserConfirmation", new { slug = model.Slug, name = model.GroupAdministratorItem.Name, groupName = group.Name });
        }

        return View(model);
    }

    [Route("/groups/manage/{slug}/edituserconfirmation")]
    public IActionResult EditUserConfirmation(string slug, string name, string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName) || string.IsNullOrWhiteSpace(name))
            return NotFound();

        ViewBag.Slug = slug;
        ViewBag.Name = name;
        ViewBag.GroupName = groupName;

        return View();
    }

    [HttpGet]
    [Route("/groups/manage/{slug}/removeuser")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> Remove(string slug, string email, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        GroupAdministratorItems groupAdministrator = group.GroupAdministrators.Items.FirstOrDefault(i => i.Email.Equals(email));
        if (groupAdministrator is null)
            return NotFound();

        RemoveUserViewModel model = new()
        {
            Slug = slug,
            Email = email,
            Name = groupAdministrator.Name,
            GroupName = group.Name,
        };

        return View(model);
    }

    [HttpPost]
    [Route("/groups/manage/{slug}/removeuser")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> RemoveUser(RemoveUserViewModel model, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(model.Slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        response = await _repository.RemoveAdministrator(model.Slug, model.Email);

        if (!response.IsSuccessful())
            return response;

        await _emailBuilder.SendEmailDeleteUser(model);

        ConfirmationViewModel viewmodel = new()
        {
            Title = "Remove user",
            SubTitle = $"You've successfully removed {model.Name} to {group.Name}",
            ConfirmationText = "<p>The change you've made will happen shortly so you won't have to do anything.</p>" +
                               "<p> If you accidently deleted this user," +
                               "you can always <a href = " + Url.Action("NewUser", "Groups", new { slug = model.Slug }) + "> add them </a> again to your group.</p> ",
            ButtonText = "Go back to add or remove users",
            ButtonLink = Url.Action("NewUser", "Groups", new { slug = model.Slug }),
            Icon = "check",
            IconColour = "green",
            Crumbs = new List<Crumb>
                {
                    new("Stockport Local", "groups", "Group"),
                    new("Manage your groups", "manage", "groups"),
                    new(model.GroupName, "manage/" + model.Slug, "groups")
                }
        };

        return View("Confirmation", viewmodel);
    }

    [Route("/groups/thank-you-message")]
    public IActionResult ThankYouMessage() =>
        View();

    [Route("/groups/manage")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public async Task<IActionResult> Manage(LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.GetAdministratorsGroups(loggedInPerson.Email);

        if (!response.IsSuccessful())
            return response;

        List<Group> groups = response.Content as List<Group>;

        GroupManagePage result = new()
        {
            Groups = groups,
            Email = loggedInPerson.Email,
            ContactPageUrl = _configuration.GetGroupManageContactUrl()
        };

        return View(result);
    }

    [Route("/groups/manage/{slug}")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> ManageGroup(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            return NotFound();

        ManageGroupViewModel result = new()
        {
            Name = group.Name,
            Slug = slug,
            Administrator = _groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"),
            IsArchived = _groupsService.DateNowIsNotBetweenHiddenRange(group.DateHiddenFrom, group.DateHiddenTo)
        };

        return View(result);
    }

    [Route("/groups/manage/{slug}/delete")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> Delete(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        return View(group);
    }

    [Route("/groups/manage/{groupSlug}/events/{eventSlug}/delete")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> DeleteEvent(string groupSlug, string eventSlug, LoggedInPerson loggedInPerson)
    {
        HttpResponse eventResponse = await _processedContentRepository.Get<Event>(eventSlug, _managementQuery);
        HttpResponse groupResponse = await _processedContentRepository.Get<Group>(groupSlug, _managementQuery);

        if (!eventResponse.IsSuccessful())
            return eventResponse;

        ProcessedEvents eventItem = eventResponse.Content as ProcessedEvents;
        ProcessedGroup group = groupResponse.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();
        ViewBag.GroupName = group.Name;
        ViewBag.GroupSlug = group.Slug;

        return View(eventItem);
    }

    [HttpPost]
    [Route("/groups/manage/{groupSlug}/events/{eventSlug}/delete")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> DeleteAnEvent(string groupSlug, string eventSlug, LoggedInPerson loggedInPerson)
    {
        HttpResponse eventResponse = await _processedContentRepository.Get<Event>(eventSlug, _managementQuery);
        HttpResponse groupResponse = await _processedContentRepository.Get<Group>(groupSlug, _managementQuery);

        if (!eventResponse.IsSuccessful())
            return eventResponse;

        ProcessedEvents eventItem = eventResponse.Content as ProcessedEvents;
        ProcessedGroup group = groupResponse.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        HttpResponse deleteResponse = await _repository.Delete<Event>(eventSlug);

        if (!deleteResponse.IsSuccessful())
            return deleteResponse;

        _emailBuilder.SendEmailEventDelete(eventItem, group);

        ConfirmationViewModel viewmodel = new()
        {
            Title = $"Delete {eventItem.Title}",
            SubTitle = "Your event has been successfully deleted",
            ConfirmationText = "The event will be removed from the events calendar shortly.",
            ButtonText = "Go back to manage your events",
            ButtonLink = Url.Action("ViewGroupsEvents", "Groups", new { slug = ViewBag.GroupSlug }),
            Icon = "check",
            IconColour = "green",
            Crumbs = new List<Crumb>
                {
                    new("Stockport Local", "groups", "Group"),
                    new("Manage your groups", "manage", "groups"),
                    new(group.Name, "manage/" + group.Slug, "groups"),
                    new("Manage your events", "manage/" + group.Slug + "/events/", "groups"),
                    new(eventItem.Title, "manage/" + group.Slug + "/events/" + eventItem.Slug, "groups")
                }
        };

        return View("Confirmation", viewmodel);
    }

    [HttpPost]
    [Route("/groups/manage/{slug}/delete")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> DeleteGroup(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _processedContentRepository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        ProcessedGroup group = response.Content as ProcessedGroup;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        response = await _repository.Delete<Group>(slug);

        if (!response.IsSuccessful())
            return response;

        _emailBuilder.SendEmailDelete(group);

        ConfirmationViewModel viewmodel = new()
        {
            Title = $"Delete {group.Name}",
            SubTitle = "Your group has been successfully deleted",
            ConfirmationText = "The group will be removed from the website shortly.",
            ButtonText = "Go back to manage your groups",
            ButtonLink = Url.Action("Manage", "Groups"),
            Icon = "check",
            IconColour = "green",
            Crumbs = new List<Crumb>
                {
                    new("Stockport Local", "groups", "Group"),
                    new("Manage your groups", "manage", "groups")
                }
        };

        return View("Confirmation", viewmodel);
    }

    [Route("/groups/manage/{slug}/archive")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> Archive(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        return View(group);
    }

    [HttpPost]
    [Route("/groups/manage/{slug}/archive")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> ArchiveGroup(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        group.DateHiddenFrom = DateTime.Now;
        group.DateHiddenTo = null;

        string jsonContent = JsonConvert.SerializeObject(group);
        StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponse putResponse = await _repository.Archive<Group>(httpContent, slug);

        if (putResponse.StatusCode.Equals((int)HttpStatusCode.OK))
        {
            _emailBuilder.SendEmailArchive(group);

            ConfirmationViewModel viewmodel = new()
            {
                Title = $"Archive {group.Name}",
                SubTitle = "We've received your request to archive your group",
                ConfirmationText = "Your group will be unpublished and will be removed from the website. <br/>" +
                                   "Any events your group have published will also be archived.<br/>" +
                                   "When you want your group to appear again, you can republish the group at any time.",
                ButtonText = "Go back to manage your groups",
                ButtonLink = @Url.Action("Manage", "Groups"),
                Icon = "check",
                IconColour = "green",
                Crumbs = new List<Crumb>
                    {
                        new("Stockport Local", "groups", "Group"),
                        new("Manage your groups", "manage", "groups"),
                        new(group.Name, "manage/" + group.Slug, "groups")
                    }
            };

            return View("Confirmation", viewmodel);
        }
        else
        {
            throw new ContentfulUpdateException($"There was an error updating the group {group.Name}");
        }
    }

    [Route("/groups/manage/{slug}/publish")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> Publish(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        ViewBag.CurrentUrl = Request?.GetDisplayUrl();

        return View(group);
    }

    [HttpPost]
    [Route("/groups/manage/{slug}/publish")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> PublishGroup(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;
        
        Group group = response.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "A"))
            return NotFound();

        group.DateHiddenFrom = DateTime.MaxValue;
        group.DateHiddenTo = DateTime.MaxValue; ;

        string jsonContent = JsonConvert.SerializeObject(group);
        StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        HttpResponse putResponse = await _repository.Publish<Group>(httpContent, slug);

        if (putResponse.StatusCode.Equals((int)HttpStatusCode.OK))
        {
            _emailBuilder.SendEmailPublish(group);

            return RedirectToAction("PublishConfirmation", new { slug = group.Slug, name = group.Name });
        }
        else
        {
            throw new ContentfulUpdateException($"There was an error updating the group{group.Name}");
        }
    }

    [Route("/groups/manage/publishconfirmation")]
    public IActionResult PublishConfirmation(string slug, string name)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return NotFound();

        ViewBag.GroupName = name;
        ViewBag.Slug = slug;

        return View();
    }

    [HttpGet]
    [Route("/groups/manage/{slug}/update")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public async Task<IActionResult> EditGroup(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            return NotFound();

        GroupSubmission model = new();

        if (group.Twitter is not null && group.Twitter.StartsWith("http"))
            model.Twitter = group.Twitter.Replace("https://www.twitter.com/", "@");

        model.Address = group.Address;
        model.Description = _markdownWrapper.ConvertToHtml(group.Description);
        model.Email = group.Email;
        model.Facebook = group.Facebook;
        model.Name = group.Name;
        model.PhoneNumber = group.PhoneNumber;
        model.Website = group.Website;
        model.Slug = group.Slug;
        model.Longitude = group.MapPosition.Lon;
        model.Latitude = group.MapPosition.Lat;
        model.Volunteering = group.Volunteering;
        model.Categories = group.CategoriesReference.Select(g => g.Name).ToList();
        model.CategoriesList = string.Join("|", model.Categories);
        model.VolunteeringText = _groupsService.GetVolunteeringText(group.VolunteeringText);
        model.AvailableCategories = await _groupsService.GetAvailableGroupCategories();
        model.AdditionalInformation = group.AdditionalInformation;
        model.Suitabilities.Where(_ => group.SuitableFor.Contains(_.Name)).ToList().ForEach(item => item.IsSelected = true);
        model.AgeRanges.Where(_ => group.AgeRange.Contains(_.Name)).ToList().ForEach(item => item.IsSelected = true);
        model.Donations = group.Donations;
        model.DonationsUrl = group.DonationsUrl;
        model.DonationsText = group.DonationsText;

        return View(model);
    }

    [HttpPost]
    [Route("/groups/manage/{slug}/update")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> EditGroup(string slug, GroupSubmission model, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(slug, _managementQuery);
        StringBuilder validationErrors = new();
        ViewBag.DisplayContentapiUpdateError = false;

        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        model.AvailableCategories = await _groupsService.GetAvailableGroupCategories();
        model.Slug = group.Slug;

        HttpResponse categoryResponse = await _repository.Get<List<GroupCategory>>();
        List<GroupCategory> listOfGroupCategories = categoryResponse.Content as List<GroupCategory>;

        if (listOfGroupCategories is not null)
            model.Categories = listOfGroupCategories.Select(logc => logc.Name).ToList();

        Converter converter = new();
        string twitterUser = model.Twitter;

        if (model.Twitter is not null && model.Twitter.StartsWith("@"))
        {
            twitterUser = twitterUser.Replace("@", "/");
            group.Twitter = @"https://www.twitter.com" + twitterUser;
        }
        
        group.Address = model.Address;
        group.Description = converter.Convert(_viewHelpers.StripUnwantedHtml(model.Description));
        group.Email = model.Email;
        group.Facebook = model.Facebook;
        group.Name = model.Name;
        group.PhoneNumber = model.PhoneNumber;
        group.Website = model.Website;
        group.Volunteering = model.Volunteering;
        group.Donations = model.Donations;
        group.MapPosition = new MapPosition { Lon = model.Longitude, Lat = model.Latitude };
        group.Volunteering = model.Volunteering;
        group.Donations = group.Donations;
        group.DonationsText = _groupsService.GetDonationsText(model.DonationsText);
        group.VolunteeringText = _groupsService.GetVolunteeringText(model.VolunteeringText);
        group.AdditionalInformation = model.AdditionalInformation;
        group.DonationsUrl = model.DonationsUrl;
        group.CategoriesReference = new(listOfGroupCategories.Where(c => model.CategoriesList.Split('|').Contains(c.Name)));
        group.SuitableFor = model.Suitabilities.Where(_ => _.IsSelected).Select(_ => _.Name).ToList();
        group.AgeRange = model.AgeRanges.Where(_ => _.IsSelected).Select(_ => _.Name).ToList();

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            return NotFound();
        else if (!ModelState.IsValid)
            validationErrors.Append(_groupsService.GetErrorsFromModelState(ModelState));
        else
        {
            string jsonContent = JsonConvert.SerializeObject(group);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponse putResponse = await _repository.Put<Group>(httpContent, slug);

            if (putResponse.StatusCode.Equals((int)HttpStatusCode.OK))
            {
                _emailBuilder.SendEmailEditGroup(model, loggedInPerson.Email);

                // if there is an image, send this in an email
                if (model.Image is not null && !string.IsNullOrEmpty(model.Image.FileName))
                    await _groupsService.SendImageViaEmail(model.Image, model.Name, model.Slug);

                return RedirectToAction("EditGroupConfirmation", new { slug = slug, groupName = group.Name });
            }
            else
            {
                _logger.LogError($"There was an error updating the group {group.Name}");

                ViewBag.DisplayContentapiUpdateError = true;
            }
        }

        ViewBag.SubmissionError = validationErrors.Length > 0 ? validationErrors : null;

        return View(model);
    }

    [HttpPost]
    [Route("/groups/{slug}/up-to-date")]
    public async Task<IActionResult> GroupUpToDate(string slug)
    {
        HttpResponse response = await _repository.Get<Group>(slug);
        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        string jsonContent = JsonConvert.SerializeObject(group);
        StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        HttpResponse putResponse = await _repository.Put<Group>(httpContent, slug);

        return Json(putResponse.StatusCode.ToString());
    }

    [HttpGet]
    [Route("/groups/favourites/clearall")]
    public IActionResult FavouriteGroupsClearAll() =>
        View("~/views/stockportgov/favourites/confirmclearall.cshtml",
            new Favourites()
            {
                Type = "groups",
                Crumbs = new List<Crumb> { new("Stockport Local", "groups", "groups") },
                FavouritesUrl = "/groups/favourites"
            });

    [HttpPost]
    [Route("/groups/favourites/clearall")]
    public IActionResult FavouriteGroupsClearAll(Favourites model)
    {
        _cookiesHelper.RemoveAllFromCookies<Group>("favourites");
        
        return RedirectToAction("FavouriteGroups");
    }

    private async Task<HttpResponse> GetFavouriteGroupResults()
    {
        GroupResults model = new();
        List<Query> queries = new();
        List<string> favouritesList = _cookiesHelper.GetCookies<Group>("favourites");
        string favourites = "-NO-FAVOURITES-SET-";

        if (favouritesList is not null && favouritesList.Any())
            favourites = string.Join(",", _cookiesHelper.GetCookies<Group>("favourites"));

        queries.Add(new Query("slugs", favourites));

        return await _repository.Get<GroupResults>(queries: queries);
    }

    [HttpGet]
    [Route("/groups/favourites/get-count")]
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public async Task<int> GetCount()
    {
        List<Query> queries = new();
        List<string> favouritesList = _cookiesHelper.GetCookies<Group>("favourites");
        string favourites = "-NO-FAVOURITES-SET-";

        if (favouritesList is not null && favouritesList.Any())
            favourites = string.Join(",", _cookiesHelper.GetCookies<Group>("favourites"));

        queries.Add(new Query("slugs", favourites));

        HttpResponse response = await _repository.Get<GroupResults>(queries: queries);

        if (!response.StatusCode.Equals((int)HttpStatusCode.OK))
            return 0;

        GroupResults groupResults = response.Content as GroupResults;

        return groupResults.Groups.ToList().Count(a => !a.Status.Equals("Archived"));
    }

    [HttpGet]
    [Route("/groups/favourites")]
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    public async Task<IActionResult> FavouriteGroups([FromQuery] int page, [FromQuery] int pageSize)
    {
        HttpResponse response = await GetFavouriteGroupResults();

        if (response.IsNotFound())
            return NotFound();

        GroupResults model = response.Content as GroupResults;

        model.AddQueryUrl(new QueryUrl(Url?.ActionContext.RouteData.Values, Request?.Query));
        _filteredUrl.SetQueryUrl(model.CurrentUrl);
        model.AddFilteredUrl(_filteredUrl);

        if (pageSize.Equals(-1))
            return View("FavouriteGroupsPrint", model.Groups);
        else
            return View(model);
    }

    // TODO: Move this and all links pointing towards it to events controller
    [HttpGet]
    [Route("/groups/manage/{groupslug}/events/{eventslug}/update")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> EditEvent(string groupslug, string eventslug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(groupslug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            return NotFound();

        HttpResponse eventResponse = await _repository.Get<Event>(eventslug, _managementQuery);

        if (!response.IsSuccessful())
            return eventResponse;

        Event eventDetail = eventResponse.Content as Event;

        EventSubmission model = new();

        model.AvailableCategories = await GetAvailableEventCategories();

        if (eventDetail.EventCategories.Any())
        {
            model.CategoriesList = eventDetail.EventCategories[0].Name;

            if (eventDetail.EventCategories.Count() > 1)
                model.CategoriesList += $",{eventDetail.EventCategories[1].Name}";

            if (eventDetail.EventCategories.Count() > 2)
                model.CategoriesList += $",{eventDetail.EventCategories[2].Name}";
        }

        model.Description = _markdownWrapper.ConvertToHtml(eventDetail.Description);
        if (!eventDetail.EventFrequency.Equals(EventFrequency.None))
            model.EndDate = _dateCalculator.GetEventEndDate(eventDetail);

        model.EndTime = DateTime.ParseExact(eventDetail.EndTime, "HH:mm", null);
        model.EventDate = eventDetail.EventDate;
        model.Fee = eventDetail.Fee;
        model.Frequency = eventDetail.EventFrequency.ToString();
        model.Location = eventDetail.Location;
        model.IsRecurring = eventDetail.EventFrequency.Equals(EventFrequency.None);
        model.StartTime = DateTime.ParseExact(eventDetail.StartTime, "HH:mm", null);
        model.Title = eventDetail.Title;
        model.SubmittedBy = eventDetail.SubmittedBy;
        model.SubmitterEmail = loggedInPerson.Email;

        model.Slug = eventDetail.Slug;
        model.GroupName = group.Name;
        model.GroupSlug = group.Slug;

        return View(model);
    }

    // TODO: Move this and all links pointing towards it to events controller
    [HttpPost]
    [Route("/groups/manage/{slug}/events/{eventslug}/update")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> EditEvent(string slug, string eventslug, EventSubmission model, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            return NotFound();

        HttpResponse eventResponse = await _repository.Get<Event>(eventslug, _managementQuery);
        StringBuilder validationErrors = new();
        ViewBag.DisplayContentapiUpdateError = false;

        if (!eventResponse.IsSuccessful())
            return eventResponse;

        Event eventDetail = eventResponse.Content as Event;

        model.Slug = eventDetail.Slug;
        model.GroupSlug = group.Slug;
        model.GroupName = group.Name;

        Converter converter = new();

        eventDetail.Title = model.Title;
        eventDetail.Description = converter.Convert(_viewHelpers.StripUnwantedHtml(model.Description));
        eventDetail.EndTime = ((DateTime)model.EndTime).ToString("HH:mm");
        eventDetail.EventDate = (DateTime)model.EventDate;
        eventDetail.Fee = model.Fee;
        eventDetail.Location = model.Location;
        eventDetail.MapPosition = new MapPosition { Lon = model.Longitude, Lat = model.Latitude };

        if (model.IsRecurring)
        {
            switch (model.Frequency)
            {
                case "Daily":
                    eventDetail.EventFrequency = EventFrequency.Daily;
                    break;
                case "Fortnightly":
                    eventDetail.EventFrequency = EventFrequency.Fortnightly;
                    break;
                case "Weekly":
                    eventDetail.EventFrequency = EventFrequency.Weekly;
                    break;
                case "Monthly":
                    eventDetail.EventFrequency = EventFrequency.Monthly;
                    break;
                case "MonthlyDate":
                    eventDetail.EventFrequency = EventFrequency.MonthlyDate;
                    break;
                case "MonthlyDay":
                    eventDetail.EventFrequency = EventFrequency.MonthlyDay;
                    break;
                case "Yearly":
                    eventDetail.EventFrequency = EventFrequency.Yearly;
                    break;
                default:
                    eventDetail.EventFrequency = EventFrequency.None;
                    break;
            }

            eventDetail.Occurences = _dateCalculator.GetEventOccurences(eventDetail.EventFrequency, (DateTime)model.EventDate, (DateTime)model.EndDate);
        }
        else
        {
            eventDetail.EventFrequency = EventFrequency.None;
            eventDetail.Occurences = 1;
        }

        eventDetail.StartTime = model.StartTime?.ToString("HH:mm");
        eventDetail.UpdatedAt = DateTime.Now;

        model.Occurrences = eventDetail.Occurences;

        HttpResponse categoryResponse = await _repository.Get<List<EventCategory>>();
        List<EventCategory> listOfEventCategories = categoryResponse.Content as List<EventCategory>;

        if (!string.IsNullOrEmpty(model.CategoriesList))
            eventDetail.EventCategories = listOfEventCategories.Where(c => model.CategoriesList.Split(',').Contains(c.Name)).ToList();

        model.AvailableCategories = await GetAvailableEventCategories();

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            return NotFound();
        else if (!ModelState.IsValid)
            validationErrors.Append(_groupsService.GetErrorsFromModelState(ModelState));
        else
        {
            string jsonContent = JsonConvert.SerializeObject(eventDetail);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponse putResponse = await _repository.Put<Event>(httpContent, eventslug);

            if (putResponse.StatusCode.Equals((int)HttpStatusCode.OK))
                return RedirectToAction("EditEventConfirmation", new { groupslug = group.Slug, groupName = group.Name, eventslug = eventDetail.Slug, eventname = eventDetail.Title });
            else
            {
                _logger.LogError($"There was an error updating the event {eventDetail.Title}");

                ViewBag.DisplayContentapiUpdateError = true;
            }
        }

        ViewBag.SubmissionError = validationErrors.Length > 0 ? validationErrors : null;

        return View(model);
    }

    [Route("/groups/manage/{slug}/updateconfirmation")]
    public IActionResult EditGroupConfirmation(string slug, string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            return NotFound();

        ViewBag.Slug = slug;
        ViewBag.GroupName = groupName;

        return View();
    }

    // TODO: Move this and all links pointing towards it to events controller
    [Route("/groups/manage/{groupslug}/events/{eventslug}/updateconfirmation")]
    public IActionResult EditEventConfirmation(string groupslug, string eventslug, string groupName, string eventName)
    {
        if (string.IsNullOrWhiteSpace(groupName) || string.IsNullOrWhiteSpace(eventName))
            return NotFound();

        ViewBag.Slug = eventslug;
        ViewBag.EventName = eventName;
        ViewBag.GroupSlug = groupslug;
        ViewBag.GroupName = groupName;

        return View();
    }

    [HttpGet]
    [Route("/groups/manage/{slug}/events")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> ViewGroupsEvents(string slug, LoggedInPerson loggedInPerson)
    {
        HttpResponse response = await _repository.Get<Group>(slug, _managementQuery);

        if (!response.IsSuccessful())
            return response;

        Group group = response.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            return NotFound();

        return View(group);
    }

    [HttpGet]
    [Route("/groups/manage/{groupSlug}/events/{eventSlug}")]
    [ServiceFilter(typeof(GroupAuthorisation))]
    public async Task<IActionResult> GroupEventsDetails(string groupSlug, string eventSlug, LoggedInPerson loggedInPerson)
    {
        HttpResponse responseEvent = await _repository.Get<Event>(eventSlug, _managementQuery);
        HttpResponse responseGroup = await _repository.Get<Group>(groupSlug, _managementQuery);

        if (!responseEvent.IsSuccessful())
            return responseEvent;
        
        if (!responseGroup.IsSuccessful())
            return responseGroup;

        Event eventItem = responseEvent.Content as Event;
        Group group = responseGroup.Content as Group;

        if (!_groupsService.HasGroupPermission(loggedInPerson.Email, group.GroupAdministrators.Items, "E"))
            return NotFound();

        ViewBag.GroupName = group.Name;
        ViewBag.GroupSlug = group.Slug;

        return View(eventItem);
    }

    [HttpGet]
    [Route("/groups/stale")]
    public async Task<IActionResult> HandeStaleGroups([FromQuery] string password)
    {
        if (!password.Equals(_configuration.GetStaleGroupsSecret()))
            return new UnauthorizedResult();

        try
        {
            await _groupsService.HandleStaleGroups();

            return new OkObjectResult("Succesfully called HandleStaleGroups");
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }
}