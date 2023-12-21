using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

namespace StockportWebapp.Extensions;
public static class DirectoryExtensions
{
    public static string GetKmlForList(this IEnumerable<DirectoryEntry> directoryEntries)
    {
        var placemark = new Placemark
        {
            Geometry = new Point
            {
                Coordinate = new Vector(-13.163959, -72.545992),
            },
            Name = "Machu Picchu"
        };

        var kml = KmlFile.Create(placemark, false);
        using (var stream = new MemoryStream())
        {
            kml.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return new StreamReader(stream).ReadToEnd();
        }
    }
}