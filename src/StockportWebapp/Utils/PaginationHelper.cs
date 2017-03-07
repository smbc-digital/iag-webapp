using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;

namespace StockportWebapp.Utils
{
    public interface IPaginationHelper
    {
    }

    public class PaginationHelper : IPaginationHelper
    {
        public int CalculateIndexOfFirstItemOnPage(int currentPageNumber, int maxItemsPerPage)
        {
            var numberOfPreviousPages = currentPageNumber - 1;
            var numberOfItemsBeforeThisPage = numberOfPreviousPages * maxItemsPerPage;
            var indexOfFirstItemOnThisPage = numberOfItemsBeforeThisPage + 1;

            return indexOfFirstItemOnThisPage;
        }

        public int CalculateIndexOfLastItemOnPage(int currentPageNumber, int numItemsOnThisPage, int maxItemsPerPage)
        {
            var numberOfPreviousPages = currentPageNumber - 1;
            var numberOfItemsBeforeThisPage = numberOfPreviousPages * maxItemsPerPage;

            return numberOfItemsBeforeThisPage + numItemsOnThisPage;
        }

        public List<VisiblePageNumber> GenerateVisiblePageNumbers(int currentPageNumber, int totalPages)
        {
            var result = new List<VisiblePageNumber>
            {
                new VisiblePageNumber {PageNumber = 1, HtmlFragment = "href"},
                new VisiblePageNumber {PageNumber = 2, HtmlFragment = "href"},
                new VisiblePageNumber {PageNumber = 3, HtmlFragment = "href"},
                new VisiblePageNumber {PageNumber = 4, HtmlFragment = "href"},
                new VisiblePageNumber {PageNumber = 5, HtmlFragment = "href"}
            };

            int currentPageIndex = CalculateCurrentPageIndex(currentPageNumber, totalPages);
            result[currentPageIndex].HtmlFragment = "";

            return result;
        }

        private int CalculateCurrentPageIndex(int currentPageNumber, int totalPages)
        {
            const int numVisiblePages = 5;
            const int middleIndexOutOfFive = 2;
            int currentPageIndex = middleIndexOutOfFive;
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

            return currentPageIndex;
        }

        private bool CurrentPageIsNearStartOfVisiblePages(int currentPageNumber)
        {
            return currentPageNumber == 1 
                || currentPageNumber == 2;
        }

        private bool CurrentPageIsLastVisiblePage(int currentPageNumber, int totalPages)
        {
            return currentPageNumber == totalPages;
        }

        private bool CurrentPageIsPenultimateVisiblePage(int currentPageNumber, int totalPages)
        {
            return currentPageNumber == (totalPages - 1);
        }


    }
}