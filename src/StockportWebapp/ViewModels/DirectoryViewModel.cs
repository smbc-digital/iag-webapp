using Directory = StockportWebapp.Models.Directory;
using Filter = StockportWebapp.Model.Filter;

namespace StockportWebapp.ViewModels;

public class DirectoryViewModel
{
    public Directory Directory { get; set; }

    public IEnumerable<DirectoryEntry> FilteredEntries { get; set; }

    public IEnumerable<Filter> AppliedFilters { get; set; }
    
    public IEnumerable<FilterTheme> AllFilterThemes { get; set; }

    public DirectoryEntry DirectoryEntry { get; set; }
}
