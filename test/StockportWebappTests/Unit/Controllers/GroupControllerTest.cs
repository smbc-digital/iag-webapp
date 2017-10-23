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

namespace StockportWebappTests.Unit.Controllers
{
    public class GroupControllerTest
    {
        private readonly GroupsController _groupController;
        private Mock<IRepository> _repository = new Mock<IRepository>();
        private Mock<IProcessedContentRepository> _processedRepository = new Mock<IProcessedContentRepository>();
        private Mock<GroupEmailBuilder> _groupEmailBuilder;
        private Mock<EventEmailBuilder> _eventEmailBuilder;
        public const int MaxNumberOfItemsPerPage = 9;
        private readonly Mock<IFilteredUrl> _filteredUrl;
        private Mock<ILogger<GroupsController>> _logger;
        private Mock<IApplicationConfiguration> _configuration = new Mock<IApplicationConfiguration>();
        private MarkdownWrapper markdownWrapper = new MarkdownWrapper();
        private DateCalculator datetimeCalculator;
        private Mock<IHttpContextAccessor> http;
        Mock<IHtmlUtilities> htmlUtilities = new Mock<IHtmlUtilities>();
        HostHelper hostHelper = new HostHelper(new CurrentEnvironment("local"));
        private Mock<ILoggedInHelper> _loggedInHelper = new Mock<ILoggedInHelper>();

        private readonly List<GroupCategory> groupCategories = new List<GroupCategory>
        {
            new GroupCategory() {Name = "name 1", Slug = "name1", Icon = "icon1", ImageUrl = "imageUrl1"},
            new GroupCategory() {Name = "name 2", Slug = "name2", Icon = "icon2", ImageUrl = "imageUrl2"},
            new GroupCategory() {Name = "name 3", Slug = "name3", Icon = "icon3", ImageUrl = "imageUrl3"},
        };

        private readonly GroupHomepage groupHomepage = new GroupHomepage
        {
            Title = "Group Homepage Title",
            BackgroundImage = "background-image.jpg"
        };

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

            

            _groupController = new GroupsController(_processedRepository.Object, _repository.Object, _groupEmailBuilder.Object, _eventEmailBuilder.Object, _filteredUrl.Object, null, _logger.Object, _configuration.Object, markdownWrapper, viewHelper, datetimeCalculator, http.Object, null, htmlUtilities.Object, hostHelper, _loggedInHelper.Object);
            
            // setup mocks
            _repository.Setup(o => o.Get<List<GroupCategory>>("", null))
                .ReturnsAsync(StockportWebapp.Http.HttpResponse.Successful(200, groupCategories));

            _repository.Setup(o => o.Get<GroupHomepage>("", null))
                .ReturnsAsync(StockportWebapp.Http.HttpResponse.Successful(200, groupHomepage));
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
            var view = AsyncTestHelper.Resolve(_groupController.Index()) as ViewResult;
            var model = view.ViewData.Model as GroupStartPage;
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
        public void ItShouldGetARedirectResultForDelete()
        {
            // Arrange
            var slug = "deleteSlug";
            var loggedInPerson = new LoggedInPerson { Name = "name", Email = "email@email.com" };
            var processedGroup = new ProcessedGroupBuilder().Build();

            // Mocks
            _repository.Setup(r => r.Delete<Group>(slug))
                .ReturnsAsync(new StockportWebapp.Http.HttpResponse((int) HttpStatusCode.OK, processedGroup, string.Empty));

            _processedRepository.Setup(o => o.Get<Group>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(new StockportWebapp.Http.HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));

            // Act
            var actionResponse = AsyncTestHelper.Resolve(_groupController.DeleteGroup(slug, loggedInPerson)) as RedirectToActionResult;

            // Assert
            actionResponse.ActionName.Should().Be("DeleteConfirmation");
        }

