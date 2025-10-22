namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Document(string title, int size, DateTime lastUpdated, string url, string fileName, string assetId, string mediaType, string sizeAndType = "")
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = title;

    [JsonPropertyName("size")]
    public int Size { get; set; } = size;

    [JsonPropertyName("lastUpdated")]
    public DateTime LastUpdated { get; set; } = lastUpdated;

    [JsonPropertyName("url")]
    public string Url { get; set; } = url;

    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = fileName;

    [JsonPropertyName("assetId")]
    public string AssetId { get; set; } = assetId;

    [JsonPropertyName("mediaType")]
    public string MediaType { get; set; } = mediaType;

    [JsonPropertyName("sizeAndType")]
    public string SizeAndType { get; set; } = sizeAndType;
}