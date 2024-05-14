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
        var yourClassInstance = new DirectoryViewModel();

        // Act
        var queryParameters = yourClassInstance.QueryParameters;

        // Assert
        Assert.Empty(queryParameters);
    }

    [Fact]
    public void DisplayIcons_ReturnsTrue_WhenDirectoryAndSubDirectoriesAreNotNull_AndAllSubDirectoriesHaveNonNullIcons()
    {
        // Arrange
        var directory = new Directory
        {
            SubItems = new List<SubItem> ()
            {
                new("slug", "title", "teaser", "icon", "directory", "image", new List<SubItem>(), "teal"),
                new("slug2", "title2", "teaser2", "icon2", "directory2", "image2", new List<SubItem>(), "teal")
            },
        };

        var directoryViewModel = new DirectoryViewModel("test-slug", directory );

        // Act
        var result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void DisplayIcons_ReturnsFalse_WhenDirectoryIsNull()
    {
        // Arrange
        var directoryViewModel = new DirectoryViewModel();

        // Act
        var result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DisplayIcons_ReturnsFalse_WhenSubDirectoriesIsNull()
    {
        // Arrange
        var directory = new Directory();
        var directoryViewModel = new DirectoryViewModel ("test-slug", directory);

        // Act
        var result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DisplayIcons_ReturnsFalse_WhenAnySubDirectoryIsNull()
    {
        // Arrange
        var directory = new Directory
        {
            SubDirectories = new List<Directory>
            {
                new() { Icon = "icon1" },
                null
            }
        };

        var directoryViewModel = new DirectoryViewModel ("test-slug", directory);

        // Act
        var result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DisplayIcons_ReturnsFalse_WhenAnySubDirectoryHasNullOrEmptyIcon()
    {
        // Arrange
        var directory = new Directory
        {
            SubItems = new List<SubItem>(){
                new("slug", "title", "teaser", "icon1", "type", "image", new List<SubItem>(), "teal"),
                new("slug2", "title2", "teaser2", "", "type2", "image2", new List<SubItem>(), "teal")
            }
        };

        var directoryViewModel = new DirectoryViewModel ("test-slug", directory);

        // Act
        var result = directoryViewModel.DisplayIcons;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRootDirectory_ReturnsTrue_WhenDirectoryTitleIsSameAsParentDirectoryTitle()
    {
        // Arrange
        var directory = new Directory { Title = "Root Directory" };
        var parentDirectory = new DirectoryViewModel { Title = "Root Directory" };

        var directoryViewModel = new DirectoryViewModel("test-slug", directory)
        {
            ParentDirectory = parentDirectory
        };

        // Act
        var result = directoryViewModel.IsRootDirectory;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRootDirectory_ReturnsFalse_WhenDirectoryTitleIsDifferentFromParentDirectoryTitle()
    {
        // Arrange
        var directory = new Directory { Title = "Sub Directory" };
        var parentDirectory = new DirectoryViewModel { Title = "Root Directory" };

        var directoryViewModel = new DirectoryViewModel("test-slug", directory)
        {
            ParentDirectory = parentDirectory
        };

        // Act
        var result = directoryViewModel.IsRootDirectory;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DoPagination_PaginatesEntriesCorrectly()
    {
        // Arrange
        var viewModel = new DirectoryViewModel
        {
            FilteredEntries = Enumerable.Range(1, 50).Select(i => new DirectoryEntryViewModel { DirectoryEntry = new DirectoryEntry{ Name = i.ToString() }}).ToList()
        };

        // Act
        viewModel.Paginate(2);
        var paginatedEntries = viewModel.PaginatedEntries.ToList();
        
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
        var viewModel = new DirectoryViewModel
        {
            PaginationInfo = new PaginationInfo
            {
                PageSize = 12,
                TotalEntries = 24
            }
        };

        Assert.True(viewModel.ShowPagination);
    }

[Fact]
    public void ShowPagination_False_EntriesLessThanOrEqualPageSize()
    {
        var viewModel = new DirectoryViewModel
        {
            PaginationInfo = new PaginationInfo
            {
                PageSize = 12,
                TotalEntries = 12
            }
        };

        Assert.False(viewModel.ShowPagination);
    }

    [Fact]
    public void DisplayTitlePopulatedCorrectly_When_SearchTerm_HasValue()
    {
        var viewModel = new DirectoryViewModel
        {
            Title = "Test Title",
            SearchTerm ="Test Search Term"
        };

        var result = viewModel.DisplayTitle;
        Assert.Equal($"Results for \"{viewModel.SearchTerm}\"", result);
    }

    [Fact]
    public void DisplayTitlePopulatedCorrectly_When_SearchTerm_HasNoValue()
    {
        var viewModel = new DirectoryViewModel
        {
            Title = "Test Title"
        };

        var result = viewModel.DisplayTitle;
        Assert.Equal($"Results for {viewModel.Title}", result);
    }

    [Fact]
    public void PageTitlePopulatedCorrectly_When_NoPagination()
    {
        var viewModel = new DirectoryViewModel
        {
            Title = "Test Title",
            PaginationInfo = new PaginationInfo
            {
                PageSize = 12,
                TotalEntries = 12
            }
        };

        var result = viewModel.PageTitle;
        Assert.Equal($"Results for {viewModel.Title}", result);
    }

    [Fact]
    public void PageTitlePopulatedCorrectly_With_Pagination()
    {
        var viewModel = new DirectoryViewModel
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

        var result = viewModel.PageTitle;
        Assert.Equal($"Results for {viewModel.Title} (page 1 of 2)", result);
    }

    [Fact]
    public void SearchBranding_Returns_Correct_Value_When_ParentValueSet()
    {
        var viewModel = new DirectoryViewModel()
        {
            ParentDirectory = new DirectoryViewModel(new Directory() { SearchBranding = "Test Branding" })
        };
        var result = viewModel.SearchBranding;
        Assert.Equal("Test Branding", result);
    }

    [Fact]
    public void SearchBranding_Returns_Default_Value_When_ParentValueSet_ButHasNoBranding()
    {
        var viewModel = new DirectoryViewModel()
        {
            ParentDirectory = new DirectoryViewModel(new Directory())
        };

        var result = viewModel.SearchBranding;
        Assert.Equal("Default", result);
    }

    [Fact]
    public void SearchBranding_Returns_Default_Value_When_ParentValueSet()
    {
        var viewModel = new DirectoryViewModel();
        var result = viewModel.SearchBranding;
        Assert.Equal("Default", result);
    }

    [Fact]
    public void SearchBranding_Returns_Default_Value_When_No_FirstSubDirectory()
    {
        var viewModel = new DirectoryViewModel()
        {
            FirstSubDirectory = new DirectoryViewModel() { ColourScheme = "" }
        };

        var result = viewModel.InheritedColourScheme;
        Assert.Equal("teal", result);
    }

    [Fact]
    public void SearchBranding_Returns_Correct_Value_When_FirstSubDirectory()
    {
        var viewModel = new DirectoryViewModel()
        {
            FirstSubDirectory = new DirectoryViewModel() { ColourScheme = "pink" }
        };

        var result = viewModel.InheritedColourScheme;
        Assert.Equal("pink", result);
    }
}