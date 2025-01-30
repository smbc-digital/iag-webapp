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
            new()
            {
                Author = "Test",
                Image = "testUrl",
                ImageAltText = "Test Alt",
                Quote = "This is a quote",
                Slug = "slug1"
            }
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
            new()
            {
                Author = "Test",
                Image = "testUrl",
                ImageAltText = "Test Alt",
                Quote = "This is a quote",
                Slug = "slug1"
            }
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
                new()
                {
                    Author = "Test",
                    Image = "testUrl",
                    ImageAltText = "Test Alt",
                    Quote = "This is a quote",
                    Slug = "slug1"
                }
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