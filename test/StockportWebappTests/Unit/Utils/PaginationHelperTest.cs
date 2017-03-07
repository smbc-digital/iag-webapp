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
        [InlineData(1, 5, true, false, false, false, false)]
        [InlineData(2, 5, false, true, false, false, false)]
        [InlineData(3, 5, false, false, true, false, false)]
        [InlineData(4, 5, false, false, false, true, false)]
        [InlineData(5, 5, false, false, false, false, true)]
        [InlineData(7, 10, false, false, true, false, false)]
        [InlineData(9, 10, false, false, false, true, false)]
        [InlineData(10, 10, false, false, false, false, true)]
        [InlineData(13, 20, false, false, true, false, false)]
        public void ForFiveVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
            int currentPageNumber, 
            int totalPages,
            bool page1IsCurrentPage,
            bool page2IsCurrentPage,
            bool page3IsCurrentPage,
            bool page4IsCurrentPage,
            bool page5IsCurrentPage)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(1, currentPageNumber, totalPages, page1IsCurrentPage));
            results[1].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(2, currentPageNumber, totalPages, page2IsCurrentPage));
            results[2].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(3, currentPageNumber, totalPages, page3IsCurrentPage));
            results[3].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(4, currentPageNumber, totalPages, page4IsCurrentPage));
            results[4].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(5, currentPageNumber, totalPages, page5IsCurrentPage));
        }

        [Theory]
        [InlineData(1, true, false, false, false)]
        [InlineData(2, false, true, false, false)]
        [InlineData(3, false, false, true, false)]
        [InlineData(4, false, false, false, true)]
        public void ForFourVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
            int currentPageNumber,
            bool page1IsCurrentPage,
            bool page2IsCurrentPage,
            bool page3IsCurrentPage,
            bool page4IsCurrentPage)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();
            const int totalPages = 4;

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(1, currentPageNumber, totalPages, page1IsCurrentPage));
            results[1].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(2, currentPageNumber, totalPages, page2IsCurrentPage));
            results[2].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(3, currentPageNumber, totalPages, page3IsCurrentPage));
            results[3].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(4, currentPageNumber, totalPages, page4IsCurrentPage));
        }

        [Theory]
        [InlineData(1, true, false, false)]
        [InlineData(2, false, true, false)]
        [InlineData(3, false, false, true)]
        public void ForThreeVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
            int currentPageNumber,
            bool page1IsCurrentPage,
            bool page2IsCurrentPage,
            bool page3IsCurrentPage)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();
            const int totalPages = 3;

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(1, currentPageNumber, totalPages, page1IsCurrentPage));
            results[1].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(2, currentPageNumber, totalPages, page2IsCurrentPage));
            results[2].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(3, currentPageNumber, totalPages, page3IsCurrentPage));
        }

        [Theory]
        [InlineData(1, true, false)]
        [InlineData(2, false, true)]
        public void ForTwoVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
            int currentPageNumber,
            bool page1IsCurrentPage,
            bool page2IsCurrentPage)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();
            const int totalPages = 2;

            // Act 
            List<VisiblePageNumber> results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(1, currentPageNumber, totalPages, page1IsCurrentPage));
            results[1].IsCurrentPage.Should().Be(page1IsCurrentPage, Error(2, currentPageNumber, totalPages, page2IsCurrentPage));
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

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1, 4)]
        public void IfTotalPagesIsFewerThanFiveThenNumVisiblePagesShouldBeEqualToTotalPages(int currentPage, int totalPages)
        {
            // Arrange
            PaginationHelper paginationHelper = new PaginationHelper();

            // Act
            var numVisiblePages = paginationHelper.GenerateVisiblePageNumbers(currentPage, totalPages);

            // Assert
            numVisiblePages.Count.Should().Be(totalPages);
        }
      
    }
}
