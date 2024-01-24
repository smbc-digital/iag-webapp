namespace StockportWebappTests_Unit.Unit.Repositories;
using Filter = StockportWebapp.Model.Filter;

public class DirectoryRepositoryTests
{
    private readonly Mock<IHttpClient> _httpClient;
    private readonly Mock<IApplicationConfiguration> _applicationConfiguration;
    private readonly UrlGenerator _urlGenerator;
    private readonly DirectoryRepository _directoryRepository;

    private readonly List<FilterTheme> filterThemes = new(){
        new(){
            Filters = new List<Filter>(){
                new() {
                    Slug = "value10",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme1"
                },
                new() {
                    Slug = "value20",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme2"
                },
                new() {
                    Slug = "value30",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme3"
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
                    Theme = "theme1"
                },
                new() {
                    Slug = "value2",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme2"
                },
                new() {
                    Slug = "value3",
                    Title = "title",
                    DisplayName = "display name",
                    Theme = "theme3"
                }
            },
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
        SubDirectories = new List<Directory>(),
        Entries = new List<DirectoryEntry>()
    };

    private readonly DirectoryEntry directoryEntryWithFilterThemes = new();

    public DirectoryRepositoryTests()
    {
        _httpClient = new Mock<IHttpClient>();
        _applicationConfiguration = new Mock<IApplicationConfiguration>();
        _urlGenerator = new UrlGenerator(_applicationConfiguration.Object, new BusinessId(""));

        _directoryRepository = new DirectoryRepository(_urlGenerator, _httpClient.Object, _applicationConfiguration.Object);

        directoryEntryWithFilterThemes = directoryEntry;
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
        directory.Entries = new List<DirectoryEntry>() { directoryEntry };

        // Act
        var result = _directoryRepository.GetFilteredEntryForDirectories(directory);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnAllEntriesForDirectory_WithFilters()
    {
        // Arrange
        directoryEntryWithFilterThemes.Themes = filterThemes2;

        directory.Entries = new List<DirectoryEntry>() { directoryEntryWithFilterThemes };

        string[] filters = { "value1", "value2", "value3" };

        // Act
        var result = _directoryRepository.GetFilteredEntryForDirectories(directory, filters);

        var resultCount = result.Where(
            _ => _.Themes.Any(theme => theme.Filters.Any(
                rFilter => filters.Any(filter => rFilter.Slug.Contains(filter))))).Count();

        // Assert
        Assert.Equal(directory.Entries.Count(), resultCount);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetFilteredEntryForDirectories_ShouldReturnEmptyList_If_DirectoryEntryNull()
    {
        // Arrange
        string[] filters = { "value1", "value2", "value3" };

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
        string[] filters = { "value1", "value2", "value3" };

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
        directoryEntryWithFilterThemes.Themes = filterThemes;
        directory.Entries = new List<DirectoryEntry>() { directoryEntry };
        string[] filters = { "value1", "value2", "value3" };

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
        directoryEntryWithFilterThemes.Themes = filterThemes;

        // Act
        var result = _directoryRepository.GetAllFilterThemes(new List<DirectoryEntry>() { directoryEntryWithFilterThemes });

        var filterThemesCount = filterThemes.Select(_ => _.Filters.Count()).FirstOrDefault();
        var resultCount = result.Select(_ => _.Filters.Count()).FirstOrDefault();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(filterThemesCount, resultCount);
    }

    [Fact]
    public void GetAppliedFilters_ShouldReturnEmptyList_If_NoFilterThemes()
    {
        // Arrange
        string[] filters = { "value1", "value2", "value3" };

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
        // Arrange
        string[] filters = { "value1", "value2", "value3" };

        // Act
        var result = _directoryRepository.GetAppliedFilters(filters, filterThemes2);

        // this was to check that all the filters provided get used, or that was the intention anyway
        // did the count initially, but have gotten slightly confused and added this, now not even sure if its needed
        var allFiltersInResults = result.All(resultTheme => filters.Any(filterTheme => resultTheme.Slug.Contains(filterTheme)));

        // Assert
        Assert.Equal(filters.Length, result.Count());
        Assert.True(allFiltersInResults);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
}