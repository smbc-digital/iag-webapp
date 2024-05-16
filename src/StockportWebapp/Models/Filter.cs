using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Model
{

    [ExcludeFromCodeCoverage]
    public class Filter
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string DisplayName { get; set; }
        public string Theme { get; set; }
        public bool Highlight { get; set; }

        public Filter() {}
    }
}