using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Markdig.Helpers;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebappTests.Unit.Fake;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Helper = StockportWebappTests.TestHelper;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using StockportWebapp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using StockportWebapp.AmazonSES;
using Newtonsoft.Json;

namespace StockportWebappTests.Unit.Controllers
{
    public class GroupControllerTest
    {
        private readonly FakeProcessedContentRepository _fakeRepository;
        private readonly GroupsController _groupController;
        private Mock<IRepository> _repository = new Mock<IRepository>();
        private Mock<GroupEmailBuilder> _groupEmailBuilder;
        public const int MaxNumberOfItemsPerPage = 9;
        private readonly Mock<IFilteredUrl> _filteredUrl;
        private MapPosition _location = new MapPosition() { Lat = 1, Lon = 1 };
        private FeatureToggles _featureToggle;
        private Mock<ILogger<GroupsController>> _logger;
        private Mock<IApplicationConfiguration> _configuration = new Mock<IApplicationConfiguration>();
        private MarkdownWrapper markdownWrapper = new MarkdownWrapper();


        private readonly List<GroupCategory> groupCategories = new List<GroupCategory>
        {
            new GroupCategory() {Name = "name 1", Slug = "name1", Icon = "icon1", ImageUrl = "imageUrl1"},
            new GroupCategory() {Name = "name 2", Slug = "name2", Icon = "icon2", ImageUrl = "imageUrl2"},
            new GroupCategory() {Name = "name 3", Slug = "name3", Icon = "icon3", ImageUrl = "imageUrl3"},
        };

        public GroupControllerTest()
        {
            _fakeRepository = new FakeProcessedContentRepository();
            _filteredUrl = new Mock<IFilteredUrl>();
            _featureToggle = new FeatureToggles() {GroupManagement = true};
            _logger = new Mock<ILogger<GroupsController>>();

            var emailLogger = new Mock<ILogger<GroupEmailBuilder>>();
            var emailClient = new Mock<IHttpEmailClient>();
            var emailConfig = new Mock<IApplicationConfiguration>();

            emailConfig.Setup(a => a.GetEmailEmailFrom(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));

            _groupEmailBuilder = new Mock<GroupEmailBuilder>(emailLogger.Object, emailClient.Object, emailConfig.Object, new BusinessId("BusinessId"));

            var mockTime = new Mock<ITimeProvider>();
            var viewHelper = new ViewHelpers(mockTime.Object);

            _groupController = new GroupsController(_fakeRepository, _repository.Object, _groupEmailBuilder.Object, _filteredUrl.Object, _featureToggle, null, _logger.Object, _configuration.Object, markdownWrapper, viewHelper);

            // setup mocks
            _repository.Setup(o => o.Get<List<GroupCategory>>("", null))
                .ReturnsAsync(HttpResponse.Successful(200, groupCategories));

        }

        [Fact]
        public void ItReturnsAGroupWithProcessedBody()
        {
            var processedGroup = new ProcessedGroup(Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, 
                Helper.AnyString, Helper.AnyString, Helper.AnyString, null, null, null, false, null, null, DateTime.MinValue, DateTime.MinValue);

            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));

            var view = AsyncTestHelper.Resolve(_groupController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as ProcessedGroup;

            model.Should().Be(processedGroup);
        }

