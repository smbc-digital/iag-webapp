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
            int indexOfFirstItemOnPage;
            const int currentPageNumber = 1;
            const int numItemsOnPage = 15;
            var paginationHelper = new PaginationHelper();

            // Act
            indexOfFirstItemOnPage = paginationHelper.CalculateIndexOfFirstItemOnPage(currentPageNumber, numItemsOnPage);

            // Assert
            indexOfFirstItemOnPage.Should().Be(1);
        }

        [Fact]
        public void IndexOfFirstItemOnPageTwoShouldBeNumberOfPageItemsPlusOne()
        {
            // Arrange
            int indexOfFirstItemOnPage;
            const int currentPageNumber = 2;
            const int numItemsOnPage = 15;
            var paginationHelper = new PaginationHelper();

            // Act
            indexOfFirstItemOnPage = paginationHelper.CalculateIndexOfFirstItemOnPage(currentPageNumber, numItemsOnPage);

            // Assert
            indexOfFirstItemOnPage.Should().Be(16);
        }
    }
}
