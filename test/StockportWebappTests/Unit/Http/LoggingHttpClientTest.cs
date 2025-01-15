namespace StockportWebappTests_Unit.Unit.Http;

public class LoggingHttpClientTest
{
    readonly FakeHttpClient _fakeHttpClient = new();

    [Fact]
    public async Task HandlesNoResponseFromRemote()
    {
        // Arrange
        _fakeHttpClient.For("a url").Throw(new AggregateException(new HttpRequestException()));
        ILogger<LoggingHttpClient> logger = new Mock<ILogger<LoggingHttpClient>>().Object;
        LoggingHttpClient httpClient = new(_fakeHttpClient, logger);
        
        // Act
        HttpResponse response = await httpClient.Get("a url", new Dictionary<string, string>());

        // Assert
        Assert.Equal(503, response.StatusCode);
        Assert.Equal("Failed to invoke the requested resource", response.Error);
    }

    [Fact]
    public async Task ReturnsSuccessfulResponseFromRemote()
    {
        // Arrange
        _fakeHttpClient.For("a url").Return(HttpResponse.Successful(200, "some data"));
        ILogger<LoggingHttpClient> logger = new Mock<ILogger<LoggingHttpClient>>().Object;
        LoggingHttpClient httpClient = new(_fakeHttpClient, logger);
        
        // Act
        HttpResponse response = await httpClient.Get("a url", new Dictionary<string, string>());

        // Assert
        Assert.Equal(200, response.StatusCode);
    }
}