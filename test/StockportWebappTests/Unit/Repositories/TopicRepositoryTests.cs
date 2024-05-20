namespace StockportWebappTests_Unit.Unit.Repositories;

public class TopicRepositoryTests
{
    private Mock<IHttpClient> _httpClient;
    private Mock<IApplicationConfiguration> _applicationConfiguration;
    private Mock<IUrlGeneratorSimple> _urlGeneratorSimple;
    private readonly UrlGenerator _urlGenerator;
    private TopicFactory _topicFactory;
    private readonly Mock<ITagParserContainer> _tagParserContainer;
    private readonly Mock<MarkdownWrapper> _markdownWrapper;
    private readonly TopicRepository _topicRepository;

    public TopicRepositoryTests()
    {
        _httpClient = new Mock<IHttpClient>();
        _applicationConfiguration = new Mock<IApplicationConfiguration>();
        _urlGeneratorSimple = new Mock<IUrlGeneratorSimple>();
        _urlGenerator = new UrlGenerator(_applicationConfiguration.Object, new BusinessId(""));
        _tagParserContainer = new Mock<ITagParserContainer>();
        _markdownWrapper = new Mock<MarkdownWrapper>();

        _topicFactory = new TopicFactory(_tagParserContainer.Object, _markdownWrapper.Object);
        _topicRepository = new TopicRepository(_topicFactory, _urlGenerator, _httpClient.Object, _applicationConfiguration.Object, _urlGeneratorSimple.Object);
    }

    [Fact]
    public async void Get_ShouldReturnHttpResponse_IfNotSuccessful(){
        // Arrange
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(new HttpResponse(404, It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _topicRepository.Get<Topic>();

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Content);
    }

    [Fact]
    public async void Get_ShouldReturnHttpResponse_Successful()
    {
        // Arrange
        ProcessedTopic processedTopic = new("Name", "slug", "<p>Summary</p>\n", "Teaser", "metaDescription", "Icon", "Image", "Image", new List<SubItem>(), new List<SubItem>(), null,
            new List<Crumb>(), new List<Alert>(), true, "test-id", null, null,
            string.Empty, true,
            new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty, null, null, string.Empty)
        {
            Video = new()
        };

        HttpResponse httpResponse = new(200, JsonConvert.SerializeObject(processedTopic), "OK");
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(httpResponse);
        
        // Act
        var result = await _topicRepository.Get<Topic>();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.IsType(processedTopic.GetType(), result.Content);
    }
}
