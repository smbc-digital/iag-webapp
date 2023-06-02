namespace StockportWebappTests_Unit.Unit.Controllers;

public class ContactUsAreaControllerTests
{
    private readonly ContactUsAreaController _controller;
    private readonly Mock<IProcessedContentRepository> _repository = new();

    public ContactUsAreaControllerTests()
    {
        _controller = new ContactUsAreaController(_repository.Object);
    }

    [Fact]
    public async Task Index_ShouldCallRepository_AndReturnView()
    {
        // Setup 
        var contactUsArea = new ContactUsArea("title",
            "contact-us-area",
            "contact-us-area",
            new List<Crumb>(),
            new List<Alert>(),
            new List<SubItem>(),
            new List<ContactUsCategory>(),
            "insetTextTitle",
            "InsetTextBody",
            string.Empty);
        var response = new HttpResponse((int)HttpStatusCode.OK, contactUsArea, string.Empty);

        _repository.Setup(_ => _.Get<ContactUsArea>(It.IsAny<string>(), null))
                   .ReturnsAsync(response);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);

        _repository.Verify(_ => _.Get<ContactUsArea>(It.IsAny<string>(), null), Times.Once);
    }
}
