namespace StockportWebappTests_Unit.Unit.Parsers;

public class TagParserContainerTests
{
    private readonly TagParserContainer _tagParserContainer;
    private readonly Mock<ISimpleTagParser> _tagParser = new();
    private readonly Mock<ISimpleTagParser> _anotherTagParser = new();
    private readonly Mock<ISimpleTagParser> _anotherAnotherTagParser = new();
    private readonly Mock<IDynamicTagParser<Alert>> _alertsTagParser = new();
    private readonly Mock<IDynamicTagParser<Document>> _documentTagParser = new();
    private readonly Mock<IDynamicTagParser<InlineQuote>> _quoteTagParser = new();
    private readonly Mock<IDynamicTagParser<PrivacyNotice>> _privacyNoticeTagParser = new();
    private readonly Mock<IDynamicTagParser<Profile>> _profileTagParser = new();
    private readonly Mock<IDynamicTagParser<CallToActionBanner>> _callToActionTagParser= new();

    public TagParserContainerTests()
    {
        List<ISimpleTagParser> tagParserList = new() { _tagParser.Object, _anotherTagParser.Object, _anotherAnotherTagParser.Object };

        _tagParserContainer = new(tagParserList,
                                _alertsTagParser.Object,
                                _documentTagParser.Object,
                                _quoteTagParser.Object,
                                _privacyNoticeTagParser.Object,
                                _profileTagParser.Object,
                                _callToActionTagParser.Object);

        _tagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("some body");
        
        _anotherTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("some body");
        
        _anotherAnotherTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("some body");
        
        _alertsTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Alert>>(), It.IsAny<bool>()))
            .Returns("some body");
        
        _documentTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Document>>(), It.IsAny<bool>()))
            .Returns("some body");
        
        _quoteTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<InlineQuote>>(), It.IsAny<bool>()))
            .Returns("some body");
        
        _privacyNoticeTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<PrivacyNotice>>(), It.IsAny<bool>()))
            .Returns("some body");
        
        _profileTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Profile>>(), It.IsAny<bool>()))
            .Returns("some body");

        _callToActionTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<CallToActionBanner>>(), It.IsAny<bool>()))
            .Returns("some body");
    }

    [Fact]
    public void ShouldUseAllParsersToParseContent()
    {
        // Act
        _tagParserContainer.ParseAll("some body");

        // Assert
        _tagParser.Verify(parser => parser.Parse("some body", It.IsAny<string>()), Times.Once);
        _anotherTagParser.Verify(parser => parser.Parse("some body", It.IsAny<string>()), Times.Once);
        _anotherAnotherTagParser.Verify(parser => parser.Parse("some body", It.IsAny<string>()), Times.Once);
        _alertsTagParser.Verify(parser => parser.Parse("some body", It.IsAny<IEnumerable<Alert>>(), It.IsAny<bool>()), Times.Once);
        _documentTagParser.Verify(parser => parser.Parse("some body", It.IsAny<IEnumerable<Document>>(), It.IsAny<bool>()), Times.Once);
        _quoteTagParser.Verify(parser => parser.Parse("some body", It.IsAny<IEnumerable<InlineQuote>>(), It.IsAny<bool>()), Times.Once);
        _privacyNoticeTagParser.Verify(parser => parser.Parse("some body", It.IsAny<IEnumerable<PrivacyNotice>>(), It.IsAny<bool>()), Times.Once);
        _profileTagParser.Verify(parser => parser.Parse("some body", It.IsAny<IEnumerable<Profile>>(), It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public void ShouldRemoveAllUnusedTags()
    {
        // Arrange
        _tagParser
            .Setup(parser => parser.Parse("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}", It.IsAny<string>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");
        
        _anotherTagParser
            .Setup(parser => parser.Parse("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}", It.IsAny<string>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");
        
        _anotherAnotherTagParser
            .Setup(parser => parser.Parse("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}", It.IsAny<string>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");
        
        _alertsTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Alert>>(), It.IsAny<bool>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");
        
        _documentTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Document>>(), It.IsAny<bool>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");
        
        _quoteTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<InlineQuote>>(), It.IsAny<bool>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");
        
        _privacyNoticeTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<PrivacyNotice>>(), It.IsAny<bool>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");
        
        _profileTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<Profile>>(), It.IsAny<bool>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");

        _callToActionTagParser
            .Setup(parser => parser.Parse(It.IsAny<string>(), It.IsAny<IEnumerable<CallToActionBanner>>(), It.IsAny<bool>()))
            .Returns("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");

        // Act
        string parsedContent = _tagParserContainer.ParseAll("{{TAG: sgsdgsdfgd}}CONTENT{{TAGS:sgdfgdfg}}{{TAGS:sgdfgdf242g}}");
        
        // Assert
        Assert.Equal("CONTENT", parsedContent);
    }

    [Fact]
    public void ShouldPassTitleToParsers()
    {
        // Act
        _tagParserContainer.ParseAll("Unimportant content", "The title");

        // Assert
        _tagParser.Verify();
        _anotherTagParser.Verify();
        _anotherAnotherTagParser.Verify();
    }
}