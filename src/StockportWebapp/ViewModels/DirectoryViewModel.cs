using StockportWebapp.Models;
using WilderMinds.RssSyndication;
using Directory = StockportWebapp.Models.Directory;
using Filter = StockportWebapp.Model.Filter;

namespace StockportWebapp.ViewModels;

public class DirectoryViewModel
{

    private NavCardList _PrimaryItems;

    // Core page details
    public string Slug { get; set; }
    public string Title => Directory.Title;
    public string MetaDescription => Directory.MetaDescription;
    public string Body => Directory.Body;
    public CallToActionBanner CallToAction => Directory.CallToAction;
    public IEnumerable<Alert> Alerts => Directory.Alerts;
    public EventCalendarBanner EventBanner => Directory.EventBanner;


    public IEnumerable<Crumb> Breadcrumbs { get; set; }

    // Search sorting and filtering options
    public string SearchTerm { get; set; }
    public string Order { get; set; }
    public PaginationInfo PaginationInfo { get; set; }
    public List<string> OrderBy = new() { "Name A to Z", "Name Z to A" };
    public IEnumerable<Filter> AppliedFilters { get; set; }
    public IEnumerable<FilterTheme> AllFilterThemes { get; set; }
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

    // Page layout properties
    public bool DisplayIcons => PrimaryItems is not null &&
                            PrimaryItems.Items.Any() &&
                            PrimaryItems.Items.All(item => item is not null && !string.IsNullOrEmpty(item.Icon));

    public bool IsRootDirectory => Title.Equals(ParentDirectory.Title);
    public Dictionary<string, int> FilterCounts { get; set; }


    // Results
    public IEnumerable<DirectoryEntry> FilteredEntries { get; set; }
    public IEnumerable<DirectoryEntry> PinnedEntries { get; set; }
    public IEnumerable<DirectoryEntry> PaginatedEntries { get; set; }

    // Base data
    public Directory Directory { get; set; }
    public Directory ParentDirectory { get; set; }
    public Directory FirstSubDirectory { get; set; }
    public DirectoryEntry DirectoryEntry { get; set; }
 
    public static void DoPagination(DirectoryViewModel viewModel, int page)
    {
        var allEntries = viewModel.PinnedEntries is not null ? viewModel.PinnedEntries.Concat(viewModel.FilteredEntries).Distinct(new DirectoryEntryComparer()) : viewModel.FilteredEntries;
        int pageSize = 12;
        int totalPages = (int)Math.Ceiling((double)allEntries.Count() / pageSize);

        page = Math.Max(1, Math.Min(page, totalPages));

        int startIndex = (page - 1) * pageSize;

        if (page.Equals(1))
        {
            viewModel.PaginatedEntries = allEntries
                .Skip(startIndex + viewModel.PinnedEntries.Count())
                .Take(pageSize - viewModel.PinnedEntries.Count())
                .ToList();
        }
        else
        {
            viewModel.PaginatedEntries = allEntries
                .Skip(startIndex)
                .Take(pageSize)
                .ToList();
        }

        viewModel.PaginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages,
            TotalEntries = allEntries.Count(),
            PageSize = pageSize
        };
    }

    public NavCardList PrimaryItems
    {
        get
        {
            if(_PrimaryItems == null)
            {
                var directoryItems = Directory.SubItems.Where(item => item.Type == "directory").Select(subItem => new NavCard(subItem.Title, Slug + subItem.NavigationLink, subItem.Teaser, subItem.Image, subItem.Icon, subItem.ColourScheme));
                var nonDirectoryItems = Directory.SubItems.Where(item => item.Type != "directory").Select(subItem => new NavCard(subItem.Title, subItem.NavigationLink, subItem.Teaser, subItem.Image, subItem.Icon, subItem.ColourScheme));
                _PrimaryItems = new NavCardList() { Items = directoryItems.Concat(nonDirectoryItems).ToList() };
            }

            return _PrimaryItems;
        }
    }
}
