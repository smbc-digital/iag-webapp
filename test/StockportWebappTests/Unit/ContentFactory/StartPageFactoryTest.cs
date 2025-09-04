namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class StartPageFactoryTests
{
    private readonly Mock<MarkdownWrapper> _mockMarkdownWrapper = new();
    private readonly Mock<ITagParserContainer> _mockTagParser = new();
    private readonly StartPageFactory _factory;
    private readonly StartPage _startPage;

    public StartPageFactoryTests()
    {
        _factory = new StartPageFactory(_mockTagParser.Object, _mockMarkdownWrapper.Object);
        _startPage = new StartPage("test-start-page",
                                "Test start page",
                                "This is a test start page",
                                "Use this page to start test processes",
                                "Test upper body content",
                                "https://www.stockport.gov.uk",
                                "Test lower body content",
                                new List<Crumb>(),
                                string.Empty,
                                "fa-test",
                                new List<Alert>(),
                                new List<Alert>());

        _mockTagParser
            .Setup(parser => parser.ParseAll(_startPage.UpperBody,
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            It.IsAny<IEnumerable<Alert>>(),
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            It.IsAny<bool>()))
            .Returns(_startPage.UpperBody);

        _mockTagParser
            .Setup(parser => parser.ParseAll(_startPage.LowerBody,
                                            It.IsAny<string>(),
                                            It.IsAny<bool>(),
                                            It.IsAny<IEnumerable<Alert>>(),
                                            null,
                                            null,
                                            null,
                                            null,
                                            null,
                                            It.IsAny<bool>()))
            .Returns(_startPage.LowerBody);

        _mockMarkdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(_startPage.UpperBody))
            .Returns(_startPage.UpperBody);

        _mockMarkdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(_startPage.LowerBody))
            .Returns(_startPage.LowerBody);
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedServicePayPayment()
    {
        // Act
        ProcessedStartPage result = _factory.Build(_startPage);

        // Assert
        Assert.Equal(_startPage.Slug, result.Slug);
        Assert.Equal(_startPage.Title, result.Title);
        Assert.Equal(_startPage.Teaser, result.Teaser);
        Assert.Equal(_startPage.Summary, result.Summary);
        Assert.Equal(_startPage.UpperBody, result.UpperBody);
        Assert.Equal(_startPage.FormLink, result.FormLink);
        Assert.Equal(_startPage.LowerBody, result.LowerBody);
        Assert.Equal(_startPage.Breadcrumbs, result.Breadcrumbs);
        Assert.Equal(_startPage.BackgroundImage, result.BackgroundImage);
        Assert.Equal(_startPage.Icon, result.Icon);
        Assert.Equal(_startPage.Alerts, result.Alerts);
    }

    [Fact]
    public void ShouldCallMarkdownWrapper()
    {
        // Act & Assert
        _factory.Build(_startPage);
        _mockMarkdownWrapper.Verify(parser => parser.ConvertToHtml(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void ShouldCallTagParser()
    {
        // Act & Assert
        _factory.Build(_startPage);
        _mockTagParser.Verify(parser => parser.ParseAll(It.IsAny<string>(),
                                                        It.IsAny<string>(),
                                                        It.IsAny<bool>(),
                                                        It.IsAny<IEnumerable<Alert>>(),
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        It.IsAny<bool>()), Times.Exactly(2));
    }
}