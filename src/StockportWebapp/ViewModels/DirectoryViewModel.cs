using SharpKml.Dom.Atom;
using StockportWebapp.Comparers;
using Directory = StockportWebapp.Models.Directory;
using Filter = StockportWebapp.Model.Filter;

namespace StockportWebapp.ViewModels;

public class DirectoryViewModel
{
    public DirectoryViewModel() { }

    public DirectoryViewModel(Directory directory) : this(directory.Slug, directory) { }

    public DirectoryViewModel(string slug, Directory directory, IEnumerable<Crumb> breadcrumbs) 
        : this(slug, directory) => Breadcrumbs = breadcrumbs;

    public DirectoryViewModel(string slug, Directory directory, IEnumerable<Crumb> breadcrumbs, IEnumerable<DirectoryEntry> pinnedEntries, IEnumerable<DirectoryEntry> filteredEntries) 
        : this(slug, directory)
    {
        Breadcrumbs = breadcrumbs;
        PinnedEntries = pinnedEntries.Select(entry => new DirectoryEntryViewModel($"{slug}/{entry.Slug}", entry, true));
        FilteredEntries = filteredEntries.Select(entry => new DirectoryEntryViewModel($"{slug}/{entry.Slug}", entry, false));
        AddMapPinIndexes();
    }

    public DirectoryViewModel(string slug, Directory directory, IEnumerable<Crumb> breadcrumbs, IEnumerable<DirectoryEntry> pinnedEntries, IEnumerable<DirectoryEntry> filteredEntries, int pageNumber)
        : this(slug, directory)
    {
        Breadcrumbs = breadcrumbs;
        PinnedEntries = pinnedEntries.Select(entry => new DirectoryEntryViewModel($"{slug}/{entry.Slug}", entry, true));
        FilteredEntries = filteredEntries.Select(entry => new DirectoryEntryViewModel($"{slug}/{entry.Slug}", entry, false));
        Paginate(pageNumber);
        AddMapPinIndexes();
    }

    public DirectoryViewModel(string slug, Directory directory)
    {
        Slug = slug;
        Title = directory.Title;
        MetaDescription = directory.MetaDescription;
        Body = directory.Body;
        CallToAction = directory.CallToAction;
        Alerts = directory.Alerts;
        EventBanner = directory.EventBanner;
        ColourScheme = directory.ColourScheme;
        RelatedContent = directory.RelatedContent;
        ExternalLinks = directory.ExternalLinks;
        _searchBranding = directory.SearchBranding;
        var directoryItems = directory.SubItems.Where(item => item.Type == "directory").Select(subItem => new NavCard(subItem.Title, subItem.GetNavigationLink(Slug), subItem.Teaser, subItem.Image, subItem.Icon, subItem.ColourScheme));
        var nonDirectoryItems = directory.SubItems.Where(item => item.Type != "directory").Select(subItem => new NavCard(subItem.Title, subItem.NavigationLink, subItem.Teaser, subItem.Image, subItem.Icon, subItem.ColourScheme));
        PrimaryItems = new NavCardList() { Items = directoryItems.Concat(nonDirectoryItems).ToList() };
    }

    // Default values
    private readonly int _defaultPageSize = 12;
    private readonly string _searchBranding = "Default";

