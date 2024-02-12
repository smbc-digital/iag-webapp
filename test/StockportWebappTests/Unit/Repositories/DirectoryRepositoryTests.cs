namespace StockportWebappTests_Unit.Unit.Repositories;
using Filter = StockportWebapp.Model.Filter;

public class DirectoryRepositoryTests
{
    private readonly Mock<IHttpClient> _httpClient;
    private readonly Mock<IApplicationConfiguration> _applicationConfiguration;
    private readonly UrlGenerator _urlGenerator;
    private readonly DirectoryRepository _directoryRepository;

    private readonly string[] filters = { "value1", "value2", "value3", "value11" };

    private readonly List<FilterTheme> filterThemes = new(){
        new(){
            Filters = new List<Filter>(){
                new() {
                    Slug = "value10",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme1",
                    Highlight = false
                },
                new() {
                    Slug = "value20",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme2",
                    Highlight = false
                },
                new() {
                    Slug = "value30",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme3",
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
            },
            Title = "Theme title"
        },
        new(){
            Filters = new List<Filter>(){
                new() {
                    Slug = "value11",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme11",
                    Highlight = false
                },
                new() {
                    Slug = "value21",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme21",
                    Highlight = false
                },
                new() {
                    Slug = "value31",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme31",
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
    };

    private readonly DirectoryEntry directoryEntry3 = new()
    {
        Slug = "slug2",
        Name = "another name",
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

    public DirectoryRepositoryTests()
    {
        _httpClient = new Mock<IHttpClient>();
        _applicationConfiguration = new Mock<IApplicationConfiguration>();
        _urlGenerator = new UrlGenerator(_applicationConfiguration.Object, new BusinessId(""));

        _directoryRepository = new DirectoryRepository(_urlGenerator, _httpClient.Object, _applicationConfiguration.Object);
    }

    [Fact]
    public async void Get_ShouldReturnHttpResponse_IfNotSuccessful()
    {
        // Arrange
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(new HttpResponse(404, It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _directoryRepository.Get<Directory>();

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Content);
    }

    [Fact]
    public async void Get_ShouldReturnHttpResponse_Successful()
    {
        // Arrange
        HttpResponse httpResponse = new(200, JsonConvert.SerializeObject(directory), "OK");
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(httpResponse);

        // Act
        var result = await _directoryRepository.Get<Directory>();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.IsType(directory.GetType(), result.Content);
    }

    [Fact]
    public async void GetEntry_ShouldReturnHttpResponse_IfNotSuccessful()
    {
        // Arrange
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(new HttpResponse(404, It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _directoryRepository.GetEntry<DirectoryEntry>();

        // Assert
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Content);
    }

    [Fact]
    public async void GetEntry_ShouldReturnHttpResponse_Successful()
    {
        // Arrange
        HttpResponse httpResponse = new(200, JsonConvert.SerializeObject(directoryEntry), "OK");
        _httpClient.Setup(_ => _.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                    .ReturnsAsync(httpResponse);

        // Act
        var result = await _directoryRepository.GetEntry<DirectoryEntry>();

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Content);
        Assert.IsType(directoryEntry.GetType(), result.Content);
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnAllEntriesForDirectory()
    {
        // Arrange
        directory.Entries = new List<DirectoryEntry>() { directoryEntry, directoryEntry2 };

        // Act
        var result = _directoryRepository.GetFilteredEntryForDirectories(directory);

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
        var result = _directoryRepository.GetFilteredEntryForDirectories(directory, filters);

        // Assert
        Assert.Single(result);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnEmptyList_If_DirectoryEntryNull()
    {
        // Act
        var result = _directoryRepository.GetFilteredEntryForDirectories(directory, filters);

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
        var result = _directoryRepository.GetFilteredEntryForDirectories(directory, filters);

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
        var result = _directoryRepository.GetFilteredEntryForDirectories(directory, filters);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAllFilterThemes_ShouldReturnEmptyList_If_NoFilteredEntries()
    {
        // Act
        var result = _directoryRepository.GetAllFilterThemes(null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAllFilterThemes_ShouldReturnEmptyList_If_EntryHasNoFilters()
    {
        // Act
        var result = _directoryRepository.GetAllFilterThemes(new List<DirectoryEntry>() { directoryEntry });

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
        var result = _directoryRepository.GetAllFilterThemes(new List<DirectoryEntry>() { directoryEntry });

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
        var result = _directoryRepository.GetAppliedFilters(filters, null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAppliedFilters_ShouldReturnEmptyList_If_NoFilters()
    {
        // Act
        var result = _directoryRepository.GetAppliedFilters(null, filterThemes);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetAppliedFilters_ShouldReturnAllAppliedFilters()
    {
        // Act
        var result = _directoryRepository.GetAppliedFilters(filters, filterThemes2);

        // Assert
        Assert.Equal(4, result.Count());
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData("Name A to Z", new[]{ "C", "B", "A" }, new[] { "A", "B", "C" })]
    [InlineData("Name Z to A", new[]{ "A", "B", "C" }, new[] { "C", "B", "A" })]
    [InlineData("name a to z", new[]{ "B", "C", "A" }, new[] { "A", "B", "C" })]
    [InlineData("name z to a", new[]{ "B", "A", "C" }, new[] { "C", "B", "A" })]
    [InlineData("", new[]{ "B", "A", "C" }, new[] { "B", "A", "C" })]
    [InlineData("another order", new[]{ "B", "A", "C" }, new[] { "B", "A", "C" })]
    public void GetOrderedEntries_ShouldReturnAlphabeticalOrderedEntries(string orderBy, string[] orderedEntries, string[] expectedEntries)
    {
        var entries = orderedEntries.Select(name => new DirectoryEntry { Name = name });

        // Act
        var result = _directoryRepository.GetOrderedEntries(entries, orderBy).ToList();

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
        var result = _directoryRepository.GetAllFilterCounts(allEntries);

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
}