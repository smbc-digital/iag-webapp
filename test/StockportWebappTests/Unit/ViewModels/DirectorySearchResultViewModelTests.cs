namespace StockportWebappTests_Unit.Unit.ViewModels;

public class DirectorySearchResultViewModelTests
{
    [Fact]
    public void Slug_ReturnCorrectlyFormedSlug()
    {
        var viewModel = new DirectorySearchResultViewModel
        {
            Entry = new DirectoryEntryViewModel { Slug = "DirectoryEntry" },
            DirectorySlug = "Directory"
        };
        
        var result = viewModel.Slug;

        Assert.Equal("Directory/DirectoryEntry", result);
    }
}