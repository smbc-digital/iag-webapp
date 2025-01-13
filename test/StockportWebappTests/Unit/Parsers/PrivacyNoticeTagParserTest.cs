namespace StockportWebappTests_Unit.Unit.Parsers;

public class PrivacyNoticeTagParserTest
{
    private readonly Mock<IViewRender> _viewRenderer = new();
    private readonly PrivacyNoticeTagParser _privacyNoticeTagParser;

    public PrivacyNoticeTagParserTest() =>
        _privacyNoticeTagParser = new(_viewRenderer.Object);

    [Fact]
    public void Parse_ShouldRenderIfPrivacyExists()
    {
        // Arrange
        List<PrivacyNotice> privacyNotice = new()
        {
            new()
            {
                Title = "title",
                Slug = "slug"
            }
        };

        _viewRenderer
            .Setup(renderer => renderer.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>()))
            .Returns(string.Empty);

        // Act
        _privacyNoticeTagParser.Parse("{{PrivacyNotice:title}}", privacyNotice);
        
        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>()), Times.Once);
    }

    [Fact]
    public void Parse_ShouldNotRenderIfPrivacyNoticeDoesNotExist()
    {
        // Arrange
        List<PrivacyNotice> privacyNotice = new()
        {
            new()
            {
                Title = "title",
                Slug = "slug"
            }
        };

        _viewRenderer
            .Setup(renderer => renderer.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>()))
            .Returns(string.Empty);
        
        // Act
        _privacyNoticeTagParser.Parse("{{PrivacyNotice:category}}", privacyNotice);
        
        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>()), Times.Never);
    }

    [Fact]
    public void Parse_ShouldReplaceContentIfPrivacyNoticeExists()
    {
        // Arrange
        List<PrivacyNotice> privacyNotice = new()
        {
            new()
            {
                Title = "title",
                Slug = "slug"
            }
        };

        _viewRenderer
            .Setup(renderer => renderer.Render("PrivacyNotice", It.IsAny<IEnumerable<PrivacyNotice>>()))
            .Returns("<h1>title</h1>");

        // Act
        string result = _privacyNoticeTagParser.Parse("{{PrivacyNotice:title}}", privacyNotice);
        
        // Assert
        Assert.Equal("<h1>title</h1>", result);
    }
}