        [Fact]
        public void ItShouldGetARedirectResultForArchive()
        {
            // Arrange
            var slug = "archiveSlug";
            var loggedInPerson = new LoggedInPerson { Name = "name", Email = "email@email.com" };
            var group = new GroupBuilder().Build();

            // Mocks
            _repository.Setup(r => r.Archive<Group>(It.IsAny<HttpContent>(), slug))
                .ReturnsAsync(new StockportWebapp.Http.HttpResponse((int)HttpStatusCode.OK, group, string.Empty));

            _repository.Setup(o => o.Get<Group>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(new StockportWebapp.Http.HttpResponse((int)HttpStatusCode.OK, group, string.Empty));

            // Act
            var actionResponse = AsyncTestHelper.Resolve(_groupController.ArchiveGroup(slug, loggedInPerson)) as RedirectToActionResult;

            // Assert
            actionResponse.ActionName.Should().Be("ArchiveConfirmation");
        }

        [Fact]
        public void ShouldReturnEmptyPaginationForNoGroups()
        {
            var emptyRepository = new Mock<IRepository>();

            var _emptyGroupResults = new GroupResults() {Groups = new List<Group>()};

            emptyRepository.Setup(o => o.Get<GroupResults>(It.IsAny<string>(), It.IsAny<List<Query>>()))
              .ReturnsAsync(StockportWebapp.Http.HttpResponse.Successful((int)HttpStatusCode.OK, _emptyGroupResults));

            var mockTime = new Mock<ITimeProvider>();
            var viewHelper = new ViewHelpers(mockTime.Object);
            var controller = new GroupsController(_processedRepository.Object, emptyRepository.Object, _groupEmailBuilder.Object, _eventEmailBuilder.Object, _filteredUrl.Object, null, _logger.Object, _configuration.Object, markdownWrapper, viewHelper, datetimeCalculator, http.Object, null, htmlUtilities.Object, hostHelper, null);

            var search = new GroupSearch
            {
                Category = "nonsense",
                Order = "a-z",
                Latitude = 0,
                Longitude = 0
            };

            var actionResponse =
               AsyncTestHelper.Resolve(
                   controller.Results(1, MaxNumberOfItemsPerPage, search)) as ViewResult;

            var viewModel = actionResponse.ViewData.Model as GroupResults;

            viewModel.Groups.Count.Should().Be(0);
            viewModel.Pagination.TotalItems.Should().Be(0);
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(2, 1, 2, 1)]
        [InlineData(MaxNumberOfItemsPerPage, 1, MaxNumberOfItemsPerPage, 1)]
        [InlineData(MaxNumberOfItemsPerPage * 3, 1, MaxNumberOfItemsPerPage, 3)]
        [InlineData(MaxNumberOfItemsPerPage + 1, 2, 1, 2)]
        public void PaginationShouldResultInCorrectNumItemsOnPageAndCorrectNumPages(
            int totalNumItems,
            int requestedPageNumber,
            int expectedNumItemsOnPage,
            int expectedNumPages)
        {
            // Arrange
            var controller = SetUpController(totalNumItems);

            var search = new GroupSearch
            {
                Category = "category",
                Order = "a-z",
                Latitude = 0,
                Longitude = 0
            };

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Results(requestedPageNumber, MaxNumberOfItemsPerPage, search)) as ViewResult;

            // Assert
            var groupResult = actionResponse.ViewData.Model as GroupResults;

            groupResult.Groups.Count.Should().Be(expectedNumItemsOnPage);
            groupResult.Pagination.TotalPages.Should().Be(expectedNumPages);
        }

        [Theory]
        [InlineData(0, 50, 1)]
        [InlineData(5, MaxNumberOfItemsPerPage * 3, 3)]
        public void IfSpecifiedPageNumIsImpossibleThenActualPageNumWillBeAdjustedAccordingly(
            int specifiedPageNumber,
            int numItems,
            int expectedPageNumber)
        {
            // Arrange
            var controller = SetUpController(numItems);

            var search = new GroupSearch
            {
                Category = "",
                Order = "a-z",
                Latitude = 0,
                Longitude = 0
            };

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Results(specifiedPageNumber, MaxNumberOfItemsPerPage, search)) as ViewResult;

            var model = actionResponse.ViewData.Model as GroupResults;
            // Assert
            model.Pagination.CurrentPageNumber.Should().Be(expectedPageNumber);
        }

        [Fact]
        public void ShouldReturnEmptyPaginationObjectIfNoGroupsExist()
        {
            // Arrange
            const int zeroItems = 0;
            var controller = SetUpController(zeroItems);

            var search = new GroupSearch
            {
                Category = "",
                Order = "a-z",
                Latitude = 0,
                Longitude = 0
            };

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Results(0, MaxNumberOfItemsPerPage, search)) as ViewResult;

            var model = actionResponse.ViewData.Model as GroupResults;

            // Assert
            model.Pagination.Should().NotBeNull();
        }

        [Fact]
        public void ShouldReturnCurrentURLForPagination()
        {
            // Arrange
            int numItems = 10;
            var controller = SetUpController(numItems);

            var search = new GroupSearch
            {
                Category = "",
                Order = "a-z",
                Latitude = 0,
                Longitude = 0
            };

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Results(0, MaxNumberOfItemsPerPage, search)) as ViewResult;
            var model = actionResponse.ViewData.Model as GroupResults;

            // Assert
            model.Pagination.CurrentUrl.Should().NotBeNull();
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

            // Act
            var view = AsyncTestHelper.Resolve(_groupController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as GroupDetailsViewModel;

            // Assert
            model.Group.Events.FirstOrDefault().Should().Be(linkedEvent);
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
            var controller = new GroupsController(_processedRepository.Object, _repository.Object, _groupEmailBuilder.Object, _eventEmailBuilder.Object, _filteredUrl.Object, null, _logger.Object, _configuration.Object, markdownWrapper, viewHelper, datetimeCalculator, http.Object, null, htmlUtilities.Object, hostHelper, null);

            return controller;
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