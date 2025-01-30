using StockportWebapp.Extensions;

namespace StockportWebappTests_Unit.Unit.Extensions;

public class WildcardSlugExtensionsTests
{
    [Fact]
    public void ProcessSlug_WithNull_ReturnsEmptyPageLocation()
    {
        // Act
        PageLocation result = WildcardSlugExtensions.ProcessAsWildcardSlug(null);

        // Assert
        Assert.True(string.IsNullOrEmpty(result.Slug));
        Assert.Empty(result.ParentSlugs);
    }

    [Fact]
    public void ProcessSlug_WithEmptyString_ReturnsEmptyPageLocation()
    {
        // Act
        PageLocation result = WildcardSlugExtensions.ProcessAsWildcardSlug(string.Empty);

        // Assert
        Assert.True(string.IsNullOrEmpty(result.Slug));
        Assert.Empty(result.ParentSlugs);
    }

    [Fact]
    public void ProcessSlug_WithValidSlug_ReturnsCorrectPageLocation()
    {
        // Act
        PageLocation result = WildcardSlugExtensions.ProcessAsWildcardSlug("home/about/us");

        // Assert
        Assert.Equal("us", result.Slug);
        Assert.Equal(new List<string> { "home", "about" }, result.ParentSlugs);
    }

    [Fact]
    public void ProcessSlug_WithValidSlug_WhenDoubleSlash_ReturnsCorrectPageLocation()
    {
        // Act
        PageLocation result = WildcardSlugExtensions.ProcessAsWildcardSlug("home/about//us");

        // Assert
        Assert.Equal("us", result.Slug);
        Assert.Equal(new List<string> { "home", "about" }, result.ParentSlugs);
    }
}