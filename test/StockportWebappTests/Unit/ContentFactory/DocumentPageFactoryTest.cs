namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class DocumentPageFactoryTest
{
    private readonly DocumentPageFactory _documentFactory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private const string AboutTheDocument = "<p>about the document</p>";
    private const string AwsDocuments = "<p>aws documents</p>";
    private const string RequestAnAccessibleFormatContactInformation = "<p>Request an accessible format contact information</p>";
    private const string FurtherInformation = "<p>further information</p>";
    private readonly DocumentPage _documentPage;

    public DocumentPageFactoryTest()
    {
        _documentFactory = new(_markdownWrapper.Object);
        _documentPage = new()
        {
            Title = "title",
            Slug = "slug",
            Teaser = "teaser",
            AboutTheDocument = AboutTheDocument,
            AwsDocuments = AwsDocuments,
            RequestAnAccessibleFormatContactInformation = RequestAnAccessibleFormatContactInformation,
            FurtherInformation = FurtherInformation,
            Breadcrumbs = new List<Crumb>()
        };
    }

    [Fact]
    public void Build_ShouldSetTheCorrespondingFieldsForADocumentPage()
    {
        // Act
        DocumentPage result = _documentFactory.Build(_documentPage);

        // Assert
        Assert.Equal("title", result.Title);
        Assert.Equal("slug", result.Slug);
        Assert.Equal(new List<Crumb>(), result.Breadcrumbs);
    }

    [Fact]
    public void Build_ShouldProcessFieldsWithMarkdown()
    {
        // Act
        _documentFactory.Build(_documentPage);

        // Assert
        _markdownWrapper.Verify(markdownWrapper => markdownWrapper.ConvertToHtml(AboutTheDocument), Times.Once);
        _markdownWrapper.Verify(markdownWrapper => markdownWrapper.ConvertToHtml(AwsDocuments), Times.Once);
        _markdownWrapper.Verify(markdownWrapper => markdownWrapper.ConvertToHtml(RequestAnAccessibleFormatContactInformation), Times.Once);
        _markdownWrapper.Verify(markdownWrapper => markdownWrapper.ConvertToHtml(FurtherInformation), Times.Once);
    }
}