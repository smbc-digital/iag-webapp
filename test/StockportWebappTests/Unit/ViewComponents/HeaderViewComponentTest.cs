using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebappTests_Unit.Unit.ViewComponents;

public class HeaderViewComponentTest
{
    private readonly Mock<IRepository> _repository = new();
    private readonly HeaderViewComponent _headerViewComponent;
    private readonly Mock<ILogger<HeaderViewComponent>> _logger = new();

    public HeaderViewComponentTest() =>
        _headerViewComponent = new HeaderViewComponent(_repository.Object, _logger.Object);

    [Fact]
    public async Task ShouldReturnHeaderAsModelInView()
    {
        // Arrange
        SiteHeader header = new("title", new List<SubItem>(), "logo.jpg");

        _repository.Setup(repo => repo.Get<SiteHeader>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Successful(200, header));

        // Act
        ViewViewComponentResult result = await _headerViewComponent.InvokeAsync() as ViewViewComponentResult;
        SiteHeader headerModel = result.ViewData.Model as SiteHeader;
        
        // Assert
        Assert.IsType<SiteHeader>(headerModel);
        Assert.Equal(header, headerModel);
    }

    [Fact]
    public void ShouldThrowException_IfViewNotFound()
    {
        // Arrange
        _repository.Setup(repo => repo.Get<SiteHeader>(It.IsAny<string>(), It.IsAny<List<Query>>())).ReturnsAsync(HttpResponse.Successful(404, null));

        // Act & Assert
        Task<FileNotFoundException> exception = Assert.ThrowsAsync<FileNotFoundException>(_headerViewComponent.InvokeAsync);
    }
}