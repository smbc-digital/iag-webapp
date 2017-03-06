using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class PaginationHelperTest
    {
        [Fact]
        public void IndexOfFirstItemOnPageOneShouldBeOne()
        {
            // Arrange
            int firstPageItem;
            const int currentPageNumber = 1;
            const int numItemsOnPage = 15;
            var paginationHelper = new PaginationHelper();

            // Act
            firstPageItem = paginationHelper.CalculateIndexOfFirstItemOnPage(currentPageNumber, numItemsOnPage);

            // Assert
            firstPageItem.Should().Be(1);
        }
    }
}
