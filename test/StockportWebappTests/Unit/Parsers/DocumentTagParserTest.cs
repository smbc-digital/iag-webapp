namespace StockportWebappTests_Unit.Unit.Parsers;

public class DocumentTagParserTest
{
    private readonly DocumentTagParser _documentTagParser;
    private readonly Mock<IViewRender> _viewRenderer = new();

    public DocumentTagParserTest() =>
        _documentTagParser = new(_viewRenderer.Object);

    [Fact]
    public void ShouldReplaceDocumentTagWithDocumentView()
    {
        // Arrange
        Document document = new("title", 2434, DateTime.Now, "url", "fileName1.jpg", string.Empty, "media");
        List<Document> documents = new() { document };

        _viewRenderer
            .Setup(renderer => renderer.Render("Document", document))
            .Returns("RENDERED DOCUMENT CONTENT");

        // Act
        string parsedHtml = _documentTagParser.Parse("this is some test {{PDF:fileName1.jpg}}", documents);

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("Document", document), Times.Once);
        Assert.Contains("RENDERED DOCUMENT CONTENT", parsedHtml);
    }

    [Fact]
    public void ShouldRemoveDocumentTagsThatDontExist()
    {
        // Act
        string parsedHtml = _documentTagParser.Parse("this is some test {{PDF:some-pdf.pdf}}", new List<Document>());

        // Assert
        _viewRenderer.Verify(renderer => renderer.Render("Document", It.IsAny<Document>()), Times.Never);
        Assert.Equal("this is some test ", parsedHtml);
    }
}