    // Core page details
    public string Slug { get; set; }
    public string Title { get; set; }
    public string DisplayTitle => "Results for " + (string.IsNullOrEmpty(SearchTerm) ? Title : $"\"{SearchTerm}\"");
    public string PageTitle => DisplayTitle + (ShowPagination? " (page " + PaginationInfo.CurrentPage + " of " + PaginationInfo.TotalPages + ")" : string.Empty);
    public string MetaDescription { get; set; }
    public string Body { get; set; }
    public CallToActionBanner CallToAction { get; set; }
    public IEnumerable<Alert> Alerts { get; set; }
    public EventCalendarBanner EventBanner { get; set; }
    public string ColourScheme { get; set; }
    public string SearchBranding => ParentDirectory != null
                                    && !string.IsNullOrEmpty(ParentDirectory.SearchBranding)
                                        ? ParentDirectory.SearchBranding
                                        : _searchBranding;
    public string InheritedColourScheme => string.IsNullOrEmpty(FirstSubDirectory.ColourScheme)
                                            ? "teal"
                                            : FirstSubDirectory.ColourScheme.ToLower();
    public NavCardList PrimaryItems { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public IEnumerable<SubItem> RelatedContent { get; set; }
    public IEnumerable<ExternalLink> ExternalLinks { get; set; } = new List<ExternalLink>();

    // Search sorting and filtering options
    public string SearchTerm { get; set; }
    public string Order { get; set; }
    public PaginationInfo PaginationInfo { get; set; }
    public bool ShowPagination => PaginationInfo is not null && PaginationInfo.TotalEntries > PaginationInfo.PageSize;
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
                queryParameters.Add(new Query("orderBy", Order.Replace(" ", "-")));

            AppliedFilters?.ToList().ForEach(filter => queryParameters.Add(new Query("filters", filter.Slug)));

            return queryParameters;
        }
    }

    // Page layout properties
    public bool DisplayIcons => PrimaryItems is not null
                                    && PrimaryItems.Items.Any()
                                    && PrimaryItems.Items.All(item => item is not null && !string.IsNullOrEmpty(item.Icon));
    public bool IsRootDirectory => Title.Equals(ParentDirectory.Title);
    public Dictionary<string, int> FilterCounts { get; set; }
    public IEnumerable<DirectoryEntryViewModel> FilteredEntries { get; set; }
    public IEnumerable<DirectoryEntryViewModel> PinnedEntries { get; set; }
    public IEnumerable<DirectoryEntryViewModel> PaginatedEntries { get; set; }

    // Base data
    public DirectoryViewModel ParentDirectory { get; set; }
    public DirectoryViewModel FirstSubDirectory { get; set; }

    public void Paginate(int page)
    {
        var allEntries = PinnedEntries is not null
                            ? PinnedEntries.Concat(FilteredEntries)
                                            .Distinct(new SlugComparer())
                                            .Select(entry => (DirectoryEntryViewModel)entry)
                            : FilteredEntries;

        int totalPages = (int)Math.Ceiling((double)allEntries.Count() / _defaultPageSize);

        page = Math.Max(1, Math.Min(page, totalPages));

        int startIndex = (page - 1) * _defaultPageSize;

        if (page.Equals(1))
        {
            PaginatedEntries = allEntries
                .Skip(startIndex + PinnedEntries.Count())
                .Take(_defaultPageSize - PinnedEntries.Count())
                .ToList();
        }
        else
        {
            PaginatedEntries = allEntries
                .Skip(startIndex)
                .Take(_defaultPageSize)
                .ToList();
        }

        PaginationInfo = new PaginationInfo
        {
            CurrentPage = page,
            TotalPages = totalPages,
            TotalEntries = allEntries.Count(),
            PageSize = _defaultPageSize
        };
    }

    public void AddMapPinIndexes()
    {
        int endIndex = 1;
        if(PaginationInfo.CurrentPage.Equals(1))
            PinnedEntries = AddMapPinIndexes(PinnedEntries, endIndex, out endIndex);
        
        PaginatedEntries = AddMapPinIndexes(PaginatedEntries, endIndex, out endIndex);
    }

    /// <summary>
    /// Todo - I'm not sure I like this - need to create a new list and then assign as can't mutatue the list values
    /// </summary>
    /// <param name="entries"></param>
    /// <param name="startIndex"></param>
    /// <param name="endIndex"></param>
    /// <returns></returns>
    private IEnumerable<DirectoryEntryViewModel> AddMapPinIndexes(IEnumerable<DirectoryEntryViewModel> entries, int startIndex, out int endIndex)
    {
        var currentIndex = startIndex;
        var mutatedEnties = new List<DirectoryEntryViewModel>();
        foreach(DirectoryEntryViewModel entry in entries)
        {
            entry.MapPinIndex = entry.DirectoryEntry.IsNotOnTheEqautor ? currentIndex : 0;
            mutatedEnties.Add(entry);
            if (entry.DirectoryEntry.IsNotOnTheEqautor)
                currentIndex += 1;
        }

        endIndex = currentIndex;
        return mutatedEnties;
    }
}