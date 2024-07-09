using StockportWebapp.TagParsers;

namespace StockportWebappTests_Unit.Unit.Parsers;

public class TagParserContainerTests
{
    private readonly TagParserContainer _tagParserContainer;
    private readonly Mock<ISimpleTagParser> _tagParser;
    private readonly Mock<ISimpleTagParser> _anotherTagParser;
    private readonly Mock<ISimpleTagParser> _anotherAnotherTagParser;
    private readonly Mock<IViewRender> _viewRenderer;
    private readonly Mock<IDynamicTagParser<Alert>> _alertsTagParser;
    private readonly Mock<IDynamicTagParser<Document>> _documentTagParser;
    private readonly Mock<IDynamicTagParser<InlineQuote>> _quoteTagParser;
    private readonly Mock<IDynamicTagParser<PrivacyNotice>> _privacyNoticeTagParser;
    private readonly Mock<IDynamicTagParser<Profile>> _profileTagParser;

    public TagParserContainerTests()
    {
        _tagParser = new Mock<ISimpleTagParser>();
        _anotherTagParser = new Mock<ISimpleTagParser>();
        _anotherAnotherTagParser = new Mock<ISimpleTagParser>();
        _alertsTagParser = new Mock<IDynamicTagParser<Alert>>();
        _documentTagParser = new Mock<IDynamicTagParser<Document>>();
        _quoteTagParser = new Mock<IDynamicTagParser<InlineQuote>>();
        _privacyNoticeTagParser = new Mock<IDynamicTagParser<PrivacyNotice>>();
        _profileTagParser = new Mock<IDynamicTagParser<Profile>>();

        _viewRenderer = new Mock<IViewRender>();

        var tagParserList = new List<ISimpleTagParser> { _tagParser.Object, _anotherTagParser.Object, _anotherAnotherTagParser.Object };

        _tagParserContainer = new TagParserContainer(tagParserList,
                                    _alertsTagParser.Object,
                                    _documentTagParser.Object,
                                    _quoteTagParser.Object,
                                    _privacyNoticeTagParser.Object,
                                    _profileTagParser.Object);
    }

    [Fact]
    public void ShouldUseAllParsersToParseContent()
    {
        const string content = "some body";

        _tagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns(content);
        _anotherTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns(content);
        _anotherAnotherTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<string>())).Returns(content);
        _alertsTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Alert>>())).Returns(content);
        _documentTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Document>>())).Returns(content);
        _quoteTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<InlineQuote>>())).Returns(content);
        _privacyNoticeTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<PrivacyNotice>>())).Returns(content);
        _profileTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Profile>>())).Returns(content);

        _tagParserContainer.ParseAll(content);

        _tagParser.Verify(o => o.Parse(content, It.IsAny<string>()), Times.Once);
        _anotherTagParser.Verify(o => o.Parse(content, It.IsAny<string>()), Times.Once);
        _anotherAnotherTagParser.Verify(o => o.Parse(content, It.IsAny<string>()), Times.Once);
        _alertsTagParser.Verify(o => o.Parse(content, It.IsAny<IEnumerable<Alert>>()), Times.Once);
        _documentTagParser.Verify(o => o.Parse(content, It.IsAny<IEnumerable<Document>>()), Times.Once);
        _quoteTagParser.Verify(o => o.Parse(content, It.IsAny<IEnumerable<InlineQuote>>()), Times.Once);
        _privacyNoticeTagParser.Verify(o => o.Parse(content, It.IsAny<IEnumerable<PrivacyNotice>>()), Times.Once);
        _profileTagParser.Verify(o => o.Parse(content, It.IsAny<IEnumerable<Profile>>()), Times.Once);
    }

    [Fact]
    public void ShouldRemoveAllUnusedTags()
    {
        const string content = "{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}";

        _tagParser.Setup(o => o.Parse(content, It.IsAny<string>())).Returns(content);
        _anotherTagParser.Setup(o => o.Parse(content, It.IsAny<string>())).Returns(content);
        _anotherAnotherTagParser.Setup(o => o.Parse(content, It.IsAny<string>())).Returns(content);
        _alertsTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Alert>>())).Returns(content);
        _documentTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Document>>())).Returns(content);
        _quoteTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<InlineQuote>>())).Returns(content);
        _privacyNoticeTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<PrivacyNotice>>())).Returns(content);
        _profileTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Profile>>())).Returns(content);

        var parsedContent = _tagParserContainer.ParseAll(content);

        parsedContent.Should().Be("CONTENT");
    }

    [Fact]
    public void ShouldPassTitleToParsers()
    {
        const string content = "Unimportant content";
        const string title = "The title";

        _tagParser.Setup(o => o.Parse(content, title)).Returns(content);
        _anotherTagParser.Setup(o => o.Parse(content, title)).Returns(content);
        _anotherAnotherTagParser.Setup(o => o.Parse(content, title)).Returns(content);
        _alertsTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Alert>>())).Returns(content);
        _documentTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Document>>())).Returns(content);
        _quoteTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<InlineQuote>>())).Returns(content);
        _privacyNoticeTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<PrivacyNotice>>())).Returns(content);
        _profileTagParser.Setup(o => o.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Profile>>())).Returns(content);

        var parsedContent = _tagParserContainer.ParseAll(content, title);

        _tagParser.Verify();
        _anotherTagParser.Verify();
        _anotherAnotherTagParser.Verify();
    }
}