using FluentAssertions;
using StockportWebapp.Utils;
using Xunit;

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

        [Fact]
        public void IfThereIsMoreThanOnePageTheFirstVisiblePageNumberShouldBeHigherThanZero()
        {
            // Arrange
            int firstLinkedPageNumber;
            const int thisNumberIsIrrelevant = 0;
            var paginationHelper = new PaginationHelper();

            // Act
            firstLinkedPageNumber = paginationHelper.CalculateFirstVisiblePageNumber(thisNumberIsIrrelevant, thisNumberIsIrrelevant);

            // Assert
            firstLinkedPageNumber.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void IfTheCurrentPageNumberIsOneOrTwoTheFirstVisiblePageNumberShouldBeOne(int currentPageNumber)
        {
            // Arrange
            int firstLinkedPageNumber;
            const int thisNumberIsIrrelevant = 0;
            var paginationHelper = new PaginationHelper();

            // Act
            firstLinkedPageNumber = paginationHelper.CalculateFirstVisiblePageNumber(currentPageNumber, thisNumberIsIrrelevant);

            // Assert
            firstLinkedPageNumber.Should().Be(1);
        }
    }
}
