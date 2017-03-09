using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdig.Helpers;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.CodeGenerators;
using Microsoft.AspNetCore.Routing;
using Moq;
using StockportWebapp.Extensions;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;
using Xunit.Sdk;

namespace StockportWebappTests.Unit.Utils
{
    public class PaginationHelperTest
    {
        private const string EmailAlertsTopicId = "test-id";
        private const bool EmailAlertsOn = true;

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
            
            // Act
            indexOfFirstItemOnPage = PaginationHelper.CalculateIndexOfFirstItemOnPage(currentPageNumber, numItemsOnPage);
            
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
            
            // Act
            indexOfLastItemOnPage = PaginationHelper.CalculateIndexOfLastItemOnPage(currentPageNumber, numItemsOnThisPage, maxItemsPerPage);

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
        [InlineData(10)]
        [InlineData(15)]
        public void IfNumItemsIsFifteenOrFewerThenNumItemsOnPageShouldBeNumItemsReturnedByContentful(int numItems)
        {
            // Arrange
            List<News> longListofNewsItems = BuildNewsList(numItems);
            var bigNewsRoom = new Newsroom(longListofNewsItems, new OrderedList<Alert>(),
                EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());
            Pagination pagination = new Pagination();

            // Act
            bigNewsRoom.News = PaginationHelper.GetPaginatedNewsForSpecifiedPage(bigNewsRoom.News, pagination, 1);

            // Assert
            bigNewsRoom.News.Count.Should().Be(numItems);
        }

        [Theory]
        [InlineData(30)]
        [InlineData(45)]
        [InlineData(60)]
        public void IfNumItemsIsEvenlyDivisibleByFifteenNumItemsOnPageShouldBeFifteen(int numItems)
        {
            // Arrange
            List<News> longListofNewsItems = BuildNewsList(numItems);
            var bigNewsRoom = new Newsroom(longListofNewsItems, new OrderedList<Alert>(),
                EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());
            Pagination pagination = new Pagination();

            // Act
            bigNewsRoom.News = PaginationHelper.GetPaginatedNewsForSpecifiedPage(bigNewsRoom.News, pagination, 1);

            // Assert
            bigNewsRoom.News.Count.Should().Be(15);
        }

        [Theory]
        [InlineData(16, 2)]
        [InlineData(37, 3)]
        [InlineData(50, 4)]
        public void
            IfNumItemsIsGreaterThanFifteenAndNotEvenlyDivisibleByFifteenThenLastPageShouldReturnNumItemsModFifteen(int numItems, int lastPageNum)
        {
            // Arrange
            List<News> longListofNewsItems = BuildNewsList(numItems);
            var bigNewsRoom = new Newsroom(longListofNewsItems, new OrderedList<Alert>(),
                EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());
            Pagination pagination = new Pagination();

            // Act
            bigNewsRoom.News = PaginationHelper.GetPaginatedNewsForSpecifiedPage(bigNewsRoom.News, pagination, lastPageNum);

            // Assert
            bigNewsRoom.News.Count.Should().Be(numItems % 15);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(15)]
        [InlineData(7)]
        public void IfNumItemsIsFifteenOrLessShouldReturnOnePage(int numItems)
        {
            // Arrange
            List<News> longListofNewsItems = BuildNewsList(numItems);
            var bigNewsRoom = new Newsroom(longListofNewsItems, new OrderedList<Alert>(),
                EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());
            Pagination pagination = new Pagination();

            // Act
            bigNewsRoom.News = PaginationHelper.GetPaginatedNewsForSpecifiedPage(bigNewsRoom.News, pagination, 1);

            // Assert
            pagination.TotalPages.Should().Be(1);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(30)]
        [InlineData(150)]
        public void IfNumItemsIsEvenlyDivisibleByFifteenNumPagesReturnedShouldBeNumItemsDividedByFifteen(int numItems)
        {
            // Arrange
            int thisNumberIsIrrelevant = 1;
            List<News> longListofNewsItems = BuildNewsList(numItems);
            var bigNewsRoom = new Newsroom(longListofNewsItems, new OrderedList<Alert>(),
                EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());
            Pagination pagination = new Pagination();

            // Act
            bigNewsRoom.News = PaginationHelper.GetPaginatedNewsForSpecifiedPage(bigNewsRoom.News, pagination, thisNumberIsIrrelevant);

            // Assert
            pagination.TotalPages.Should().Be(numItems / 15);
        }

        [Theory]
        [InlineData(53)]
        [InlineData(16)]
        [InlineData(29)]
        public void IfNumItemsAboveFifteenAndNotEvenlyDivisibleByFifteenNumPagesReturnedShouldBeNumItemsDividedByFifteenPlusOne(int numItems)
        {
            // Arrange
            int thisNumberIsIrrelevant = 1;
            List<News> longListofNewsItems = BuildNewsList(numItems);
            var bigNewsRoom = new Newsroom(longListofNewsItems, new OrderedList<Alert>(),
                EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());
            Pagination pagination = new Pagination();

            // Act
            bigNewsRoom.News = PaginationHelper.GetPaginatedNewsForSpecifiedPage(bigNewsRoom.News, pagination, thisNumberIsIrrelevant);

            // Assert
            pagination.TotalPages.Should().Be((numItems / 15) + 1);
        }

        [Fact]
        public void IfSpecifiedPageNumIsZeroThenActualPageNumIsOne()
        {
            // Arrange
            int thisNumberIsIrrelevant = 3;
            List<News> longListofNewsItems = BuildNewsList(thisNumberIsIrrelevant);
            var bigNewsRoom = new Newsroom(longListofNewsItems, new OrderedList<Alert>(),
                EmailAlertsOn, EmailAlertsTopicId, new List<string>(), new List<DateTime>());
            Pagination pagination = new Pagination();

            // Act
            bigNewsRoom.News = PaginationHelper.GetPaginatedNewsForSpecifiedPage(bigNewsRoom.News, pagination, 0);

            // Assert
            pagination.Page.Should().Be(1);
        }

        [Fact]
        public void IfSpecifiedPageNumIsTooHighThenActualPageNumIsLastPageNum()
        {
        }

        private List<News> BuildNewsList(int numberOfItems)
        {
            List<News> longListofNewsItems = new List<News>();
            for (int i = 0; i < numberOfItems; i++)
            {
                var NewsItem = new News("News Article " + i.ToString(),
                    "another-news-article",
                    "This is another news article",
                    "image.jpg",
                    "thumbnail.jpg",
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
                    new List<Crumb>(), new DateTime(2015, 9, 10), new DateTime(2015, 9, 20), new List<Alert>(),
                    new List<string>(), new List<Document>());

                longListofNewsItems.Add(NewsItem);
            }
            return longListofNewsItems;
        }

        //[Fact]
        //public void BuildUrlShouldUseUrlHelperToCreateUrlWithPageQueryParamWithCorrectPageNumber()
        //{
        //    // Arrange
        //    int pageNumber = 5;
        //    QueryUrl queryUrl = new QueryUrl(new RouteValueDictionary(), new QueryCollection());
        //    var urlHelper = new Mock<IUrlHelper>();
        //    urlHelper
        //        .Setup(u => u.RouteUrl(It.IsAny<QueryUrl>()))
        //        .Returns("this string is not relevant");

        //    // Act
        //    PaginationHelper.BuildUrl(pageNumber, queryUrl, urlHelper.Object);

        //    // Assert 
        //    urlHelper.Verify();
        //}
    }
}
