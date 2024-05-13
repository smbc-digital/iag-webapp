using StockportWebapp.Comparers;
using SharpKml.Base;
using Filter = StockportWebapp.Model.Filter;
using SharpKml.Dom;
using System.Web;

namespace StockportWebapp.ViewModels;

public class DirectoryEntryViewModel : ISlugComparable
{
    public DirectoryEntryViewModel() { }

    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry)
    {
        Slug = slug;
        DirectoryEntry = directoryEntry;

    }

    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry, IEnumerable<Crumb> breadcrumbs, MapDetails mapDetails)
        : this(slug, directoryEntry)
    {
        Breadcrumbs = breadcrumbs;
        MapDetails = mapDetails;
    }

    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry, bool isPinned)
        : this(slug, directoryEntry)
        => IsPinned = isPinned;

    public DirectoryEntryViewModel(string slug, DirectoryEntry directoryEntry, IEnumerable<Crumb> breadcrumbs, MapDetails mapDetails, bool isPinned)
        : this(slug, directoryEntry, breadcrumbs, mapDetails)
        => IsPinned = isPinned;

    public DirectoryEntry DirectoryEntry { get; set; }
    public string Slug { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public bool IsPinned { get; set; } = false;
    public int MapPinIndex { get; set; } = 0;
    public MapDetails MapDetails { get; set; }
    public bool ShowMapPin => DirectoryEntry.IsNotOnTheEqautor;
    public IEnumerable<Filter> HighlightedFilters => DirectoryEntry.Themes?
                                                        .SelectMany(_ => _.Filters.Where(_ => _.Highlight.Equals(true)))
                                                        .ToList();

    public bool DisplaySocials => !string.IsNullOrEmpty(DirectoryEntry.Facebook)
                                    || !string.IsNullOrEmpty(DirectoryEntry.Twitter)
                                    || !string.IsNullOrEmpty(DirectoryEntry.Youtube)
                                    || !string.IsNullOrEmpty(DirectoryEntry.Instagram)
                                    || !string.IsNullOrEmpty(DirectoryEntry.LinkedIn);
    public bool HasPrimaryContact
        => !string.IsNullOrEmpty(DirectoryEntry.PhoneNumber) || !string.IsNullOrEmpty(DirectoryEntry.Email);
    public bool DisplayContactUs => !string.IsNullOrEmpty(DirectoryEntry.Website) || HasPrimaryContact || DisplaySocials;
    public bool DisplayMap => MapDetails.MapPosition is not null && DirectoryEntry.IsNotOnTheEqautor;
    public string AddressWithoutTags => Regex.Replace(DirectoryEntry.Address, "<.*?>", ""); 
    public string ParentSlug { get; set; }  

    public string FullyResolvedSlug => $"{ParentSlug}/{Slug}";
    
    public string ToString(string url) => string.Format("position: {{ lat: {0}, lng: {1} }}, title: \"{2}\", content: \"<h1 class='h-m'>{2}</h1><p class='body'>{3}</p><a class='body' href='{6}'>View {2}</a>\", isPinned: {4}, mapPinIndex: {5}", DirectoryEntry.MapPosition.Lat, DirectoryEntry.MapPosition.Lon, HttpUtility.HtmlEncode(DirectoryEntry.Name), HttpUtility.HtmlEncode(DirectoryEntry.Teaser), IsPinned ? "true" : "false", MapPinIndex, url);
    
    public Placemark ToKmlPlacemark(string pinnedStyle ="") => new()
    {
        // Ref
        // https://developers.google.com/kml/documentation/kml_tut?csw=1#descriptive_html
        // https://github.com/samcragg/sharpkml/blob/main/docs/BasicUsage.md

        Geometry = new Point
        {
            Coordinate = new Vector(DirectoryEntry.MapPosition.Lat, DirectoryEntry.MapPosition.Lon),
        },
        Name = DirectoryEntry.Name,
        // Need to find a better way of doing this!
        Description = new Description() { Text = $@"<a href='/directories/entry/{FullyResolvedSlug}'><h1><{DirectoryEntry.Name}</h1><p>{DirectoryEntry.Teaser}</p></a>" },
        PhoneNumber = DirectoryEntry.PhoneNumber,
        Address = DirectoryEntry.Address,
        AtomLink = new SharpKml.Dom.Atom.Link { Href = new Uri("https://www.stockport.gov.uk"), Title = $"Visit {DirectoryEntry.Name}" },
        StyleUrl = string.IsNullOrEmpty(pinnedStyle) ? null : new Uri($"#{pinnedStyle}", UriKind.Relative),
    };
}