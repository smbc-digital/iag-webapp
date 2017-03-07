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
        
        [Theory]
        [InlineData(1, 5, false, true, true, true, true)]
        [InlineData(2, 5, true, false, true, true, true)]
        [InlineData(3, 5, true, true, false, true, true)]
        [InlineData(4, 5, true, true, true, false, true)]
        [InlineData(5, 5, true, true, true, true, false)]
        [InlineData(7, 10, true, true, false, true, true)]
        [InlineData(9, 10, true, true, true, false, true)]
        [InlineData(10, 10, true, true, true, true, false)]
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
            results[0].HtmlFragment.Contains("href").Should().Be(page1ContainsHref, Error(1, currentPageNumber, totalPages, page1ContainsHref));
            results[1].HtmlFragment.Contains("href").Should().Be(page2ContainsHref, Error(2, currentPageNumber, totalPages, page2ContainsHref));
            results[2].HtmlFragment.Contains("href").Should().Be(page3ContainsHref, Error(3, currentPageNumber, totalPages, page3ContainsHref));
            results[3].HtmlFragment.Contains("href").Should().Be(page4ContainsHref, Error(4, currentPageNumber, totalPages, page4ContainsHref));
            results[4].HtmlFragment.Contains("href").Should().Be(page5ContainsHref, Error(5, currentPageNumber, totalPages, page5ContainsHref));
        }

        [Theory]
        [InlineData(1, false, true, true, true)]
        [InlineData(2, true, false, true, true)]
        [InlineData(3, true, true, false, true)]
        [InlineData(4, true, true, true, false)]
        public void ForFourVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
            int currentPageNumber,
            bool page1ContainsHref,
            bool page2ContainsHref,
            bool page3ContainsHref,
            bool page4ContainsHref)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();
            const int totalPages = 4;

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].HtmlFragment.Contains("href").Should().Be(page1ContainsHref, Error(1, currentPageNumber, totalPages, page1ContainsHref));
            results[1].HtmlFragment.Contains("href").Should().Be(page2ContainsHref, Error(2, currentPageNumber, totalPages, page2ContainsHref));
            results[2].HtmlFragment.Contains("href").Should().Be(page3ContainsHref, Error(3, currentPageNumber, totalPages, page3ContainsHref));
            results[3].HtmlFragment.Contains("href").Should().Be(page4ContainsHref, Error(4, currentPageNumber, totalPages, page4ContainsHref));
        }

        [Theory]
        [InlineData(1, false, true, true)]
        [InlineData(2, true, false, true)]
        [InlineData(3, true, true, false)]
        public void ForThreeVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
            int currentPageNumber,
            bool page1ContainsHref,
            bool page2ContainsHref,
            bool page3ContainsHref)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();
            const int totalPages = 3;

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].HtmlFragment.Contains("href").Should().Be(page1ContainsHref, Error(1, currentPageNumber, totalPages, page1ContainsHref));
            results[1].HtmlFragment.Contains("href").Should().Be(page2ContainsHref, Error(2, currentPageNumber, totalPages, page2ContainsHref));
            results[2].HtmlFragment.Contains("href").Should().Be(page3ContainsHref, Error(3, currentPageNumber, totalPages, page3ContainsHref));
        }

        [Theory]
        [InlineData(1, false, true)]
        [InlineData(2, true, false)]
        public void ForTwoVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
            int currentPageNumber,
            bool page1ContainsHref,
            bool page2ContainsHref)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();
            const int totalPages = 2;

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].HtmlFragment.Contains("href").Should().Be(page1ContainsHref, Error(1, currentPageNumber, totalPages, page1ContainsHref));
            results[1].HtmlFragment.Contains("href").Should().Be(page2ContainsHref, Error(2, currentPageNumber, totalPages, page2ContainsHref));
        }

        private string Error(int visiblePageIndex, int currentPageNumber, int totalPages, bool containsHref)
        {
            return string.Format("When current page is {0} out of {1}, visible page with index {2} should {3}contain href",
                currentPageNumber,
                totalPages,
                visiblePageIndex,
                containsHref ? "" : "NOT ");
        }

        [Theory]
        // is this three (current page number) relevant in current test??
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

        [Theory]
        [InlineData(5)]
        [InlineData(9)]
        public void IfNumTotalPagesIsFiveOrMoreThenNumVisiblePagesShouldBeFive(int totalPages)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();
            int thisNumberIsIrrelevant = 0;

            // Act
            int numVisiblePages = paginationHelper.GenerateVisiblePageNumbers(thisNumberIsIrrelevant, totalPages).Count;

            // Assert
            numVisiblePages.Should().Be(5);
        }

        [Fact]
        public void IfTotalPagesIsFewerThanFiveThenNumVisiblePagesShouldBeEqualToTotalPages()
        {

            // Act

            // Assert
            //numVisiblePages.Should().Be(totalPages);
        }
    }
}
