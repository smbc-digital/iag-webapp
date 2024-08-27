using Filter = StockportWebapp.Model.Filter;

namespace StockportWebappTests_Unit.Unit.Controllers;

public class DirectoryControllerTest
{
    private readonly DirectoryController _directoryController;
    private readonly Mock<IDirectoryService> _directoryService = new();
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
        Title = "title directory",
        Slug = "slug-directory",
        ContentfulId = "contentfulId",
        Teaser = "teaser directory",
        MetaDescription = "metaDescription directory",
        BackgroundImage = "backgroundImage directory",
        Body = "body directory",
        CallToAction = new CallToActionBanner(),
        Alerts = new List<Alert>(),
        Entries  = new List<DirectoryEntry>(),
        SubDirectories = new List<Directory>(),
        PinnedEntries = new List<DirectoryEntry>(),
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
        directory.PinnedEntries = new List<DirectoryEntry>() { directoryEntry };
        processedDirectoryWithSubdirectories.Entries = new List<DirectoryEntry>() { directoryEntry };
        processedDirectoryWithSubdirectories.SubDirectories = new List<Directory>() { directory };
        processedDirectoryWithSubdirectories.SubItems = new List<SubItem>() {
            new("slug", "title", "teaser", "icon", "type", "contentType", "image", string.Empty, "test body", new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Teal, string.Empty, string.Empty)
        };
        
        string[] filters = { "value1", "value2", "value3" };

