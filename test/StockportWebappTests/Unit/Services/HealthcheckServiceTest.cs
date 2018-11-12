using System;
using System.IO;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Moq;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using Xunit;
using StockportWebapp;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebappTests.Unit.Fake;
using HttpClient = System.Net.Http.HttpClient;

namespace StockportWebappTests.Unit.Services
{
    public class HealthcheckServiceTest : TestingBaseClass
    {
        private readonly HealthcheckService _healthcheckService;
        private readonly string _shaPath;
        private readonly string _appVersionPath;
        private readonly Mock<IFileWrapper> _fileWrapperMock;
        private readonly FakeResponseHandler _fakeHandler;
        private readonly Mock<IStubToUrlConverter> _mockUrlGenerator;
        private const string healthcheckUrl = "http://localhost:5000/_healthcheck";
        private readonly Mock<IApplicationConfiguration> _configuration;
        private readonly BusinessId _businessId;

        public HealthcheckServiceTest()
        {
            _appVersionPath = "./Unit/version.txt";
            _shaPath = "./Unit/sha.txt";
            _fileWrapperMock = new Mock<IFileWrapper>();
            _businessId = new BusinessId("businessId");
            _mockUrlGenerator = new Mock<IStubToUrlConverter>();
            _mockUrlGenerator.Setup(o => o.HealthcheckUrl()).Returns(healthcheckUrl);
            _configuration = new Mock<IApplicationConfiguration>();
            _configuration.Setup(_ => _.GetContentApiAuthenticationKey()).Returns("AuthKey");

            var httpResponseMessage = new HttpResponseMessage
            {
                Content = new StringContent(GetStringResponseFromFile("StockportWebappTests.Unit.MockResponses.Healthcheck.json"))
            };
            _fakeHandler = new FakeResponseHandler();
            _fakeHandler.AddFakeResponse(new Uri(healthcheckUrl), httpResponseMessage);

            SetUpFakeFileSystem();
            _healthcheckService = CreateHealthcheckService(_appVersionPath, _shaPath, new FeatureToggles());
        }

        private void SetUpFakeFileSystem()
        {
            _fileWrapperMock.Setup(x => x.Exists(_appVersionPath)).Returns(true);
            _fileWrapperMock.Setup(x => x.ReadAllLines(_appVersionPath)).Returns(new[] {"0.0.3"});
            _fileWrapperMock.Setup(x => x.Exists(_shaPath)).Returns(true);
            _fileWrapperMock.Setup(x => x.ReadAllLines(_shaPath))
                .Returns(new[] {"d8213ee84c7d8c119c401b7ddd0adef923692188"});
        }

        private HealthcheckService CreateHealthcheckService(string appVersionPath, string shaPath,
            FeatureToggles featureToggles)
        {
            return new HealthcheckService(appVersionPath, shaPath, _fileWrapperMock.Object, featureToggles, new HttpClient(_fakeHandler), _mockUrlGenerator.Object, "local", _configuration.Object, _businessId);
        }

        private HealthcheckService CreateHealthcheckServiceWithDefaultFeatureToggles(string appVersionPath,
            string shaPath)
        {
            return CreateHealthcheckService(appVersionPath, shaPath, new FeatureToggles());
        }

        [Fact]
        public void ShouldContainTheAppVersionInTheResponse()
        {
            var check = AsyncTestHelper.Resolve(_healthcheckService.Get());

            check.AppVersion.Should().Be("0.0.3");
        }

        [Fact]
        public void ShouldContainTheGitShaInTheResponse()
        {
            var check = AsyncTestHelper.Resolve(_healthcheckService.Get());

            check.SHA.Should().Be("d8213ee84c7d8c119c401b7ddd0adef923692188");
        }

        [Fact]
        public void ShouldSetAppVersionToDevIfFileNotFound()
        {
            var notFoundVersionPath = "notfound";
            _fileWrapperMock.Setup(x => x.Exists(notFoundVersionPath)).Returns(false);

            var healthCheckServiceWithNotFoundVersion =
                CreateHealthcheckServiceWithDefaultFeatureToggles(notFoundVersionPath, _shaPath);
            var check = AsyncTestHelper.Resolve(healthCheckServiceWithNotFoundVersion.Get());

            check.AppVersion.Should().Be("dev");
        }

        [Fact]
        public void ShouldSetAppVersionToDevIfFileEmpty()
        {
            string newFile = "newFile";
            _fileWrapperMock.Setup(x => x.Exists(newFile)).Returns(true);
            _fileWrapperMock.Setup(x => x.ReadAllLines(newFile)).Returns(new string[] { });

            var healthCheckServiceWithNotFoundVersion = CreateHealthcheckServiceWithDefaultFeatureToggles(newFile,
                _shaPath);
            var check = AsyncTestHelper.Resolve(healthCheckServiceWithNotFoundVersion.Get());

            check.AppVersion.Should().Be("dev");
        }

