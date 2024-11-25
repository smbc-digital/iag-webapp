namespace StockportWebapp.Utils;

public static class PaginationHelper
{
    public static int CalculateIndexOfFirstItemOnPage(int currentPageNumber, int maxItemsPerPage)
    {
        int numberOfPreviousPages = currentPageNumber - 1;
        int numberOfItemsBeforeThisPage = numberOfPreviousPages * maxItemsPerPage;
        int indexOfFirstItemOnThisPage = numberOfItemsBeforeThisPage + 1;

        return indexOfFirstItemOnThisPage;
    }

    public static int CalculateIndexOfLastItemOnPage(int currentPageNumber, int numItemsOnThisPage, int maxItemsPerPage)
    {
        int numberOfPreviousPages = currentPageNumber - 1;
        int numberOfItemsBeforeThisPage = numberOfPreviousPages * maxItemsPerPage;

        return numberOfItemsBeforeThisPage + numItemsOnThisPage;
    }

    public static List<VisiblePageNumber> GenerateVisiblePageNumbers(int currentPageNumber, int totalPages)
    {
        const int maxVisiblePages = 5;
        List<VisiblePageNumber> result = new();

        if (totalPages > 1)
        {
            int numVisiblePages = Math.Min(totalPages, maxVisiblePages);
            int firstVisiblePage = CalculateFirstVisiblePageNumber(currentPageNumber, totalPages);
            int lastVisiblePage = firstVisiblePage + numVisiblePages - 1;
            for (int count = firstVisiblePage; count <= lastVisiblePage; count++)
            {
                result.Add(new VisiblePageNumber { PageNumber = count });
            }

            int currentPageIndex = CalculateCurrentPageIndex(currentPageNumber, totalPages);
            result[currentPageIndex].IsCurrentPage = true;
        }

        return result;
    }

    public static bool ShowPreviousLink(int currentPageNumber) =>
        currentPageNumber > 1;

    public static bool ShowNextLink(int currentPageNumber, int totalPages) =>
        currentPageNumber < totalPages;

    public static PaginatedItems<T> GetPaginatedItemsForSpecifiedPage<T>(List<T> items, int currentPageNumber, string itemDescription, int maxNumberOfItemsPerPage, int defaultPageSize)
    {
        Pagination pagination = new Pagination(
            items.Count,
            currentPageNumber,
            itemDescription,
            maxNumberOfItemsPerPage.Equals(-1)
                ? items.Count()
                    : maxNumberOfItemsPerPage.Equals(0)
                    ? defaultPageSize
                : maxNumberOfItemsPerPage,
            defaultPageSize);

        int ExistingPageNumber = MakeSurePageNumberExists(currentPageNumber, items.Count, pagination.MaxItemsPerPage);
        pagination.CurrentPageNumber = ExistingPageNumber;

        int itemsOnPreviousPages = pagination.MaxItemsPerPage * (pagination.CurrentPageNumber - 1);

        List<T> itemsOnCurrentPage = items
                .Skip(itemsOnPreviousPages)
                .Take(pagination.MaxItemsPerPage).ToList();

        pagination.TotalItemsOnPage = itemsOnCurrentPage.Count;

        return new PaginatedItems<T>
        {
            Items = itemsOnCurrentPage,
            Pagination = pagination
        };
    }

    public static string BuildUrl(int pageNumber, QueryUrl queryUrl, IUrlHelperWrapper urlHelper)
    {
        RouteValueDictionary routeValueDictionary = queryUrl.AddQueriesToUrl(
            new Dictionary<string, string>
            {
                {
                    "Page", pageNumber.ToString()
                }
            });

        return urlHelper.RouteUrl(routeValueDictionary);
    }

    public static string BuildPageSizeUrl(int defaultPageSize, int maxItemsPerPage, int totalItems, QueryUrl queryUrl, IUrlHelperWrapper urlHelper)
    {
        int pageSize = GetOtherPageSizeByCurrentPageSize(maxItemsPerPage, totalItems, defaultPageSize);
        RouteValueDictionary routeValueDictionary = queryUrl.AddQueriesToUrl(
            new Dictionary<string, string>
            {
                {
                    "pageSize" , pageSize.ToString()
                },
                {
                    "page" , 1.ToString()
                }
            });

        return urlHelper.RouteUrl(routeValueDictionary);
    }

