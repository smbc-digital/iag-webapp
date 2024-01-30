using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

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
        public IEnumerable<GroupBranding> Branding { get; set; } = new List<GroupBranding>();
        public MapPosition MapPosition { get; set; } = new MapPosition();
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Twitter { get; set; } = string.Empty;
        public string Facebook { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public Placemark ToKmlPlacemark(string styleUrl = null) => new Placemark
        {
            
            // Ref
            // https://developers.google.com/kml/documentation/kml_tut?csw=1#descriptive_html
            // https://github.com/samcragg/sharpkml/blob/main/docs/BasicUsage.md

            Geometry = new Point
            {
                Coordinate = new Vector(this.MapPosition.Lat, this.MapPosition.Lon),
            },
            Name = Name,
            StyleUrl = string.IsNullOrEmpty(styleUrl) ? null :  new Uri(styleUrl, UriKind.Relative),
            Description = new Description() { Text = $@"<![CDATA[<p>{ this.Teaser }</p>]]>" },
            PhoneNumber = this.PhoneNumber,
            Address = this.Address,
            Snippet  = new Snippet()
            {
                Text = "This is some descriptive text"
            },
            GXBalloonVisibility = true,
            AtomLink = new SharpKml.Dom.Atom.Link { Href = new Uri("https://www.stockport.gov.uk"), Title=$"Visit {this.Name}" }
        };
    }
}