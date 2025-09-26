namespace StockportWebappTests_Unit.Unit.Controllers;

public class ShedControllerTests
{
    private readonly ShedController _controller;
    private readonly Mock<IShedService> _mockShedService = new();
    private readonly Mock<IApplicationConfiguration> _mockConfig = new();
    private readonly Mock<IFilteredUrl> _mockFilteredUrl = new();
    private readonly Mock<IFeatureManager> _featureManager = new();

    public ShedControllerTests() =>
        _controller = new ShedController(_mockShedService.Object,
                                        _mockConfig.Object,
                                        _mockFilteredUrl.Object,
                                        _featureManager.Object);

    [Fact]
    public async Task Detail_ReturnsNotFound_WhenShedDoesNotExist()
    {
        // Arrange
        _mockShedService
            .Setup(service => service.GetSHEDDataByHeRef(It.IsAny<string>()))
            .ReturnsAsync(new List<ShedItem>());

        // Act
        IActionResult result = await _controller.Detail("non-existent-shed");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Detail_ReturnsViewResult_WithShedItem()
    {
        // Arrange
        var shedItem = new ShedItem { Name = "Existing Shed" };
        _mockShedService
            .Setup(service => service.GetSHEDDataByHeRef(It.IsAny<string>()))
            .ReturnsAsync(new List<ShedItem> { shedItem });

        // Act
        IActionResult result = await _controller.Detail("existing-shed");

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(shedItem, viewResult.Model);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithShedViewModel()
    {
        // Arrange
        List<ShedItem> shedItems = new()
        {
            new ShedItem { Name = "Shed 1" },
            new ShedItem { Name = "Shed 2" }
        };

        _mockShedService
            .Setup(service => service.GetSHEDDataByNameWardsAndListingTypes(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
            .ReturnsAsync(shedItems);

        // Act
        IActionResult result = await _controller.Index(new List<string>(), new List<string>(), "searchTerm", 1, 10);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        ShedViewModel model = Assert.IsType<ShedViewModel>(viewResult.Model);
        Assert.Equal(2, model.ShedItems.Count());
    }

    [Fact]
    public async Task Index_SetsAppliedFilters_Correctly()
    {
        // Arrange
        List<ShedItem> shedItems = new()
        {
            new ShedItem { Name = "Shed 1" },
            new ShedItem { Name = "Shed 2" }
        };

        _mockShedService
            .Setup(service => service.GetSHEDDataByNameWardsAndListingTypes(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
            .ReturnsAsync(shedItems);

        List<string> wards = new() { "Ward1", "Ward2" };
        List<string> listingTypes = new() { "Type1" };

        // Act
        IActionResult result = await _controller.Index(wards, listingTypes, "searchTerm", 1, 10);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        ShedViewModel model = Assert.IsType<ShedViewModel>(viewResult.Model);
        Assert.Contains("Ward1", model.AppliedFilters);
        Assert.Contains("Ward2", model.AppliedFilters);
        Assert.Contains("Type1", model.AppliedFilters);
    }

    [Fact]
    public async Task Index_PaginatesResults_Correctly()
    {
        // Arrange
        List<ShedItem> shedItems = Enumerable.Range(1, 25).Select(i => new ShedItem { Name = $"Shed {i}" }).ToList();

        _mockShedService
            .Setup(service => service.GetSHEDDataByNameWardsAndListingTypes(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
            .ReturnsAsync(shedItems);

        int pageSize = 10;

        // Act
        IActionResult result = await _controller.Index(new List<string>(), new List<string>(), "searchTerm", 2, pageSize);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        ShedViewModel model = Assert.IsType<ShedViewModel>(viewResult.Model);
        Assert.Equal(pageSize, model.ShedItems.Count());
        Assert.Equal("Shed 11", model.ShedItems.First().ShedItem.Name);
        Assert.Equal("Shed 20", model.ShedItems.Last().ShedItem.Name);
    }

    [Fact]
    public async Task Index_ReturnsEmptyModel_WhenNoResults()
    {
        // Arrange
        _mockShedService
            .Setup(service => service.GetSHEDDataByNameWardsAndListingTypes(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
            .ReturnsAsync(new List<ShedItem>());

        // Act
        IActionResult result = await _controller.Index(new List<string>(), new List<string>(), "searchTerm", 1, 10);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        ShedViewModel model = Assert.IsType<ShedViewModel>(viewResult.Model);
        Assert.Empty(model.ShedItems);
    }
}