using StockportWebapp.Extensions;

namespace StockportWebappTests_Unit.Unit.Extensions
{
    public class WildcardSlugExtensionsTests
    {
        [Fact]
        public void ProcessSlug_WithNull_ReturnsEmptyPageLocation()
        {
            // Arrange
            string slug = null;

            // Act
            var result = WildcardSlugExtensions.ProcessAsWildcardSlug(slug);

            // Assert
            Assert.True(string.IsNullOrEmpty(result.Slug));
            Assert.Empty(result.ParentSlugs);
        }

        [Fact]
        public void ProcessSlug_WithEmptyString_ReturnsEmptyPageLocation()
        {
            // Arrange
            string slug = string.Empty;

            // Act
            var result = WildcardSlugExtensions.ProcessAsWildcardSlug(slug);

            // Assert
            Assert.True(string.IsNullOrEmpty(result.Slug));
            Assert.Empty(result.ParentSlugs);
        }

        [Fact]
        public void ProcessSlug_WithValidSlug_ReturnsCorrectPageLocation()
        {
            // Arrange
            string slug = "home/about/us";

            // Act
            var result = WildcardSlugExtensions.ProcessAsWildcardSlug(slug);

            // Assert
            Assert.Equal("us", result.Slug);
            Assert.Equal(new List<string> { "home", "about" }, result.ParentSlugs);
        }

        [Fact]
        public void ProcessSlug_WithValidSlug_WhenDoubleSlash_ReturnsCorrectPageLocation()
        {
            // Arrange
            string slug = "home/about//us";

            // Act
            var result = WildcardSlugExtensions.ProcessAsWildcardSlug(slug);

            // Assert
            Assert.Equal("us", result.Slug);
            Assert.Equal(new List<string> { "home", "about" }, result.ParentSlugs);
        }

    }
}
