namespace StockportWebappTests_Unit.Unit.Repositories;

public class DirectoryRepositoryTests
{
    private Mock<IHttpClient> _httpClient;
    private Mock<IApplicationConfiguration> _applicationConfiguration;
    private readonly UrlGenerator _urlGenerator;
    
    private readonly DirectoryRepository _directoryRepository;

    public DirectoryRepositoryTests()
    {
        _httpClient = new Mock<IHttpClient>();
        _applicationConfiguration = new Mock<IApplicationConfiguration>();
        _urlGenerator = new UrlGenerator(_applicationConfiguration.Object, new BusinessId(""));
        
        _directoryRepository = new DirectoryRepository(_urlGenerator, _httpClient.Object, _applicationConfiguration.Object);
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
        Directory processedDirectory = new()
        {
            Title = "title",
            Slug = "slug",
            ContentfulId = "contentfulId",
            Teaser = "teaser",
            MetaDescription = "metaDescription",
            BackgroundImage = "backgroundImage",
            Body = "body",
            Entries = null,
            SubDirectories = null,
            Alerts = null,
            CallToAction = null
        };

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
        DirectoryEntry directoryEntry = new()
        {
            Slug = "slug",
            Name = "name",
            Provider = "provider",
            Description = "description",
            Teaser = "teaser",
            MetaDescription = "metaDescription",
            Email = "email",
            Website = "website",
            Twitter = "twitter",
            Facebook = "facebook",
            Address = "address",
            Themes = null,
            Directories = null,
            Alerts = null,
            Branding = null,
            MapPosition = new MapPosition()
        };

        HttpResponse httpResponse = new(200, JsonConvert.SerializeObject(directoryEntry), "OK");
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(httpResponse);
        
        // Act
        var result = await _directoryRepository.GetEntry<DirectoryEntry>();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.IsType(directoryEntry.GetType(), result.Content);
    }
}
