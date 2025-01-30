namespace StockportWebappTests_Unit.Unit.Repositories;

public class TopicRepositoryTests
{
    private readonly Mock<IHttpClient> _httpClient = new();
    private readonly Mock<IApplicationConfiguration> _applicationConfiguration = new();
    private readonly UrlGenerator _urlGenerator;
    private readonly TopicFactory _topicFactory;
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly TopicRepository _topicRepository;

    public TopicRepositoryTests()
    {
        _urlGenerator = new(_applicationConfiguration.Object, new BusinessId(string.Empty));
        _topicFactory = new(_tagParserContainer.Object, _markdownWrapper.Object);

        _topicRepository = new(_topicFactory, _urlGenerator, _httpClient.Object, _applicationConfiguration.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnHttpResponse_IfNotSuccessful(){
        // Arrange
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(404, It.IsAny<string>(), It.IsAny<string>()));

        // Act
        HttpResponse result = await _topicRepository.Get<Topic>();

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Content);
    }

    [Fact]
    public async Task Get_ShouldReturnHttpResponse_Successful()
    {
        // Arrange
        ProcessedTopic processedTopic = new("Name",
                                            "slug",
                                            "<p>Summary</p>\n",
                                            "Teaser",
                                            "metaDescription",
                                            "Icon",
                                            "Image",
                                            "Image",
                                            new List<SubItem>(),
                                            new List<SubItem>(),
                                            null,
                                            new List<Crumb>(),
                                            new List<Alert>(),
                                            true,
                                            "test-id",
                                            null,
                                            null,
                                            true,
                                            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty, new DateTime()),
                                            string.Empty,
                                            null,
                                            null,
                                            string.Empty)
        {
            Video = new()
        };

        HttpResponse httpResponse = new(200, JsonConvert.SerializeObject(processedTopic), "OK");
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(httpResponse);

        // Act
        HttpResponse result = await _topicRepository.Get<Topic>();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.IsType(processedTopic.GetType(), result.Content);
    }
}