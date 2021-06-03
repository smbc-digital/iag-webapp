using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Services
{
    public class HealthcheckServiceTest
    {
        private readonly HealthcheckService _healthcheckService;
        private readonly string _shaPath;
        private readonly string _appVersionPath;
        private readonly Mock<IFileWrapper> _fileWrapperMock;
        private readonly Mock<IHttpClient> _mockHttpClient = new Mock<IHttpClient>();
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
            
            var httpResponseMessage = new HttpResponse(200, "{\"appVersion\":\"dev\",\"sha\":\"test-sha\",\"environment\":\"local\",\"redisValueData\":[]}", "");

            _mockHttpClient
                .Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(httpResponseMessage);

            SetUpFakeFileSystem();
            _healthcheckService = CreateHealthcheckService(_appVersionPath, _shaPath, new FeatureToggles());
        }

        private void SetUpFakeFileSystem()
        {
            _fileWrapperMock.Setup(x => x.Exists(_appVersionPath)).Returns(true);
            _fileWrapperMock.Setup(x => x.ReadAllLines(_appVersionPath)).Returns(new[] { "0.0.3" });
            _fileWrapperMock.Setup(x => x.Exists(_shaPath)).Returns(true);
            _fileWrapperMock.Setup(x => x.ReadAllLines(_shaPath))
                .Returns(new[] { "d8213ee84c7d8c119c401b7ddd0adef923692188" });
        }

        private HealthcheckService CreateHealthcheckService(string appVersionPath, string shaPath,
            FeatureToggles featureToggles)
        {
            return new HealthcheckService(appVersionPath, shaPath, _fileWrapperMock.Object, featureToggles, _mockHttpClient.Object, _mockUrlGenerator.Object, "local", _configuration.Object, _businessId);
        }

        private HealthcheckService CreateHealthcheckServiceWithDefaultFeatureToggles(string appVersionPath,
            string shaPath)
        {
            return CreateHealthcheckService(appVersionPath, shaPath, new FeatureToggles());
        }

        [Fact]
        public async Task ShouldContainTheAppVersionInTheResponse()
        {
            var check = await _healthcheckService.Get();

            check.AppVersion.Should().Be("0.0.3");
        }

        [Fact]
        public async Task ShouldContainTheGitShaInTheResponse()
        {
            var check = await _healthcheckService.Get();

            check.SHA.Should().Be("d8213ee84c7d8c119c401b7ddd0adef923692188");
        }

        [Fact]
        public async Task ShouldSetAppVersionToDevIfFileNotFound()
        {
            var notFoundVersionPath = "notfound";
            _fileWrapperMock.Setup(x => x.Exists(notFoundVersionPath)).Returns(false);

            var healthCheckServiceWithNotFoundVersion =
                CreateHealthcheckServiceWithDefaultFeatureToggles(notFoundVersionPath, _shaPath);
            var check = await healthCheckServiceWithNotFoundVersion.Get();

            check.AppVersion.Should().Be("dev");
        }

        [Fact]
        public async Task ShouldSetAppVersionToDevIfFileEmpty()
        {
            string newFile = "newFile";
            _fileWrapperMock.Setup(x => x.Exists(newFile)).Returns(true);
            _fileWrapperMock.Setup(x => x.ReadAllLines(newFile)).Returns(new string[] { });

            var healthCheckServiceWithNotFoundVersion = CreateHealthcheckServiceWithDefaultFeatureToggles(newFile,
                _shaPath);
            var check = await healthCheckServiceWithNotFoundVersion.Get();

            check.AppVersion.Should().Be("dev");
        }

        [Fact]
        public async Task ShouldSetAppVersionToDevIfFileHasAnEmptyAString()
        {
            string newFile = "newFile";
            _fileWrapperMock.Setup(x => x.Exists(newFile)).Returns(true);
            _fileWrapperMock.Setup(x => x.ReadAllLines(newFile)).Returns(new[] { "" });

            var healthCheckServiceWithNotFoundVersion = CreateHealthcheckServiceWithDefaultFeatureToggles(newFile,
                _shaPath);
            var check = await healthCheckServiceWithNotFoundVersion.Get();

            check.AppVersion.Should().Be("dev");
        }

        [Fact]
        public async Task ShouldSetSHAToEmptyIfFileNotFound()
        {
            var notFoundShaPath = "notfound";
            _fileWrapperMock.Setup(x => x.Exists(notFoundShaPath)).Returns(false);

            var healthCheckServiceWithNotFoundVersion =
                CreateHealthcheckServiceWithDefaultFeatureToggles(_appVersionPath, notFoundShaPath);
            var check = await healthCheckServiceWithNotFoundVersion.Get();

            check.SHA.Should().Be("");
        }

        [Fact]
        public async Task ShouldIncludeFeatureTogglesInHealthcheck()
        {
            var featureToggles = new FeatureToggles();

            var healthCheckServiceWithNotFoundVersion = CreateHealthcheckService(_appVersionPath, _shaPath,
                featureToggles);
            var check = await healthCheckServiceWithNotFoundVersion.Get();

            check.FeatureToggles.Should().NotBeNull();
            check.FeatureToggles.Should().BeEquivalentTo(featureToggles);
        }

        [Fact]
        public async Task ShouldSetAppDependenciesGotFromTheContentApi()
        {
            var check = await _healthcheckService.Get();

            check.Dependencies.Should().NotBeNull();
            check.Dependencies.Should().ContainKey("contentApi");
            var dependency = check.Dependencies["contentApi"];
            dependency.AppVersion.Should().Be("dev");
            dependency.SHA.Should().Be("test-sha");
            dependency.FeatureToggles.Should().BeNull();
        }

        [Fact]
        public async Task ShouldSetAppDependenciesToNullIfNoResponseGotFromContentApi()
        {
            var httpResponseMessage = new HttpResponse((int)HttpStatusCode.BadRequest, new StringContent(""), null);

            _mockHttpClient
                .Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(httpResponseMessage);

            var healthcheckService = new HealthcheckService(_appVersionPath, _shaPath, _fileWrapperMock.Object,
                new FeatureToggles(), _mockHttpClient.Object, _mockUrlGenerator.Object, "local", _configuration.Object, _businessId);

            var check = await healthcheckService.Get();

            check.Dependencies.Should().NotBeNull();
            check.Dependencies.Should().ContainKey("contentApi");
            var dependency = check.Dependencies["contentApi"];
            dependency.AppVersion.Should().Be("Not available");
            dependency.SHA.Should().Be("Not available");
            dependency.FeatureToggles.Should().BeNull();
            dependency.Dependencies.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldSetAppDependenciesToNullIfRequestToContentApi()
        {
            _mockHttpClient
                .Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(default(HttpResponse));

            var healthcheckService = new HealthcheckService(_appVersionPath, _shaPath, _fileWrapperMock.Object,
                new FeatureToggles(), _mockHttpClient.Object, _mockUrlGenerator.Object, "local", _configuration.Object, _businessId);

            var check = await healthcheckService.Get();

            check.Dependencies.Should().NotBeNull();
            check.Dependencies.Should().ContainKey("contentApi");
            var dependency = check.Dependencies["contentApi"];
            dependency.AppVersion.Should().Be("Not available");
            dependency.SHA.Should().Be("Not available");
            dependency.FeatureToggles.Should().BeNull();
            dependency.Dependencies.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldContainTheBusinessIdInTheResponse()
        {
            var check = await _healthcheckService.Get();

            check.BusinessId.Should().Be("businessId");
        }
    }
}