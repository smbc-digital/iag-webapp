using Directory = StockportWebapp.Models.Directory;

namespace StockportWebapp.ViewModels;

public class DirectoryViewModel
{
    public string Slug { get; set; }
    public Directory Directory { get; set; }

    public IEnumerable<DirectoryEntry> FilteredEntries { get; set; }

    public IEnumerable<Filter> AppliedFilters { get; set; }

    public DirectoryEntry DirectoryEntry { get; set; }

    public IEnumerable<Crumb> Breadcrumbs { get; set; }
}
