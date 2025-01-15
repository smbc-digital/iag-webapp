namespace StockportWebappTests_Unit.Unit.Services;

public class HealthcheckServiceTest
{
    private readonly HealthcheckService _healthcheckService;
    private readonly string _shaPath = "./Unit/sha.txt";
    private readonly string _appVersionPath = "./Unit/version.txt";
    private readonly Mock<IFileWrapper> _fileWrapperMock = new();
    private readonly Mock<IHttpClient> _mockHttpClient = new();
    private readonly Mock<IStubToUrlConverter> _mockUrlGenerator = new();
    private const string healthcheckUrl = "http://localhost:5000/_healthcheck";
    private readonly Mock<IApplicationConfiguration> _configuration = new();
    private readonly BusinessId _businessId;

    public HealthcheckServiceTest()
    {
        _businessId = new BusinessId("businessId");
        _mockUrlGenerator
            .Setup(urlGenerator => urlGenerator.HealthcheckUrl())
            .Returns(healthcheckUrl);

        _configuration
            .Setup(conf => conf.GetContentApiAuthenticationKey())
            .Returns("AuthKey");

        HttpResponse httpResponseMessage = new(200, "{\"appVersion\":\"dev\",\"sha\":\"test-sha\",\"environment\":\"local\",\"redisValueData\":[]}", "");

        _mockHttpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(httpResponseMessage);

        SetUpFakeFileSystem();
        _healthcheckService = CreateHealthcheckService(_appVersionPath, _shaPath);
    }

    private void SetUpFakeFileSystem()
    {
        _fileWrapperMock
            .Setup(x => x.Exists(_appVersionPath))
            .Returns(true);
        
        _fileWrapperMock
            .Setup(x => x.ReadAllLines(_appVersionPath))
            .Returns(["0.0.3"]);
        
        _fileWrapperMock
            .Setup(x => x.Exists(_shaPath))
            .Returns(true);
        
        _fileWrapperMock
            .Setup(x => x.ReadAllLines(_shaPath))
            .Returns(["d8213ee84c7d8c119c401b7ddd0adef923692188"]);
    }

    private HealthcheckService CreateHealthcheckService(string appVersionPath, string shaPath) =>
        new(appVersionPath, shaPath, _fileWrapperMock.Object, _mockHttpClient.Object, _mockUrlGenerator.Object, "local", _configuration.Object, _businessId);

    private HealthcheckService CreateHealthcheckServiceWithDefaultFeatureToggles(string appVersionPath, string shaPath) =>
        CreateHealthcheckService(appVersionPath, shaPath);

    [Fact]
    public async Task ShouldContainTheAppVersionInTheResponse()
    {
        // Act
        Healthcheck check = await _healthcheckService.Get();

        // Assert
        Assert.Equal("0.0.3", check.AppVersion);
    }

    [Fact]
    public async Task ShouldContainTheGitShaInTheResponse()
    {
        // Act
        Healthcheck check = await _healthcheckService.Get();

        // Assert
        Assert.Equal("d8213ee84c7d8c119c401b7ddd0adef923692188", check.SHA);
    }

    [Fact]
    public async Task ShouldSetAppVersionToDevIfFileNotFound()
    {
        // Arrange
        _fileWrapperMock
            .Setup(x => x.Exists("notfound"))
            .Returns(false);

        HealthcheckService healthCheckServiceWithNotFoundVersion = CreateHealthcheckServiceWithDefaultFeatureToggles("notfound", _shaPath);

        // Act
        Healthcheck check = await healthCheckServiceWithNotFoundVersion.Get();

        // Assert
        Assert.Equal("dev", check.AppVersion);
    }

    [Fact]
    public async Task ShouldSetAppVersionToDevIfFileEmpty()
    {
        // Arrange
        _fileWrapperMock
            .Setup(x => x.Exists("newFile"))
            .Returns(true);
        
        _fileWrapperMock
            .Setup(x => x.ReadAllLines("newFile"))
            .Returns([]);

        HealthcheckService healthCheckServiceWithNotFoundVersion = CreateHealthcheckServiceWithDefaultFeatureToggles("newFile", _shaPath);

        // Act
        Healthcheck check = await healthCheckServiceWithNotFoundVersion.Get();

        // Assert
        Assert.Equal("dev", check.AppVersion);
    }

