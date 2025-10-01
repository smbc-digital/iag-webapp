using StockportWebapp.Extensions;
using Filter = StockportWebapp.Models.Filter;

namespace StockportWebappTests_Unit.Unit.Extensions;

public class DirectoryExtensionsTests
{
    [Fact]
    public void DirectoryEntrySatisfiesTheme_WhenThemesMatch_ReturnsTrue()
    {
        // Arrange
        KeyValuePair<string, List<string>> themes = new("ThemeA", new List<string> { "Filter1", "Filter2" });
        DirectoryEntry entry = new()
        {
            Themes = new List<FilterTheme>
            {
                new() { Title = "ThemeA", Filters = new List<Filter> { new() { Slug = "Filter1" } } },
                new() { Title = "ThemeB", Filters = new List<Filter> { new() { Slug = "Filter3" } } }
            }
        };

        // Act
        bool result = themes.DirectoryEntrySatisfiesTheme(entry);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DirectoryEntrySatisfiesTheme_WhenThemesDoNotMatch_ReturnsFalse()
    {
        // Arrange
        KeyValuePair<string, List<string>> themes = new("ThemeA", new List<string> { "Filter1", "Filter2" });
        DirectoryEntry entry = new()
        {
            Themes = new List<FilterTheme>
            {
                new() { Title = "ThemeB", Filters = new List<Filter> { new() { Slug = "Filter1" } } },
                new() { Title = "ThemeC", Filters = new List<Filter> { new() { Slug = "Filter3" } } }
            }
        };

        // Act
        bool result = themes.DirectoryEntrySatisfiesTheme(entry);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DirectoryEntrySatisfiesTheme_WhenNoMatchingFilters_ReturnsFalse()
    {
        // Arrange
        KeyValuePair<string, List<string>> themes = new("ThemeA", new List<string> { "Filter1", "Filter2" });
        DirectoryEntry entry = new()
        {
            Themes = new List<FilterTheme>
            {
                new() { Title = "ThemeA", Filters = new List<Filter> { new() { Slug = "Filter3" } } },
                new() { Title = "ThemeB", Filters = new List<Filter> { new() { Slug = "Filter4" } } }
            }
        };

        // Act
        bool result = themes.DirectoryEntrySatisfiesTheme(entry);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsDirectoryEntryRelevant_WhenNoThemesOrFiltersSpecified_ReturnsTrue()
    {
        // Arrange
        Dictionary<string, List<string>> emptyThemes = new();
        DirectoryEntry entry = new();

        // Act
        bool result = emptyThemes.IsDirectoryEntryRelevant(entry);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDirectoryEntryRelevant_WhenEntrySatisfiesAllThemes_ReturnsTrue()
    {
        // Arrange
        Dictionary<string, List<string>> themes = new()
        {
            { "Theme1", new List<string> { "filter1", "filter2" } },
            { "Theme2", new List<string> { "filter3", "filter4" } }
        };

        DirectoryEntry entry = new()
        {
            Themes = new List<FilterTheme>
            {
                new() { Title = "Theme1", Filters = new List<Filter> { new() { Slug = "filter1" } } },
                new() { Title = "Theme2", Filters = new List<Filter> { new() { Slug = "filter4" } } }
            }
        };

        // Act
        bool result = themes.IsDirectoryEntryRelevant(entry);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDirectoryEntryRelevant_WhenEntryDoesNotSatisfyAnyTheme_ReturnsFalse()
    {
        // Arrange
        Dictionary<string, List<string>> themes = new()
        {
            { "Theme1", new List<string> { "filter1", "filter2" } },
            { "Theme2", new List<string> { "filter3", "filter4" } }
        };

        DirectoryEntry entry = new()
        {
            Themes = new List<FilterTheme> {
                new() { Title = "Theme1", Filters = new List<Filter> { new() { Slug = "filter5" }, new() { Slug = "filter6" } } }
            }
        };

        // Act
        bool result = themes.IsDirectoryEntryRelevant(entry);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetFilterThemesFromFilters_ReturnsEmptyDictionary_WhenNoFilters()
    {
        // Arrange
        IEnumerable<Filter> emptyFilters = Enumerable.Empty<Filter>();

        // Act
        Dictionary<string, List<string>> result = emptyFilters.GetFilterThemesFromFilters();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetFilterThemesFromFilters_ReturnsCorrectThemesAndSlugs()
    {
        // Arrange
        List<Filter> filters = new()
        {
            new Filter { Theme = "Colour", Slug = "red" },
            new Filter { Theme = "Colour", Slug = "blue" },
            new Filter { Theme = "Size", Slug = "small" },
            new Filter { Theme = "Size", Slug = "medium" }
        };

        // Act
        Dictionary<string, List<string>> result = filters.GetFilterThemesFromFilters();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result.ContainsKey("Colour"));
        Assert.True(result.ContainsKey("Size"));
        Assert.Equal("Colour", result.First().Key);
        Assert.Contains("red", result.First().Value);
        Assert.Equal("Size", result.Skip(1).First().Key);
        Assert.Contains("small", result.Skip(1).First().Value);

    }
}