using Filter = StockportWebapp.Model.Filter;

namespace StockportWebappTests_Unit.Unit.Controllers;

public class DirectoryControllerTest
{
    private readonly DirectoryController _directoryController;
    private Mock<IDirectoryService> _directoryService = new();


    private readonly List<Filter> filtersList = new() {
        new() {
            Slug = "value1",
            Title = "title",
            DisplayName = "display name",
            Theme = "theme1",
            Highlight = false
        },
        new() {
            Slug = "value2",
            Title = "title",
            DisplayName = "display name",
            Theme = "theme2",
            Highlight = false
        },
        new() {
            Slug = "value3",
            Title = "title",
            DisplayName = "display name",
            Theme = "theme3",
            Highlight = false
        }
    };

    private readonly List<FilterTheme> filterThemes = new(){
        new(){
            Title = "Theme title"
        }
    };
    
    private readonly DirectoryEntry directoryEntry = new()
    {
        Slug = "slug",
        Name = "name",
        Provider = "provider",
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
        Address = "address",
    };

    private readonly Directory directory = new()
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
        SubDirectories = new List<Directory>(),
    };

    public DirectoryControllerTest()
    {
        _directoryController = new DirectoryController(_directoryService.Object);
        processedDirectoryWithSubdirectories.Entries = new List<DirectoryEntry>() { directoryEntry };
        processedDirectoryWithSubdirectories.SubDirectories = new List<Directory>() { directory };
        
        string[] filters = { "value1", "value2", "value3" };

        _directoryService.Setup(_ => _.GetFilteredEntryForDirectories(directory)).Returns(new List<DirectoryEntry> { directoryEntry });
        _directoryService.Setup(_ => _.GetAllFilterThemes(new List<DirectoryEntry>())).Returns(filterThemes);
        _directoryService.Setup(_ => _.GetAppliedFilters(filters, filterThemes)).Returns(filtersList);
        _directoryService.Setup(_ => _.GetOrderedEntries(new List<DirectoryEntry> { directoryEntry }, "Name A to Z")).Returns(new List<DirectoryEntry> { directoryEntry, directoryEntry });
    }

    [Fact]
    public async Task Directory_ShouldReturnUnsuccessfulStatusCode()
    {
        // Arrange
        _ = _directoryService.Setup(_ => _.Get<Directory>("not-slug")).ReturnsAsync((Directory)null);

        // Act
        var result = await _directoryController.Directory("slug");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Directory_ShouldReturnViewModel_And_CorrectView(){
        // Arrange
        _ = _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(directory);

        // Act
        var result = await _directoryController.Directory("slug") as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;
        
        // Assert
        Assert.NotNull(result.Model);
        Assert.Equal("slug", model.Directory.Slug);
        Assert.Equal("results", result.ViewName);
    }

    [Fact]
    public async Task Directory_ShouldReturnCorrectView_WithSubdirectories(){
        // Arrange
        _ = _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(processedDirectoryWithSubdirectories);

        // Act
        var result = await _directoryController.Directory("slug") as ViewResult;

        // Assert
        Assert.Null(result.ViewName);
        Assert.NotNull(result.Model);
    }
    
    [Fact]
    public async Task DirectoryResults_ShouldReturnCorrectView_WithoutSubdirectories()
    {
        // Arrange
        _ = _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(directory);

        // Act
        var result = await _directoryController.DirectoryResults("slug", Array.Empty<string>(), string.Empty) as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;

        // Assert
        Assert.Equal("results", result.ViewName);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task DirectoryResults_ShouldReturnUnsuccessfulStatusCode(){
        // Arrange
        _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync((Directory)null);

        // Act
        var result = await _directoryController.DirectoryResults("slug", Array.Empty<string>(), string.Empty);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DirectoryResults_ShouldReturnCorrectView_WithFilters(){
        // Arrange
        string[] filters = { "value1", "value2", "value3" };
        _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(directory);

        // Act
        var result = await _directoryController.DirectoryResults("slug", filters, string.Empty) as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;

        // Assert
        Assert.Equal("results", result.ViewName);
        Assert.Equal(filterThemes, model.AllFilterThemes);
        Assert.Equal(filterThemes.First().Filters, model.AllFilterThemes.First().Filters);
        Assert.Equal("slug", model.Directory.Slug);
        Assert.Equal(filtersList, model.AppliedFilters);
    }

    [Fact]
    public async Task DirectoryEntry_ShouldReturnViewModel(){
        // Arrange
        _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(directory);

        _directoryService.Setup(_ => _.GetEntry<DirectoryEntry>(It.IsAny<string>()))
            .ReturnsAsync(directoryEntry);
        // Act
        var result = await _directoryController.DirectoryEntry("slug/entry-slug") as ViewResult;
        var model = result.ViewData.Model as DirectoryViewModel;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("slug", model.Directory.Slug);
        Assert.Null(result.ViewName);
    }

    [Fact]
    public async Task DirectoryEntry_ShouldReturnUnsuccessfulStatusCode()
    {
        _directoryService.Setup(_ => _.GetEntry<DirectoryEntry>(It.IsAny<string>())).ReturnsAsync((DirectoryEntry)null);

        // Act
        var result = await _directoryController.DirectoryEntry("slug/entry-slug") as HttpResponse;

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DirectoryAsKml_ShouldReturnUnsuccessfulStatusCode()
    {
        // Arrange
        _ = _directoryService.Setup(_ => _.Get<Directory>("not-slug")).ReturnsAsync((Directory)null);

        // Act
        var result = await _directoryController.DirectoryAsKml("slug");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DirectoryAsKml_ShouldReturnContentInKmlFormat(){
        // Act
        var result = await _directoryController.DirectoryAsKml("slug");

        // Assert
        Assert.NotNull(result);
    }
}