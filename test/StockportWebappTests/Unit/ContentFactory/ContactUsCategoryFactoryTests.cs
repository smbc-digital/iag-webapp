namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ContactUsCategoryFactoryTests
{
    private readonly Mock<ITagParserContainer> _mockTagParser = new();
    private readonly MarkdownWrapper _markdownWrapper = new();
    private readonly ContactUsCategoryFactory _factory;
    private readonly string _bodyTextLeft = "<p>left</p>\n";
    private readonly string _bodyTextRight = "<p>right</p>\n";
    private readonly ContactUsCategory _contactUsCategory = new("title", "left", "right", "icon");

    public ContactUsCategoryFactoryTests()
    {
        _mockTagParser
            .Setup(tagParser => tagParser.ParseAll(_bodyTextLeft,
                null,
                It.IsAny<bool>(),
                null,
                null,
                null,
                null,
                null,
                null,
                It.IsAny<bool>()))
            .Returns(_bodyTextLeft);

        _mockTagParser
            .Setup(tagParser => tagParser.ParseAll(_bodyTextRight,
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<IEnumerable<Alert>>(),
                It.IsAny<IEnumerable<Document>>(),
                It.IsAny<IEnumerable<InlineQuote>>(),
                It.IsAny<IEnumerable<PrivacyNotice>>(),
                It.IsAny<IEnumerable<Profile>>(),
                null,
                It.IsAny<bool>()))
            .Returns(_bodyTextRight);

        _factory = new ContactUsCategoryFactory(_mockTagParser.Object, _markdownWrapper);
    }

    [Fact]
    public void Build_ShouldReturnCorrectProcessedContactUsCategory()
    {
        // Act
        ProcessedContactUsCategory result = _factory.Build(_contactUsCategory);

        // Assert
        Assert.Equal(_contactUsCategory.Title, result.Title);
        Assert.Equal(_bodyTextLeft, result.BodyTextLeft);
        Assert.Equal(_bodyTextRight, result.BodyTextRight);
        Assert.Equal(_contactUsCategory.Icon, result.Icon);
    }

    [Fact]
    public void Build_ShouldCallTagParserContainerTwice()
    {
        // Act
        _factory.Build(_contactUsCategory);

        // Assert
        _mockTagParser.Verify(tagParser => tagParser.ParseAll(_bodyTextLeft,
            null,
            It.IsAny<bool>(),
            null,
            null,
            null,
            null,
            null,
            null,
            It.IsAny<bool>()), Times.Once);

        _mockTagParser.Verify(tagParser => tagParser.ParseAll(_bodyTextRight,
            null,
            It.IsAny<bool>(),
            null,
            null,
            null,
            null,
            null,
            null,
            It.IsAny<bool>()), Times.Once);
    }
}