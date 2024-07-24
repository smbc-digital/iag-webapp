namespace StockportWebappTests_Unit.Unit.Services;

public class ProfileServiceTests
{
    private readonly ProfileService _service;
    private readonly Mock<IRepository> _repository;
    private readonly Mock<ITagParserContainer> _parser;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly Mock<IViewRender> _viewRender;
    private readonly Mock<ITriviaFactory> _triviaFactory;

    public ProfileServiceTests()
    {
        _triviaFactory = new Mock<ITriviaFactory>();
        _repository = new Mock<IRepository>();
        _parser = new Mock<ITagParserContainer>();
        _markdownWrapper = new MarkdownWrapper();
        _viewRender = new Mock<IViewRender>();
        _service = new ProfileService(_repository.Object, _parser.Object, _markdownWrapper, _triviaFactory.Object);
    }

    [Fact]
    public async Task GetProfile_ShouldReturnNullWhenFailure()
    {
        // Arrange
        var response = HttpResponse.Failure(500, "Test Error");
        _repository
            .Setup(_ => _.Get<Profile>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(response);

        // Act
        var result = await _service.GetProfile("testing slug");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProfile_ShouldReturnProfileWhenSuccessful()
    {
        // Arrange
        var response = HttpResponse.Successful(200, new Profile
        {
            Body = "Test",
            Slug = "test",
            InlineAlerts = new List<Alert>(),
            Alerts = new List<Alert>(),
            Breadcrumbs = new List<Crumb>(),
            TriviaSection = new List<Trivia>(),
            Image = new MediaAsset(),
            ImageCaption = "image caption",
            Teaser = "test",
            Title = "test",
            Colour = "blue"
        });
        _repository
            .Setup(_ => _.Get<Profile>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(response);
        _parser
            .Setup(_ => _.ParseAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null, It.IsAny<bool>()))
            .Returns("testProcessedBody");

        // Act
        var result = await _service.GetProfile("testing slug");

        // Assert
        result.Should().BeOfType<Profile>();
    }
}