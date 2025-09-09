namespace StockportWebappTests_Unit.Unit.Utils;

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
        // Act
        int indexOfFirstItemOnPage = PaginationHelper.CalculateIndexOfFirstItemOnPage(currentPageNumber, MaxNumberOfItemsPerPage);

        // Assert
        Assert.Equal(expectedResult, indexOfFirstItemOnPage);
    }

    [Theory]
    [InlineData(1, 4, 4)]
    [InlineData(2, MaxNumberOfItemsPerPage, MaxNumberOfItemsPerPage * 2)]
    [InlineData(3, 2, (MaxNumberOfItemsPerPage * 2) + 2)]
    [InlineData(11, 3, (MaxNumberOfItemsPerPage * 10) + 3)]
    public void CalculateIndexOfLastItemOnPage_IndexOfLastItemOnPageShouldBeNumberOfItemsBeforeThisPagePlusNumberOfItemsOnThisPage(
        int currentPageNumber,
        int numItemsOnThisPage,
        int expectedResult)
    {
        // Act
        int indexOfLastItemOnPage = PaginationHelper.CalculateIndexOfLastItemOnPage(currentPageNumber, numItemsOnThisPage, MaxNumberOfItemsPerPage);

        // Assert
        Assert.Equal(expectedResult, indexOfLastItemOnPage);
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
        Assert.Equal(page1IsCurrentPage, results[0].IsCurrentPage);
        Assert.Equal(page2IsCurrentPage, results[1].IsCurrentPage);
        Assert.Equal(page3IsCurrentPage, results[2].IsCurrentPage);
        Assert.Equal(page4IsCurrentPage, results[3].IsCurrentPage);
        Assert.Equal(page5IsCurrentPage, results[4].IsCurrentPage);
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
        // Act 
        List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, 4);

        // Assert
        Assert.Equal(page1IsCurrentPage, results[0].IsCurrentPage);
        Assert.Equal(page2IsCurrentPage, results[1].IsCurrentPage);
        Assert.Equal(page3IsCurrentPage, results[2].IsCurrentPage);
        Assert.Equal(page4IsCurrentPage, results[3].IsCurrentPage);
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
        // Act 
        List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, 3);

        // Assert
        Assert.Equal(page1IsCurrentPage, results[0].IsCurrentPage);
        Assert.Equal(page2IsCurrentPage, results[1].IsCurrentPage);
        Assert.Equal(page3IsCurrentPage, results[2].IsCurrentPage);
    }

    [Theory]
    [InlineData(1, true, false)]
    [InlineData(2, false, true)]
    public void ForTwoVisiblePagesVisiblePageNumbersShouldAllHaveLinksApartFromCurrentPage(
        int currentPageNumber,
        bool page1IsCurrentPage,
        bool page2IsCurrentPage)
    {
        // Act 
        List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, 2);

        // Assert
        Assert.Equal(page1IsCurrentPage, results[0].IsCurrentPage);
        Assert.Equal(page2IsCurrentPage, results[1].IsCurrentPage);

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
        Assert.Equal(1, results[0].PageNumber);
        Assert.Equal(2, results[1].PageNumber);
        Assert.Equal(3, results[2].PageNumber);
        Assert.Equal(4, results[3].PageNumber);
        Assert.Equal(5, results[4].PageNumber);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(9)]
    public void IfNumTotalPagesIsFiveOrMoreThenNumVisiblePagesShouldBeFive(int totalPages)
    {
        // Act
        int numVisiblePages = PaginationHelper.GenerateVisiblePageNumbers(0, totalPages).Count;

        // Assert
        Assert.Equal(5, numVisiblePages);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(1, 4)]
    public void IfTotalPagesIsFewerThanFiveThenNumVisiblePagesShouldBeEqualToTotalPages(int currentPage, int totalPages)
    {
        // Act
        List<VisiblePageNumber> numVisiblePages = PaginationHelper.GenerateVisiblePageNumbers(currentPage, totalPages);

        // Assert
        Assert.Equal(totalPages, numVisiblePages.Count);
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
        List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

        // Assert
        Assert.Equal(1, results[0].PageNumber);
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
        List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

        // Assert
        Assert.Equal(totalPages - 4, results[0].PageNumber);
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
        List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

        // Assert
        Assert.Equal(currentPageNumber - 2, results[0].PageNumber);
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
        int numVisiblePages = Math.Min(totalPages, 5);
        int indexOfLastVisiblePage = numVisiblePages - 1;

        // Act
        List<VisiblePageNumber> results = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

        // Assert
        int expectedResult = firstVisiblePageNumber + numVisiblePages - 1;
        Assert.Equal(expectedResult, results[indexOfLastVisiblePage].PageNumber);
    }

    [Fact]
    public void IfThereIsOnlyOnePageThereShouldBeNoVisiblePageNumbers()
    {
        // Arrange
        const int currentPageNumber = 1;
        const int totalPages = 1;

        // Act
        List<VisiblePageNumber> result = PaginationHelper.GenerateVisiblePageNumbers(currentPageNumber, totalPages);

        // Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(MaxNumberOfItemsPerPage)]
    public void IfNumItemsIsMaxPageSizeOrLessThenNumItemsOnPageShouldBeNumItemsReturnedByContentful(int numItems)
    {
        // Act
        List<News> newListofNewsItems = PaginationHelper.GetPaginatedItemsForSpecifiedPage(BuildListofNewsItems(numItems),
                                                                                        1,
                                                                                        "Display Name",
                                                                                        MaxNumberOfItemsPerPage,
                                                                                        12).Items;

        // Assert
        Assert.Equal(numItems, newListofNewsItems.Count);
    }

    [Theory]
    [InlineData(MaxNumberOfItemsPerPage * 2)]
    [InlineData(MaxNumberOfItemsPerPage * 3)]
    [InlineData(MaxNumberOfItemsPerPage * 10)]
    public void IfNumItemsIsEvenlyDivisibleByMaxPageSizeThenNumItemsOnPageShouldBeFifteen(int numItems)
    {
        // Act
        List<News> newListofNewsItems = PaginationHelper.GetPaginatedItemsForSpecifiedPage(BuildListofNewsItems(numItems),
                                                                                        1,
                                                                                        string.Empty,
                                                                                        MaxNumberOfItemsPerPage,
                                                                                        12).Items;

        // Assert
        Assert.Equal(MaxNumberOfItemsPerPage, newListofNewsItems.Count);
    }

    [Theory]
    [InlineData(MaxNumberOfItemsPerPage + 1, 2)]
    [InlineData((MaxNumberOfItemsPerPage * 2) + 4, 3)]
    [InlineData((MaxNumberOfItemsPerPage * 3) + 2, 4)]
    public void
        IfNumItemsIsGreaterThanMaxPageSizeAndNotEvenlyDivisibleByMaxPageSizeThenThenLastPageShouldReturnNumItemsModMaxPageSize(
        int numItems, int lastPageNum)
    {
        // Act
        List<News> newListofNewsItems = PaginationHelper.GetPaginatedItemsForSpecifiedPage(BuildListofNewsItems(numItems),
                                                                                        lastPageNum,
                                                                                        "item description",
                                                                                        MaxNumberOfItemsPerPage,
                                                                                        12).Items;

        // Assert
        Assert.Equal(numItems % MaxNumberOfItemsPerPage, newListofNewsItems.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(MaxNumberOfItemsPerPage)]
    [InlineData(4)]
    public void IfNumItemsIsMaxPageSizeOrLessShouldReturnOnePage(int numItems)
    {
        // Act
        Pagination pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(BuildListofNewsItems(numItems),
                                                                                1,
                                                                                "item description",
                                                                                MaxNumberOfItemsPerPage,
                                                                                12).Pagination;

        // Assert
        Assert.Equal(1, pagination.TotalPages);
    }

    [Theory]
    [InlineData(MaxNumberOfItemsPerPage)]
    [InlineData(MaxNumberOfItemsPerPage * 2)]
    [InlineData(MaxNumberOfItemsPerPage * 10)]
    public void IfNumItemsIsEvenlyDivisibleByMaxPageSizeNumPagesReturnedShouldBeNumItemsDividedByMaxPageSize(int numItems)
    {
        // Act
        Pagination pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(BuildListofNewsItems(numItems),
                                                                                1,
                                                                                "item description",
                                                                                MaxNumberOfItemsPerPage,
                                                                                12).Pagination;

        // Assert
        Assert.Equal(numItems / MaxNumberOfItemsPerPage, pagination.TotalPages);
    }

    [Theory]
    [InlineData((MaxNumberOfItemsPerPage * 3) + 2)]
    [InlineData(MaxNumberOfItemsPerPage + 1)]
    [InlineData(MaxNumberOfItemsPerPage + 4)]
    public void IfNumItemsAboveMaxPageSizeAndNotEvenlyDivisibleByMaxPageSizeNumPagesReturnedShouldBeNumItemsDividedByMaxPageSizePlusOne(int numItems)
    {
        // Act
        Pagination pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(BuildListofNewsItems(numItems),
                                                                                1,
                                                                                "item description",
                                                                                MaxNumberOfItemsPerPage,
                                                                                12).Pagination;

        // Assert
        Assert.Equal((numItems / MaxNumberOfItemsPerPage) + 1, pagination.TotalPages);
    }

    [Fact]
    public void IfSpecifiedPageNumIsZeroThenActualPageNumIsOne()
    {
        // Act
        Pagination pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(BuildListofNewsItems(3),
                                                                                0,
                                                                                "item description",
                                                                                MaxNumberOfItemsPerPage,
                                                                                12).Pagination;

        // Assert
        Assert.Equal(1, pagination.CurrentPageNumber);
    }

    [Theory]
    [InlineData((MaxNumberOfItemsPerPage * 3) + 2)]
    [InlineData((MaxNumberOfItemsPerPage * 2))]
    [InlineData(MaxNumberOfItemsPerPage + 12)]
    public void IfSpecifiedPageNumIsTooHighThenActualPageNumIsLastPageNum(int numItems)
    {
        // Arrange
        int lastPageNumber = numItems / MaxNumberOfItemsPerPage;
        if (numItems % MaxNumberOfItemsPerPage > 0)
            lastPageNumber++;

        // Act
        Pagination pagination = PaginationHelper.GetPaginatedItemsForSpecifiedPage(BuildListofNewsItems(numItems),
                                                                                lastPageNumber + 10,
                                                                                "item description",
                                                                                MaxNumberOfItemsPerPage,
                                                                                12).Pagination;

        // Assert
        Assert.Equal(lastPageNumber, pagination.CurrentPageNumber);
    }

    [Fact]
    public void PreviousLinkIsShownWhenPageNumberIsGreaterThanOne()
    {
        // Act & Assert
        Assert.True(PaginationHelper.ShowPreviousLink(5));
    }

    [Fact]
    public void PreviousLinkIsNotShownWhenPageNumberIsEqualToOne()
    {
        // Act & Assert
        Assert.False(PaginationHelper.ShowPreviousLink(1));
    }

    [Fact]
    public void NextLinkIsNotShownWhenPageNumberIsEqualToTotalPages()
    {
        // Act & Assert
        Assert.False(PaginationHelper.ShowNextLink(10, 10));
    }

    [Fact]
    public void NextLinkIsShownWhenPageNumberIsLessThanTotalPages()
    {
        // Act & Assert
        Assert.True(PaginationHelper.ShowNextLink(9, 10));
    }

    private List<News> BuildListofNewsItems(int numberOfItems)
    {
        List<News> listofNewsItems = new();

        for (int i = 0; i < numberOfItems; i++)
        {
            News NewsItem = new("News Article " + i.ToString(),
                                "another-news-article",
                                "This is another news article",
                                "purpose",
                                "hero-image.png",
                                "image.jpg",
                                "thumbnail.jpg",
                                "hero image caption",
                                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
                                new DateTime(2015, 9, 10),
                                "test",
                                new DateTime(2015, 9, 20),
                                new DateTime(2015, 9, 15),
                                new List<Alert>(),
                                new List<string>(),
                                new List<Document>(),
                                new List<Profile>(),
                                new List<InlineQuote>(),
                                null,
                                string.Empty,
                                new List<TrustedLogo>(),
                                null,
                                string.Empty,
                                null);

            listofNewsItems.Add(NewsItem);
        }

        return listofNewsItems;
    }

    [Fact]
    public void BuildUrlShouldUseUrlHelperToCreateUrlWithPageQueryParamWithCorrectPageNumber()
    {
        // Arrange
        QueryUrl queryUrl = new(new RouteValueDictionary(), new QueryCollection());
        Mock<IUrlHelperWrapper> urlHelper = new();
        urlHelper
            .Setup(helper => helper.RouteUrl(It.Is<RouteValueDictionary>(x => x.ContainsKey("Page") && x.Values.Contains(5.ToString()))))
            .Returns("this string is not relevant");

        // Act
        PaginationHelper.BuildUrl(5, queryUrl, urlHelper.Object);

        // Assert 
        urlHelper.Verify();
    }

    [Fact]
    public void GeneratePageSequence_ReturnsAllPages_WhenTotalPagesLessThanOrEqualToMaxVisiblePages()
    {
        // Act
        List<int?> result = PaginationHelper.GeneratePageSequence(1, 7);

        // Assert
        Assert.Equal(new int?[] { 1, 2, 3, 4, 5, 6, 7 }, result);
    }

    [Fact]
    public void GeneratePageSequence_ReturnsTheFirst5PagesAndLast_WhenCurrentPageIs4()
    {
        // Act
        List<int?> result = PaginationHelper.GeneratePageSequence(4, 8);

        // Assert
        Assert.Equal(new int?[] { 1, 2, 3, 4, 5, null, 8 }, result);
    }

    [Fact]
    public void GeneratePageSequence_Returns2Ellipses_WhenCurrentPageIs5AndTotalPagesGreaterThan7()
    {
        // Act
        List<int?> result = PaginationHelper.GeneratePageSequence(5, 8);

        // Assert
        Assert.Equal(new int?[] { 1, null, 4, 5, 6, null, 8 }, result);
    }

    [Fact]
    public void GeneratePageSequence_ReturnsCorrectSequence_WhenOnly1TotalPage()
    {
        // Act
        List<int?> result = PaginationHelper.GeneratePageSequence(1, 1);

        // Assert
        Assert.Equal(new int?[] { 1 }, result);
    }

    [Fact]
    public void GeneratePageSequence_ReturnsNoEllipsis_WhenTotalPagesEquals7OrLess()
    {
        // Act
        List<int?> result = PaginationHelper.GeneratePageSequence(4, 6);

        // Assert
        Assert.Equal(new int?[] { 1, 2, 3, 4, 5, 6 }, result);
    }
}