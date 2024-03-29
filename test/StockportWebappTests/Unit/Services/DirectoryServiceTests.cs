using StockportWebapp.Model;

namespace StockportWebappTests_Unit.Unit.Services;

public class DirectoryServiceTests
{
    private readonly DirectoryService _service;
    private readonly Mock<IApplicationConfiguration> _mockApplicationConfiguration = new();
    private readonly Mock<MarkdownWrapper> _mockMarkdownWrapper = new();
    private readonly Mock<IRepository> _mockRepository = new();

    private readonly string[] filters = { "value1", "value2", "value3", "value11" };

    private readonly List<FilterTheme> filterThemes = new(){
        new(){
            Filters = new List<Filter>(){
                new() {
                    Slug = "value10",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title",
                    Highlight = false
                },
                new() {
                    Slug = "value20",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title",
                    Highlight = false
                },
                new() {
                    Slug = "value30",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title",
                    Highlight = false
                }
            },
            Title = "Theme title"
        }
    };

    private readonly List<FilterTheme> filterThemes2 = new(){
        new(){
            Filters = new List<Filter>(){
                new() {
                    Slug = "value1",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title2",
                    Highlight = false
                },
                new() {
                    Slug = "value2",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title2",
                    Highlight = false
                },
                new() {
                    Slug = "value3",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title2",
                    Highlight = false
                }
            },
            Title = "Theme title2"
        },
        new(){
            Filters = new List<Filter>(){
                new() {
                    Slug = "value11",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title1",
                    Highlight = false
                },
                new() {
                    Slug = "value21",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title1",
                    Highlight = false
                },
                new() {
                    Slug = "value31",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "Theme title1",
                    Highlight = false
                }
            },
            Title = "Theme title1"
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
        Tags = new List<string>()
    };

    private readonly DirectoryEntry directoryEntry2 = new()
    {
        Slug = "slug2",
        Name = "name2",
        Provider = "provider2",
        Description = "description2",
        Teaser = "teaser2",
        MetaDescription = "metaDescription2",
        Themes = new List<FilterTheme>(),
        Directories = new List<MinimalDirectory>(),
        Alerts = new List<Alert>(),
        Branding = new List<GroupBranding>(),
        MapPosition = new MapPosition(),
        PhoneNumber = "phone number2",
        Email = "email2",
        Website = "website2",
        Twitter = "twitter2",
        Facebook = "facebook2",
        Address = "address2",
        Tags = new List<string>()
    };

    private readonly DirectoryEntry directoryEntry3 = new()
    {
        Slug = "slug2",
        Name = "another name",
        Provider = "provider3",
        Description = "description3",
        Teaser = "teaser3",
        MetaDescription = "metaDescription3",
        Themes = new List<FilterTheme>(),
        Directories = new List<MinimalDirectory>(),
        Alerts = new List<Alert>(),
        Branding = new List<GroupBranding>(),
        MapPosition = new MapPosition(),
        PhoneNumber = "phone number2",
        Email = "email2",
        Website = "website2",
        Twitter = "twitter2",
        Facebook = "facebook2",
        Address = "address2",
        Tags = new List<string>() { "tagged" }
    };

    private readonly DirectoryEntry directoryEntry4 = new()
    {
        Slug = "slug2",
        Name = "another name",
        Provider = "provider3",
        Description = "description3",
        Teaser = "teaser3",
        MetaDescription = "metaDescription3",
        Themes = new List<FilterTheme>() { new FilterTheme { Title = "Test Theme", Filters = new List<Filter> { new Filter { Title="Test Theme: Test Filter", DisplayName="Test filter" } } } },
        Directories = new List<MinimalDirectory>(),
        Alerts = new List<Alert>(),
        Branding = new List<GroupBranding>(),
        MapPosition = new MapPosition(),
        PhoneNumber = "phone number2",
        Email = "email2",
        Website = "website2",
        Twitter = "twitter2",
        Facebook = "facebook2",
        Address = "address2",
        Tags = new List<string>() { "tagged" }
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
        SubDirectories = new List<Directory>(),
        Entries = new List<DirectoryEntry>()
    };

    public DirectoryServiceTests()
    {
        _mockMarkdownWrapper
            .Setup(_ => _.ConvertToHtml(It.IsAny<string>()))
            .Returns(It.IsAny<string>());

        _service = new DirectoryService(
            _mockApplicationConfiguration.Object,
            _mockMarkdownWrapper.Object,
            _mockRepository.Object
        );
    }

    [Fact]
    public async Task Get_ShouldReturnDirectory()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<Directory>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, new Directory { Title = It.IsAny<string>() }));

        // Act
        var result = await _service.Get<Directory>(It.IsAny<string>());

        // Assert
        Assert.IsType<Directory>(result);
    }

    [Fact]
    public void Get_ShouldThrowException_IfNotSuccessful()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<Directory>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Failure(404, It.IsAny<string>()));

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => await _service.Get<Directory>(It.IsAny<string>()));
    }

    [Fact]
    public async Task GetEntry_ShouldReturnDirectoryEntry()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<DirectoryEntry>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, new DirectoryEntry { Name = It.IsAny<string>() }));

        // Act
        var result = await _service.GetEntry<DirectoryEntry>(It.IsAny<string>());

        // Assert
        Assert.IsType<DirectoryEntry>(result);
    }

    [Fact]
    public async Task GetEntry_ShouldCall_MarkdownWrapper()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<DirectoryEntry>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, new DirectoryEntry { Name = It.IsAny<string>() }));

        // Act
        var result = await _service.GetEntry<DirectoryEntry>(It.IsAny<string>());

        // Assert
        _mockMarkdownWrapper.Verify(_ => _.ConvertToHtml(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public void GetEntry_ShouldThrowException_IfNotSuccessful()
    {
        // Arrange
        _mockRepository
            .Setup(_ => _.Get<DirectoryEntry>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Failure(404, It.IsAny<string>()));

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => await _service.GetEntry<DirectoryEntry>(It.IsAny<string>()));
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnAllEntriesForDirectory()
    {
        // Arrange
        directory.Entries = new List<DirectoryEntry>() { directoryEntry, directoryEntry2 };

        // Act
        var result = _service.GetFilteredEntries(directory.AllEntries);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnAllEntriesForDirectory_WithFilters()
    {
        // Arrange
        directoryEntry.Themes = filterThemes2;
        directory.Entries = new List<DirectoryEntry>() { directoryEntry, directoryEntry2 };

        // Act
        var result = _service.GetFilteredEntries(directory.AllEntries, filters);

        // Assert
        Assert.Single(result);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnEmptyList_If_DirectoryEntryNull()
    {
        // Act
        var result = _service.GetFilteredEntries(directory.AllEntries, filters);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnEmptyList_If_EntryHasNoFilters()
    {
        // Arrange
        directory.Entries = new List<DirectoryEntry>() { directoryEntry };

        // Act
        var result = _service.GetFilteredEntries(directory.AllEntries, filters);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnEmptyList_If_NoFiltersMatchTheEntries()
    {
        // Arrange
        directoryEntry.Themes = filterThemes;
        directory.Entries = new List<DirectoryEntry>() { directoryEntry };

        // Act
        var result = _service.GetFilteredEntries(directory.AllEntries, filters);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAllFilterThemes_ShouldReturnEmptyList_If_NoFilteredEntries()
    {
        // Act
        var result = _service.GetFilterThemes(null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAllFilterThemes_ShouldReturnEmptyList_If_EntryHasNoFilters()
    {
        // Act
        var result = _service.GetFilterThemes(new List<DirectoryEntry>() { directoryEntry });

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAllFilterThemes_ShouldReturnAllFilterThemes()
    {
        // Arrange
        directoryEntry.Themes = filterThemes;

        // Act
        var result = _service.GetFilterThemes(new List<DirectoryEntry>() { directoryEntry });

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(3, result.First().Filters.Count());
    }

    [Fact]
    public void GetAppliedFilters_ShouldReturnEmptyList_If_NoFilterThemes()
    {
        // Act
        var result = _service.GetFilters(filters, null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAppliedFilters_ShouldReturnEmptyList_If_NoFilters()
    {
        // Act
        var result = _service.GetFilters(null, filterThemes);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAppliedFilters_ShouldReturnAllAppliedFilters()
    {
        // Act
        var result = _service.GetFilters(filters, filterThemes2);

        // Assert
        Assert.Equal(4, result.Count());
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData("Name A to Z", new[] { "C", "B", "A" }, new[] { "A", "B", "C" })]
    [InlineData("Name Z to A", new[] { "A", "B", "C" }, new[] { "C", "B", "A" })]
    [InlineData("name a to z", new[] { "B", "C", "A" }, new[] { "A", "B", "C" })]
    [InlineData("name z to a", new[] { "B", "A", "C" }, new[] { "C", "B", "A" })]
    [InlineData("", new[] { "B", "A", "C" }, new[] { "B", "A", "C" })]
    [InlineData("another order", new[] { "B", "A", "C" }, new[] { "B", "A", "C" })]
    public void GetOrderedEntries_ShouldReturnAlphabeticalOrderedEntries(string orderBy, string[] orderedEntries, string[] expectedEntries)
    {
        var entries = orderedEntries.Select(name => new DirectoryEntry { Name = name });

        // Act
        var result = _service.GetOrderedEntries(entries, orderBy).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(expectedEntries, result.Select(_ => _.Name).ToArray());
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetAllFilterCounts_ShouldReturnCorrectCounts()
    {
        // Arrange
        directoryEntry.Themes = filterThemes;
        directoryEntry2.Themes = filterThemes2;
        directoryEntry3.Themes = filterThemes;

        List<DirectoryEntry> allEntries = new() { directoryEntry, directoryEntry2, directoryEntry3 };

        // Act
        var result = _service.GetAllFilterCounts(allEntries);

        // Assert
        Assert.Equal(2, result["value10"]);
        Assert.Equal(2, result["value20"]);
        Assert.Equal(2, result["value30"]);
        Assert.Equal(1, result["value1"]);
        Assert.Equal(1, result["value2"]);
        Assert.Equal(1, result["value3"]);
        Assert.Equal(1, result["value11"]);
        Assert.Equal(1, result["value21"]);
        Assert.Equal(1, result["value31"]);
        Assert.Equal(9, result.Count);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_ForNameFieldHit()
    {
        List<DirectoryEntry> allEntries = new() { directoryEntry, directoryEntry2, directoryEntry3 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "NAME");
        Assert.Equal(3, result.Count());

        result = _service.GetSearchedEntryForDirectories(allEntries, "name2");
        Assert.Single(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturn0_ForNoHits()
    {
        List<DirectoryEntry> allEntries = new() { directoryEntry, directoryEntry2, directoryEntry3 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "NonExistantResult");
        Assert.Empty(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_ForTagHit()
    {
        List<DirectoryEntry> allEntries = new() { directoryEntry, directoryEntry2, directoryEntry3 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "tagged");
        Assert.Single(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_ForDescriptionFieldHit()
    {
        List<DirectoryEntry> allEntries = new() { directoryEntry, directoryEntry2, directoryEntry3 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "description");
        Assert.Equal(3, result.Count());

        result = _service.GetSearchedEntryForDirectories(allEntries, "description2");
        Assert.Single(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_ForTeaserFieldHit()
    {
        List<DirectoryEntry> allEntries = new() { directoryEntry, directoryEntry2, directoryEntry3 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "teaser");
        Assert.Equal(3, result.Count());

        result = _service.GetSearchedEntryForDirectories(allEntries, "teaser2");
        Assert.Single(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_ForFilterFieldHit()
    {
        List<DirectoryEntry> allEntries = new() { directoryEntry, directoryEntry2, directoryEntry3, directoryEntry4 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "filter");
        Assert.Single(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_ForFilterFieldHit_CaseInsensitive()
    {
        List<DirectoryEntry> allEntries = new() { directoryEntry, directoryEntry2, directoryEntry3, directoryEntry4 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "FILTER");
        Assert.Single(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_WhenEntriesIsEmpty()
    {
        List<DirectoryEntry> allEntries = new() { };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "FILTER");
        Assert.Empty(result);
    }


    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_WhenEntryFilterThemesIsNull()
    {
        directoryEntry3.Themes = null;
        List<DirectoryEntry> allEntries = new() { directoryEntry3 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "FILTER");
        Assert.Empty(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_WhenEntryFilterThemeFiltersIsNull()
    {
        directoryEntry3.Themes = new List<FilterTheme> { new FilterTheme { Title = "Test Theme" } };
        List<DirectoryEntry> allEntries = new() { directoryEntry3 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "FILTER");
        Assert.Empty(result);
    }

    [Fact]
    public void GetSearchedEntryForDirectories_ShouldReturnCorrectCount_WhenEntryFilterThemesDoesNotContainMatchingValue()
    {
        directoryEntry3.Themes = new List<FilterTheme> { new FilterTheme { Title = "Test Theme", Filters = new List<Filter> { new Filter { Title = "Test" } } } };
        List<DirectoryEntry> allEntries = new() { directoryEntry3 };

        var result = _service.GetSearchedEntryForDirectories(allEntries, "FILTER");
        Assert.Empty(result);
    }
}
