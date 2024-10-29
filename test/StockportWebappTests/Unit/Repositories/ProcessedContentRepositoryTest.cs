namespace StockportWebappTests_unit.unit.repositories;

public class ProcessedContentRepositoryTest
{
    private readonly ProcessedContentRepository _processedContentRepository;
    private readonly ContentTypeFactory _contentTypeFactory;
    private readonly Mock<IHttpClient> _mockHttpClient = new();
    private readonly Mock<IStubToUrlConverter> _mockUrlGenerator = new();
    private readonly Mock<IApplicationConfiguration> _appConfig = new();

    private readonly Mock<ITagParserContainer> _mockTagParserContainer = new();
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor = new();
    private readonly Mock<IRepository> _mockRepository = new();

    private readonly PrivacyNotice _privacyNoticeModel = new()
    {
        OutsideEu = false,
        AutomatedDecision = false,
        Breadcrumbs = new List<Crumb>()
    };

    public ProcessedContentRepositoryTest()
    {
        _appConfig
            .Setup(config => config.GetContentApiAuthenticationKey())
            .Returns("token");

        _appConfig
            .Setup(config => config.GetWebAppClientId())
            .Returns("id");

        _mockUrlGenerator
            .Setup(generator => generator.UrlFor<PrivacyNotice>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .Returns("url");

        _mockHttpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, JsonConvert.SerializeObject(_privacyNoticeModel), ""));

        _contentTypeFactory = new ContentTypeFactory(_mockTagParserContainer.Object, new MarkdownWrapper(), _mockHttpContextAccessor.Object, _mockRepository.Object);
        _processedContentRepository = new ProcessedContentRepository(_mockUrlGenerator.Object, _mockHttpClient.Object, _contentTypeFactory, _appConfig.Object);
    }
    
    [Fact]
    public async Task Get_PrivacyNotice_ShouldCall_UrlGenerator()
    {
        // Act
        await _processedContentRepository.Get<PrivacyNotice>("slug", new List<Query>());

        // Assert
        _mockUrlGenerator.Verify(generator => generator.UrlFor<PrivacyNotice>(It.IsAny<string>(), It.IsAny<List<Query>>()), Times.Once);
    }

    [Fact]
    public async Task Get_PrivacyNotice_ShouldCall_HttpClient()
    {
        // Act
        await _processedContentRepository.Get<PrivacyNotice>("slug", new List<Query>());

        // Assert
        _mockHttpClient.Verify(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task Get_PrivacyNotice_ShouldReturn_SuccessfulResponse()
    {
        // Act
        HttpResponse result = await _processedContentRepository.Get<PrivacyNotice>("slug", new List<Query>());

        // Assert
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Get_PrivacyNotice_ShouldReturn_UnsuccessfulStatusCode_IfResponseNotSuccessful()
    {
        // Arrange
        _mockHttpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(400, null, ""));

        // Act
        HttpResponse result = await _processedContentRepository.Get<PrivacyNotice>("slug", new List<Query>());

        // Assert
        Assert.Equal(400, result.StatusCode);
    }
}

