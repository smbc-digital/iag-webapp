using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class MediaAsset
{
    public string Url { get; set; }
    public string Description { get; set; }

    public MediaAsset() { }

    public MediaAsset(string url, string description)
    {
        Url = url;
        Description = description;
    }
}