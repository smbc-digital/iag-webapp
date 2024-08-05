namespace StockportWebappTests_Unit.Unit.Controllers;

public class DocumentControllerTests
{
    private readonly Mock<IProcessedContentRepository> _mockRepository = new();
    private readonly Mock<IContactUsMessageTagParser> _mockContactUsMessageParser = new();
    private readonly DocumentController _controller;

    public DocumentControllerTests()
    {
        _controller = new DocumentController(
            _mockRepository.Object,
            _mockContactUsMessageParser.Object
        );
    }

    [Fact]
    public async Task Index_ReturnsUnsuccessfulResponse_WhenDocumentPageHttpResponseIsUnsuccessful()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<DocumentPage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(500, null));

        // Act
        StatusCodeResult result = await _controller.Index("some-slug") as StatusCodeResult;

        // Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task Index_ReturnsViewWithViewModel_WhenDocumentPageHttpResponseIsSuccessful()
    {
        // Arrange
        string documentPageSlug = "some-slug";
        DocumentPage documentPage = new()
        {
            Slug = "some-slug",
            Title = "title",
            Teaser = "teaser"
        };

        _mockRepository
            .Setup(_ => _.Get<DocumentPage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, documentPage));

        // Act
        ViewResult result = await _controller.Index(documentPageSlug) as ViewResult;
        DocumentPageViewModel resultModel = result.ViewData.Model as DocumentPageViewModel;

        // Assert
        Assert.Equal(documentPage, resultModel.DocumentPage);
    }
}