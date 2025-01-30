namespace StockportWebappTests_Unit.Unit.Repositories;

public class DocumentPageRepositoryTests
{
    private readonly Mock<IHttpClient> _httpClient = new();
    private readonly Mock<IApplicationConfiguration> _applicationConfiguration = new();
    private readonly UrlGenerator _urlGenerator;
    private readonly DocumentPageFactory _documentPageFactory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly DocumentPageRepository _documentPageRepository;

    public DocumentPageRepositoryTests()
    {
        _urlGenerator = new(_applicationConfiguration.Object, new BusinessId(string.Empty));

        _documentPageFactory = new(_markdownWrapper.Object);
        _documentPageRepository = new(_urlGenerator, _httpClient.Object, _documentPageFactory, _applicationConfiguration.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnHttpResponse_IfNotSuccessful()
    {
        // Arrange
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(404, It.IsAny<string>(), It.IsAny<string>()));

        // Act
        HttpResponse result = await _documentPageRepository.Get();

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Content);
    }

    [Fact]
    public async Task Get_ShouldReturnHttpResponse_Successful()
    {
        // Arrange
        DocumentPage documentPage = new()
        {
            Slug = "slug",
            Title = "title"
        };

        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, JsonConvert.SerializeObject(documentPage), "OK"));

        // Act
        HttpResponse result = await _documentPageRepository.Get("slug");

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.IsType(documentPage.GetType(), result.Content);
    }
}