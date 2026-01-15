namespace StockportWebappTests_Unit.Unit.Parsers;

public class InlineQuoteTagParserTests
{
    private readonly Mock<IViewRender> _viewRenderer = new();
    private readonly InlineQuoteTagParser _parser;

    public InlineQuoteTagParserTests() =>
        _parser = new(_viewRenderer.Object);

    [Fact]
    public void Parse_ShouldRenderInlineQuoteIfExistsInList()
    {
        // Arrange
        List<InlineQuote> inlineQuotes = new()
        {
            new("testUrl", "Test Alt", "This is a quote", "Test", "slug1", EColourScheme.Pink)
        };

        _viewRenderer
            .Setup(renderer => renderer.Render(It.IsAny<string>(), It.IsAny<InlineQuote>()))
            .Returns("test");

        // Act
        _parser.Parse("this is body {{QUOTE:slug1}}", inlineQuotes);

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render(It.IsAny<string>(), It.IsAny<InlineQuote>()), Times.Once);
    }

    [Fact]
    public void Parse_ShouldNotRenderInlineQuoteIfSlugDoesntExistsInList()
    {
        // Arrange
        List<InlineQuote> inlineQuotes = new()
        {
            new("testUrl", "Test Alt", "This is a quote", "Test", "slug1", EColourScheme.Blue)
        };

        _viewRenderer
            .Setup(renderer => renderer.Render(It.IsAny<string>(), It.IsAny<InlineQuote>()))
            .Returns("test");

        // Act
        _parser.Parse("this is body {{QUOTE:slug2}}", inlineQuotes);

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render(It.IsAny<string>(), It.IsAny<InlineQuote>()), Times.Never);
    }

    [Fact]
    public void Parse_ShouldRemoveUnusedTags()
    {
        // Arrange
        List<InlineQuote> inlineQuotes = new()
            {
                new("TestUrl", "Test Alt", "This is a quote", "Test", "slug1", EColourScheme.Teal)
            };

        _viewRenderer
            .Setup(renderer => renderer.Render(It.IsAny<string>(), It.IsAny<InlineQuote>()))
            .Returns("test");

        // Act
        string bodyResult = _parser.Parse("this is body {{QUOTE:slug2}}", inlineQuotes);

        // Assert
        Assert.Equal("this is body ", bodyResult);
    }
}