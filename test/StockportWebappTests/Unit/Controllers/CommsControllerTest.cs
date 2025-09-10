namespace StockportWebappTests_Unit.Unit.Controllers;

public class CommsControllerTest
{
    private readonly Mock<IRepository> _mockRepository = new();
    private readonly Mock<ILogger<CommsController>> _mockLogger = new();
    private readonly CommsController _controller;

    public CommsControllerTest() =>
        _controller = new CommsController(_mockRepository.Object, _mockLogger.Object);

    [Fact]
    public async Task Index_ShouldGetLatestNews()
    {
        News exampleNews = new("News 2nd September",
                            "news-2nd-september",
                            "test",
                            "purpose",
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            "test",
                            new DateTime(2019, 9, 2),
                            string.Empty,
                            new DateTime(2019, 9, 2),
                            new DateTime(2019, 9, 2),
                            new List<Alert>(),
                            new List<string>(),
                            new List<Document>(),
                            new List<InlineQuote>(),
                            null,
                            string.Empty,
                            new List<TrustedLogo>(),
                            null,
                            string.Empty,
                            null);

        // Arrange
        _mockRepository
            .Setup(_ => _.GetLatest<List<News>>(It.IsAny<int>()))
            .ReturnsAsync(HttpResponse.Successful(200, new List<News> { exampleNews }));
        
        _mockRepository
            .Setup(_ => _.Get<CommsHomepage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, new CommsHomepage()));

        // Act
        ViewResult view = await _controller.Index() as ViewResult;
        CommsHomepageViewModel model = view.Model as CommsHomepageViewModel;

        // Assert
        Assert.Equal(exampleNews, model.LatestNews);
    }
}