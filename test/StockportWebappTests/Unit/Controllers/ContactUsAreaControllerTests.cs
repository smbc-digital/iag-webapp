namespace StockportWebappTests_Unit.Unit.Controllers;

public class ContactUsAreaControllerTests
{
    private readonly ContactUsAreaController _controller;
    private readonly Mock<IProcessedContentRepository> _repository = new();
    private readonly ContactUsArea contactUsArea = new("title",
                                                    "contact-us-area",
                                                    new List<Crumb>(),
                                                    new List<Alert>(),
                                                    new List<SubItem>(),
                                                    new List<ContactUsCategory>(),
                                                    "insetTextTitle",
                                                    "InsetTextBody",
                                                    string.Empty);

    public ContactUsAreaControllerTests() =>
        _controller = new ContactUsAreaController(_repository.Object);

    [Fact]
    public async Task Index_ShouldCallRepository_AndReturnView()
    {
        // Setup 
        HttpResponse response = new((int)HttpStatusCode.OK, contactUsArea, string.Empty);

        _repository
            .Setup(repo => repo.Get<ContactUsArea>(It.IsAny<string>(), null))
            .ReturnsAsync(response);

        // Act
        IActionResult result = await _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
        _repository.Verify(repo => repo.Get<ContactUsArea>(It.IsAny<string>(), null), Times.Once);
    }

    [Fact]
    public async Task Index_ReturnsNotFound()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<ContactUsArea>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(404, "error", string.Empty));

        // Act
        StatusCodeResult result = await _controller.Index() as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }
}