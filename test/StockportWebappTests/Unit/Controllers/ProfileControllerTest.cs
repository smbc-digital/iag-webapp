using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.ProcessedModels;
using System.Threading.Tasks;
using StockportWebapp.Repositories;
using Moq;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Services.Profile;
using StockportWebappTests_Unit.Helpers;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class ProfileControllerTest
    {
        private readonly Mock<IProcessedContentRepository> _fakeRepository = new Mock<IProcessedContentRepository>();
        private readonly Mock<IProfileService> _profileService = new Mock<IProfileService>();
        private readonly ProfileController _profileController;
        private readonly FeatureToggles _featureToggles;


        public ProfileControllerTest()
        {
            _featureToggles = new FeatureToggles
            {
                SemanticProfile = false
            };
            _profileController = new ProfileController(_fakeRepository.Object, _featureToggles, _profileService.Object);
        }

        [Fact]
        public async Task ItReturnsAProfileWithProcessedBody()
        {
            var processedProfile = new ProcessedProfile(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, new List<Crumb>(), new List<Alert>(), "", "");

            _fakeRepository
                .Setup(_ => _.Get<Profile>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedProfile, string.Empty));

            var view = await _profileController.Index("slug") as ViewResult; ;
            var model = view.ViewData.Model as ProcessedProfile;

            model.Should().Be(processedProfile);
        }

        [Fact]
        public async Task GetsA404NotFoundProfile()
        {
            _fakeRepository
                .Setup(_ => _.Get<Profile>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = await _profileController.Index("not-found-slug") as HttpResponse; ;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }

}