namespace StockportWebappTests_Unit.Unit.Repositories;

public class StockportApiRepositoryTests
{
    private readonly Mock<IHttpClient> _httpClient = new();
    private readonly Mock<IApplicationConfiguration> _applicationConfiguraiton = new();
    private readonly Mock<IUrlGeneratorSimple> _simpleUrlGenerator = new();
    private readonly Mock<ILogger<BaseRepository>> _logger = new();
    private readonly StockportApiRepository _stockportApiRepository;

    public StockportApiRepositoryTests() =>
        _stockportApiRepository = new(_httpClient.Object, _applicationConfiguraiton.Object, _simpleUrlGenerator.Object, _logger.Object);

    [Fact]
    public async Task GetResponse_ShouldReturnEvent()
    {
        // Arrange
        List<Event> builtEvents = new()
        {
            new EventBuilder().Build()
        };

        string seralisedEvents = JsonConvert.SerializeObject(builtEvents);

        _simpleUrlGenerator
            .Setup(urlGenerator => urlGenerator.StockportApiUrl<List<Event>>())
            .Returns("url");
        
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, seralisedEvents, string.Empty));

        // Act
        List<Event> apiResponse = await _stockportApiRepository.GetResponse<List<Event>>();

        // Assert
        Assert.NotNull(apiResponse);
        Assert.Equivalent(builtEvents, apiResponse);
    }

    [Fact]
    public async Task GetResponseWithSlugAndQueries_ShouldReturnEvent()
    {
        // Arrange
        List<Event> builtEvents = new()
        {
            new EventBuilder().Build()
        };

        string seralisedEvents = JsonConvert.SerializeObject(builtEvents);

        _simpleUrlGenerator
            .Setup(urlGenerator => urlGenerator.StockportApiUrl<List<Event>>())
            .Returns("url");
        
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ReturnsAsync(new HttpResponse(200, seralisedEvents, string.Empty));

        // Act
        List<Event> apiResponse = await _stockportApiRepository.GetResponse<List<Event>>("slug", new List<Query>() { new("name", "value") });

        // Assert
        Assert.NotNull(apiResponse);
        Assert.Equivalent(builtEvents, apiResponse);
    }

    [Fact]
    public async Task GetResponse_LogIfExceptionIsThrown()
    {
        //Arrange
        List<Event> builtEvents = new()
        {
            new EventBuilder().Build()
        };

        string seralisedEvents = JsonConvert.SerializeObject(builtEvents);

        _simpleUrlGenerator
            .Setup(urlGenerator => urlGenerator.StockportApiUrl<List<Event>>())
            .Returns("url");
        
        _httpClient
            .Setup(client => client.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .ThrowsAsync(new Exception());

        // Act
        List<Event> apiResponse = await _stockportApiRepository.GetResponse<List<Event>>();

        LogTesting.Assert(_logger, LogLevel.Error, "Error getting response for url url");
        Assert.Null(apiResponse);
    }
}