namespace StockportWebappTests_Unit.Unit.Controllers;

public class DirectoryControllerTest
{
    private readonly DirectoryController _directoryController;
    private Mock<IDirectoryRepository> _directoryRepository = new();

    private readonly ProcessedDirectory processedDirectory = new
    (
        "title",
        "slug",
        "contentfulId",
        "teaser",
        "metaDescription",
        "backgroundImage",
        "body",
        new CallToActionBanner(),
        new List<Alert>(),
        new List<DirectoryEntry>()
    );

    private readonly ProcessedDirectoryEntry processedDirectoryEntry = new
   (
        "slug",
        "title",
        "body",
        "teaser",
        "metaDescription",
        new List<FilterTheme>(),
        new List<MinimalDirectory>()
   );

    public DirectoryControllerTest()
    {
        _directoryController = new DirectoryController(_directoryRepository.Object);
    }

    [Fact]
    public async Task Directory_ShouldReturnUnsuccessfulStatusCode(){
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        var result = await _directoryController.Directory("slug") as HttpResponse;

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task Directory_ShouldReturnViewModel(){
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedDirectory, string.Empty));

        // Act
        var result = await _directoryController.Directory("slug") as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("slug", model.Directory.Slug);
    }

    [Fact]
    public async Task DirectoryEntry_ShouldReturnUnsuccessfulStatusCode(){
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        var result = await _directoryController.DirectoryEntry("slug", "entry-slug") as HttpResponse;
        
        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task DirectoryEntry_ShouldReturnViewModel(){
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedDirectory, string.Empty));

        _directoryRepository.Setup(_ => _.GetEntry<DirectoryEntry>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedDirectoryEntry, string.Empty));

        // Act
        var result = await _directoryController.DirectoryEntry("slug", "entry-slug") as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("slug", model.Directory.Slug);
    }

    [Fact]
    public async Task DirectoryAsKml_ShouldReturnUnsuccessfulStatusCode(){
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        var result = await _directoryController.DirectoryAsKml("slug") as HttpResponse;

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task DirectoryAsKml_ShouldReturnContentInKmlFormat(){
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedDirectory, string.Empty));

        // Act
        var result = await _directoryController.DirectoryAsKml("slug");

        // Assert
        Assert.NotNull(result);
    }
}