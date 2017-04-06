using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebappTests.Unit.Fake;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Helper = StockportWebappTests.TestHelper;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.FeatureToggling;
using StockportWebapp.ViewModels;

namespace StockportWebappTests.Unit.Controllers
{
    public class GroupControllerTest
    {
        private readonly FakeProcessedContentRepository _fakeRepository;
        private readonly GroupsController _groupController;
        private Mock<IRepository> _repository = new Mock<IRepository>();
        private Mock<IGroupRepository> _groupRepository;

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
            _groupController = new GroupsController(_fakeRepository, _repository.Object, _groupRepository.Object, new FeatureToggles() {GroupResultsPage = true, GroupStartPage = true});
            

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

            _fakeRepository.Set(new HttpResponse((int) HttpStatusCode.OK, processedGroup, string.Empty));

            var view = AsyncTestHelper.Resolve(_groupController.Detail("slug")) as ViewResult;
            var model = view.ViewData.Model as ProcessedGroup;

            model.Should().Be(processedGroup);
        }

        [Fact]
        public void GetsA404NotFoundGroup()
        {
            _fakeRepository.Set(new HttpResponse((int) HttpStatusCode.NotFound, null, string.Empty));

            var response = AsyncTestHelper.Resolve(_groupController.Detail("not-found-slug")) as HttpResponse;

            response.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
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
                Website  = "http://www.group.com",
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
    }

}