        _directoryService.Setup(_ => _.GetFilterThemes(new List<DirectoryEntry>())).Returns(filterThemes);
        _directoryService.Setup(_ => _.GetFilters(filters, filterThemes)).Returns(filtersList);
        _directoryService.Setup(_ => _.GetOrderedEntries(new List<DirectoryEntry> { directoryEntry }, "Name A to Z")).Returns(new List<DirectoryEntry> { directoryEntry, directoryEntry });
    }

    [Fact]
    public async Task Directory_ShouldReturnNotFoundStatusCode()
    {
        // Arrange
        _ = _directoryService.Setup(_ => _.Get<Directory>("not-slug")).ReturnsAsync((Directory)null);

        // Act
        IActionResult result = await _directoryController.Directory("slug");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Directory_ShouldReturnNotFoundIfSlugIsEmpty()
    {
        // Act
        IActionResult result = await _directoryController.Directory(string.Empty);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Directory_ShouldRedirectToResults_If_NoSubdirectories(){
        // Arrange
        _ = _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(directory);

        // Act
        IActionResult result = await _directoryController.Directory("slug");

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("DirectoryResults", ((RedirectToActionResult)await _directoryController.Directory("slug")).ActionName);
    }

    [Fact]
    public async Task Directory_ShouldReturnCorrectView_WithPrimaryItems(){
        // Arrange
        _ = _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(processedDirectoryWithSubdirectories);
        DirectoryViewModel expectedDirectoryViewModel = new() {
            Breadcrumbs = new List<Crumb>(),
            Slug = "slug",
            PrimaryItems = new NavCardList()
            {
                Items = new List<NavCard> { 
                    new("title", "/slug", "teaser", "image", "icon", EColourScheme.Teal)
                }
            }
        };

        // Act
        ViewResult result = await _directoryController.Directory("slug") as ViewResult;
        DirectoryViewModel actualViewModel = result.Model as DirectoryViewModel;

        // Assert
        Assert.Null(result.ViewName);
        Assert.Equal(expectedDirectoryViewModel.Breadcrumbs, actualViewModel.Breadcrumbs);
        Assert.Equal(expectedDirectoryViewModel.Slug, actualViewModel.Slug);
        Assert.Equal(expectedDirectoryViewModel.PrimaryItems.Items.First().Icon, actualViewModel.PrimaryItems.Items.First().Icon);
        Assert.Equal(expectedDirectoryViewModel.PrimaryItems.Items.First().Title, actualViewModel.PrimaryItems.Items.First().Title);
        Assert.Equal(expectedDirectoryViewModel.PrimaryItems.Items.First().Url, actualViewModel.PrimaryItems.Items.First().Url);
        Assert.Equal(expectedDirectoryViewModel.PrimaryItems.Items.First().ColourScheme, actualViewModel.PrimaryItems.Items.First().ColourScheme);
        Assert.Equal(expectedDirectoryViewModel.PrimaryItems.Items.First().Teaser, actualViewModel.PrimaryItems.Items.First().Teaser);
        Assert.Equal(expectedDirectoryViewModel.PrimaryItems.Items.First().Image, actualViewModel.PrimaryItems.Items.First().Image);
    }
    
    [Theory]
    [InlineData(new string[] { "value1", "value2", "value3" }, "Name A to Z", "description")]
    [InlineData(new string[] { "value1", "value2", "value3" }, "Name Z to A", "description")]
    [InlineData(new string[] { "value1", "value2", "value3" }, "", "tea")]
    public async Task DirectoryResults_ShouldReturnCorrectView(string[] filters, string orderBy, string searchTerm)
    {
        // Arrange
        processedDirectoryWithSubdirectories.PinnedEntries = new List<DirectoryEntry>() { directoryEntry };
        _ = _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(processedDirectoryWithSubdirectories);

        // Act
        ViewResult result = await _directoryController.DirectoryResults("slug", filters, orderBy, searchTerm, 0) as ViewResult;
        DirectoryViewModel model = result.ViewData.Model as DirectoryViewModel;

        // Assert
        Assert.Equal("results", result.ViewName);
        Assert.NotNull(model);
        Assert.Equal(filterThemes, model.AllFilterThemes);
        Assert.Equal(filterThemes.First().Filters, model.AllFilterThemes.First().Filters);
        Assert.Equal("slug", model.Slug);
        Assert.Equal(filtersList, model.AppliedFilters);
        Assert.Empty(model.PinnedEntries);
        _directoryService.Verify(service => service.GetSearchedEntryForDirectories(It.IsAny<IEnumerable<DirectoryEntry>>(), It.IsAny<string>()), Times.Exactly(1));
        _directoryService.Verify(service => service.GetFilteredEntries(It.IsAny<IEnumerable<DirectoryEntry>>(), It.IsAny<string[]>()), Times.Exactly(1));
    }

    [Fact]
    public async Task DirectoryResults_ShouldReturnCorrectView_WithEmptyFilters()
    {
        // Arrange
        processedDirectoryWithSubdirectories.PinnedEntries = new List<DirectoryEntry>() { directoryEntry };
        _ = _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(processedDirectoryWithSubdirectories);

        // Act
        ViewResult result = await _directoryController.DirectoryResults("slug", Array.Empty<string>(), string.Empty, string.Empty, 0) as ViewResult;
        DirectoryViewModel model = result.ViewData.Model as DirectoryViewModel;

        // Assert
        Assert.Equal("results", result.ViewName);
        Assert.NotNull(model);
    }

    [Fact]
    public async Task DirectoryResults_ShouldReturnNotFoundStatusCode(){
        // Arrange
        _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync((Directory)null);

        // Act
        IActionResult result = await _directoryController.DirectoryResults("slug", Array.Empty<string>(), string.Empty, string.Empty, 0);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DirectoryResults_ShouldReturnCorrectView_WithFilters(){
        // Arrange
        string[] filters = { "value1", "value2", "value3" };
        _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>())).ReturnsAsync(directory);

        // Act
        ViewResult result = await _directoryController.DirectoryResults("slug", filters, string.Empty, string.Empty, 0) as ViewResult;
        DirectoryViewModel model = result.ViewData.Model as DirectoryViewModel;

        // Assert
        Assert.Equal("results", result.ViewName);
        Assert.Equal(filterThemes, model.AllFilterThemes);
        Assert.Equal(filterThemes.First().Filters, model.AllFilterThemes.First().Filters);
        Assert.Equal("slug", model.Slug);
        Assert.Equal(filtersList, model.AppliedFilters);
        Assert.Empty(model.PinnedEntries);
    }

    [Fact]
    public async Task DirectoryEntry_ShouldReturnViewModel(){
        // Arrange
        _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(directory);

        _directoryService.Setup(_ => _.GetEntry<DirectoryEntry>(It.IsAny<string>()))
            .ReturnsAsync(directoryEntry);

        // Act
        ViewResult result = await _directoryController.DirectoryEntry("slug/entry-slug") as ViewResult;
        DirectoryEntryViewModel model = result.ViewData.Model as DirectoryEntryViewModel;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("slug/entry-slug", model.Slug);
        Assert.Null(result.ViewName);
    }

    [Fact]
    public async Task DirectoryEntry_ShouldReturnNotFoundStatusCode()
    {
        _directoryService.Setup(_ => _.GetEntry<DirectoryEntry>(It.IsAny<string>())).ReturnsAsync((DirectoryEntry)null);

        // Act
        IActionResult result = await _directoryController.DirectoryEntry("slug/entry-slug");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Directory_ShouldCallDirectoryService_IfSearchTermSpecified()
    {
        // Arrange
        _directoryService.Setup(_ => _.Get<Directory>(It.IsAny<string>()))
            .ReturnsAsync(directory);

        // Act
        IActionResult result = await _directoryController.DirectoryResults("slug", Array.Empty<string>(), string.Empty, "search me", 0);

        // Assert
        _directoryService.Verify(service => service.GetSearchedEntryForDirectories(It.IsAny<IEnumerable<DirectoryEntry>>(), It.IsAny<string>()), Times.Exactly(1));
    }
}