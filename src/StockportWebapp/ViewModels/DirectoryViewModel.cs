using Directory = StockportWebapp.Models.Directory;
using Filter = StockportWebapp.Model.Filter;

namespace StockportWebapp.ViewModels;

public class DirectoryViewModel
{
    public string Slug { get; set; }
    
    public Directory Directory { get; set; }
    
    public Directory ParentDirectory { get; set; }
    
    public Directory FirstSubDirectory { get; set; }

    public IEnumerable<DirectoryEntry> FilteredEntries { get; set; }

    public IEnumerable<Filter> AppliedFilters { get; set; }

    public string SearchTerm { get; set; }

    public IEnumerable<FilterTheme> AllFilterThemes { get; set; }

    public DirectoryEntry DirectoryEntry { get; set; }

    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public string Order { get; set; }

    public List<string> OrderBy = new() { "Name A to Z", "Name Z to A" };

    public bool DisplayIcons => Directory.SubDirectories.All(item => !string.IsNullOrEmpty(item.Icon));

    public bool IsRootDirectory => Directory.Title.Equals(ParentDirectory.Title);

    public List<Query> QueryParameters
    {
        get
        {
            List<Query> queryParameters = new();
            if (!string.IsNullOrEmpty(SearchTerm))
                queryParameters.Add(new Query("searchTerm", SearchTerm));

            if (!string.IsNullOrEmpty(Order))
                queryParameters.Add(new Query("orderBy", Order));

            if (AppliedFilters is not null)
                AppliedFilters.ToList().ForEach(filter => queryParameters.Add(new Query("filters", filter.Slug)));

            return queryParameters;
        }
    }

    public Dictionary<string, int> FilterCounts { get; set; }


}
