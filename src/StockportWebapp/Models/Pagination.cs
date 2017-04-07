using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class Pagination
    {
        public int MaxItemsPerPage;
        public int TotalItems { get; set; }
        public int CurrentPageNumber { get; set; }
        public int TotalPages { get; set; }
        public QueryUrl CurrentUrl { get; set; }
        public int TotalItemsOnPage { get; set; }
        public string ItemDescription { get; set; }

        public Pagination(int totalNumItems, int currentPageNumber, string itemDescription, int maxNumberOfItemsPerPage)
        {
            CurrentPageNumber = currentPageNumber;
            ItemDescription = itemDescription;
            MaxItemsPerPage = maxNumberOfItemsPerPage;
            TotalItems = totalNumItems;
            TotalPages = CalculateTotalPages(totalNumItems);
            
        }

        public Pagination()
        {
        }

        private int CalculateTotalPages(int totalNumItems)
        {
            bool numItemsIsDivisibleByPageSize = (totalNumItems % MaxItemsPerPage == 0);
            int pageCount = numItemsIsDivisibleByPageSize
                ? (totalNumItems / MaxItemsPerPage)
                : (totalNumItems / MaxItemsPerPage) + 1;

            return pageCount;
        }
    }
}
