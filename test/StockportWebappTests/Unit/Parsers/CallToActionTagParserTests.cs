namespace StockportWebappTests_Unit.Unit.Parsers;

public class CallToActionTagParserTests
{
    private readonly CallToActionTagParser _parser;
    private readonly Mock<IViewRender> _viewRender = new();
    private readonly List<CallToActionBanner> _callToActionBanners = new()
    {
        new()
        {
            Title = "call to action one",
            Link = "/one-article",
            Image = "call-to-action.jpg",
            ButtonText = "read more about this article",
            AltText = "This is the description of the image"
        },
        new()
        {
            Title = "call to action two",
            Link = "/one-event",
            Image = "call-to-action-event.jpg",
            ButtonText = "read more about this event",
            AltText = "This is the description of the image"
        }
    };

    public CallToActionTagParserTests() =>
        _parser = new CallToActionTagParser(_viewRender.Object);

    [Fact]
    public void HasMatches_WithValidTag_ReturnsTrue()
    {
        // Act
        bool hasMatches = _parser.HasMatches("This is some text {{Call-To-Action:call to action one}}. Even more text after the CTA");
        
        // Assert
        Assert.True(hasMatches);
    }

    [Fact]
    public void HasMatches_WithNoTag_ReturnsFalse()
    {
        // Act
        bool hasMatches = _parser.HasMatches("This is a sentence with call to action text.");
        
        // Assert
        Assert.False(hasMatches);
    }

    [Fact]
    public void Parse_ReplacesTagWithCallToActionHtml()
    {
        // Arrange
        _viewRender
            .Setup(viewRender => viewRender.Render(It.IsAny<string>(), It.IsAny<CallToActionBanner>()))
            .Returns("<section><h2>Call to action title</h2><a>This is just a random button</a></section>");

        // Act
        string result = _parser.Parse("First sentence before a cta, {{Call-To-Action:call to action two}}", _callToActionBanners, It.IsAny<bool>());

        // Assert
        Assert.Equal("First sentence before a cta, <section><h2>Call to action title</h2><a>This is just a random button</a></section>", result);
    }

    [Fact]
    public void Parse_WithNonExistingCallToAction_RemovesTag()
    {
        // Act
        string result = _parser.Parse("Hello, {{Call-To-Action:this cta does not exist}}", _callToActionBanners, It.IsAny<bool>());
        
        // Assert
        Assert.Equal("Hello, ", result);
    }
}