using System;

namespace StockportWebapp.Models
{
    public class Document
    {
        public readonly string Title;
        public readonly int Size;
        public readonly DateTime LastUpdated;
        public readonly string Url;
        public readonly string FileName;

        public Document(string title, int size, DateTime lastUpdated, string url, string fileName)
        {
            Title = title;
            Size = size;
            LastUpdated = lastUpdated;
            Url = url;
            FileName = fileName;
        }
    }
}