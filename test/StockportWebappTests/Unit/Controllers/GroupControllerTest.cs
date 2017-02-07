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

namespace StockportWebappTests.Unit.Controllers
{
    public class GroupControllerTest
    {
        private readonly FakeProcessedContentRepository _fakeRepository;
        private readonly GroupController _groupController;


        public GroupControllerTest()
        {
            _fakeRepository = new FakeProcessedContentRepository();
            _groupController = new GroupController(_fakeRepository);
        }

        [Fact]
        public void ItReturnsAGroupWithProcessedBody()
        { 
            var processedGroup = new ProcessedGroup(Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, 
                Helper.AnyString, Helper.AnyString, Helper.AnyString);

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
    }

}