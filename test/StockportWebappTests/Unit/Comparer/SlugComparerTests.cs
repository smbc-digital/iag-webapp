using StockportWebapp.Comparers;

namespace StockportWebappTests_Unit.Unit.Comparers;

public class SlugComparerTests
{
    [Fact]
    public void Slug_ReturnCorrectlyFormedSlug()
    {
        var viewModel = new DirectorySearchResultViewModel
        {
            Entry = new DirectoryEntryViewModel { Slug = "DirectoryEntry" },
            DirectorySlug = "Directory"
        };
        
        SlugComparer comparer = new SlugComparer();
        
        var result = viewModel.Slug;

        Assert.Equal("Directory/DirectoryEntry", result);
    }
}