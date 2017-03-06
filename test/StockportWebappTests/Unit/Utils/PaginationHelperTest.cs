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

        [Fact]
        public void IndexofLastItemOnFirstPageShouldBeNumberOfItemsOnFirstPage()
        {
            // Arrange
            int indexOfLastItemOnPage;
            const int numItemsOnThisPage = 15;
            const int currentPageNumber = 1;
            var paginationHelper = new PaginationHelper();

            // Act
            indexOfLastItemOnPage = paginationHelper.CalculateIndexOfLastItemOnPage(currentPageNumber, numItemsOnThisPage);
            
            // Assert
            indexOfLastItemOnPage.Should().Be(numItemsOnThisPage);
        }

        [Fact]
        public void IndexofLastItemOnSecondPageShouldBeNumberOfItemsOnFirstPagePlusNumberOfItemsOnSecondPage()
        {
            // Arrange
            int indexOfLastItemOnPage;
            int indexOfFirstItemOnSecondPage = 16;
            int numberOfItemsOnSecondPage = 14;
            var paginationHelper = new PaginationHelper();

            //Act
            indexOfLastItemOnPage = paginationHelper.CalculateIndexOfLastItemOnPage(indexOfFirstItemOnSecondPage, numberOfItemsOnSecondPage);

            //Assert
            indexOfLastItemOnPage.Should().Be(indexOfFirstItemOnSecondPage + numberOfItemsOnSecondPage);
        }

        [Theory]
        [InlineData(1, 15, 15)]
        [InlineData(1, 14, 14)]
        [InlineData(16, 15, 30)]
        [InlineData(151, 15, 165)]
        public void IndexOfLastItemOnPagesTwoOnwardsShouldBeIndexOfFirstItemOnThisPagePlusNumberOfItemsOnThisPage(int indexOfFirstItemOnThisPage, int numItemsOnPage,
            int expectedResult)
        {
            // Arrange
            int indexOfLastItemOnPage;
            var paginationHelper = new PaginationHelper();

            // Act
            indexOfLastItemOnPage = paginationHelper.CalculateIndexOfLastItemOnPage(indexOfFirstItemOnThisPage, numItemsOnPage);

            // Assert
            indexOfLastItemOnPage.Should().Be(expectedResult);
        }

    }
}
