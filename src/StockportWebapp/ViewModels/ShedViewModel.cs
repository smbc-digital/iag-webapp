using StockportWebapp.Comparers;

namespace StockportWebapp.ViewModels;

public class ShedViewModel(IEnumerable<ShedItem> filteredEntries) : ISlugComparable
{
    public IEnumerable<ShedEntryViewModel> FilteredEntries { get; set; } = filteredEntries.Select(entry => new ShedEntryViewModel(entry));
    public IEnumerable<ShedEntryViewModel> ShedItems { get; set; }
    public Pagination Pagination { get; set; } = new Pagination();
    public QueryUrl CurrentUrl { get; set; }

    public List<string> Wards = new()
    {
        "Bramhall North",
        "Bramhall South and Woodford",
        "Bredbury and Woodley",
        "Bredbury Green and Romiley",
        "Brinnington and Stockport Central",
        "Cheadle East and Cheadle Hulme North",
        "Cheadle Hulme South",
        "Cheadle West and Gatley",
        "Davenport and Cale Green",
        "Edgeley",
        "Hazel Grove",
        "Heald Green",
        "Heatons North",
        "Heatons South",
        "Manor",
        "Marple North",
        "Marple South and High Lane",
        "Norbury and Woodsmoor",
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

    public IFilteredUrl FilteredUrl { get; private set; }

    public void AddQueryUrl(QueryUrl queryUrl) =>
        CurrentUrl = queryUrl;
    
    public void AddFilteredUrl(IFilteredUrl filteredUrl) =>
        FilteredUrl = filteredUrl;
    
    public List<string> AppliedFilters { get; set; } = new();

    // Core page details
    public string Slug { get; set; }

    public string DisplayTitle =>
        string.IsNullOrEmpty(SearchTerm)
            ? "Results in Stockport's heritage assets"
            : $"Results for \"{SearchTerm}\"";

    public string PageTitle =>
        $"{DisplayTitle}{(ShowPagination
            ? $" (page {Pagination.CurrentPageNumber} of {Pagination.TotalPages})"
            : string.Empty)}";

    public string SearchTerm { get; set; }
    
    public bool ShowPagination =>
        Pagination is not null && Pagination.TotalItems > Pagination.MaxItemsPerPage;
}