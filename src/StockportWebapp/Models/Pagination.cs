using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class Pagination
    {
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public QueryUrl CurrentUrl { get; set; }
        public int PageSize { get; set; } = 15;
        public int TotalItemsOnPage { get; set; }
        public string DisplayName { get; set; }
    }
}
