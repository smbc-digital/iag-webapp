namespace StockportWebappTests_Unit.Unit.Controllers;

public class LandingPageControllerTest
{
    private readonly LandingPageController _landingPageController;
    private readonly Mock<IRepository> _repository = new();
    
    private readonly LandingPage landingPage = new()
    {
        Slug = "slug",
        Title = "title",
        Subtitle = "subtitle",
        Breadcrumbs = new List<Crumb>(),
        Alerts = new List<Alert>(),
        Teaser = "teaser",
        MetaDescription = "metaDescription",
        Image = new MediaAsset(),
        HeaderType = "header type",
        HeaderImage = "header image",
        ContentBlocks = new List<SubItem>()
    };

    public LandingPageControllerTest()
    {
        _repository
            .Setup(_ => _.Get<LandingPage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, landingPage));

        _landingPageController = new LandingPageController(_repository.Object);
    }

    [Fact]
    public async Task Index_ShouldReturnNotFoundStatusCode()
    {
        // Arrange
        _repository
            .Setup(_ => _.Get<LandingPage>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Failure(404, "Error"));

        // Act
        StatusCodeResult result = await _landingPageController.Index("slug") as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task Index_ShouldReturnView_WithLandingPageViewModel(){
        // Arrange
        LandingPageViewModel expectedViewModel = new(landingPage);

        // Act
        ViewResult result = await _landingPageController.Index("slug") as ViewResult;
        LandingPageViewModel actualViewModel = result.Model as LandingPageViewModel;

        // Assert
        Assert.Null(result.ViewName);
        Assert.Equal(expectedViewModel.LandingPage, actualViewModel.LandingPage);
    }
}