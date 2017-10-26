using System;

namespace StockportWebapp.Models
{
    public class Document
    {
        public readonly string Title;
        public readonly int Size;
        public readonly DateTime LastUpdated;
        public string Url;
        public readonly string FileName;
        public string AssetId { get; set; }

        public Document(string title, int size, DateTime lastUpdated, string url, string fileName, string assetId)
        {
            Title = title;
            Size = size;
            LastUpdated = lastUpdated;
            Url = url;
            FileName = fileName;
            AssetId = assetId;
        }
    }
}