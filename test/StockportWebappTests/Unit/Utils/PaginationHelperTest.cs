using System.Collections.Generic;
using FluentAssertions;
using StockportWebapp.Utils;
using Xunit;
using Xunit.Sdk;

namespace StockportWebappTests.Unit.Utils
{
    public class PaginationHelperTest
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 16)]
        [InlineData(3, 31)]
        [InlineData(13, 181)]
        public void IndexOfFirstItemOnAnyPageShouldBeNumberOfItemsOnPreviousPagesPlusOne(int currentPageNumber, int expectedResult)
        {
            // Arrange
            int indexOfFirstItemOnPage;
            const int numItemsOnPage = 15;
            var paginationHelper = new PaginationHelper();

            // Act
            indexOfFirstItemOnPage = paginationHelper.CalculateIndexOfFirstItemOnPage(currentPageNumber, numItemsOnPage);
            
            // Assert
            indexOfFirstItemOnPage.Should().Be(expectedResult);
        }
        
        [Theory]
        [InlineData(1, 10, 10)]
        [InlineData(2, 15, 30)]
        [InlineData(3, 2, 32)]
        [InlineData(11, 9, 159)]
        public void IndexOfLastItemOnPageShouldBeNumberOfItemsBeforeThisPagePlusNumberOfItemsOnThisPage(
            int currentPageNumber, 
            int numItemsOnThisPage, 
            int expectedResult)
        {
            // Arrange
            int indexOfLastItemOnPage;
            const int maxItemsPerPage = 15;
            var paginationHelper = new PaginationHelper();

            // Act
            indexOfLastItemOnPage = paginationHelper.CalculateIndexOfLastItemOnPage(currentPageNumber, numItemsOnThisPage, maxItemsPerPage);

            // Assert
            indexOfLastItemOnPage.Should().Be(expectedResult);
        }
        
        [Theory(Skip = "Still working on this area of the code")]
        [InlineData(1, 5, false, true, true, true, true)]
        [InlineData(2, 5, true, false, true, true, true)]
        [InlineData(3, 5, true, true, false, true, true)]
        [InlineData(4, 5, true, true, true, false, true)]
        [InlineData(5, 5, true, true, true, true, false)]
        [InlineData(7, 10, true, true, false, true, true)]
        [InlineData(13, 20, true, true, false, true, true)]
        public void ForFiveVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
            int currentPageNumber, 
            int totalPages,
            bool page1ContainsHref,
            bool page2ContainsHref,
            bool page3ContainsHref,
            bool page4ContainsHref,
            bool page5ContainsHref)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            Assert.Equal(page1ContainsHref, results[0].HtmlFragment.Contains("href"));
            Assert.Equal(page2ContainsHref, results[1].HtmlFragment.Contains("href"));
            Assert.Equal(page3ContainsHref, results[2].HtmlFragment.Contains("href"));
            Assert.Equal(page4ContainsHref, results[3].HtmlFragment.Contains("href"));
            Assert.Equal(page5ContainsHref, results[4].HtmlFragment.Contains("href"));
        }

        [Theory]
        [InlineData(3, 5)]
        public void WhenThereAreFivePagesThenTheVisiblePageNumbersShouldBeNumberedOneToFive(
            int currentPageNumber,
            int totalPages)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].PageNumber.Should().Be(1);
            results[1].PageNumber.Should().Be(2);
            results[2].PageNumber.Should().Be(3);
            results[3].PageNumber.Should().Be(4);
            results[4].PageNumber.Should().Be(5);
        }
    }
}
