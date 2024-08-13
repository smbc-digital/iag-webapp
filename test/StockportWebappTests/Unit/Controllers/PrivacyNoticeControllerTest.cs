namespace StockportWebappTests_Unit.Unit.Controllers;

public class PrivacyNoticeControllerTest
{
    private readonly PrivacyNoticeController _privacyNoticeController;
    private readonly Mock<IProcessedContentRepository> _processedRepository = new();


    public PrivacyNoticeControllerTest() => _privacyNoticeController = new(_processedRepository.Object);

    [Fact]
    public async Task Index_ReturnsPrivacyNoticeView()
    {
        // Arrange
        ProcessedPrivacyNotice privacyNotice = new("slug", "title", "category", "purpose", "type of data",
            "legislation", "obtained", "externally shared", "retention period",
            false, false, new List<Crumb>(), new NullTopic()
        );

        _processedRepository
            .Setup(_ => _.Get<PrivacyNotice>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, privacyNotice));

        // Act
        ViewResult view = await _privacyNoticeController.Index("slug") as ViewResult;
        PrivacyNoticeViewModel viewModel = view.ViewData.Model as PrivacyNoticeViewModel;

        // Assert
        Assert.Equal(privacyNotice.Title, viewModel.PrivacyNotice.Title);
    }

    [Fact]
    public async Task Index_ReturnsNotFound()
    {
        // Arrange
        _processedRepository
            .Setup(_ => _.Get<PrivacyNotice>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(404, "error", string.Empty));

        // Act
        StatusCodeResult result = await _privacyNoticeController.Index("slug") as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }
}