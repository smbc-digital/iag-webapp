namespace StockportWebappTests_Unit.Unit.Controllers;

public class DirectoryControllerTest
{
    private readonly DirectoryController _directoryController;
    private Mock<IDirectoryRepository> _directoryRepository = new();

    private readonly DirectoryEntry processedDirectoryEntry = new()
    {
        Slug = "slug",
        Name = "name",
        Description = "description",
        Teaser = "teaser",
        MetaDescription = "metaDescription",
        Themes = new List<FilterTheme>(),
        Directories = new List<MinimalDirectory>(),
        Alerts = new List<Alert>(),
        Branding = new List<GroupBranding>(),
        MapPosition = new MapPosition(),
        PhoneNumber = "phone number",
        Email = "email",
        Website = "website",
        Twitter = "twitter",
        Facebook = "facebook",
        Address = "address"
    };

    private readonly Directory processedDirectory = new()
    {
        Title = "title",
        Slug = "slug",
        ContentfulId = "contentfulId",
        Teaser = "teaser",
        MetaDescription = "metaDescription",
        BackgroundImage = "backgroundImage",
        Body = "body",
        CallToAction = new CallToActionBanner(),
        Alerts = new List<Alert>(),
        Entries  = new List<DirectoryEntry>(),
        SubDirectories = new List<Directory>(),
    };

    private readonly Directory processedDirectoryWithSubdirectories = new()
    {
        Title = "title",
        Slug = "slug",
        ContentfulId = "contentfulId",
        Teaser = "teaser",
        MetaDescription = "metaDescription",
        BackgroundImage = "backgroundImage",
        Body = "body",
        CallToAction = new CallToActionBanner(),
        Alerts = new List<Alert>(),
        Entries = new List<DirectoryEntry>() {  } ,
        SubDirectories = new List<Directory>(),
    };

    public DirectoryControllerTest()
    {
        _directoryController = new DirectoryController(_directoryRepository.Object);
        processedDirectoryWithSubdirectories.Entries = new List<DirectoryEntry>() { processedDirectoryEntry };
        processedDirectoryWithSubdirectories.SubDirectories = new List<Directory>() { processedDirectory };
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
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedDirectoryWithSubdirectories, string.Empty));

        // Act
        var result = await _directoryController.Directory("slug") as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("slug", model.Directory.Slug);
        Assert.Null(result.ViewName);
    }

    [Fact]
    public async Task Directory_ShouldReturnCorrectView()
    {
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedDirectory, string.Empty));

        // Act
        var result = await _directoryController.Directory("slug") as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;

        // Assert
        Assert.Equal("results", result.ViewName);
    }

    [Fact]
    public async Task DirectoryResults_ShouldReturnCorrectView_RegardlessOfSubdirectories()
    {
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedDirectory, string.Empty));

        // Act
        var result = await _directoryController.DirectoryResults("slug") as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;

        // Assert
        Assert.Equal("results", result.ViewName);
    }

    [Fact]
    public async Task DirectoryResults_ShouldReturnCorrectView_WithoutSubdirectories()
    {
        // Arrange
        _directoryRepository.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, processedDirectoryWithSubdirectories, string.Empty));

        // Act
        var result = await _directoryController.DirectoryResults("slug") as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;

        // Assert
        Assert.Equal("results", result.ViewName);

    }

    [Fact]
    public async Task DirectoryResults_ShouldReturnCorrectView_WithSubdirectories(){
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
        Assert.Null(result.ViewName);
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