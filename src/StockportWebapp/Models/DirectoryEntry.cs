using SharpKml.Base;
using SharpKml.Dom;

namespace StockportWebapp.Models
{
    public class DirectoryEntry
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string Description { get; set; }
        public string Teaser { get; set; }
        public string MetaDescription { get; set; }
        public IEnumerable<FilterTheme> Themes { get; set; } = new List<FilterTheme>();
        public IEnumerable<MinimalDirectory> Directories { get; set; }
        public IEnumerable<Alert> Alerts { get; set; }
        public List<GroupBranding> Branding { get; set; } = new List<GroupBranding>();
        public MapPosition MapPosition { get; set; } = new MapPosition();
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Twitter { get; set; } = string.Empty;
        public string Facebook { get; set; } = string.Empty;
        public string Youtube { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Image { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public MapDetails MapDetails { get; set; }

        public Placemark ToKmlPlacemark() => new()
        {
            // Ref
            // https://developers.google.com/kml/documentation/kml_tut?csw=1#descriptive_html
            // https://github.com/samcragg/sharpkml/blob/main/docs/BasicUsage.md

            Geometry = new Point
            {
                Coordinate = new Vector(MapPosition.Lat, MapPosition.Lon),
            },
            Name = Name,
            Description = new Description() { Text = $@"<![CDATA[{ Teaser }]]>" },
            PhoneNumber = PhoneNumber,
            Address = Address,
            AtomLink = new SharpKml.Dom.Atom.Link { Href = new Uri("https://www.stockport.gov.uk"), Title=$"Visit {Name}" }
        };
    }

    public class DirectoryEntryComparer : IEqualityComparer<DirectoryEntry>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(DirectoryEntry x, DirectoryEntry y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Slug == y.Slug;
        }

        public int GetHashCode(DirectoryEntry directoryEntry)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(directoryEntry, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashDirectorySlug = directoryEntry.Slug == null ? 0 : directoryEntry.Slug.GetHashCode();
            return hashDirectorySlug;
        }
    }
}