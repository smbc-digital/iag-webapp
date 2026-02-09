namespace StockportWebappTests_Unit.Unit.Repositories;

public class PublicationTemplateRepositoryTests
{
    private readonly Mock<IHttpClient> _httpClient = new();
    private readonly Mock<IApplicationConfiguration> _applicationConfiguration = new();
    private readonly UrlGenerator _urlGenerator;
    private readonly PublicationTemplateRepository _repository;

    public PublicationTemplateRepositoryTests()
    {
        _urlGenerator = new(_applicationConfiguration.Object, new BusinessId(string.Empty));
        _repository = new(_urlGenerator, _httpClient.Object, _applicationConfiguration.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnHttpResponse_IfNotSuccessful(){
        // Arrange
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(404, It.IsAny<string>(), It.IsAny<string>()));

        // Act
        HttpResponse result = await _repository.Get("slug");

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Content);
    }

    [Fact]
    public async Task Get_ShouldReturnHttpResponse_Successful()
    {
        // Arrange
        PublicationTemplate publicationTemplate = new()
        {
            Slug = "slug",
            Title = "title",
            Subtitle = "sub",
            PublicationPages = new List<PublicationPage>
            {
                new()
                {
                    Slug = "page",
                    Title = "page title",
                    Body = new JsonElement()
                }
            }
        };

        string json = JsonConvert.SerializeObject(publicationTemplate);

        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, json, "OK"));

        // Act
        HttpResponse result = await _repository.Get("slug");

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.IsType<PublicationTemplate>(result.Content);

        PublicationTemplate returned = result.Content as PublicationTemplate;
        Assert.Equal(publicationTemplate.Slug, returned.Slug);
        Assert.Equal(publicationTemplate.Title, returned.Title);
    }

    [Fact]
    public async Task Get_ShouldReturnFailure_WhenResponseContentIsEmpty()
    {
        // Arrange
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, string.Empty, "OK"));

        // Act
        HttpResponse result = await _repository.Get("slug");

        // Assert
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Empty response from API", result.Error);
    }

    [Fact]
    public async Task Get_ShouldReturnFailure_WhenDeserializedNull()
    {
        // Arrange
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, "null", "OK"));

        // Act
        HttpResponse result = await _repository.Get("slug");

        // Assert
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Failed to deserialize PublicationTemplate", result.Error);
    }

    [Fact]
    public async Task Get_ShouldReturnFailure_WhenDeserializationThrows()
    {
        // Arrange
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, "{ not: valid json }", "OK"));

        // Act
        HttpResponse result = await _repository.Get("slug");

        // Assert
        Assert.Equal(500, result.StatusCode);
        Assert.Contains("Error deserializing PublicationTemplate", result.Error);
    }
}