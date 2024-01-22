using SharpKml.Dom;
using SharpKml.Engine;

namespace StockportWebapp.Extensions;
public static class DirectoryExtensions
{
    // TODO Needs tests
    public static string GetKmlForList(this IEnumerable<DirectoryEntry> directoryEntries)
    {
        // Ref 
        // https://github.com/samcragg/sharpkml/blob/main/docs/BasicUsage.md

        var kml = new Kml();

        // Map sttyling - https://github.com/samcragg/sharpkml/blob/main/Examples/CreateIconStyle.cs
        var style = new Style();
        style.Id = "RedDot";
        style.Icon = new IconStyle();   
        style.Icon.Icon = new IconStyle.IconLink(new Uri("http://maps.google.com/mapfiles/ms/icons/red-dot.png"));
        style.Icon.Scale = 1.5;

        var mainFolder = new Folder()
        {
            Name = "Directory Entries for ..."
        };

        mainFolder.AddStyle(style);

        directoryEntries.ToList().ForEach(entry =>  mainFolder.AddFeature(entry.ToKmlPlacemark("RedDot")));
        
        kml.Feature = mainFolder;

        var kmlStream = KmlFile.Create(kml, false);
        using (var stream = new MemoryStream())
        {
            kmlStream.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return new StreamReader(stream).ReadToEnd();
        }
    }
}