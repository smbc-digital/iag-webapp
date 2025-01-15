namespace StockportWebappTests_Unit.Unit.Repositories;

public class ContentApiRepositoryTests
{
    private readonly Mock<IUrlGeneratorSimple> _mockUrlGenerator = new();
    private readonly Mock<IHttpClient> _mockHttpClient = new();
    private readonly Mock<IApplicationConfiguration> _mockApplicationConfiguration = new();
    private readonly Mock<ILogger<BaseRepository>> _mockLogger = new();
    private readonly Mock<HttpContent> _mockHttpContent = new();

    private readonly ContentApiRepository _repository;

    private readonly List<GroupCategory> _categories = new();

    public ContentApiRepositoryTests()
    {
        _mockHttpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, JsonConvert.SerializeObject(_categories), string.Empty));

        _mockHttpClient
            .Setup(client => client.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, JsonConvert.SerializeObject(_categories), string.Empty));

        _mockUrlGenerator
            .Setup(generator => generator.BaseContentApiUrl<List<GroupCategory>>())
            .Returns("url");

        _repository = new(_mockHttpClient.Object,
                        _mockApplicationConfiguration.Object,
                        _mockUrlGenerator.Object,
                        _mockLogger.Object);
    }

    [Fact]
    public async Task GetResponse_NoParams_ShouldCallClientAndUrlGenerator()
    {
        // Act
        await _repository.GetResponse<List<GroupCategory>>();

        // Assert
        _mockUrlGenerator.Verify(generator => generator.BaseContentApiUrl<List<GroupCategory>>(), Times.Once);
        _mockHttpClient.Verify(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task GetResponse_Extra_ShouldCallClientAndUrlGenerator()
    {
        // Act
        await _repository.GetResponse<List<GroupCategory>>("");

        // Assert
        _mockUrlGenerator.Verify(generator => generator.BaseContentApiUrl<List<GroupCategory>>(), Times.Once);
        _mockHttpClient.Verify(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task GetResponse_Queries_ShouldCallClientAndUrlGenerator()
    {
        // Act
        await _repository.GetResponse<List<GroupCategory>>(new List<Query>());

        // Assert
        _mockUrlGenerator.Verify(generator => generator.BaseContentApiUrl<List<GroupCategory>>(), Times.Once);
        _mockHttpClient.Verify(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task GetResponse_ExtraAndQueries_ShouldCallClientAndUrlGenerator()
    {
        // Act
        await _repository.GetResponse<List<GroupCategory>>("", new List<Query>());

        // Assert
        _mockUrlGenerator.Verify(generator => generator.BaseContentApiUrl<List<GroupCategory>>(), Times.Once);
        _mockHttpClient.Verify(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task PutResponse_HttpContext_ShouldCallClientAndUrlGenerator()
    {
        // Act
        await _repository.PutResponse<List<GroupCategory>>(_mockHttpContent.Object);

        // Assert
        _mockUrlGenerator.Verify(generator => generator.BaseContentApiUrl<List<GroupCategory>>(), Times.Once);
        _mockHttpClient.Verify(client => client.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }

    [Fact]
    public async Task PutResponse_HttpContextAndExtra_ShouldCallClientAndUrlGenerator()
    {
        // Act
        await _repository.PutResponse<List<GroupCategory>>(_mockHttpContent.Object, string.Empty);

        // Assert
        _mockUrlGenerator.Verify(generator => generator.BaseContentApiUrl<List<GroupCategory>>(), Times.Once);
        _mockHttpClient.Verify(client => client.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
    }
}