using SharpKml.Dom;
using SharpKml.Engine;
using Filter = StockportWebapp.Model.Filter;

namespace StockportWebapp.Extensions;
public static class DirectoryExtensions
{
    // TODO Needs tests
    public static string GetKmlForList(this IEnumerable<DirectoryEntry> directoryEntries)
    {
        // Ref 
        // https://github.com/samcragg/sharpkml/blob/main/docs/BasicUsage.md

        var kml = new Kml();
        var mainFolder = new Folder()
        {
            Name = "Directory Entries for ..."
        };

        directoryEntries.ToList().ForEach(entry =>  mainFolder.AddFeature(entry.ToKmlPlacemark()));
        
        kml.Feature = mainFolder;

        var kmlStream = KmlFile.Create(kml, false);
        using (var stream = new MemoryStream())
        {
            kmlStream.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return new StreamReader(stream).ReadToEnd();
        }
    }

    // Check if the entry satisfies all the applied filters
    // All themes in the in the dictionary must have at least one associated matching filter
    public static bool IsDirectoryEntryRelevant(this Dictionary<string, List<string>> themes, DirectoryEntry entry) =>        
        themes.All(theme => theme.DirectoryEntrySatisfiesTheme(entry));

    // Checks a single theme to ensure if the entry satisfies filter conditions
    // Gets applied filters relevant to the current theme
    // Ensure that there is at least one matching applied filter
    public static bool DirectoryEntrySatisfiesTheme(this KeyValuePair<string, List<string>> themes, 
                                                    DirectoryEntry entry) =>
        themes.Value  
            .Any(appliedFilter => entry.Themes.Any(theme => !string.IsNullOrEmpty(theme.Title) && theme.Title.Equals(themes.Key)
                                    && theme.Filters.Any(filter => filter.Slug.Equals(appliedFilter))));

    /// <summary>
    /// Takes a master list of FilterThemes and a list of filters and creates a list of FilterThemes for just referenced Filters
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public static Dictionary<string, List<string>> GetFilterThemesFromFilters(this IEnumerable<Filter> filters)
    {
        var appliedThemes = new Dictionary<string, List<string>>();

        foreach (var filter in filters)
        {
            if (!appliedThemes.ContainsKey(filter.Theme))
                appliedThemes[filter.Theme] = new List<string>();

            appliedThemes[filter.Theme].Add(filter.Slug);
        }

        return appliedThemes;
    }
}    