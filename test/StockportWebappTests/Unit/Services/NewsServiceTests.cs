namespace StockportWebappTests_Unit.Unit.Services;

public class NewsServiceTests
{
    private readonly NewsService _service;
    private readonly Mock<IRepository> _repository;
    private readonly News News = new(
            "News 2nd September",
            "news-2nd-september",
            "test",
            "purpose",
            string.Empty,
            string.Empty,
            "test",
            new List<Crumb>(), new DateTime(2019, 9, 2), new DateTime(2019, 9, 2), new DateTime(2019, 9, 2), new List<Alert>(),
            new List<string>(), new List<Document>(), new List<Profile>()
    );

    public NewsServiceTests()
    {
        _repository = new Mock<IRepository>();
        _service = new NewsService(_repository.Object);
    }

    [Fact]
    public async Task GetNewsByLimit_ShouldReturnListOfNews()
    {
        // Arrange
        _repository.Setup(_ => _.GetLatest<List<News>>(It.IsAny<int>()))
            .ReturnsAsync(HttpResponse.Successful(200, new List<News> { News }));
        
        // Act
        var result = await _service.GetNewsByLimit(1);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetLatestNewsItem_ShouldReturnNews()
    {
        // Arrange
        _repository.Setup(_ => _.GetLatest<List<News>>(It.IsAny<int>()))
            .ReturnsAsync(HttpResponse.Successful(200, new List<News> { News }));
        
        // Act
        var result = await _service.GetLatestNewsItem();


        // Assert
        Assert.Equal(News, result);
    }
}