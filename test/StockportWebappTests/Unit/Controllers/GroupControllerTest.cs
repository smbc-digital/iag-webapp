using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using StockportWebapp.ViewModels;
using StockportWebapp.AmazonSES;
using Microsoft.AspNetCore.Http;
using StockportWebappTests.Builders;
using StockportWebappTests.Unit.Utils;
using StockportWebapp.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StockportWebappTests.Unit.Controllers
{
    public class GroupControllerTest
    {
        private readonly GroupsController _groupController;
        private Mock<IRepository> _repository = new Mock<IRepository>();
        private Mock<IProcessedContentRepository> _processedRepository = new Mock<IProcessedContentRepository>();
        private Mock<GroupEmailBuilder> _groupEmailBuilder;
        private Mock<EventEmailBuilder> _eventEmailBuilder;
        private readonly Mock<IFilteredUrl> _filteredUrl;
        private Mock<ILogger<GroupsController>> _logger;
        private Mock<IApplicationConfiguration> _configuration = new Mock<IApplicationConfiguration>();
        private Mock<MarkdownWrapper> _markdownWrapper = new Mock<MarkdownWrapper>();
        private DateCalculator datetimeCalculator;
        private Mock<IHttpContextAccessor> http;
        private Mock<IHtmlUtilities> htmlUtilities = new Mock<IHtmlUtilities>();
        private HostHelper hostHelper = new HostHelper(new CurrentEnvironment("local"));
        private Mock<ILoggedInHelper> _loggedInHelper = new Mock<ILoggedInHelper>();
        private Mock<IGroupsService> _groupsService = new Mock<IGroupsService>();
        private Mock<ICookiesHelper> _cookiesHelper = new Mock<ICookiesHelper>();

        private readonly List<GroupCategory> groupCategories = new List<GroupCategory>
        {
            new GroupCategory() {Name = "name 1", Slug = "name1", Icon = "icon1", ImageUrl = "imageUrl1"},
            new GroupCategory() {Name = "name 2", Slug = "name2", Icon = "icon2", ImageUrl = "imageUrl2"},
            new GroupCategory() {Name = "name 3", Slug = "name3", Icon = "icon3", ImageUrl = "imageUrl3"},
        };

        private readonly ProcessedGroupHomepage groupHomepage = new ProcessedGroupHomepage
        (
           "title",
           "background-image.jpg",
           "featured group heading",
           new List<Group>(),
           new GroupCategory(),
           new GroupSubCategory(),
           new List<Alert>(),
           "bodyHeading",
           "body",
           "secondaryBodyHeading",
           "secondary body",
            new EventBanner.NullEventBanner()
           );

        public GroupControllerTest()
        {
            _filteredUrl = new Mock<IFilteredUrl>();
            _logger = new Mock<ILogger<GroupsController>>();

            var emailLogger = new Mock<ILogger<GroupEmailBuilder>>();
            var eventEmailLogger = new Mock<ILogger<EventEmailBuilder>>();
            var emailClient = new Mock<IHttpEmailClient>();
            var emailConfig = new Mock<IApplicationConfiguration>();

            emailConfig.Setup(a => a.GetEmailEmailFrom(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));

            _groupEmailBuilder = new Mock<GroupEmailBuilder>(emailLogger.Object, emailClient.Object, emailConfig.Object, new BusinessId("BusinessId"));
            _eventEmailBuilder = new Mock<EventEmailBuilder>(eventEmailLogger.Object, emailClient.Object, emailConfig.Object, new BusinessId("BusinessId"));

            var mockTime = new Mock<ITimeProvider>();
            var viewHelper = new ViewHelpers(mockTime.Object);

            datetimeCalculator = new DateCalculator(mockTime.Object);

            http = new Mock<IHttpContextAccessor>();

            var cookies = new FakeCookie(true);
            http.Setup(_ => _.HttpContext.Request.Cookies).Returns(cookies);

            _groupController = new GroupsController(_processedRepository.Object, _repository.Object, _groupEmailBuilder.Object, _eventEmailBuilder.Object, _filteredUrl.Object, null, _logger.Object, _configuration.Object, _markdownWrapper.Object, viewHelper, datetimeCalculator, htmlUtilities.Object, hostHelper, _loggedInHelper.Object, _groupsService.Object, _cookiesHelper.Object, new StockportWebapp.FeatureToggling.FeatureToggles());

            // setup mocks
            _groupsService.Setup(o => o.GetGroupCategories()).ReturnsAsync(groupCategories);
            _groupsService.Setup(o => o.GetGroupHomepage()).ReturnsAsync(groupHomepage);
            _groupsService.Setup(o => o.GetAvailableGroupCategories()).ReturnsAsync(new List<string>());
            _groupsService.Setup(o => o.GetErrorsFromModelState(It.IsAny<ModelStateDictionary>())).Returns("");
        }

        [Fact]
        public void ItReturnsAGroupWithProcessedBody()
        {
            // Arrange
            var processedGroup = new ProcessedGroupBuilder().Build();

            // Mocks
            _processedRepository.Setup(o => o.Get<Group>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new StockportWebapp.Http.HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));
            
            _loggedInHelper.Setup(_ => _.GetLoggedInPerson()).Returns(new LoggedInPerson());

            _configuration.Setup(_ => _.GetArchiveEmailPeriods())
                .Returns(new List<ArchiveEmailPeriod> {new ArchiveEmailPeriod {NumOfDays = 1}});

            // Act
            var view = AsyncTestHelper.Resolve(_groupController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as GroupDetailsViewModel;

            // Assert
            model.Group.Name.Should().Be(processedGroup.Name);
            model.Group.Slug.Should().Be(processedGroup.Slug);
            model.Group.Address.Should().Be(processedGroup.Address);
            model.Group.Email.Should().Be(processedGroup.Email);
            model.Group.MapDetails.AccessibleTransportLink.Should().Be(processedGroup.MapDetails.AccessibleTransportLink);
        }

        [Fact]
        public void GetsA404NotFoundGroup()
        {
            _processedRepository.Setup(o => o.Get<Group>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new StockportWebapp.Http.HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = AsyncTestHelper.Resolve(_groupController.Detail("not-found-slug")) as StockportWebapp.Http.HttpResponse;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public void ShouldGetListOfGroupCategories()
        {
            // Act
            var view = AsyncTestHelper.Resolve(_groupController.Index()) as ViewResult;
            var model = view.ViewData.Model as GroupStartPage;

            // Assert
            model.Categories.Count.Should().Be(groupCategories.Count);
        }

        [Fact]
        public void ItShouldGetARedirectResultForAValidGroupSubmission()
        {
            var groupSubmission = new GroupSubmission()
            {
                Name = "Group",
                Description = "Description",
                Website = "http://www.group.com",
                Email = "info@group.com",
                Address = "Address",
                PhoneNumber = "phone",
                CategoriesList = "Category",
                Image = null
            };
            _groupEmailBuilder.Setup(o => o.SendEmailAddNew(It.IsAny<GroupSubmission>())).ReturnsAsync(HttpStatusCode.OK);

            var actionResponse = AsyncTestHelper.Resolve(_groupController.AddAGroup(groupSubmission)) as RedirectToActionResult;
            actionResponse.ActionName.Should().Be("ThankYouMessage");
        }

        [Fact]
        public void ItShouldReturnBackToTheViewForAnInvalidEmailSubmission()
        {
            var groupSubmission = new GroupSubmission();

            _groupEmailBuilder.Setup(o => o.SendEmailAddNew(It.IsAny<GroupSubmission>())).ReturnsAsync(HttpStatusCode.BadRequest);

            var actionResponse = AsyncTestHelper.Resolve(_groupController.AddAGroup(groupSubmission));

            actionResponse.Should().BeOfType<ViewResult>();
            _groupEmailBuilder.Verify(o => o.SendEmailAddNew(groupSubmission), Times.Once);
        }

        [Fact]
        public void ItShouldNotSendAnEmailForAnInvalidFormSumbission()
        {
            var groupSubmission = new GroupSubmission();

            _groupController.ModelState.AddModelError("Name", "an invalid name was provided");
           
            var actionResponse = AsyncTestHelper.Resolve(_groupController.AddAGroup(groupSubmission));

            actionResponse.Should().BeOfType<ViewResult>();
            _groupEmailBuilder.Verify(o => o.SendEmailAddNew(groupSubmission), Times.Never);
        }

        [Fact]
        public void ShouldReturnLocationIfOneIsSelected()
        {
            // Arrange
            var location = new MapDetails(){
                MapPosition = new MapPosition() { Lat = 1, Lon = 1 },
                AccessibleTransportLink = ""
            };

            var processedGroup = new ProcessedGroupBuilder().MapDetails(location).Build();

            // Mocks
            _loggedInHelper.Setup(_ => _.GetLoggedInPerson()).Returns(new LoggedInPerson());
            _processedRepository.Setup(o => o.Get<Group>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new StockportWebapp.Http.HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));

            _configuration.Setup(_ => _.GetArchiveEmailPeriods())
                .Returns(new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { NumOfDays = 1 } });

            // Act
            var view = AsyncTestHelper.Resolve(_groupController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as GroupDetailsViewModel;

            // Assert
            model.Group.MapDetails.Should().Be(location);
        }

        [Fact]
        public void ShouldReturnAListOfLinkedEvents()
        {
            // Arrange
            var linkedEvent = new Event() {Slug = "event-slug"};
            var listOfLinkedEvents = new List<Event> { linkedEvent };
            var group = new ProcessedGroup() { Events = listOfLinkedEvents, Slug = "test" };

            // Mocks
            _processedRepository.Setup(o => o.Get<Group>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new StockportWebapp.Http.HttpResponse((int)HttpStatusCode.OK, group, string.Empty));
            _loggedInHelper.Setup(_ => _.GetLoggedInPerson()).Returns(new LoggedInPerson());

            _configuration.Setup(_ => _.GetArchiveEmailPeriods())
                .Returns(new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { NumOfDays = 1 } });

            // Act
            var view = AsyncTestHelper.Resolve(_groupController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as GroupDetailsViewModel;

            // Assert
            model.Group.Events.FirstOrDefault().Should().Be(linkedEvent);
        }

        [Fact]
        public void EditGroup_ShouldReturnCorrectTwitterHandleFormat()
        {
            var groupAdmins = new GroupAdministrators();
            groupAdmins.Items.Add(new GroupAdministratorItems() {Email= "test@email.com"});


            var group = new Group("name", "slug", "010101010", "email@mail.com", "www.website.com",
                "https://www.twitter.com/testHandle", "www.facebook.com", "address", "description", "image-url",
                "thumnail-url", new List<GroupCategory>{new GroupCategory {Name = "testCategory" } }, new List<GroupSubCategory>(), new List<Crumb>(),
                new MapPosition{Lat = 100, Lon = 200}, false, new List<Event>(), new GroupAdministrators(), DateTime.MinValue,
                DateTime.MinValue, "status", new List<string>(), "£1", "ability", false, "volunteer text",
                new Organisation(), new List<Group>(), false, "tenaport-link", "aditional-info",
                new List<Document>(), DateTime.MinValue, new List<string>(), new List<string>(), "donation-text", "donation-url");

            var loggedInPerson = new LoggedInPerson
            {
                Email = "test@email.com"
            };
            _repository.Setup(_ => _.Get<Group>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(StockportWebapp.Http.HttpResponse.Successful((int)HttpStatusCode.OK, group));
            _groupsService.Setup(_ =>
                _.HasGroupPermission(It.IsAny<string>(), It.IsAny<List<GroupAdministratorItems>>(),
                    It.IsAny<string>())).Returns(true);
            _markdownWrapper.Setup(_ => _.ConvertToHtml(It.IsAny<string>()));
            _groupsService.Setup(_ => _.GetVolunteeringText(It.IsAny<string>()));
            _groupsService.Setup(_ => _.GetAvailableGroupCategories()).ReturnsAsync(new List<string>());

            // Act
            var view = AsyncTestHelper.Resolve(_groupController.EditGroup("slug", loggedInPerson)) as ViewResult;
            var model = view.ViewData.Model as GroupSubmission;

            Assert.Equal("@testHandle", model.Twitter);
        }


        private GroupsController SetUpController(int numGroups)
        {
            var listOfGroups = BuildGroupList(numGroups);

            var bigGroupResults = new GroupResults {Groups = listOfGroups};

            _repository.Setup(o =>
                o.Get<GroupResults>(
                    It.IsAny<string>(),
                    It.IsAny<List<Query>>()))
                .ReturnsAsync(StockportWebapp.Http.HttpResponse.Successful((int)HttpStatusCode.OK, bigGroupResults));

            var mockTime = new Mock<ITimeProvider>();
            var viewHelper = new ViewHelpers(mockTime.Object);
            return new GroupsController(_processedRepository.Object, _repository.Object, _groupEmailBuilder.Object, _eventEmailBuilder.Object, _filteredUrl.Object, null, _logger.Object, _configuration.Object, _markdownWrapper.Object, viewHelper, datetimeCalculator, htmlUtilities.Object, hostHelper, null, _groupsService.Object, _cookiesHelper.Object, new StockportWebapp.FeatureToggling.FeatureToggles());
        }

        private List<Group> BuildGroupList(int numberOfItems)
        {
            var listOfGroups = new List<Group>();

            for (var i = 0; i < numberOfItems; i++)
            {
                listOfGroups.Add(new GroupBuilder().Build());
            }

            return listOfGroups;
        }
    }
}