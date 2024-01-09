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
        var mainFolder = new Folder()
        {
            Name = "Directory Entries for ..."
        };

        directoryEntries.ToList().ForEach(entry =>  mainFolder.AddFeature(entry.ToKmlPlacemark));
        
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