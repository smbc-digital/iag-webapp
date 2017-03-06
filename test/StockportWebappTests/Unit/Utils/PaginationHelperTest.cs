using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class PaginationHelperTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void NewViewLogicShouldGiveSameResultsAsOldViewLogicForStartIndex(int currentPageNumber)
        {
            // Arrange
            Pagination paginationModel = new Pagination
            {
                TotalItemsOnPage = 15,
                Page = currentPageNumber,
                TotalItems = 150,
                TotalPages = 10,
                PageSize = 15
            };
            var paginationHelper = new PaginationHelper();
            int oldStart = ((paginationModel.Page - 1) * paginationModel.PageSize) + 1;

            // Act
            int newStart = paginationHelper.CalculateIndexOfFirstItemOnPage(paginationModel.Page, paginationModel.PageSize);

            // Assert
            newStart.Should().Be(oldStart);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 15)]
        [InlineData(3, 2)]
        [InlineData(11, 9)]
        public void NewViewLogicShouldGiveSameResultsAsOldViewLogicForEndIndex(
            int currentPageNumber,
            int totalItemsOnPage)
        {
            // Arrange
            Pagination paginationModel = new Pagination
            {
                TotalItemsOnPage = totalItemsOnPage,
                Page = currentPageNumber,
                TotalItems = 150,
                TotalPages = 10,
                PageSize = 15
            };
            var paginationHelper = new PaginationHelper();
            int oldStart = ((paginationModel.Page - 1) * paginationModel.PageSize) + 1;
            int oldEnd = oldStart + paginationModel.TotalItemsOnPage - 1;

            // Act
            int newEnd = paginationHelper.CalculateIndexOfLastItemOnPage(paginationModel.Page, paginationModel.TotalItemsOnPage, paginationModel.PageSize);

            // Assert
            newEnd.Should().Be(oldEnd);
        }

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

        [Fact]
        public void IfThereIsMoreThanOnePageTheFirstLinkedPageNumberShouldBeHigherThanZero()
        {
            // Arrange
            int firstLinkedPageNumber;
            var paginationHelper = new PaginationHelper();

            // Act
            firstLinkedPageNumber = paginationHelper.CalculateFirstLinkedPageNumber();

            // Assert
            firstLinkedPageNumber.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public void IfTheCurrentPageNumberIsOneOrTwoTheFirstLinkedPageNumberShouldBeOne()
        {
            // Arrange
            int firstLinkedPageNumber;
            var paginationHelper = new PaginationHelper();

            // Act
            firstLinkedPageNumber = paginationHelper.CalculateFirstLinkedPageNumber();

            // Assert
            firstLinkedPageNumber.Should().Be(1);
        }

        [Fact]
        public void IfTheCurrentPageIsThePenultimateOrLastPageTheFirstLinkedPageNumberShouldBeTheCurrentPageNumberMinusFive()
        {
            // Arrange
            int firstLinkedPageNumber;
            int currentPageNumber = 3;
            int totalNumberOfPages = 4;
            var paginationHelper = new PaginationHelper();

            // Act
            firstLinkedPageNumber = paginationHelper.CalculateFirstLinkedPageNumber();

            // Assert
            firstLinkedPageNumber.Should().Be(currentPageNumber - 5);
        }
    }
}
