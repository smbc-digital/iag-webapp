namespace StockportWebappTests_Unit.Unit.Controllers;

public class DocumentControllerTests
{
    private readonly Mock<IDocumentPageRepository> _mockRepository = new();
    private readonly Mock<IContactUsMessageTagParser> _mockContactUsMessageParser = new();
    private readonly DocumentController _controller;

    public DocumentControllerTests() =>
        _controller = new DocumentController(_mockRepository.Object, _mockContactUsMessageParser.Object);

    [Fact]
    public async Task Index_ReturnsUnsuccessfulResponse_WhenDocumentPageHttpResponseIsUnsuccessful()
    {
        // Arrange
        _mockRepository
            .Setup(mockRepository => mockRepository.Get(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse(500, "error", string.Empty));

        // Act
        StatusCodeResult result = await _controller.Index("some-slug") as StatusCodeResult;

        // Assert
        Assert.Equal(500, result.StatusCode);
    }

    [Fact]
    public async Task Index_ReturnsViewWithViewModel_WhenDocumentPageHttpResponseIsSuccessful()
    {
        // Arrange
        DocumentPage documentPage = new()
        {
            Slug = "some-slug",
            Title = "title",
            Teaser = "teaser"
        };

        _mockRepository
            .Setup(mockRepository => mockRepository.Get(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse(200, documentPage, string.Empty));

        // Act
        ViewResult result = await _controller.Index("some-slug") as ViewResult;
        DocumentPageViewModel resultModel = result.ViewData.Model as DocumentPageViewModel;

        // Assert
        Assert.Equal(documentPage, resultModel.DocumentPage);
    }
}