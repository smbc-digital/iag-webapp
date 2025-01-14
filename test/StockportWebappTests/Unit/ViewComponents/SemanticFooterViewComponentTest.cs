namespace StockportWebappTests_Unit.Unit.ViewComponents;

public class SemanticFooterViewComponentTest
{
    private readonly SemanticFooterViewComponent _semanticFooterViewComponent;
    private readonly Mock<IRepository> _repository = new();
    private readonly Mock<ILogger<SemanticFooterViewComponent>> _logger = new();

    public SemanticFooterViewComponentTest() =>
        _semanticFooterViewComponent = new(_repository.Object, _logger.Object);

    [Fact]
    public async Task ShouldReturnFooterAsModelInView()
    {
        // Arrange
        Footer footer = new("Title", "Slug", new List<SubItem>(), new List<SocialMediaLink>(), string.Empty, string.Empty, string.Empty);
        _repository
            .Setup(repo => repo.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, footer));

        // Act
        ViewViewComponentResult result = await _semanticFooterViewComponent.InvokeAsync() as ViewViewComponentResult;

        // Assert
        Footer footerModel = result.ViewData.Model as Footer;
        Assert.IsType<Footer>(result.ViewData.Model);
        Assert.Equal(footer, footerModel);
        LogTesting.Assert(_logger, LogLevel.Information, "Call to retrieve the footer");
    }

    [Fact]
    public async Task ShouldNotReturnAFooterInViewIfViewNotFound()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Failure(404, "No Footer Found"));

        // Act
        ViewViewComponentResult result = await _semanticFooterViewComponent.InvokeAsync() as ViewViewComponentResult;

        // Assert
        Assert.Null(result.ViewData.Model);
        Assert.Equal("NoFooterFound", result.ViewName);
        LogTesting.Assert(_logger, LogLevel.Information, "Call to retrieve the footer");
    }
}