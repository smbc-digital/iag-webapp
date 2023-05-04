using StockportWebapp.TagParsers;

namespace StockportWebappTests_Unit.Unit.Parsers;

public class InlineQuoteTagParserTests
{
    private readonly Mock<IViewRender> _viewRenderer = new Mock<IViewRender>();
    private readonly Mock<ILogger<InlineQuoteTagParser>> _logger = new Mock<ILogger<InlineQuoteTagParser>>();
    private readonly InlineQuoteTagParser _parser;

    public InlineQuoteTagParserTests()
    {
        _parser = new InlineQuoteTagParser(_viewRenderer.Object, _logger.Object);
    }

    [Fact]
    public void Parse_ShouldRenderInlineQuoteIfExistsInList()
    {
        // Arrange
        var body = "this is body {{QUOTE:slug1}}";
        var inlineQuotes = new List<InlineQuote>
        {
            new InlineQuote
            {
                Author = "Test",
                Image = "testUrl",
                ImageAltText = "Test Alt",
                Quote = "This is a quote",
                Slug = "slug1"
            }
        };

        _viewRenderer.Setup(_ => _.Render(It.IsAny<string>(), It.IsAny<InlineQuote>())).Returns("test");

        // Act
        _parser.Parse(body, inlineQuotes);

        // Assert
        _viewRenderer.Verify(_ => _.Render(It.IsAny<string>(), It.IsAny<InlineQuote>()), Times.Once);
    }

    [Fact]
    public void Parse_ShouldNotRenderInlineQuoteIfSlugDoesntExistsInList()
    {
        // Arrange
        var body = "this is body {{QUOTE:slug2}}";
        var inlineQuotes = new List<InlineQuote>
        {
            new InlineQuote
            {
                Author = "Test",
                Image = "testUrl",
                ImageAltText = "Test Alt",
                Quote = "This is a quote",
                Slug = "slug1"
            }
        };

        _viewRenderer.Setup(_ => _.Render(It.IsAny<string>(), It.IsAny<InlineQuote>())).Returns("test");

        // Act
        _parser.Parse(body, inlineQuotes);

        // Assert
        _viewRenderer.Verify(_ => _.Render(It.IsAny<string>(), It.IsAny<InlineQuote>()), Times.Never);
    }

    [Fact]
    public void Parse_ShouldRemoveUnusedTags()
    {
        // Arrange
        var body = "this is body {{QUOTE:slug2}}";
        var inlineQuotes = new List<InlineQuote>
            {
                new InlineQuote
                {
                    Author = "Test",
                    Image = "testUrl",
                    ImageAltText = "Test Alt",
                    Quote = "This is a quote",
                    Slug = "slug1"
                }
            };

        _viewRenderer.Setup(_ => _.Render(It.IsAny<string>(), It.IsAny<InlineQuote>())).Returns("test");

        // Act
        var bodyResult = _parser.Parse(body, inlineQuotes);

        // Assert
        bodyResult.Should().Be("this is body ");
    }
}
