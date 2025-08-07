using StockportWebapp.Comparers;

namespace StockportWebapp.ViewModels;

public class ShedViewModel : ISlugComparable
{
    public ShedViewModel() { }

    public ShedViewModel(IEnumerable<ShedItem> filteredEntries)
    {
        FilteredEntries = filteredEntries.Select(entry => new ShedEntryViewModel(entry));

        Paginate(1);
    }

    public Pagination Pagination { get; set; }

    // Core page details
    public string Slug { get; set; }
    public string Title { get; set; }

    public string DisplayTitle =>
        string.IsNullOrEmpty(SearchTerm)
            ? $"Results in {Title}"
            : $"Results for \"{SearchTerm}\"";

    public string PageTitle =>
        $"{DisplayTitle}{(ShowPagination
            ? $" (page {PaginationInfo.CurrentPage} of {PaginationInfo.TotalPages})"
            : string.Empty)}";

    public string MetaDescription { get; set; }
    public string Body { get; set; }
    public CallToActionBanner CallToAction { get; set; }
    public IEnumerable<Alert> Alerts { get; set; }
    public EventCalendarBanner EventBanner { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }

    // Search sorting and filtering options
    public string SearchTerm { get; set; }
    public string Order { get; set; }
    public PaginationInfo PaginationInfo { get; set; }
    public bool ShowPagination =>
        PaginationInfo is not null && PaginationInfo.TotalEntries > PaginationInfo.PageSize;

    public List<string> OrderBy = new() { "Name A to Z", "Name Z to A" };
    public IEnumerable<FilterTheme> AllFilterThemes { get; set; }
    public List<Query> QueryParameters
    {
        get
        {
            List<Query> queryParameters = new();
            if (!string.IsNullOrEmpty(SearchTerm))
                queryParameters.Add(new Query("searchTerm", SearchTerm));

            if (!string.IsNullOrEmpty(Order))
                queryParameters.Add(new Query("orderBy", Order.Replace(" ", "-")));

            return queryParameters;
        }
    }

    // Page layout properties
    public Dictionary<string, int> FilterCounts { get; set; }
    public IEnumerable<ShedEntryViewModel> FilteredEntries { get; set; }
    public IEnumerable<ShedEntryViewModel> PinnedEntries { get; set; }
    public IEnumerable<ShedEntryViewModel> PaginatedEntries { get; set; }

    // Base data
    public DirectoryViewModel ParentDirectory { get; set; }
    public DirectoryViewModel FirstSubDirectory { get; set; }

    public void Paginate(int page)
    {
        IEnumerable<ShedEntryViewModel> allEntries = FilteredEntries;

        int totalPages = (int)Math.Ceiling((double)allEntries.Count() / 12);

        page = Math.Max(1, Math.Min(page, totalPages));

        int startIndex = (page - 1) * 12;

        PaginatedEntries = allEntries
            .Skip(startIndex)
            .Take(12)
            .ToList();

        PaginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages,
            TotalEntries = allEntries.Count(),
            PageSize = 12
        };
    }
}