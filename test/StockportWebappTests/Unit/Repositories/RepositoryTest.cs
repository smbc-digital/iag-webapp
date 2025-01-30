namespace StockportWebappTests_Unit.Unit.Repositories;

public class RepositoryTest
{
    private readonly Repository _repository;
    private readonly Mock<IHttpClient> _httpClientMock = new();
    private readonly UrlGenerator _urlGenerator;
    private readonly Mock<IUrlGeneratorSimple> _urlGeneratorSimple = new();
    private readonly Mock<ILogger<Repository>> _mockLogger = new();

    private readonly PrivacyNotice _privacyNoticeModel = new()
    {
        OutsideEu = false,
        AutomatedDecision = false,
        Breadcrumbs = new List<Crumb>()
    };

    public RepositoryTest()
    {
        Mock<IApplicationConfiguration> appConfig = new();

        appConfig
            .Setup(config => config.GetContentApiUri())
            .Returns(new Uri("http://localhost:5000/"));

        _urlGenerator = new(appConfig.Object, new BusinessId(string.Empty));

        _urlGeneratorSimple
            .Setup(generator => generator.BaseContentApiUrl<Event>())
            .Returns("url");

        _urlGeneratorSimple
            .Setup(generator => generator.BaseContentApiUrl<PrivacyNotice>())
            .Returns("url");

        _httpClientMock
            .Setup(httpClient => httpClient.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, null, string.Empty));

        _httpClientMock
            .Setup(httpClient => httpClient.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, JsonConvert.SerializeObject(_privacyNoticeModel), string.Empty));

        _httpClientMock
            .Setup(httpClient => httpClient.DeleteAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, null, string.Empty));

        _httpClientMock
            .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, null, string.Empty));

        _repository = new(_urlGenerator, _httpClientMock.Object, appConfig.Object, _urlGeneratorSimple.Object);
    }

    [Fact]
    public async Task Get_PrivacyNotice_ShouldCall_HttpClient()
    {
        // Act
        await _repository.Get<PrivacyNotice>();

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task Put_ShouldCallPutAsyncWithHeaders()
    {
        // Act
        await _repository.Put<Group>(new StringContent("url", Encoding.UTF8, "application/json"));

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()), Times.Once());
    }

    [Fact]
    public async Task Delete_ShouldCall_HttpClient()
    {
        // Act
        await _repository.Delete<PrivacyNotice>();

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.DeleteAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task Archive_ShouldCallPutAsyncWithHeaders()
    {
        // Act
        await _repository.Archive<Group>(new StringContent("url", Encoding.UTF8, "application/json"));

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()), Times.Once());
    }

    [Fact]
    public async Task Publish_ShouldCallPutAsyncWithHeaders()
    {
        // Act
        await _repository.Publish<Group>(new StringContent("url", Encoding.UTF8, "application/json"));

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()), Times.Once());
    }

    [Fact]
    public async Task GetLatest_PrivacyNotice_ShouldCall_HttpClient()
    {
        // Act
        await _repository.GetLatest<PrivacyNotice>(1);

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task GetLatestOrderByFeatured_PrivacyNotice_ShouldCall_HttpClient()
    {
        // Act
        await _repository.GetLatestOrderByFeatured<PrivacyNotice>(1);

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task GetRedirects_ShouldCall_HttpClient()
    {
        // Act
        await _repository.GetRedirects();

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task GetAdministratorGroups_ShouldCall_HttpClient()
    {
        // Arrange
        List<Group> listOfGroups = new();

        _httpClientMock
            .Setup(httpClient => httpClient.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, JsonConvert.SerializeObject(listOfGroups), string.Empty));

        // Act
        await _repository.GetAdministratorsGroups("email");

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task RemoveAdministrator()
    {
        // Act
        await _repository.RemoveAdministrator("test_slug", "test@test.com");

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.DeleteAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task AddAdministrator()
    {
        // Act
        await _repository.AddAdministrator(new StringContent("content", Encoding.UTF8, "application/json"), "test_slug", "test@test.com");

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()), Times.Once());
    }

    [Fact]
    public async Task UpdateAdministrator()
    {
        // Act
        await _repository.UpdateAdministrator(new StringContent("content", Encoding.UTF8, "application/json"), "test_slug", "test@test.com");

        // Assert
        _httpClientMock.Verify(httpClient => httpClient.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()), Times.Once());
    }
}