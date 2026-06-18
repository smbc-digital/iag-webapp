namespace StockportWebappTests_Unit.Unit.ViewModels;

public class NewsroomViewModelTest
{
    private static readonly List<string> emptyList = new();
    private const string EmailAlertsUrl = "url";
    private readonly Newsroom _newsroom;

    public NewsroomViewModelTest() =>
        _newsroom = BuildNewsRoom();




    [Fact]
    public void ShouldGiveCategoriesInAlphabeticalOrder()
    {
        // Act
        Newsroom newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });
        NewsroomViewModel newsroomViewModel = new(newsroom);

        // Assert
        List<string> expectedOrder = new() { "Asses", "Oxen", "Zebras" };
        Assert.Equal(expectedOrder, newsroomViewModel.Categories);
    }

    [Fact]
    public void ShouldReturnToAndFromDatesIfDateRangeIsCustomDate()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateRange = "customdate",
            DateFrom = new DateTime(2016, 01, 01),
            DateTo = new DateTime(2016, 02, 01)
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("01/01/2016 to 01/02/2016", result);
    }

    [Fact]
    public void ShouldReturnMonthNameIfDateRangeIsMonth()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2016, 01, 01),
            DateTo = new DateTime(2016, 01, 31)
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("January 2016", result);
    }

    [Fact]
    public void ShouldDisplaySingleDateIfToDateAndFromDateAreTheSame()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateRange = "customdate",
            DateFrom = new DateTime(2016, 01, 01),
            DateTo = new DateTime(2016, 01, 01)
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("01/01/2016", result);
    }

    [Fact]
    public void HasActiveFilterShouldReturnFalseWhenNoActiveFiltersExsist()
    {
        // Arrange
        Newsroom newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });
        NewsroomViewModel newsroomViewModel = new(newsroom);
        
        // Act
        bool result = newsroomViewModel.HasActiveFilter();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasActiveFilterShouldReturnTrueWhenActiveFiltersExist()
    {
        // Arrange
        Newsroom newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });
        NewsroomViewModel newsroomViewModel = new(newsroom);
        newsroomViewModel.Category = "Zebras";
        newsroomViewModel.Tag = "Tag";
        newsroomViewModel.DateFrom = DateTime.Now.AddDays(-5);
        newsroomViewModel.DateTo = DateTime.Now;

        // Act
        bool result = newsroomViewModel.HasActiveFilter();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasActiveFilterShouldReturnFalseWhenDateFilterIsInvalid()
    {
        // Arrange
        Newsroom newsroom = BuildNewsRoom(categories: new List<string> { "Zebras", "Asses", "Oxen" });
        NewsroomViewModel newsroomViewModel = new(newsroom);
        newsroomViewModel.DateFrom = DateTime.Now;
        newsroomViewModel.DateTo = DateTime.Now.AddDays(-5);

        // Act
        bool result = newsroomViewModel.HasActiveFilter();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetActiveDateFilter_ReturnsEmptyString_WhenDateFromIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = null,
            DateTo = DateTime.Today,
            DateRange = "customdate"
        };


        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Empty(result);
    }

       [Fact]
    public void GetActiveDateFilter_ReturnsEmptyString_WhenDateToIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = DateTime.Today,
            DateTo = null,
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetActiveDateFilter_ReturnsSingleDate_WhenDateRangeIsCustomdate_AndDatesAreEqual()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 7),
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("07/07/2025", result);
    }

      [Fact]
    public void GetActiveDateFilter_ReturnsDateRange_WhenDateRangeIsCustomdate_AndDatesAreDifferent()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 10),
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("07/07/2025 to 10/07/2025", result);
    }

    [Fact]
    public void GetActiveDateFilter_ReturnsMonthYear_WhenDateRangeIsNotCustomdate()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 20),
            DateRange = "month"
        };

        // Act
        string result = newsroomViewModel.GetActiveDateFilter();

        // Assert
        Assert.Equal("July 2025", result);
    }

     [Fact]
    public void GetActiveYearFilter_ReturnsEmptyString_WhenDateFromIsNull()
    {
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = null,
            DateTo = DateTime.Today,
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetActiveYearFilter_ReturnsEmptyString_WhenDateToIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = DateTime.Today,
            DateTo = null,
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetActiveYearFilter_ReturnsSingleDate_WhenDateRangeIsCustomdate_AndDatesAreEqual()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 7),
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Equal("07/07/2025", result);
    }

    [Fact]
    public void GetActiveYearFilter_ReturnsDateRange_WhenDateRangeIsCustomdate_AndDatesAreDifferent()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 10),
            DateRange = "customdate"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Equal("07/07/2025 to 10/07/2025", result);
    }

    [Fact]
    public void GetActiveYearFilter_ReturnsYear_WhenDateRangeIsNotCustomdate()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            DateFrom = new DateTime(2025, 7, 7),
            DateTo = new DateTime(2025, 7, 20),
            DateRange = "year"
        };

        // Act
        string result = newsroomViewModel.GetActiveYearFilter();

        // Assert
        Assert.Equal("2025", result);
    }

    [Fact]
    public void IsFirstPage_ShouldReturnTrue_WhenCurrentPageNumberIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            Pagination = new Pagination()
        };

        // Act
        bool result = newsroomViewModel.IsFirstPage;

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void IsFirstPage_ShouldReturnTrue_WhenCurrentPageNumberIsZero(int currentPageNumber)
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = currentPageNumber
            }
        };

        // Act
        bool result = newsroomViewModel.IsFirstPage;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsFirstPage_ShouldReturnFalse_WhenCurrentPageNumberGreaterThan1()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new()
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = 5
            }
        };

        // Act
        bool result = newsroomViewModel.IsFirstPage;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasLatestArticle_ShouldReturnFalse_WhenLatestArticleHasNoItems()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom());

        // Act
        bool result = newsroomViewModel.HasLatestArticle;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasLatestArticle_ShouldReturnTrue_WhenLatestArticleHasItems()
    {
        // Arrange
        NavCardList latestArticle = new()
        {
            Items = new List<NavCard>
            {
                new("Title",
                    "slug",
                    "Teaser",
                    "thumbnail.jpg",
                    "image.jpg",
                    string.Empty,
                    EColourScheme.Teal,
                    DateTime.Now,
                    string.Empty)
            }
        };

        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom(null, latestArticle));

        // Act
        bool result = newsroomViewModel.HasLatestArticle;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasLatestNews_ShouldReturnTrue_WhenLatestNewsHasItems()
    {
        // Arrange
        NavCardList latestNews = new()
        {
            Items = new List<NavCard>
            {
                new("Title",
                    "slug",
                    "Teaser",
                    "thumbnail.jpg",
                    "image.jpg",
                    string.Empty,
                    EColourScheme.Teal,
                    DateTime.Now,
                    string.Empty)
            }
        };

        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom(null, null, latestNews));

        // Act
        bool result = newsroomViewModel.HasLatestNews;

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void HasLatestNews_ShouldReturnFalse_WhenLatestNewsHasNoItems()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom());

        // Act
        bool result = newsroomViewModel.HasLatestNews;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasNewsItems_ShouldReturnTrue_WhenNewsItemsHasItems()
    {
        // Arrange
        NavCardList newsItems = new()
        {
            Items = new List<NavCard>
            {
                new("Title",
                    "slug",
                    "Teaser",
                    "thumbnail.jpg",
                    "image.jpg",
                    string.Empty,
                    EColourScheme.Teal,
                    DateTime.Now,
                    string.Empty)
            }
        };

        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom(null, null, null, newsItems));

        // Act
        bool result = newsroomViewModel.HasNewsItems;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasNewsItems_ShouldReturnFalse_WhenNewsItemsHasNoItems()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom());

        // Act
        bool result = newsroomViewModel.HasNewsItems;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasCallToAction_ShouldReturnTrue_WhenCallToActionIsNotNull()
    {
        // Arrange
        CallToActionBanner callToAction = new()
        {
            Title = "Title",
            Teaser = "Teaser",
            ButtonText = "Button Text",
            Link = "Button Link",
            Image = "Image"
        };

        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom(callToAction: callToAction));

        // Act
        bool result = newsroomViewModel.HasCallToAction;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasCallToAction_ShouldReturnFalse_WhenCallToActionIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom());

        // Act
        bool result = newsroomViewModel.HasCallToAction;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShowFeaturedNews_ShouldReturnTrue_WhenIsFirstPageAndHasLatestArticle()
    {
        // Arrange
        NavCardList latestArticle = new()
        {
            Items = new List<NavCard>
            {
                new("Title",
                    "slug",
                    "Teaser",
                    "thumbnail.jpg",
                    "image.jpg",
                    string.Empty,
                    EColourScheme.Teal,
                    DateTime.Now,
                    string.Empty)
            }
        };

        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom(latestArticle: latestArticle))
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = 1
            }
        };

        // Act
        bool result = newsroomViewModel.ShowFeaturedNews;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShowFeaturedNews_ShouldReturnFalse_WhenNotFirstPageOrHasNoLatestArticle()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom())
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = 2
            }
        };

        // Act
        bool result = newsroomViewModel.ShowFeaturedNews;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShowLatestNews_ShouldReturnTrue_WhenIsFirstPageAndHasLatestNews()
    {
        // Arrange
        NavCardList latestNews = new()
        {
            Items = new List<NavCard>
            {
                new("Title",
                    "slug",
                    "Teaser",
                    "thumbnail.jpg",
                    "image.jpg",
                    string.Empty,
                    EColourScheme.Teal,
                    DateTime.Now,
                    string.Empty)
            }
        };

        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom(latestNews: latestNews))
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = 1
            }
        };

        // Act
        bool result = newsroomViewModel.ShowLatestNews;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShowLatestNews_ShouldReturnFalse_WhenNotFirstPageOrHasNoLatestNews()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom())
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = 2
            }
        };

        // Act
        bool result = newsroomViewModel.ShowLatestNews;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void PageTitle_ShouldReturnCorrectTitle_WhenTotalItemsExceedMaxItemsPerPage()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom())
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = 2,
                TotalPages = 7,
                TotalItems = 85,
                MaxItemsPerPage = 12
            }
        };

        // Act
        string result = newsroomViewModel.PageTitle;

        // Assert
        Assert.Equal("- Page 2 of 7", result);
    }

    [Fact]
    public void PageTitle_ShouldReturnEmptyString_WhenPaginationIsNull()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom())
        {
            Pagination = null
        };

        // Act
        string result = newsroomViewModel.PageTitle;

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void PageTitle_ShouldReturnCorrectTitle_WhenOnPageOneWithPagination()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom())
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = 1,
                TotalPages = 4,
                TotalItems = 50,
                MaxItemsPerPage = 10
            }
        };

        // Act
        string result = newsroomViewModel.PageTitle;

        // Assert
        Assert.Equal("- Page 1 of 4", result);
    }

    [Fact]
    public void PageTitle_ShouldReturnEmtpyString_WhenPaginationHasNoTotalItems()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom())
        {
            Pagination = new Pagination
            {
                CurrentPageNumber = 1,
                TotalPages = 0,
                TotalItems = 0,
                MaxItemsPerPage = 10
            }
        };

        // Act
        string result = newsroomViewModel.PageTitle;

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void IsFromSearch_ShouldReturnFalse_WhenNoCategoryOrTagIsPresent()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom());

        // Act
        bool result = newsroomViewModel.IsFromSearch();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsFromSearch_ShouldReturnTrue_WhenCategoryIsPresent()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom())
        {
            Category = "Zebras"
        };

        // Act
        bool result = newsroomViewModel.IsFromSearch();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsFromSearch_ShouldReturnTrue_WhenTagIsPresent()
    {
        // Arrange
        NewsroomViewModel newsroomViewModel = new(BuildNewsRoom())
        {
            Tag = "Tag"
        };

        // Act
        bool result = newsroomViewModel.IsFromSearch();

        // Assert
        Assert.True(result);
    }

    private static Newsroom BuildNewsRoom(List<string> categories = null,
                                        NavCardList latestArticle = null,
                                        NavCardList latestNews = null,
                                        NavCardList newsItems = null,
                                        CallToActionBanner callToAction = null) =>
        new(new List<News>(),
            null,
            latestArticle,
            latestNews,
            newsItems,
            new List<Alert>(),
            categories ?? emptyList,
            new List<DateTime>(),
            new List<int>(),
            callToAction);
}