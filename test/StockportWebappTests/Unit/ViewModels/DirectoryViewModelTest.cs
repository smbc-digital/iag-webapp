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
            SubDirectories = new List<Directory>
            {
                new() { Icon = "icon1" },
                new() { Icon = "icon2" }
            }
        };

        var directoryViewModel = new DirectoryViewModel { Directory = directory };

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
        var directoryViewModel = new DirectoryViewModel { Directory = directory };

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

        var directoryViewModel = new DirectoryViewModel { Directory = directory };

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
            SubDirectories = new List<Directory>
            {
                new() { Icon = "icon1" },
                new(),
            }
        };

        var directoryViewModel = new DirectoryViewModel { Directory = directory };

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
        var parentDirectory = new Directory { Title = "Root Directory" };

        var directoryViewModel = new DirectoryViewModel { Directory = directory, ParentDirectory = parentDirectory };

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
        var parentDirectory = new Directory { Title = "Root Directory" };

        var directoryViewModel = new DirectoryViewModel { Directory = directory, ParentDirectory = parentDirectory };

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
        DirectoryViewModel.DoPagination(viewModel, 2);

        // Assert
        Assert.Equal(12, viewModel.PaginatedEntries.Count);
        Assert.Equal(2, viewModel.PaginationInfo.CurrentPage);
        Assert.Equal(5, viewModel.PaginationInfo.TotalPages);
        Assert.Equal(50, viewModel.PaginationInfo.TotalEntries);
        Assert.Equal(12, viewModel.PaginationInfo.PageSize);
        Assert.Equal("13", viewModel.PaginatedEntries[0].Name);
        Assert.Equal("14", viewModel.PaginatedEntries[1].Name);
        Assert.Equal("15", viewModel.PaginatedEntries[2].Name);
        Assert.Equal("16", viewModel.PaginatedEntries[3].Name);
        Assert.Equal("17", viewModel.PaginatedEntries[4].Name);
        Assert.Equal("18", viewModel.PaginatedEntries[5].Name);
        Assert.Equal("19", viewModel.PaginatedEntries[6].Name);
        Assert.Equal("20", viewModel.PaginatedEntries[7].Name);
        Assert.Equal("21", viewModel.PaginatedEntries[8].Name);
        Assert.Equal("22", viewModel.PaginatedEntries[9].Name);
        Assert.Equal("23", viewModel.PaginatedEntries[10].Name);
        Assert.Equal("24", viewModel.PaginatedEntries[11].Name);
    }
}