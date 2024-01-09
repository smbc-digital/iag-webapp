using Directory = StockportWebapp.Models.Directory;

namespace StockportWebapp.ViewModels;

public class DirectoryViewModel
{
    public Directory Directory { get; set; }

    public IEnumerable<DirectoryEntry> FilteredEntries { get; set; }

    public IEnumerable<Filter> AppliedFilters { get; set; }

    public DirectoryEntry DirectoryEntry { get; set; }
}
