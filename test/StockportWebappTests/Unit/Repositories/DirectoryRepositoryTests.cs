namespace StockportWebappTests_Unit.Unit.Repositories;

public class DirectoryRepositoryTests
{
    private Mock<IHttpClient> _httpClient;
    private Mock<IApplicationConfiguration> _applicationConfiguration;
    private readonly UrlGenerator _urlGenerator;
    private DirectoryFactory _directoryFactory;
    private readonly DirectoryRepository _directoryRepository;

    public DirectoryRepositoryTests()
    {
        _httpClient = new Mock<IHttpClient>();
        _applicationConfiguration = new Mock<IApplicationConfiguration>();
        _urlGenerator = new UrlGenerator(_applicationConfiguration.Object, new BusinessId(""));

        _directoryFactory = new DirectoryFactory();
        _directoryRepository = new DirectoryRepository(_directoryFactory, _urlGenerator, _httpClient.Object, _applicationConfiguration.Object);
    }

    [Fact]
    public async void Get_ShouldReturnHttpResponse_IfNotSuccessful(){
        // Arrange
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(new HttpResponse(404, It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _directoryRepository.Get<Directory>();

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Content);
    }

    [Fact]
    public async void Get_ShouldReturnHttpResponse_Successful()
    {
        // Arrange
        ProcessedDirectory processedDirectory = new("title", "slug", "contentfulId", "teaser", "metaDescription", "backgroundImage", "body", null, null, null, null);

        HttpResponse httpResponse = new(200, JsonConvert.SerializeObject(processedDirectory), "OK");
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(httpResponse);
        
        // Act
        var result = await _directoryRepository.Get<Directory>();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.IsType(processedDirectory.GetType(), result.Content);
    }

     [Fact]
    public async void GetEntry_ShouldReturnHttpResponse_IfNotSuccessful(){
        // Arrange
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(new HttpResponse(404, It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _directoryRepository.GetEntry<DirectoryEntry>();

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Content);
    }

    [Fact]
    public async void GetEntry_ShouldReturnHttpResponse_Successful()
    {
        // Arrange
        ProcessedDirectoryEntry processedDirectoryEntry = new("slug", "name", "description", "teaser", "metaDescription", null, null, null, null, null, "phone number",
            "email", "website", "twitter", "facebook", "address");

        HttpResponse httpResponse = new(200, JsonConvert.SerializeObject(processedDirectoryEntry), "OK");
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(httpResponse);
        
        // Act
        var result = await _directoryRepository.GetEntry<DirectoryEntry>();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.IsType(processedDirectoryEntry.GetType(), result.Content);
    }
}
