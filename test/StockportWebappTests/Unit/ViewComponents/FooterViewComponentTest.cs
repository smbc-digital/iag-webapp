namespace StockportWebappTests_Unit.Unit.ViewComponents;

public class FooterViewComponentTest
{
    private readonly Mock<IRepository> _repository = new();
    private readonly FooterViewComponent _footerViewComponent;
    private readonly Mock<ILogger<FooterViewComponent>> _logger = new();
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();

    public FooterViewComponentTest() =>
        _footerViewComponent = new(_repository.Object, _logger.Object, _markdownWrapper.Object);

    [Fact]
    public async Task ShouldReturnFooterAsModelInView()
    {
        // Arrange
        Footer footer = new("Title", "Slug", new List<SubItem>(), new List<SocialMediaLink>(), string.Empty, string.Empty, string.Empty);
        _repository
            .Setup(repo => repo.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, footer));

        // Act
        ViewViewComponentResult result = await _footerViewComponent.InvokeAsync() as ViewViewComponentResult;
        Footer footerModel = result.ViewData.Model as Footer;
        
        // Assert
        Assert.IsType<Footer>(footerModel);
        Assert.Equal(footer, footerModel);
    }

    [Fact]
    public async Task ShouldNotReturnAFooterInViewIfViewNotFound()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Failure(404, "No Footer Found"));

        // Act
        ViewViewComponentResult result = await _footerViewComponent.InvokeAsync() as ViewViewComponentResult;

        // Assert
        Assert.Null(result.ViewData.Model);
    }

    [Fact]
    public async Task ShouldCallMarkdowWrapperForFooterContent()
    {
        // Arrange
        Footer footer = new("Title",
                            "Slug",
                            new List<SubItem>(),
                            new List<SocialMediaLink>(),
                            "<a>footer content 1</a>",
                            "<h2>footer content 2</h2>",
                            "<p>footer content 3</p>");
        _repository
            .Setup(repo => repo.Get<Footer>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, footer));
        
        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(footer.FooterContent1))
            .Returns("<a>footer content 1</a>");

        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(footer.FooterContent2))
            .Returns("<h2>footer content 2</h2>");
        
        _markdownWrapper
            .Setup(wrapper => wrapper.ConvertToHtml(footer.FooterContent3))
            .Returns("<p>footer content 3</p>");

        // Act
        ViewViewComponentResult result = await _footerViewComponent.InvokeAsync() as ViewViewComponentResult;
        Footer footerModel = result.ViewData.Model as Footer;

        // Assert
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml(footerModel.FooterContent1), Times.Once);
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml(footerModel.FooterContent2), Times.Once);
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml(footerModel.FooterContent3), Times.Once);
    }
}