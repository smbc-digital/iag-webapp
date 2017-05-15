using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebappTests.Unit.Fake;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.ProcessedModels;
using Helper = StockportWebappTests.TestHelper;

namespace StockportWebappTests.Unit.Controllers
{
    public class ProfileControllerTest
    {
        private readonly FakeProcessedContentRepository _fakeRepository;
        private readonly ProfileController _profileController;


        public ProfileControllerTest()
        {
            _fakeRepository = new FakeProcessedContentRepository();
            _profileController = new ProfileController(_fakeRepository);
        }

        [Fact]
        public void ItReturnsAProfileWithProcessedBody()
        { 
            var processedProfile = new ProcessedProfile(Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                Helper.AnyString, new List<Crumb>());

            _fakeRepository.Set(new HttpResponse((int) HttpStatusCode.OK, processedProfile, string.Empty));

            var view = AsyncTestHelper.Resolve(_profileController.Index("slug")) as ViewResult;
            var model = view.ViewData.Model as ProcessedProfile;

            model.Should().Be(processedProfile);
        }

        [Fact]
        public void GetsA404NotFoundProfile()
        {
            _fakeRepository.Set(new HttpResponse((int) HttpStatusCode.NotFound, null, string.Empty));

            var response = AsyncTestHelper.Resolve(_profileController.Index("not-found-slug")) as HttpResponse;

            response.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
        }
    }

}