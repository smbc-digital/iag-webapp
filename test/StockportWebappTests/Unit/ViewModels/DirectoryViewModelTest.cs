using StockportWebapp.Model;

namespace StockportWebappTests_Unit.Unit.ViewModels;

public class DirectoryViewModelTest
{
    [Fact]
    public void QueryParameters_ReturnsCorrectQueries_WhenAllParametersAreSet()
    {
        // Arrange
        var directoryViewModel = new DirectoryViewModel
        {
            SearchTerm = "test",
            Order = "Name A to Z",
            AppliedFilters = new List<Filter>
            {
                new() { Slug = "filter1" },
                new() { Slug = "filter2" }
            }
        };

        // Act
        var queryParameters = directoryViewModel.QueryParameters;

        // Assert
        Assert.Collection(queryParameters,
            query =>
            {
                Assert.Equal("searchTerm", query.Name);
                Assert.Equal("test", query.Value);
            },
            query =>
            {
                Assert.Equal("orderBy", query.Name);
                Assert.Equal("Name-A-to-Z", query.Value);
            },
            query =>
            {
                Assert.Equal("filters", query.Name);
                Assert.Equal("filter1", query.Value);
            },
            query =>
            {
                Assert.Equal("filters", query.Name);
                Assert.Equal("filter2", query.Value);
            }
        );
    }

    [Fact]
    public void QueryParameters_ReturnsCorrectQueries_WhenSomeParametersAreEmpty()
    {
        // Arrange
        var directoryViewModel = new DirectoryViewModel
        {
            Order = "Name A to Z"
        };

        // Act
        var queryParameters = directoryViewModel.QueryParameters;

        // Assert
        Assert.Collection(queryParameters,
            query =>
            {
                Assert.Equal("orderBy", query.Name);
                Assert.Equal("Name-A-to-Z", query.Value);
            }
        );
    }

    [Fact]
    public void QueryParameters_ReturnsEmptyList_WhenNoParametersAreSet()
    {
        // Arrange
        var directoryViewModel = new DirectoryViewModel();

        // Act
        var queryParameters = directoryViewModel.QueryParameters;

        // Assert
        Assert.Empty(queryParameters);
    }

    [Fact]
    public void DisplayIcons_ReturnsTrue_WhenDirectoryAndSubDirectoriesAreNotNull_AndAllSubDirectoriesHaveNonNullIcons()
    {
        // Arrange
        Directory directory = new()
        {
            SubItems = new List<SubItem> ()
            {
                new("slug", "title", "teaser", "icon", "directory", string.Empty, "image", 0, string.Empty, new List<SubItem>(), "teal"),
                new("slug2", "title2", "teaser2", "icon2", "directory2", string.Empty, "image2", 0, string.Empty, new List<SubItem>(), "teal")
            },
        };

        DirectoryViewModel directoryViewModel = new("test-slug", directory );

        // Act
        bool result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DisplayIcons_ReturnsFalse_WhenDirectoryIsNull()
    {
        // Arrange
        DirectoryViewModel directoryViewModel = new();

        // Act
        bool result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DisplayIcons_ReturnsFalse_WhenSubDirectoriesIsNull()
    {
        // Arrange
        Directory directory = new();
        DirectoryViewModel directoryViewModel = new("test-slug", directory);

        // Act
        bool result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DisplayIcons_ReturnsFalse_WhenAnySubDirectoryIsNull()
    {
        // Arrange
        Directory directory = new()
        {
            SubDirectories = new List<Directory>
            {
                new() { Icon = "icon1" },
                null
            }
        };

        DirectoryViewModel directoryViewModel = new("test-slug", directory);

        // Act
        bool result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DisplayIcons_ReturnsFalse_WhenAnySubDirectoryHasNullOrEmptyIcon()
    {
        // Arrange
        Directory directory = new()
        {
            SubItems = new List<SubItem>(){
                new("slug", "title", "teaser", "icon1", "type", "contentType", "image", 0, string.Empty, new List<SubItem>(), "teal"),
                new("slug2", "title2", "teaser2", "", "type2", "contentType", "image2", 0, string.Empty, new List<SubItem>(), "teal")
            }
        };

        DirectoryViewModel directoryViewModel = new("test-slug", directory);

        // Act
        bool result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRootDirectory_ReturnsTrue_WhenDirectoryTitleIsSameAsParentDirectoryTitle()
    {
        // Arrange
        Directory directory = new() { Title = "Root Directory" };
        DirectoryViewModel parentDirectory = new() { Title = "Root Directory" };

        DirectoryViewModel directoryViewModel = new("test-slug", directory)
        {
            ParentDirectory = parentDirectory
        };

        // Act
        bool result = directoryViewModel.IsRootDirectory;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRootDirectory_ReturnsFalse_WhenDirectoryTitleIsDifferentFromParentDirectoryTitle()
    {
        // Arrange
        Directory directory = new() { Title = "Sub Directory" };
        DirectoryViewModel parentDirectory = new() { Title = "Root Directory" };

        DirectoryViewModel directoryViewModel = new("test-slug", directory)
        {
            ParentDirectory = parentDirectory
        };

        // Act
        bool result = directoryViewModel.IsRootDirectory;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DoPagination_PaginatesEntriesCorrectly()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            FilteredEntries = Enumerable.Range(1, 50).Select(i => new DirectoryEntryViewModel { DirectoryEntry = new DirectoryEntry{ Name = i.ToString() }}).ToList()
        };

        // Act
        viewModel.Paginate(2);
        List<DirectoryEntryViewModel> paginatedEntries = viewModel.PaginatedEntries.ToList();
        
        // Assert
        Assert.Equal(12, paginatedEntries.Count);
        Assert.Equal(2, viewModel.PaginationInfo.CurrentPage);
        Assert.Equal(5, viewModel.PaginationInfo.TotalPages);
        Assert.Equal(50, viewModel.PaginationInfo.TotalEntries);
        Assert.Equal(12, viewModel.PaginationInfo.PageSize);
        Assert.Equal("13", paginatedEntries[0].DirectoryEntry.Name);
        Assert.Equal("14", paginatedEntries[1].DirectoryEntry.Name);
        Assert.Equal("15", paginatedEntries[2].DirectoryEntry.Name);
        Assert.Equal("16", paginatedEntries[3].DirectoryEntry.Name);
        Assert.Equal("17", paginatedEntries[4].DirectoryEntry.Name);
        Assert.Equal("18", paginatedEntries[5].DirectoryEntry.Name);
        Assert.Equal("19", paginatedEntries[6].DirectoryEntry.Name);
        Assert.Equal("20", paginatedEntries[7].DirectoryEntry.Name);
        Assert.Equal("21", paginatedEntries[8].DirectoryEntry.Name);
        Assert.Equal("22", paginatedEntries[9].DirectoryEntry.Name);
        Assert.Equal("23", paginatedEntries[10].DirectoryEntry.Name);
        Assert.Equal("24", paginatedEntries[11].DirectoryEntry.Name);
    }

    [Fact]
    public void ShowPagination_True_EntriesGreaterThanPageSize()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            PaginationInfo = new PaginationInfo
            {
                PageSize = 12,
                TotalEntries = 24
            }
        };

        // Act & Assert
        Assert.True(viewModel.ShowPagination);
    }

    [Fact]
    public void ShowPagination_False_EntriesLessThanOrEqualPageSize()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            PaginationInfo = new PaginationInfo
            {
                PageSize = 12,
                TotalEntries = 12
            }
        };

        // Act & Assert
        Assert.False(viewModel.ShowPagination);
    }

    [Fact]
    public void DisplayTitlePopulatedCorrectly_When_SearchTerm_HasValue()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            Title = "Test Title",
            SearchTerm ="Test Search Term"
        };

