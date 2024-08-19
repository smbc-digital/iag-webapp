namespace StockportWebappTests_Unit.Unit.Services;

public class ProfileServiceTests
{
    private readonly ProfileService _service;
    private readonly Mock<IRepository> _repository;
    private readonly Mock<ITagParserContainer> _parser;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly Mock<ITriviaFactory> _triviaFactory;

    public ProfileServiceTests()
    {
        _triviaFactory = new Mock<ITriviaFactory>();
        _repository = new Mock<IRepository>();
        _parser = new Mock<ITagParserContainer>();
        _markdownWrapper = new MarkdownWrapper();
        _service = new ProfileService(_repository.Object, _parser.Object, _markdownWrapper, _triviaFactory.Object);
    }

    [Fact]
    public async Task GetProfile_ShouldReturnNullWhenFailure()
    {
        // Arrange
        HttpResponse response = HttpResponse.Failure(500, "Test Error");
        _repository
            .Setup(_ => _.Get<Profile>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(response);

        // Act
        Profile result = await _service.GetProfile("testing slug");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProfile_ShouldReturnProfileWhenSuccessful()
    {
        // Arrange
        HttpResponse response = HttpResponse.Successful(200, new Profile
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
            Colour = EColourScheme.Blue
        });
        _repository
            .Setup(_ => _.Get<Profile>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(response);
        _parser
            .Setup(_ => _.ParseAll(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null, It.IsAny<bool>()))
            .Returns("testProcessedBody");

        // Act
        Profile result = await _service.GetProfile("testing slug");

        // Assert
        Assert.IsType<Profile>(result);
    }
}