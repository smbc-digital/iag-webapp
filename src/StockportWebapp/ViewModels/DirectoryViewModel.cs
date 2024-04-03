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

    public List<DirectoryEntry> PaginatedEntries { get; set; }
    
    public PaginationInfo PaginationInfo { get; set; }
    public IEnumerable<DirectoryEntry> PinnedEntries { get; set; }
    public bool DisplayIcons => Directory?.SubDirectories is not null && 
                                Directory.SubDirectories.Any() && 
                                Directory.SubDirectories.All(item => item is not null && !string.IsNullOrEmpty(item.Icon));

    public bool IsRootDirectory => Directory.Title.Equals(ParentDirectory.Title);

    public List<Query> QueryParameters
    {
        get
        {
            List<Query> queryParameters = new();
            if (!string.IsNullOrEmpty(SearchTerm))
                queryParameters.Add(new Query("searchTerm", SearchTerm));

            if (!string.IsNullOrEmpty(Order))
            {
                queryParameters.Add(new Query("orderBy", Order.Replace(" ", "-")));
            }

            AppliedFilters?.ToList().ForEach(filter => queryParameters.Add(new Query("filters", filter.Slug)));
           
            return queryParameters;
        }
    }
    
    public Dictionary<string, int> FilterCounts { get; set; }

    public static void DoPagination(DirectoryViewModel viewModel, int page)
    {
        int totalEntries = viewModel.FilteredEntries.Count();
        int pageSize = 12;
        int totalPages = (int)Math.Ceiling((double)totalEntries / pageSize);
        var pinnedEntries = viewModel.PinnedEntries.Select(entry => entry.Name).ToList();

        page = Math.Max(1, Math.Min(page, totalPages));

        int startIndex = (page - 1) * pageSize;

        if (page.Equals(1))
        {
            viewModel.PaginatedEntries = viewModel.FilteredEntries
                .Where(entry => !pinnedEntries.Contains(entry.Name))
                .Skip(startIndex + viewModel.PinnedEntries.Count())
                .Take(pageSize - viewModel.PinnedEntries.Count())
                .ToList();
        }
        else
        {
            viewModel.PaginatedEntries = viewModel.FilteredEntries
                .Skip(startIndex)
                .Take(pageSize)
                .ToList();
        }

        viewModel.PaginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages,
            TotalEntries = totalEntries,
            PageSize = pageSize
        };
    }
}

public class PaginationInfo
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalEntries { get; set; }
    public int PageSize { get; set; }
}