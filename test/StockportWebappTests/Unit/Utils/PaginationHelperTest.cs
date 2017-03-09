using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.CodeGenerators;
using Microsoft.AspNetCore.Routing;
using Moq;
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
            results[1].IsCurrentPage.Should().Be(page2IsCurrentPage, Error(2, currentPageNumber, totalPages, page2IsCurrentPage));
            results[2].IsCurrentPage.Should().Be(page3IsCurrentPage, Error(3, currentPageNumber, totalPages, page3IsCurrentPage));
            results[3].IsCurrentPage.Should().Be(page4IsCurrentPage, Error(4, currentPageNumber, totalPages, page4IsCurrentPage));
            results[4].IsCurrentPage.Should().Be(page5IsCurrentPage, Error(5, currentPageNumber, totalPages, page5IsCurrentPage));
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
            results[1].IsCurrentPage.Should().Be(page2IsCurrentPage, Error(2, currentPageNumber, totalPages, page2IsCurrentPage));
            results[2].IsCurrentPage.Should().Be(page3IsCurrentPage, Error(3, currentPageNumber, totalPages, page3IsCurrentPage));
            results[3].IsCurrentPage.Should().Be(page4IsCurrentPage, Error(4, currentPageNumber, totalPages, page4IsCurrentPage));
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
            results[1].IsCurrentPage.Should().Be(page2IsCurrentPage, Error(2, currentPageNumber, totalPages, page2IsCurrentPage));
            results[2].IsCurrentPage.Should().Be(page3IsCurrentPage, Error(3, currentPageNumber, totalPages, page3IsCurrentPage));
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
            results[1].IsCurrentPage.Should().Be(page2IsCurrentPage, Error(2, currentPageNumber, totalPages, page2IsCurrentPage));
        }

        private string Error(int visiblePageIndex, int currentPageNumber, int totalPages, bool isCurrentPage)
        {
            return string.Format("When current page is {0} out of {1}, visible page with index {2} should {3}be current page",
                currentPageNumber,
                totalPages,
                visiblePageIndex,
                isCurrentPage ? "" : "NOT ");
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

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        [InlineData(1, 3)]
        [InlineData(2, 3)]
        [InlineData(1, 4)]
        [InlineData(2, 4)]
        [InlineData(1, 5)]
        [InlineData(2, 5)]
        [InlineData(1, 10)]
        [InlineData(2, 10)]
        public void IfCurrentPageNumberIsOneOrTwoThenFirstVisiblePageNumberShouldBeOne(int currentPageNumber, int totalPages)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();

            // Act
            var results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages); 

            // Assert
            results[0].PageNumber.Should().Be(1);
        }

        [Theory]
        [InlineData(4, 5)]
        [InlineData(5, 5)]
        [InlineData(9, 10)]
        [InlineData(10, 10)]
        public void IfCurrentPageIsPenultimateOrLastAndTotalPagesGreaterThanFourThenFirstVisiblePageNumShouldBeTotalPagesMinusFour
            (int currentPageNumber, int totalPages)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();

            // Act
            var results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].PageNumber.Should().Be(totalPages - 4);
        }

        [Theory]
        [InlineData(3, 5)]
        [InlineData(5, 10)]
        [InlineData(8, 10)]
        public void
            IfTotalPagesIsGreaterThanOrEqualToFiveAndCurrentPageIsCentralVisiblePageThenFirstVisiblePageNumShouldBeCurrentPageMinusTwo
            (int currentPageNumber, int totalPages)
        {
            // Arrange
            var paginationHelper = new PaginationHelper();

            // Act
            var results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            results[0].PageNumber.Should().Be(currentPageNumber - 2);
        }

        [Theory]
        [InlineData(1, 1, 5)]
        [InlineData(1, 2, 5)]
        [InlineData(1, 3, 5)]
        [InlineData(1, 4, 5)]
        [InlineData(1, 5, 5)]
        [InlineData(1, 1, 4)]
        [InlineData(1, 2, 4)]
        [InlineData(1, 3, 4)]
        [InlineData(1, 4, 4)]
        [InlineData(1, 1, 3)]
        [InlineData(1, 2, 3)]
        [InlineData(1, 3, 3)]
        [InlineData(1, 1, 2)]
        [InlineData(1, 2, 2)]
        [InlineData(3, 5, 10)]
        [InlineData(6, 8, 10)]
        [InlineData(6, 9, 10)]
        [InlineData(6, 10, 10)]
        public void LastVisiblePageNumberShouldBeFirstVisiblePageNumberPlusNumVisiblePagesMinusOne(
            int firstVisiblePageNumber,
            int currentPageNumber, 
            int totalPages)
        {
            // Arrange
            const int maxVisiblePages = 5;
            int numVisiblePages = Math.Min(totalPages, maxVisiblePages);
            int indexOfLastVisiblePage = numVisiblePages - 1;
            var paginationHelper = new PaginationHelper();

            // Act
            var results = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            int expectedResult = firstVisiblePageNumber + numVisiblePages - 1;
            results[indexOfLastVisiblePage].PageNumber.Should().Be(expectedResult);
        }

        [Fact]
        public void IfThereIsOnlyOnePageThereShouldBeNoVisiblePageNumbers()
        {
            // Arrange
            const int currentPageNumber = 1;
            const int totalPages = 1;
            var paginationHelper = new PaginationHelper();

            // Act
            var result = paginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            result.Count.Should().Be(0);
        }

        [Fact (Skip = "still working on this area of code")]
        public void UrlShouldBeOriginalUrlPlusPageQueryParamContainingSpecifiedPageNumber()
        {
            // Arrange
            int pageNumber = 5;
            QueryUrl queryUrl = new QueryUrl(new RouteValueDictionary(), new QueryCollection());
            var urlHelper = new Mock<IUrlHelper>();
            //urlHelper.Setup(urlHelper => urlHelper.RouteUrl())

            // Act
            string url = PaginationHelper.BuildUrl(pageNumber, queryUrl, urlHelper.Object);

            // Assert 
            url.Should().Be("http://stockport.gov.uk/events?Page=5");
        }
    }
}