    private static int MakeSurePageNumberExists(int suggestedPageNumber, int totalItems, int numberOfItemsPerPage)
    {
        int actualPageNumber = suggestedPageNumber;
        int highestPageNumber = CalculateHighestPageNumber(totalItems, numberOfItemsPerPage);

        if (suggestedPageNumber.Equals(0))
            actualPageNumber = 1;
        else if (suggestedPageNumber > highestPageNumber)
            actualPageNumber = highestPageNumber;

        return actualPageNumber;
    }

    private static int CalculateHighestPageNumber(int totalItems, int numberOfItemsPerPage)
    {
        int highestPageNumber = totalItems / numberOfItemsPerPage;
        if (totalItems % numberOfItemsPerPage > 0)
            highestPageNumber++;

        return highestPageNumber;
    }

    private static int CalculateFirstVisiblePageNumber(int currentPageNumber, int totalPages)
    {
        int firstVisiblePage;

        bool currentPageIsNearStartOfVisiblePages = CurrentPageIsNearStartOfVisiblePages(currentPageNumber);
        bool currentPageIsPenultimateVisiblePage = CurrentPageIsPenultimateVisiblePage(currentPageNumber, totalPages);
        bool currentPageIsLastVisiblePage = CurrentPageIsLastVisiblePage(currentPageNumber, totalPages);

        if (totalPages < 5 || currentPageIsNearStartOfVisiblePages)
            firstVisiblePage = 1;
        else if (currentPageIsLastVisiblePage || currentPageIsPenultimateVisiblePage)
            firstVisiblePage = totalPages - 4;
        else
            firstVisiblePage = currentPageNumber - 2;

        return firstVisiblePage;
    }

    private static int CalculateCurrentPageIndex(int currentPageNumber, int totalPages)
    {
        int currentPageIndex;
        const int maxVisiblePages = 5;
        int numVisiblePages = Math.Min(totalPages, maxVisiblePages);

        bool currentPageIsNearStartOfVisiblePages = CurrentPageIsNearStartOfVisiblePages(currentPageNumber);
        bool currentPageIsPenultimateVisiblePage = CurrentPageIsPenultimateVisiblePage(currentPageNumber, totalPages);
        bool currentPageIsLastVisiblePage = CurrentPageIsLastVisiblePage(currentPageNumber, totalPages);

        if (currentPageIsNearStartOfVisiblePages)
            currentPageIndex = currentPageNumber - 1;
        else if (currentPageIsPenultimateVisiblePage)
            currentPageIndex = numVisiblePages - 2;
        else if (currentPageIsLastVisiblePage)
            currentPageIndex = numVisiblePages - 1;
        else
            currentPageIndex = 2;

        return currentPageIndex;
    }

    private static bool CurrentPageIsNearStartOfVisiblePages(int currentPageNumber) =>
        currentPageNumber.Equals(1) || currentPageNumber.Equals(2);

    private static bool CurrentPageIsLastVisiblePage(int currentPageNumber, int totalPages) =>
        currentPageNumber.Equals(totalPages);

    private static bool CurrentPageIsPenultimateVisiblePage(int currentPageNumber, int totalPages) =>
        currentPageNumber.Equals(totalPages - 1);

    public static int GetOtherPageSizeByCurrentPageSize(int maxItemsPerPage, int totalItems, int defaultPageSize)
    {
        if (maxItemsPerPage.Equals(defaultPageSize) && !totalItems.Equals(60))
            return 60;
        else
            return defaultPageSize;
    }

    public static List<int?> GeneratePageSequence(int currentPage, int totalPages)
    {
        // add all pages
        if (totalPages <= 7)
            return Enumerable.Range(1, totalPages).Cast<int?>().ToList();

        // always add first page
        List<int?> pages = new(){ 1 };

        // add ellipses between page 1 and page before current
        if (currentPage > 4 && totalPages > 7)
            pages.Add(null);

        if (currentPage.Equals(4))
        {
            // add second, third and fifth pages
            for (int i = Math.Max(2, currentPage - 2); i <= Math.Min(totalPages - 1, currentPage + 1); i++)
                pages.Add(i);
        }
        else
        {
            // add page before and after current
            for (int i =  Math.Max(2, currentPage - 1); i <= Math.Min(totalPages - 1, currentPage + 1); i++)
                pages.Add(i);
        }
        
        // add ellipses between page after current and last page
        if (totalPages - currentPage > 2)
            pages.Add(null);

        pages.Add(totalPages);

        return pages;
    }
}