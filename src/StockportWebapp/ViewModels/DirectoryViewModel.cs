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

    public string Order { get; set; }

    public List<string> OrderBy = new() { "Name A to Z", "Name Z to A" };

    public Dictionary<string, int> FilterCounts { get; set; }
}
