using StockportWebapp.Comparers;

namespace StockportWebapp.ViewModels;

public class ShedViewModel : ISlugComparable
{
    public ShedViewModel() { }

    public ShedViewModel(IEnumerable<ShedItem> filteredEntries)
    {
        FilteredEntries = filteredEntries.Select(entry => new ShedEntryViewModel(entry));
    }

    public List<string> Wards = new()
    {
        "Bramhall North",
        "Bramhall South & Woodford",
        "Bredbury & Woodley",
        "Bredbury Green & Romiley",
        "Brinnington & Stockport Central",
        "Cheadle East & Cheadle Hulme North",
        "Cheadle Hulme South",
        "Cheadle West & Gatley",
        "Davenport & Cale Green",
        "Edgeley",
        "Hazel Grove",
        "Heald Green",
        "Heatons North",
        "Heatons South",
        "Manor",
        "Marple North",
        "Marple South & High Lane",
        "Norbury & Woodsmoor",
        "Offerton",
        "Reddish North",
        "Reddish South"
    };

    public List<string> Grades = new()
    {
        "I",
        "II",
        "II*",
        "Local"
    };

    public List<string> AppliedFilters { get; set; } = new();

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

    // Search sorting and filtering options
    public string SearchTerm { get; set; }
    public string Order { get; set; } // not implemented in the UI yet, but can be used for sorting
    public PaginationInfo PaginationInfo { get; set; } // not implemented in the UI yet, but can be used for pagination
    public bool ShowPagination =>
        PaginationInfo is not null && PaginationInfo.TotalEntries > PaginationInfo.PageSize;

    public List<string> OrderBy = new() { "Name A to Z", "Name Z to A" };

    // Page layout properties
    public IEnumerable<ShedEntryViewModel> FilteredEntries { get; set; }
    public IEnumerable<ShedEntryViewModel> PaginatedEntries { get; set; }

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