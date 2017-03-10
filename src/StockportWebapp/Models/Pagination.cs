using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class Pagination
    {
        public const int MaxItemsPerPage = 15;

        public Pagination(int totalNumItems, int currentPageNumber, string displayName)
        {
            CurrentPageNumber = currentPageNumber;
            DisplayName = displayName;
            TotalItems = totalNumItems;
            TotalPages = CalculateTotalPages(totalNumItems);
        }

        public Pagination()
        {
        }

        public int TotalItems { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPages { get; set; }
        public QueryUrl CurrentUrl { get; set; }
        public int TotalItemsOnPage { get; set; }
        public string DisplayName { get; set; }

        private int CalculateTotalPages(int totalNumItems)
        {
            bool numItemsIsDivisibleByPageSize = (totalNumItems % MaxItemsPerPage == 0);
            int pageCount = numItemsIsDivisibleByPageSize
                ? (totalNumItems / MaxItemsPerPage)
                : totalNumItems / MaxItemsPerPage + 1;

            return pageCount;
        }
    }
}
