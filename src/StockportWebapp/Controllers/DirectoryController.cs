using System.Net.Mime;
using Filter = StockportWebapp.Model.Filter;
using Directory = StockportWebapp.Models.Directory;
namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
public class DirectoryController : Controller
{
    private readonly IDirectoryRepository _directoryRepository;
    private readonly MarkdownWrapper _markdownWrapper;

    public DirectoryController(IDirectoryRepository directoryRepository, MarkdownWrapper markdownWrapper)
    {
        _directoryRepository = directoryRepository;
        _markdownWrapper = markdownWrapper;
    }

    [Route("/directories/{**slug}")]
    public async Task<IActionResult> Directory(string slug)
    {
        var pageLocation = ProcessWildcardSlug(slug);

        var directoryHttpResponse = await _directoryRepository.Get<Directory>(pageLocation.Slug);
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        var directory = directoryHttpResponse.Content as Directory;
        directory.Body = _markdownWrapper.ConvertToHtml(directory.Body ?? "");

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = directory,
            FilteredEntries = _directoryRepository.GetFilteredEntryForDirectories(directory),
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, false),
            Slug = slug
        };

        if(directory.SubDirectories.Any())
            return View(directoryViewModel);

        return View("results", directoryViewModel);
    }

    [HttpGet]
    [Route("/directories/results/{**slug}")]
    public async Task<IActionResult> DirectoryResults([Required][FromRoute]string slug, string[] filters, string orderBy)
    {
        var pageLocation = ProcessWildcardSlug(slug);

        var directoryHttpResponse = await _directoryRepository.Get<Directory>(pageLocation.Slug);
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        var directory = directoryHttpResponse.Content as Directory;

        var filteredEntries =  filters.Any() 
            ? _directoryRepository.GetFilteredEntryForDirectories(directory, filters) 
            : _directoryRepository.GetFilteredEntryForDirectories(directory);

        var allFilterThemes = _directoryRepository.GetAllFilterThemes(filteredEntries);
        var appliedFilters = _directoryRepository.GetAppliedFilters(filters, allFilterThemes);
        
        filteredEntries = _directoryRepository.GetOrderedEntries(filteredEntries, orderBy);

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = directory,
            FilteredEntries = filteredEntries,
            AllFilterThemes = allFilterThemes,
            AppliedFilters = appliedFilters,
            Order = orderBy,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, false),
            Slug = slug
        };        
        
        return View("results", directoryViewModel);
    }


    [Route("/directories/results/kml/{slug}")]  
    [Produces(MediaTypeNames.Application.Xml)]
    public async Task<IActionResult> DirectoryAsKml(string slug)
    {
        var directoryHttpResponse = await _directoryRepository.Get<Directory>(slug);
        if (!directoryHttpResponse.IsSuccessful())
            return directoryHttpResponse;

        var directory = (Directory)directoryHttpResponse.Content;
        var kmlString = directory.ToKml();
        return Content(kmlString);
    }


    [Route("directories/entry/{**slug}")]
    public async Task<IActionResult> DirectoryEntry(string slug)
    {
        var pageLocation = ProcessWildcardSlug(slug);

        // Get the requested entry
        var directoryEntryHttpResponse = await _directoryRepository.GetEntry<DirectoryEntry>(pageLocation.Slug);
        if (!directoryEntryHttpResponse.IsSuccessful())
            return directoryEntryHttpResponse;

        var processedDirectoryEntry = directoryEntryHttpResponse.Content as DirectoryEntry;
        processedDirectoryEntry.Description = _markdownWrapper.ConvertToHtml(processedDirectoryEntry.Description ?? "");
        processedDirectoryEntry.Address = _markdownWrapper.ConvertToHtml(processedDirectoryEntry.Address ?? "");

        // Get the parent directories
        List<Directory> parentDirectories = await GetParentDirectories(pageLocation.ParentSlugs);

        DirectoryViewModel directoryViewModel = new()
        {
            Directory = parentDirectories.First(),
            DirectoryEntry = processedDirectoryEntry,
            Breadcrumbs = GetBreadcrumbsForDirectories(parentDirectories, true),
            Slug = slug
        };

        return View(directoryViewModel);
    }

    private PageLocation ProcessWildcardSlug(string slug)
    {
        var slugValues = slug?.Split('/') ?? Array.Empty<string>();
        return new PageLocation(slugValues.Last(), slugValues.SkipLast(1).ToList());
    }

    private List<Crumb> GetBreadcrumbsForDirectories(IList<Directory> parentDirectories, bool viewLastBreadcrumbAsResults = false) 
    {
        List<Crumb> breadcrumbs = new();
        for (int i = 0; i < parentDirectories.Count; i++)
        {
            var directory = parentDirectories[i];
            var relativeUrl = string.Join("/", parentDirectories.Take(i + 1).Select(_ => _.Slug));
            if(i == parentDirectories.Count - 1 && viewLastBreadcrumbAsResults)
            {
                breadcrumbs.Add(new Crumb(directory.Title, $"directories/results/{relativeUrl}", "Directories"));
            }
            else
            {
                breadcrumbs.Add(new Crumb(directory.Title, $"directories/{relativeUrl}", "Directories"));
            }
            
        }

        return breadcrumbs;
    }

    private async Task<List<Directory>> GetParentDirectories(IEnumerable<string> parentSlugs)
    {
        List<Directory> parentDirectories = new();
        HttpResponse directoryHttpResponse;
        foreach (var directorySlug in parentSlugs)
        {
            directoryHttpResponse = await _directoryRepository.Get<Directory>(directorySlug);
            if (directoryHttpResponse.IsSuccessful())
                parentDirectories.Add(directoryHttpResponse.Content as Directory);
        }
        return parentDirectories;
    }
}

public class PageLocation
{
    public string Slug { get; private set; }
    public IEnumerable<string> ParentSlugs { get; private set; }

    public PageLocation(string slug, IEnumerable<string> parentSlugs)
    {
        Slug = slug;
        ParentSlugs = parentSlugs;
    }
}