        [Fact]
        public void ShouldSetAppVersionToDevIfFileHasAnEmptyAString()
        {
            string newFile = "newFile";
            _fileWrapperMock.Setup(x => x.Exists(newFile)).Returns(true);
            _fileWrapperMock.Setup(x => x.ReadAllLines(newFile)).Returns(new[] {""});

            var healthCheckServiceWithNotFoundVersion = CreateHealthcheckServiceWithDefaultFeatureToggles(newFile,
                _shaPath);
            var check = AsyncTestHelper.Resolve(healthCheckServiceWithNotFoundVersion.Get());

            check.AppVersion.Should().Be("dev");
        }

        [Fact]
        public void ShouldSetSHAToEmptyIfFileNotFound()
        {
            var notFoundShaPath = "notfound";
            _fileWrapperMock.Setup(x => x.Exists(notFoundShaPath)).Returns(false);

            var healthCheckServiceWithNotFoundVersion =
                CreateHealthcheckServiceWithDefaultFeatureToggles(_appVersionPath, notFoundShaPath);
            var check = AsyncTestHelper.Resolve(healthCheckServiceWithNotFoundVersion.Get());

            check.SHA.Should().Be("");
        }

        [Fact]
        public void ShouldIncludeFeatureTogglesInHealthcheck()
        {
            var featureToggles = new FeatureToggles();

            var healthCheckServiceWithNotFoundVersion = CreateHealthcheckService(_appVersionPath, _shaPath,
                featureToggles);
            var check = AsyncTestHelper.Resolve(healthCheckServiceWithNotFoundVersion.Get());

            check.FeatureToggles.Should().NotBeNull();
            check.FeatureToggles.Should().BeEquivalentTo(featureToggles);
        }

        [Fact]
        public void ShouldSetAppDependenciesGotFromTheContentApi()
        {
            var check = AsyncTestHelper.Resolve(_healthcheckService.Get());

            check.Dependencies.Should().NotBeNull();
            check.Dependencies.Should().ContainKey("contentApi");
            var dependency = check.Dependencies["contentApi"];
            dependency.AppVersion.Should().Be("dev");
            dependency.SHA.Should().Be("test-sha");
            dependency.FeatureToggles.Should().BeNull();
        }

        [Fact]
        public void ShouldSetAppDependenciesToNullIfNoResponseGotFromContentApi()
        {
            var NoneSuccessfulResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var fakeHandler = new FakeResponseHandler();
            fakeHandler.AddFakeResponse(new Uri(healthcheckUrl), NoneSuccessfulResponse);

            var healthcheckService = new HealthcheckService(_appVersionPath, _shaPath, _fileWrapperMock.Object,
                new FeatureToggles(), new HttpClient(fakeHandler), _mockUrlGenerator.Object, "local", _configuration.Object, _businessId);

            var check = AsyncTestHelper.Resolve(healthcheckService.Get());

            check.Dependencies.Should().NotBeNull();
            check.Dependencies.Should().ContainKey("contentApi");
            var dependency = check.Dependencies["contentApi"];
            dependency.AppVersion.Should().Be("Not available");
            dependency.SHA.Should().Be("Not available");
            dependency.FeatureToggles.Should().BeNull();
            dependency.Dependencies.Should().BeEmpty();
        }

        [Fact]
        public void ShouldSetAppDependenciesToNullIfRequestToContentApi()
        {
            var fakeHandler = new FakeResponseHandler();
            fakeHandler.ThrowException(new Uri(healthcheckUrl), new HttpRequestException());

            var healthcheckService = new HealthcheckService(_appVersionPath, _shaPath, _fileWrapperMock.Object,
                new FeatureToggles(), new HttpClient(fakeHandler), _mockUrlGenerator.Object, "local", _configuration.Object, _businessId);

            var check = AsyncTestHelper.Resolve(healthcheckService.Get());

            check.Dependencies.Should().NotBeNull();
            check.Dependencies.Should().ContainKey("contentApi");
            var dependency = check.Dependencies["contentApi"];
            dependency.AppVersion.Should().Be("Not available");
            dependency.SHA.Should().Be("Not available");
            dependency.FeatureToggles.Should().BeNull();
            dependency.Dependencies.Should().BeEmpty();
        }

        [Fact]
        public void ShouldContainTheBusinessIdInTheResponse()
        {
            var check = AsyncTestHelper.Resolve(_healthcheckService.Get());

            check.BusinessId.Should().Be("businessId");
        }
    }
}