        // Act & Assert
        Assert.Equal($"Results for \"{viewModel.SearchTerm}\"", viewModel.DisplayTitle);
    }

    [Fact]
    public void DisplayTitlePopulatedCorrectly_When_SearchTerm_HasNoValue()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            Title = "Test Title"
        };

        // Act & Assert
        Assert.Equal($"Results for {viewModel.Title}", viewModel.DisplayTitle);
    }

    [Fact]
    public void PageTitlePopulatedCorrectly_When_NoPagination()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            Title = "Test Title",
            PaginationInfo = new PaginationInfo
            {
                PageSize = 12,
                TotalEntries = 12
            }
        };

        // Act & Assert
        Assert.Equal($"Results for {viewModel.Title}", viewModel.PageTitle);
    }

    [Fact]
    public void PageTitlePopulatedCorrectly_With_Pagination()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            Title = "Test Title",
            PaginationInfo = new PaginationInfo
            {
                PageSize = 12,
                TotalEntries = 24,
                CurrentPage = 1,
                TotalPages = 2
            }
        };

        // Act & Assert
        Assert.Equal($"Results for {viewModel.Title} (page 1 of 2)", viewModel.PageTitle);
    }

    [Fact]
    public void SearchBranding_Returns_Correct_Value_When_ParentValueSet()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            ParentDirectory = new DirectoryViewModel(new Directory() { SearchBranding = "Test Branding" })
        };

        // Act & Assert
        Assert.Equal("Test Branding", viewModel.SearchBranding);
    }

    [Fact]
    public void SearchBranding_Returns_Default_Value_When_ParentValueSet_ButHasNoBranding()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            ParentDirectory = new DirectoryViewModel(new Directory())
        };

        // Act & Assert
        Assert.Equal("Default", viewModel.SearchBranding);
    }

    [Fact]
    public void SearchBranding_Returns_Default_Value_When_ParentValueSet()
    {
        // Arrange
        DirectoryViewModel viewModel = new();
        
        // Act & Assert
        Assert.Equal("Default", viewModel.SearchBranding);
    }

    [Fact]
    public void SearchBranding_Returns_Default_Value_When_No_FirstSubDirectory()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            FirstSubDirectory = new DirectoryViewModel() { ColourScheme = "" }
        };

        // Act & Assert
        Assert.Equal("teal", viewModel.InheritedColourScheme);
    }

    [Fact]
    public void SearchBranding_Returns_Correct_Value_When_FirstSubDirectory()
    {
        // Arrange
        DirectoryViewModel viewModel = new()
        {
            FirstSubDirectory = new DirectoryViewModel() { ColourScheme = "pink" }
        };

        // Act & Assert
        Assert.Equal("pink", viewModel.InheritedColourScheme);
    }
}