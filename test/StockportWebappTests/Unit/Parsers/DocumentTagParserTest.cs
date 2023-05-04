using StockportWebapp.TagParsers;

namespace StockportWebappTests_Unit.Unit.Parsers;

public class DocumentTagParserTest
{
    private readonly Mock<IViewRender> _viewRenderer;
    private readonly DocumentTagParser _documentTagParser;
    private readonly Mock<ILogger<DocumentTagParser>> _mockLogger;

    public DocumentTagParserTest()
    {
        _viewRenderer = new Mock<IViewRender>();
        _mockLogger = new Mock<ILogger<DocumentTagParser>>();
        _documentTagParser = new DocumentTagParser(_viewRenderer.Object, _mockLogger.Object);
    }

    [Fact]
    public void ShouldReplaceDocumentTagWithDocumentView()
    {
        var content = "this is some test {{PDF:fileName1.jpg}}";
        var document = new Document("title", 2434, DateTime.Now, "url", "fileName1.jpg", string.Empty, "media");
        var documents = new List<Document>() { document };
        var renderResult = "RENDERED DOCUMENT CONTENT";

        _viewRenderer.Setup(o => o.Render("Document", document)).Returns(renderResult);

        var parsedHtml = _documentTagParser.Parse(content, documents);

        _viewRenderer.Verify(o => o.Render("Document", document), Times.Once);
        parsedHtml.Should().Contain(renderResult);
    }

    [Fact]
    public void ShouldRemoveDocumentTagsThatDontExist()
    {
        var content = "this is some test {{PDF:some-pdf.pdf}}";

        var parsedHtml = _documentTagParser.Parse(content, new List<Document>());

        _viewRenderer.Verify(o => o.Render("Document", It.IsAny<Document>()), Times.Never);
        parsedHtml.Should().Be("this is some test ");
    }
}
