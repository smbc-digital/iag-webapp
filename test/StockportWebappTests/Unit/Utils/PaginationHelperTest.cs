using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdig.Helpers;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class PaginationHelperTest
    {
        public const int MaxNumberOfItemsPerPage = 15;

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, MaxNumberOfItemsPerPage + 1)]
        [InlineData(3, (MaxNumberOfItemsPerPage * 2) + 1)]
        [InlineData(13, (MaxNumberOfItemsPerPage * 12) + 1)]
        public void IndexOfFirstItemOnAnyPageShouldBeNumberOfItemsOnPreviousPagesPlusOne(int currentPageNumber, int expectedResult)
        {
            // Arrange
            int indexOfFirstItemOnPage;
            
            // Act
            indexOfFirstItemOnPage = PaginationHelper.CalculateIndexOfFirstItemOnPage(currentPageNumber, MaxNumberOfItemsPerPage);
            
            // Assert
            indexOfFirstItemOnPage.Should().Be(expectedResult);
        }
        
        [Theory]
        [InlineData(1, 4, 4)]
        [InlineData(2, MaxNumberOfItemsPerPage, MaxNumberOfItemsPerPage * 2)]
        [InlineData(3, 2, (MaxNumberOfItemsPerPage * 2) + 2)]
        [InlineData(11, 3, (MaxNumberOfItemsPerPage * 10) + 3)]
        public void IndexOfLastItemOnPageShouldBeNumberOfItemsBeforeThisPagePlusNumberOfItemsOnThisPage(
            int currentPageNumber, 
            int numItemsOnThisPage, 
            int expectedResult)
        {
            // Arrange
            int indexOfLastItemOnPage;
            
            // Act
            indexOfLastItemOnPage = PaginationHelper.CalculateIndexOfLastItemOnPage(currentPageNumber, numItemsOnThisPage, MaxNumberOfItemsPerPage);

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
            // Act 
            List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

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
            const int totalPages = 4;

            // Act 
            List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

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
            const int totalPages = 3;

            // Act 
            List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

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
            const int totalPages = 2;

            // Act 
            List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

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
            // Act 
            List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

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
            int thisNumberIsIrrelevant = 0;

            // Act
            int numVisiblePages = PaginationHelper.GenerateVisiblePageNumbers(thisNumberIsIrrelevant, totalPages).Count;

            // Assert
            numVisiblePages.Should().Be(5);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1, 4)]
        public void IfTotalPagesIsFewerThanFiveThenNumVisiblePagesShouldBeEqualToTotalPages(int currentPage, int totalPages)
        {
            // Act
            var numVisiblePages = PaginationHelper.GenerateVisiblePageNumbers(currentPage, totalPages);

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
            // Act
            var results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages); 

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
            // Act
            var results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

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
            // Act
            var results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

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

            // Act
            var results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

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
            
            // Act
            var result = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

            // Assert
            result.Count.Should().Be(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(MaxNumberOfItemsPerPage)]
        public void IfNumItemsIsMaxPageSizeOrLessThenNumItemsOnPageShouldBeNumItemsReturnedByContentful(int numItems)
        {
            // Arrange
            List<News> listofNewsItems = BuildListofNewsItems(numItems);

            // Act
            var newListofNewsItems = PaginationHelper.GetPaginatedItemsForSpecifiedPage(listofNewsItems, 1, "Display Name", MaxNumberOfItemsPerPage).Items;

            // Assert
            newListofNewsItems.Count.Should().Be(numItems);
        }

        [Theory]
        [InlineData(MaxNumberOfItemsPerPage * 2)]
        [InlineData(MaxNumberOfItemsPerPage * 3)]
        [InlineData(MaxNumberOfItemsPerPage * 10)]
        public void IfNumItemsIsEvenlyDivisibleByMaxPageSizeThenNumItemsOnPageShouldBeFifteen(int numItems)
        {
            // Arrange
            List<News> listofNewsItems = BuildListofNewsItems(numItems);

            // Act
            var newListofNewsItems = PaginationHelper.GetPaginatedItemsForSpecifiedPage(listofNewsItems, 1, "", MaxNumberOfItemsPerPage).Items;

            // Assert
            newListofNewsItems.Count.Should().Be(MaxNumberOfItemsPerPage);
        }

        [Theory]
        [InlineData(MaxNumberOfItemsPerPage + 1, 2)]
        [InlineData((MaxNumberOfItemsPerPage * 2) + 4, 3)]
        [InlineData((MaxNumberOfItemsPerPage * 3) + 2, 4)]
        public void
            IfNumItemsIsGreaterThanMaxPageSizeAndNotEvenlyDivisibleByMaxPageSizeThenThenLastPageShouldReturnNumItemsModMaxPageSize(
            int numItems, int lastPageNum)
        {
            // Arrange
            List<News> listofNewsItems = BuildListofNewsItems(numItems);

            // Act
            var newListofNewsItems = PaginationHelper.GetPaginatedItemsForSpecifiedPage(listofNewsItems, lastPageNum, "item description", MaxNumberOfItemsPerPage).Items;

            // Assert
            newListofNewsItems.Count.Should().Be(numItems % MaxNumberOfItemsPerPage);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(MaxNumberOfItemsPerPage)]
        [InlineData(4)]
        public void IfNumItemsIsMaxPageSizeOrLessShouldReturnOnePage(int numItems)
        {
            // Arrange
            List<News> listofNewsItems = BuildListofNewsItems(numItems);

            // Act
            var pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(listofNewsItems, 1, "item description", MaxNumberOfItemsPerPage).Pagination;

            // Assert
            pagination.TotalPages.Should().Be(1);
        }

        [Theory]
        [InlineData(MaxNumberOfItemsPerPage)]
        [InlineData(MaxNumberOfItemsPerPage * 2)]
        [InlineData(MaxNumberOfItemsPerPage * 10)]
        public void IfNumItemsIsEvenlyDivisibleByMaxPageSizeNumPagesReturnedShouldBeNumItemsDividedByMaxPageSize(int numItems)
        {
            // Arrange
            int thisNumberIsIrrelevant = 1;
            List<News> listofNewsItems = BuildListofNewsItems(numItems);

            // Act
            var pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(listofNewsItems, thisNumberIsIrrelevant, "item description", MaxNumberOfItemsPerPage).Pagination;

            // Assert
            pagination.TotalPages.Should().Be(numItems / MaxNumberOfItemsPerPage);
        }

        [Theory]
        [InlineData((MaxNumberOfItemsPerPage * 3) + 2)]
        [InlineData(MaxNumberOfItemsPerPage + 1)]
        [InlineData(MaxNumberOfItemsPerPage + 4)]
        public void IfNumItemsAboveMaxPageSizeAndNotEvenlyDivisibleByMaxPageSizeNumPagesReturnedShouldBeNumItemsDividedByMaxPageSizePlusOne(int numItems)
        {
            // Arrange
            int thisNumberIsIrrelevant = 1;
            List<News> listofNewsItems = BuildListofNewsItems(numItems);

            // Act
            var pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(listofNewsItems, thisNumberIsIrrelevant, "item description", MaxNumberOfItemsPerPage).Pagination;

            // Assert
            pagination.TotalPages.Should().Be((numItems / MaxNumberOfItemsPerPage) + 1);
        }

        [Fact]
        public void IfSpecifiedPageNumIsZeroThenActualPageNumIsOne()
        {
            // Arrange
            int thisNumberIsIrrelevant = 3;
            List<News> listofNewsItems = BuildListofNewsItems(thisNumberIsIrrelevant);

            // Act
            var pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(listofNewsItems, 0, "item description", MaxNumberOfItemsPerPage).Pagination;

            // Assert
            pagination.CurrentPageNumber.Should().Be(1);
        }

        [Fact]
        public void IfSpecifiedPageNumIsTooHighThenActualPageNumIsLastPageNum()
        {
            // Arrange
            const int numItems = MaxNumberOfItemsPerPage * 2;
            const int lastPageNumber = numItems / MaxNumberOfItemsPerPage;
            const int tooHigh = lastPageNumber + 10;
            List<News> listofNewsItems = BuildListofNewsItems(numItems);

            // Act
            var pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(listofNewsItems, tooHigh, "item description", MaxNumberOfItemsPerPage).Pagination;

            // Assert
            pagination.CurrentPageNumber.Should().Be(lastPageNumber);
        }

        [Fact]
        public void PreviousLinkIsShownWhenPageNumberIsGreaterThanOne()
        {
            // Arrange
            const int currentPageNumber = 5;

            // Act
            var result = PaginationHelper.ShowPreviousLink(currentPageNumber);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public void PreviousLinkIsNotShownWhenPageNumberIsEqualToOne()
        {
            // Arrange
            const int currentPageNumber = 1;

            // Act
            var result = PaginationHelper.ShowPreviousLink(currentPageNumber);

            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public void NextLinkIsNotShownWhenPageNumberIsEqualToTotalPages()
        {
            // Arrange
            const int totalPages = 10;
            const int currentPageNumber = totalPages;

            // Act
            var result = PaginationHelper.ShowNextLink(currentPageNumber, totalPages);

            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public void NextLinkIsShownWhenPageNumberIsLessThanTotalPages()
        {
            // Arrange
            const int totalPages = 10;
            const int currentPageNumber = totalPages - 1;

            // Act
            var result = PaginationHelper.ShowNextLink(currentPageNumber, totalPages);

            // Assert
            result.Should().Be(true);
        }

        private List<News> BuildListofNewsItems(int numberOfItems)
        {
            List<News> listofNewsItems = new List<News>();

            for (int i = 0; i < numberOfItems; i++)
            {
                var NewsItem = new News("News Article " + i.ToString(),
                    "another-news-article",
                    "This is another news article",
                    "image.jpg",
                    "thumbnail.jpg",
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
                    new List<Crumb>(), 
                    new DateTime(2015, 9, 10), 
                    new DateTime(2015, 9, 20), 
                    new List<Alert>(),
                    new List<string>(), 
                    new List<Document>());

                listofNewsItems.Add(NewsItem);
            }

            return listofNewsItems;
        }

        [Fact]
        public void BuildUrlShouldUseUrlHelperToCreateUrlWithPageQueryParamWithCorrectPageNumber()
        {
            // Arrange
            int pageNumber = 5;
            QueryUrl queryUrl = new QueryUrl(new RouteValueDictionary(), new QueryCollection());
            var urlHelper = new Mock<IUrlHelperWrapper>();
            urlHelper
                .Setup(u => u.RouteUrl(It.Is<RouteValueDictionary>(x => 
                    x.ContainsKey("Page")
                    && x.Values.Contains(pageNumber.ToString()))))
                .Returns("this string is not relevant");

            // Act
            PaginationHelper.BuildUrl(pageNumber, queryUrl, urlHelper.Object);

            // Assert 
            urlHelper.Verify();
        }
    }
}