        [Fact]
        public void GetsA404NotFoundGroup()
        {
            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = AsyncTestHelper.Resolve(_groupController.Detail("not-found-slug")) as HttpResponse;

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
            var slug = "deleteSlug";
            var loggedInPerson = new LoggedInPerson { Name = "name", Email = "email@email.com" };
            var processedGroup = new ProcessedGroup(Helper.AnyString, Helper.AnyString, Helper.AnyString,
               Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
               Helper.AnyString, Helper.AnyString, Helper.AnyString, null, null, null, false, null, new GroupAdministrators { Items = new List<GroupAdministratorItems> { new GroupAdministratorItems { Email = "email@email.com", Permission = "A" } } }, DateTime.MinValue, DateTime.MaxValue);
            _repository.Setup(r => r.Delete<Group>(slug))
                .ReturnsAsync(new HttpResponse((int) HttpStatusCode.OK, processedGroup, string.Empty));
            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));
            var actionResponse = AsyncTestHelper.Resolve(_groupController.DeleteGroup(slug, loggedInPerson)) as RedirectToActionResult;
            actionResponse.ActionName.Should().Be("DeleteConfirmation");
        }

        [Fact]
        public void ItShouldGetARedirectResultForArchive()
        {
            var slug = "archiveSlug";
            var loggedInPerson = new LoggedInPerson { Name = "name", Email = "email@email.com" };
            var processedGroup = new ProcessedGroup(Helper.AnyString, Helper.AnyString, Helper.AnyString,
               Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
               Helper.AnyString, Helper.AnyString, Helper.AnyString, null, null, null, false, null, new GroupAdministrators { Items =  new List<GroupAdministratorItems> { new GroupAdministratorItems { Email = "email@email.com", Permission = "A"} } }, DateTime.MinValue, DateTime.MinValue);
            _repository.Setup(r => r.Archive<Group>(It.IsAny<HttpContent>(), slug))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));
            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));
            var actionResponse = AsyncTestHelper.Resolve(_groupController.ArchiveGroup(slug, loggedInPerson)) as RedirectToActionResult;
            actionResponse.ActionName.Should().Be("ArchiveConfirmation");
        }

        [Fact]
        public void ShouldReturnEmptyPaginationForNoGroups()
        {
            var emptyRepository = new Mock<IRepository>();

            var _emptyGroupResults = new GroupResults() {Groups = new List<Group>()};

            emptyRepository.Setup(o => o.Get<GroupResults>(It.IsAny<string>(), It.IsAny<List<Query>>()))
              .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _emptyGroupResults));

            _featureToggle = new FeatureToggles();
            var mockTime = new Mock<ITimeProvider>();
            var viewHelper = new ViewHelpers(mockTime.Object);
            var controller = new GroupsController(_fakeRepository, emptyRepository.Object, _groupEmailBuilder.Object, _filteredUrl.Object, _featureToggle, null, _logger.Object, _configuration.Object, markdownWrapper, viewHelper);

            var actionResponse =
               AsyncTestHelper.Resolve(
                   controller.Results("nonsense", 1, 0 , 0, "a-z")) as ViewResult;

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

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Results("category", requestedPageNumber, 0, 0, "a-z")) as ViewResult;

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

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Results("", specifiedPageNumber, 0, 0, "a-z")) as ViewResult;

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

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Results("", 0, 0, 0, "a-z")) as ViewResult;

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

            // Act
            var actionResponse = AsyncTestHelper.Resolve(controller.Results("", 0, 0, 0, "a-z")) as ViewResult;
            var model = actionResponse.ViewData.Model as GroupResults;

            // Assert
            model.Pagination.CurrentUrl.Should().NotBeNull();
        }

        [Fact]
        public void ShouldReturnLocationIfOneIsSelected()
        {
            var location = new MapPosition() { Lat = 1, Lon = 1 };

            var processedGroup = new ProcessedGroup(Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, null, null, location, false, null, null, DateTime.MinValue, DateTime.MinValue);

            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));

            var view = AsyncTestHelper.Resolve(_groupController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as ProcessedGroup;

            model.MapPosition.Should().Be(location);
        }

        [Fact]
        public void ShouldReturnAListOfLinkedEvents()
        {
            var location = new MapPosition() { Lat = 1, Lon = 1 };
            var linkedEvent = new Event() {Slug = "event-slug"};
            var listOfLinkedEvents = new List<Event> { linkedEvent };
 
            var processedGroup = new ProcessedGroup(Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, null, null, location, false, listOfLinkedEvents, null, DateTime.MinValue, DateTime.MinValue);

            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.OK, processedGroup, string.Empty));

            var view = AsyncTestHelper.Resolve(_groupController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as ProcessedGroup;

            model.Events.FirstOrDefault().Should().Be(linkedEvent);
        }


        private GroupsController SetUpController(int numGroups)
        {
            var listOfGroups = BuildGroupList(numGroups);

            var bigGroupResults = new GroupResults {Groups = listOfGroups};

            _repository.Setup(o =>
                o.Get<GroupResults>(
                    It.IsAny<string>(),
                    It.IsAny<List<Query>>()))
                .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, bigGroupResults));

            _featureToggle = new FeatureToggles();
            var mockTime = new Mock<ITimeProvider>();
            var viewHelper = new ViewHelpers(mockTime.Object);
            var controller = new GroupsController(_fakeRepository, _repository.Object, _groupEmailBuilder.Object, _filteredUrl.Object, _featureToggle, null, _logger.Object, _configuration.Object, markdownWrapper, viewHelper);

            return controller;
        }

        private List<Group> BuildGroupList(int numberOfItems)
        {
            var listOfGroups = new List<Group>();

            for (var i = 0; i < numberOfItems; i++)
            {
                var group = new Group("name", "slug" + i, "phoneNumber", "email", "website", "twitter", "facebook",
                    "address", "description", "imageUrl", "thumbnailImageUrl",
                    new List<GroupCategory>()
                    {
                        new GroupCategory() {Icon = "icon", ImageUrl = "imageUrl", Slug = "slug" + (i + 100)}
                    }, new List<Crumb>(), _location, false, null, new GroupAdministrators(), DateTime.MinValue, DateTime.MinValue, "published");

                listOfGroups.Add(group);
            }

            return listOfGroups;
        }
    }

}