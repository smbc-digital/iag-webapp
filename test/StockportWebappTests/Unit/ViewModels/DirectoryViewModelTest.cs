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
            FilteredEntries = Enumerable.Range(1, 50).Select(i => new DirectoryEntry { Name = i.ToString() }).ToList()
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
        Assert.Equal("13", paginatedEntries[0].Name);
        Assert.Equal("14", paginatedEntries[1].Name);
        Assert.Equal("15", paginatedEntries[2].Name);
        Assert.Equal("16", paginatedEntries[3].Name);
        Assert.Equal("17", paginatedEntries[4].Name);
        Assert.Equal("18", paginatedEntries[5].Name);
        Assert.Equal("19", paginatedEntries[6].Name);
        Assert.Equal("20", paginatedEntries[7].Name);
        Assert.Equal("21", paginatedEntries[8].Name);
        Assert.Equal("22", paginatedEntries[9].Name);
        Assert.Equal("23", paginatedEntries[10].Name);
        Assert.Equal("24", paginatedEntries[11].Name);
    }
}