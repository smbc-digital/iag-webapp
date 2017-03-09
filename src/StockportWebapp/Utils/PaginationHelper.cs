using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace StockportWebapp.Utils
{
    public static class PaginationHelper
    {
        public static int CalculateIndexOfFirstItemOnPage(int currentPageNumber, int maxItemsPerPage)
        {
            var numberOfPreviousPages = currentPageNumber - 1;
            var numberOfItemsBeforeThisPage = numberOfPreviousPages * maxItemsPerPage;
            var indexOfFirstItemOnThisPage = numberOfItemsBeforeThisPage + 1;

            return indexOfFirstItemOnThisPage;
        }

        public static int CalculateIndexOfLastItemOnPage(int currentPageNumber, int numItemsOnThisPage, int maxItemsPerPage)
        {
            var numberOfPreviousPages = currentPageNumber - 1;
            var numberOfItemsBeforeThisPage = numberOfPreviousPages * maxItemsPerPage;

            return numberOfItemsBeforeThisPage + numItemsOnThisPage;
        }

        public static List<VisiblePageNumber> GenerateVisiblePageNumbers(int currentPageNumber, int totalPages)
        {
            const int maxVisiblePages = 5;
            var result = new List<VisiblePageNumber>();

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

        private static int CalculateFirstVisiblePageNumber(int currentPageNumber, int totalPages)
        {
            int firstVisiblePage;

            bool currentPageIsNearStartOfVisiblePages = CurrentPageIsNearStartOfVisiblePages(currentPageNumber);
            bool currentPageIsPenultimateVisiblePage = CurrentPageIsPenultimateVisiblePage(currentPageNumber, totalPages);
            bool currentPageIsLastVisiblePage = CurrentPageIsLastVisiblePage(currentPageNumber, totalPages);

            if (totalPages < 5 || currentPageIsNearStartOfVisiblePages)
            {
                firstVisiblePage = 1;
            }
            else if (currentPageIsLastVisiblePage || currentPageIsPenultimateVisiblePage)
            {
                firstVisiblePage = totalPages - 4;
            }
            else
            {
                firstVisiblePage = currentPageNumber - 2;
            }

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
            {
                currentPageIndex = currentPageNumber - 1;
            }
            else if (currentPageIsPenultimateVisiblePage)
            {
                currentPageIndex = numVisiblePages - 2;
            }
            else if (currentPageIsLastVisiblePage)
            {
                currentPageIndex = numVisiblePages - 1;
            }
            else
            {
                const int middleIndexOutOfFive = 2;
                currentPageIndex = middleIndexOutOfFive;
            }

            return currentPageIndex;
        }

        private static bool CurrentPageIsNearStartOfVisiblePages(int currentPageNumber)
        {
            return currentPageNumber == 1
                || currentPageNumber == 2;
        }

        private static bool CurrentPageIsLastVisiblePage(int currentPageNumber, int totalPages)
        {
            return currentPageNumber == totalPages;
        }

        private static bool CurrentPageIsPenultimateVisiblePage(int currentPageNumber, int totalPages)
        {
            return currentPageNumber == (totalPages - 1);
        }

        public static string BuildUrl(int pageNumber, QueryUrl queryUrl, IUrlHelper urlHelper)
        {
            return "?Page=";
        }
    }
}