    [Fact]
    public async Task ShouldSetAppVersionToDevIfFileHasAnEmptyAString()
    {
        // Arrange
        _fileWrapperMock
            .Setup(x => x.Exists("newFile"))
            .Returns(true);
        
        _fileWrapperMock
            .Setup(x => x.ReadAllLines("newFile"))
            .Returns([string.Empty]);

        HealthcheckService healthCheckServiceWithNotFoundVersion = CreateHealthcheckServiceWithDefaultFeatureToggles("newFile", _shaPath);
        
        // Act
        Healthcheck check = await healthCheckServiceWithNotFoundVersion.Get();

        // Assert
        Assert.Equal("dev", check.AppVersion);
    }

    [Fact]
    public async Task ShouldSetSHAToEmptyIfFileNotFound()
    {
        // Arrange
        _fileWrapperMock
            .Setup(x => x.Exists("notfound"))
            .Returns(false);

        HealthcheckService healthCheckServiceWithNotFoundVersion = CreateHealthcheckServiceWithDefaultFeatureToggles(_appVersionPath, "notfound");

        // Act
        Healthcheck check = await healthCheckServiceWithNotFoundVersion.Get();

        // Assert
        Assert.Empty(check.SHA);
    }

    [Fact]
    public async Task ShouldSetAppDependenciesGotFromTheContentApi()
    {
        // Act
        Healthcheck check = await _healthcheckService.Get();

        // Assert
        Healthcheck dependency = check.Dependencies["contentApi"];
        Assert.NotNull(check.Dependencies);
        Assert.Contains("contentApi", check.Dependencies.Keys);
        Assert.Contains("dev", dependency.AppVersion);
        Assert.Contains("test-sha", dependency.SHA);
    }

    [Fact]
    public async Task ShouldSetAppDependenciesToNullIfNoResponseGotFromContentApi()
    {
        // Arrange
        HttpResponse httpResponseMessage = new((int)HttpStatusCode.BadRequest, new StringContent(string.Empty), null);

        _mockHttpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(httpResponseMessage);

        HealthcheckService healthcheckService = new(_appVersionPath,
                                                    _shaPath,
                                                    _fileWrapperMock.Object,
                                                    _mockHttpClient.Object,
                                                    _mockUrlGenerator.Object,
                                                    "local",
                                                    _configuration.Object,
                                                    _businessId);

        // Act
        Healthcheck check = await healthcheckService.Get();

        // Assert
        Healthcheck dependency = check.Dependencies["contentApi"];
        Assert.NotNull(check.Dependencies);
        Assert.Contains("contentApi", check.Dependencies.Keys);
        Assert.Contains("Not available", dependency.AppVersion);
        Assert.Contains("Not available", dependency.SHA);
        Assert.Empty(dependency.Dependencies);
    }

    [Fact]
    public async Task ShouldSetAppDependenciesToNullIfRequestToContentApi()
    {
        // Arrange
        _mockHttpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(default(HttpResponse));

        HealthcheckService healthcheckService = new(_appVersionPath,
                                                    _shaPath,
                                                    _fileWrapperMock.Object,
                                                    _mockHttpClient.Object,
                                                    _mockUrlGenerator.Object,
                                                    "local",
                                                    _configuration.Object,
                                                    _businessId);

        // Act
        Healthcheck check = await healthcheckService.Get();

        // Assert
        Healthcheck dependency = check.Dependencies["contentApi"];
        Assert.NotNull(check.Dependencies);
        Assert.Contains("contentApi", check.Dependencies.Keys);
        Assert.Equal("Not available", dependency.AppVersion);
        Assert.Equal("Not available", dependency.SHA);
        Assert.Empty(dependency.Dependencies);
    }

    [Fact]
    public async Task ShouldContainTheBusinessIdInTheResponse()
    {
        // Act
        Healthcheck check = await _healthcheckService.Get();

        // Assert
        Assert.Equal("businessId", check.BusinessId);
    }
}