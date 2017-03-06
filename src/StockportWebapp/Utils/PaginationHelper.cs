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
        public int CalculateIndexOfFirstItemOnPage(int currentPageNumber, int numItemsOnPage)
        {
            var numberOfItemsBeforeThisPage = ((currentPageNumber - 1) * numItemsOnPage);
            var indexOfFirstItemOnThisPage = numberOfItemsBeforeThisPage + 1;

            return indexOfFirstItemOnThisPage;
        }
    }
}