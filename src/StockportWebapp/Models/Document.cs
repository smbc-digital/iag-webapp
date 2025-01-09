namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Document(string title, int size, DateTime lastUpdated, string url, string fileName, string assetId, string mediaType)
{
    public readonly string Title = title;
    public readonly int Size = size;
    public readonly DateTime LastUpdated = lastUpdated;
    public string Url = url;
    public readonly string FileName = fileName;
    public string AssetId = assetId;
    public string MediaType = mediaType;
}