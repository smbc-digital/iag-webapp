namespace StockportWebappTests_Unit.Unit.Controllers;

public class CommsControllerTest
{
    private readonly Mock<IRepository> _mockRepository = new Mock<IRepository>();
    private readonly Mock<ILogger<CommsController>> _mockLogger = new Mock<ILogger<CommsController>>();
    private readonly CommsController _controller;

    public CommsControllerTest()
    {
        _controller = new CommsController(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Index_ShouldGetLatestNews()
    {
        var exampleNews = new News(
            "News 2nd September",
            "news-2nd-september",
            "test",
            "purpose",
            "",
            "",
            "test",
            new List<Crumb>(), new DateTime(2019, 9, 2), new DateTime(2019, 9, 2), new List<Alert>(),
            new List<string>(), new List<Document>());

        // Arrange
        _mockRepository
            .Setup(_ => _.GetLatest<List<News>>(It.IsAny<int>()))
            .ReturnsAsync(HttpResponse.Successful(200, new List<News> { exampleNews }));
        _mockRepository
            .Setup(_ => _.Get<CommsHomepage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, new CommsHomepage()));

        // Act
        var view = await _controller.Index() as ViewResult;
        var model = view.Model as CommsHomepageViewModel;

        // Assert
        model.LatestNews.Should().Be(exampleNews);
    }
}
