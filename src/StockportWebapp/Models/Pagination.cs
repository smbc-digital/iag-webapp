using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class Pagination
    {
        public Pagination(int totalNumItems, int currentPageNumber, string displayName)
        {
            Page = currentPageNumber == 0 ? 1 : currentPageNumber;
            DisplayName = displayName;
            TotalItems = totalNumItems;
            TotalPages = CalculateTotalPages(totalNumItems);
        }

        public Pagination()
        {
        }

        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public QueryUrl CurrentUrl { get; set; }
        public int PageSize { get; set; } = 15;
        public int TotalItemsOnPage { get; set; }
        public string DisplayName { get; set; }

        private int CalculateTotalPages(int totalNumItems)
        {
            bool numItemsIsDivisibleByPageSize = (totalNumItems % PageSize == 0);
            int pageCount = numItemsIsDivisibleByPageSize
                ? (totalNumItems / PageSize)
                : totalNumItems / PageSize + 1;

            return pageCount;
        }
    }
}
