using StockportWebapp.Model;

namespace StockportWebappTests_Unit.Unit.ViewModels;

public class DirectoryEntryViewModelTest
{
    [Fact]
    public void HighlightedFilters_ReturnsCorrectFilters()
    {
        // Arrange
        List<FilterTheme> themes = new()
        {
            new()
            {
                Filters = new List<Filter>
                {
                    new() { Highlight = true },
                    new() { Highlight = false },
                    new() { Highlight = true }
                }
            },
            new()
            {
                Filters = new List<Filter>
                {
                    new() { Highlight = false },
                    new() { Highlight = true }
                }
            }
        };

        DirectoryEntryViewModel directoryViewModel = new("slug", new())
        {
            DirectoryEntry = new DirectoryEntry
            {
                Themes = themes
            }
        };

        // Act
        var highlightedFilters = directoryViewModel.HighlightedFilters;

        // Assert
        Assert.NotNull(highlightedFilters);
        Assert.Equal(3, highlightedFilters.Count());
    }

    [Fact]
    public void HighlightedFilters_EmptyThemes_ReturnsEmptyList()
    {
        // Arrange
        DirectoryEntryViewModel directoryViewModel = new("slug", new()){
            DirectoryEntry = new DirectoryEntry
            {
                Themes = new List<FilterTheme>()
            }
        };

        // Act
        var highlightedFilters = directoryViewModel.HighlightedFilters;

        // Assert
        Assert.NotNull(highlightedFilters);
        Assert.Empty(highlightedFilters);
    }

    [Fact]
    public void HighlightedFilters_NullThemes_ReturnsNull()
    {
        // Arrange
        DirectoryEntryViewModel directoryViewModel = new("slug", new()){
            DirectoryEntry = new DirectoryEntry
            {
                Themes = null
            }
        };

        // Act
        var highlightedFilters = directoryViewModel.HighlightedFilters;

        // Assert
        Assert.Null(highlightedFilters);
    }

    [Fact]
    public void HighlightedFilters_NoHighlightedFilters_ReturnsEmptyList()
    {
        // Arrange
        List<FilterTheme> themes = new()
        {
            new()
            {
                Filters = new List<Filter>
                {
                    new() { Highlight = false },
                    new() { Highlight = false }
                }
            },
            new()
            {
                Filters = new List<Filter>
                {
                    new() { Highlight = false },
                    new() { Highlight = false }
                }
            }
        };

        DirectoryEntryViewModel directoryViewModel = new("slug", new()){
            DirectoryEntry = new DirectoryEntry
            {
                Themes = themes
            }
        };

        // Act
        var highlightedFilters = directoryViewModel.HighlightedFilters;

        // Assert
        Assert.NotNull(highlightedFilters);
        Assert.Empty(highlightedFilters);
    }

    [Theory]
    [InlineData(null, null, null, null, null, false)]
    [InlineData("", "", "", "", "", false)]
    [InlineData("facebook", null, null, null, null, true)]
    [InlineData(null, "twitter", null, null, null, true)]
    [InlineData(null, null, "youtube", null, null, true)]
    [InlineData(null, null, null, "instagram", null, true)]
    [InlineData(null, null, null, null, "linkedin", true)]
    [InlineData("facebook", "twitter", "youtube", "instagram", "linkedin", true)]
    public void DisplaySocials_ReturnsCorrectValue(string facebook, string twitter, string youtube, string instagram, string linkedIn, bool expected)
    {
        // Arrange
        var viewModel = new DirectoryEntryViewModel
        {
            DirectoryEntry = new DirectoryEntry
            {
                Facebook = facebook,
                Twitter = twitter,
                Youtube = youtube,
                Instagram = instagram,
                LinkedIn = linkedIn
            }
        };

        // Act
        var displaySocials = viewModel.DisplaySocials;

        // Assert
        Assert.Equal(expected, displaySocials);
    }

    [Theory]
    [InlineData(null, null, false)]
    [InlineData("", "", false)]
    [InlineData("1234567890", null, true)]
    [InlineData(null, "test@example.com", true)]
    [InlineData("1234567890", "test@example.com", true)]
    public void HasPrimaryContact_ReturnsCorrectValue(string phoneNumber, string email, bool expected)
    {
        // Arrange
        var viewModel = new DirectoryEntryViewModel
        {
            DirectoryEntry = new DirectoryEntry
            {
                PhoneNumber = phoneNumber,
                Email = email
            }
        };

        // Act
        var hasPrimaryContact = viewModel.HasPrimaryContact;

        // Assert
        Assert.Equal(expected, hasPrimaryContact);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("https://example.com", true)]
    public void DisplayContactUs_ReturnsCorrectValue(string website, bool expected)
    {
        // Arrange
        var viewModel = new DirectoryEntryViewModel
        {
            DirectoryEntry = new DirectoryEntry
            {
                Website = website   
            }
        };

        // Act
        var displayContactUs = viewModel.DisplayContactUs;

        // Assert
        Assert.Equal(expected, displayContactUs);
    }
}