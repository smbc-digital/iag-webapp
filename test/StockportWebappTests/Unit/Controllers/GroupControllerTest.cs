using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Markdig.Helpers;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebappTests.Unit.Fake;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Helper = StockportWebappTests.TestHelper;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Utils;
using StockportWebapp.ViewModels;

namespace StockportWebappTests.Unit.Controllers
{
    public class GroupControllerTest
    {
        private readonly FakeProcessedContentRepository _fakeRepository;
        private readonly GroupsController _groupController;
        private Mock<IRepository> _repository = new Mock<IRepository>();
        private Mock<IGroupRepository> _groupRepository;
        public const int MaxNumberOfItemsPerPage = 9;
        private readonly Mock<IFilteredUrl> _filteredUrl;

        private readonly List<GroupCategory> groupCategories = new List<GroupCategory>
        {
            new GroupCategory() {Name = "name 1", Slug = "name1", Icon = "icon1", ImageUrl = "imageUrl1"},
            new GroupCategory() {Name = "name 2", Slug = "name2", Icon = "icon2", ImageUrl = "imageUrl2"},
            new GroupCategory() {Name = "name 3", Slug = "name3", Icon = "icon3", ImageUrl = "imageUrl3"},
        };

        public GroupControllerTest()
        {
            _fakeRepository = new FakeProcessedContentRepository();
            _groupRepository = new Mock<IGroupRepository>();
            _filteredUrl = new Mock<IFilteredUrl>();
            _groupController = new GroupsController(_fakeRepository, _repository.Object, _groupRepository.Object, new FeatureToggles() { GroupResultsPage = true, GroupStartPage = true },_filteredUrl.Object);


            // setup mocks
            _repository.Setup(o => o.Get<List<GroupCategory>>("", null))
                .ReturnsAsync(HttpResponse.Successful(200, groupCategories));

        }

        [Fact]
        public void ItReturnsAGroupWithProcessedBody()
        {
            var processedGroup = new ProcessedGroup(Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, null);

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
                Category1 = "Category",
                Image = null
            };
            _groupRepository.Setup(o => o.SendEmailMessage(It.IsAny<GroupSubmission>())).ReturnsAsync(HttpStatusCode.OK);

            var actionResponse = AsyncTestHelper.Resolve(_groupController.AddAGroup(groupSubmission)) as RedirectToActionResult;
            actionResponse.ActionName.Should().Be("ThankYouMessage");
        }

        [Fact]
        public void ItShouldReturnBackToTheViewForAnInvalidEmailSubmission()
        {
            var groupSubmission = new GroupSubmission();

            _groupRepository.Setup(o => o.SendEmailMessage(It.IsAny<GroupSubmission>())).ReturnsAsync(HttpStatusCode.BadRequest);

            var actionResponse = AsyncTestHelper.Resolve(_groupController.AddAGroup(groupSubmission));

            actionResponse.Should().BeOfType<ViewResult>();
            _groupRepository.Verify(o => o.SendEmailMessage(groupSubmission), Times.Once);
        }

        [Fact]
        public void ItShouldNotSendAnEmailForAnInvalidFormSumbission()
        {
            var groupSubmission = new GroupSubmission();

            _groupController.ModelState.AddModelError("Name", "an invalid name was provided");

            var actionResponse = AsyncTestHelper.Resolve(_groupController.AddAGroup(groupSubmission));

            actionResponse.Should().BeOfType<ViewResult>();
            _groupRepository.Verify(o => o.SendEmailMessage(groupSubmission), Times.Never);
        }

        [Fact]
        public void ShouldReturnEmptyPaginationForNoGroups()
        {
            var emptyRepository = new Mock<IRepository>();

            var _emptyGroupResults = new GroupResults() {Groups = new List<Group>()};

            emptyRepository.Setup(o => o.Get<GroupResults>(It.IsAny<string>(), It.IsAny<List<Query>>()))
              .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _emptyGroupResults));

            var controller = new GroupsController(_fakeRepository, emptyRepository.Object, _groupRepository.Object,
                new FeatureToggles() {GroupResultsPage = true, GroupStartPage = true}, _filteredUrl.Object);

            var actionResponse =
               AsyncTestHelper.Resolve(
                   controller.Results("nonsense", 1)) as ViewResult;

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
            var actionResponse = AsyncTestHelper.Resolve(controller.Results("category", requestedPageNumber)) as ViewResult;

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
            var actionResponse = AsyncTestHelper.Resolve(controller.Results("", specifiedPageNumber)) as ViewResult;

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
            var actionResponse = AsyncTestHelper.Resolve(controller.Results("", 0)) as ViewResult;

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
            var actionResponse = AsyncTestHelper.Resolve(controller.Results("", 0)) as ViewResult;
            var model = actionResponse.ViewData.Model as GroupResults;

            // Assert
            model.Pagination.CurrentUrl.Should().NotBeNull();
        }

        private GroupsController SetUpController(int numGroups)
        {
            List<Group> listOfGroups = BuildGroupList(numGroups);

            var bigGroupResults = new GroupResults();
            bigGroupResults.Groups = listOfGroups;
            _repository.Setup(o =>
                o.Get<GroupResults>(
                    It.IsAny<string>(),
                    It.IsAny<List<Query>>()))
                .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, bigGroupResults));

            var controller = new GroupsController(_fakeRepository, _repository.Object, _groupRepository.Object, new FeatureToggles() { GroupResultsPage = true, GroupStartPage = true }, _filteredUrl.Object);

            return controller;
        }

        private List<Group> BuildGroupList(int numberOfItems)
        {
            List<Group> listOfGroups = new List<Group>();

            for (int i = 0; i < numberOfItems; i++)
            {
                var group = new Group("name", "slug" + i, "phoneNumber", "email", "website", "twitter", "facebook",
                    "address", "description", "imageUrl", "thumbnailImageUrl",
                    new List<GroupCategory>()
                    {
                        new GroupCategory() {Icon = "icon", ImageUrl = "imageUrl", Slug = "slug" + (i + 100)}
                    });

                listOfGroups.Add(group);
            }

            return listOfGroups;
        }
    }

}