using Microsoft.AspNetCore.Routing;
using StockportWebapp.Extensions;
using Filter = StockportWebapp.Model.Filter;

namespace StockportWebappTests_Unit.Unit.Extensions;

public class DirectoryExtensionsTests
{

    [Fact]
    public void DirectoryEntrySatisfiesTheme_WhenThemesMatch_ReturnsTrue()
    {
        // Arrange
        var themes = new KeyValuePair<string, List<string>>("ThemeA", new List<string> { "Filter1", "Filter2" });
        var entry = new DirectoryEntry
        {
            Themes = new List<FilterTheme>
            {
                new FilterTheme { Title = "ThemeA", Filters = new List<Filter> { new Filter { Slug = "Filter1" } } },
                new FilterTheme { Title = "ThemeB", Filters = new List<Filter> { new Filter { Slug = "Filter3" } } }
            }
        };

        // Act
        var result = themes.DirectoryEntrySatisfiesTheme(entry);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DirectoryEntrySatisfiesTheme_WhenThemesDoNotMatch_ReturnsFalse()
    {
        // Arrange
        var themes = new KeyValuePair<string, List<string>>("ThemeA", new List<string> { "Filter1", "Filter2" });
        var entry = new DirectoryEntry
        {
            Themes = new List<FilterTheme>
            {
                new FilterTheme { Title = "ThemeB", Filters = new List<Filter> { new Filter { Slug = "Filter1" } } },
                new FilterTheme { Title = "ThemeC", Filters = new List<Filter> { new Filter { Slug = "Filter3" } } }
            }
        };

        // Act
        var result = themes.DirectoryEntrySatisfiesTheme(entry);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DirectoryEntrySatisfiesTheme_WhenNoMatchingFilters_ReturnsFalse()
    {
        // Arrange
        var themes = new KeyValuePair<string, List<string>>("ThemeA", new List<string> { "Filter1", "Filter2" });
        var entry = new DirectoryEntry
        {
            Themes = new List<FilterTheme>
            {
                new FilterTheme { Title = "ThemeA", Filters = new List<Filter> { new Filter { Slug = "Filter3" } } },
                new FilterTheme { Title = "ThemeB", Filters = new List<Filter> { new Filter { Slug = "Filter4" } } }
            }
        };

        // Act
        var result = themes.DirectoryEntrySatisfiesTheme(entry);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsDirectoryEntryRelevant_WhenNoThemesOrFiltersSpecified_ReturnsTrue()
    {
        // Arrange
        var emptyThemes = new Dictionary<string, List<string>>();
        var entry = new DirectoryEntry();

        // Act
        var result = emptyThemes.IsDirectoryEntryRelevant(entry);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDirectoryEntryRelevant_WhenEntrySatisfiesAllThemes_ReturnsTrue()
    {
        // Arrange
        var themes = new Dictionary<string, List<string>>
        {
            { "Theme1", new List<string> { "filter1", "filter2" } },
            { "Theme2", new List<string> { "filter3", "filter4" } }
            // Add more themes as needed
        };

        var entry = new DirectoryEntry
        {
            Themes = new List<FilterTheme>
            {
                new FilterTheme { Title = "Theme1", Filters = new List<Filter> { new Filter { Slug = "filter1" } } },
                new FilterTheme { Title = "Theme2", Filters = new List<Filter> { new Filter { Slug = "filter4" } } }
            }
            // Set other properties of the entry
        };

        // Act
        var result = themes.IsDirectoryEntryRelevant(entry);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDirectoryEntryRelevant_WhenEntryDoesNotSatisfyAnyTheme_ReturnsFalse()
    {
        // Arrange
        var themes = new Dictionary<string, List<string>>
        {
            { "Theme1", new List<string> { "filter1", "filter2" } },
            { "Theme2", new List<string> { "filter3", "filter4" } }
            // Add more themes as needed
        };

        var entry = new DirectoryEntry
        {
            Themes = new List<FilterTheme> {
                new FilterTheme { Title = "Theme1", Filters = new List<Filter> { new Filter { Slug = "filter5" }, new Filter { Slug = "filter6" } } }
            }
            // Set other properties of the entry
        };

        // Act
        var result = themes.IsDirectoryEntryRelevant(entry);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetFilterThemesFromFilters_ReturnsEmptyDictionary_WhenNoFilters()
    {
        // Arrange
        var emptyFilters = Enumerable.Empty<Filter>();

        // Act
        var result = emptyFilters.GetFilterThemesFromFilters();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetFilterThemesFromFilters_ReturnsCorrectThemesAndSlugs()
    {
        // Arrange
        var filters = new List<Filter>
        {
            new Filter { Theme = "Colour", Slug = "red" },
            new Filter { Theme = "Colour", Slug = "blue" },
            new Filter { Theme = "Size", Slug = "small" },
            new Filter { Theme = "Size", Slug = "medium" }
        };

        // Act
        var result = filters.GetFilterThemesFromFilters();

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

