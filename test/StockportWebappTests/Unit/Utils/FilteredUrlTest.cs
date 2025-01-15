namespace StockportWebappTests_Unit.Unit.Utils;

public class FilteredUrlTest
{
    private readonly Mock<ITimeProvider> _timeProvider = new();
    private readonly FilteredUrl _filteredUrl;

    public FilteredUrlTest()
    {
        _timeProvider
            .Setup(provider => provider.Now())
            .Returns(new DateTime(2017, 02, 21));

        _filteredUrl = new(_timeProvider.Object);
    }

    [Fact]
    public void WillRemoveCategoryQueryParamFromUrl()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "Category",
                        new StringValues(["business"])
                    },
                    {
                        "tag",
                        new StringValues(["healthy"])
                    }
                }
            ));

        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        RouteValueDictionary newQueryUrl = _filteredUrl.WithoutCategory();

        // Assert
        Assert.False(newQueryUrl.ContainsKey("Category"));
    }

    [Fact]
    public void WillAddCategoryFilterToUrl()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "tag",
                        new StringValues(["healthy"])
                    }
                }
            ));

        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        RouteValueDictionary newQueryUrl = _filteredUrl.AddCategoryFilter("business");

        // Assert
        Assert.True(newQueryUrl.ContainsKey("Category"));
    }

    [Fact]
    public void WillIdentifyWhenCategoryFilterIsPresent()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "Category",
                        new StringValues(["business"])
                    }
                }
            ));

        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        bool hasNoCategoryfilter = _filteredUrl.HasNoCategoryFilter();

        // Assert
        Assert.False(hasNoCategoryfilter);
    }

    [Fact]
    public void WillIdentifyWhenCategoryFilterIsNotPresent()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(new Dictionary<string, StringValues>()));

        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        bool hasNoCategoryfilter = _filteredUrl.HasNoCategoryFilter();

        // Assert
        Assert.True(hasNoCategoryfilter);
    }

    [Fact]
    public void WillAddDateFromFilterToUrl()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "tag",
                        new StringValues(["healthy"])
                    }
                }
            )
            );

        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        RouteValueDictionary newQueryUrl = _filteredUrl.AddMonthFilter(new DateTime(2017, 01, 01));

        // Assert
        Assert.True(newQueryUrl.ContainsKey("dateFrom"));
    }

    [Fact]
    public void WillAddDateToFilterToUrl()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "tag",
                        new StringValues(["healthy"])
                    }
                }
            )
            );

        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        RouteValueDictionary newQueryUrl = _filteredUrl.AddMonthFilter(new DateTime(2017, 01, 01));

        // Assert
        Assert.True(newQueryUrl.ContainsKey("dateTo"));
    }

    [Fact]
    public void WillPopulateDateFromFilterInUrl()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "tag",
                        new StringValues(["healthy"])
                    }
                }
            ));

        _filteredUrl.SetQueryUrl(queryUrl);
        DateTime startDate = new(2017, 01, 01);

        // Act
        RouteValueDictionary newQueryUrl = _filteredUrl.AddMonthFilter(startDate);

        // Assert
        Assert.Equal(startDate.ToString("yyyy-MM-dd"), newQueryUrl["dateFrom"]);
    }

    [Fact]
    public void WillPopulateDateToFilterInUrl()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "tag",
                        new StringValues(["healthy"])
                    }
                }
            ));

        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        RouteValueDictionary newQueryUrl = _filteredUrl.AddMonthFilter(new DateTime(2017, 01, 01));

        // Assert
        Assert.Equal(new DateTime(2017, 01, 31).ToString("yyyy-MM-dd"), newQueryUrl["dateTo"]);
    }

    [Fact]
    public void WillRemoveDateFilterQueryParamFromUrl()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "DateFrom",
                        new StringValues(["irrelevant"])
                    },
                    {
                        "DateTo",
                        new StringValues(["irrelevant"])
                    }
                }
            )
            );
        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        RouteValueDictionary newQueryUrl = _filteredUrl.WithoutDateFilter();

        // Assert
        Assert.False(newQueryUrl.ContainsKey("DateFrom"));
        Assert.False(newQueryUrl.ContainsKey("DateTo"));
    }

    [Fact]
    public void WillIdentifyWhenDateFilterIsPresent()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "DateFrom",
                        new StringValues(["irrelevant"])
                    }
                }
            ));
        
        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        bool hasNoDatefilter = _filteredUrl.HasNoDateFilter();

        // Assert
        Assert.False(hasNoDatefilter);
    }

    [Fact]
    public void WillIdentifyWhenDateFilterIsNotPresent()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(new Dictionary<string, StringValues>()));

        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        bool hasNoDatefilter = _filteredUrl.HasNoDateFilter();

        // Assert
        Assert.True(hasNoDatefilter);
    }

    [Fact]
    public void WillRemoveTagQueryParamFromUrl()
    {
        // Arrange
        QueryUrl queryUrl = new(
            new RouteValueDictionary(),
            new QueryCollection(
                new Dictionary<string, StringValues>
                {
                    {
                        "Category",
                        new StringValues(["business"])
                    },
                    {
                        "tag",
                        new StringValues(["healthy"])
                    }
                }
            )
        );
        
        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        RouteValueDictionary newQueryUrl = _filteredUrl.WithoutTagFilter();

        // Assert
        Assert.False(newQueryUrl.ContainsKey("tag"));
    }

    [Fact]
    public void ShouldReturnTodaysDateIfDateToIsWithinTheCurrentMonth()
    {
        // Arrange
        QueryUrl queryUrl = new(new RouteValueDictionary(), new QueryCollection());
        _filteredUrl.SetQueryUrl(queryUrl);

        // Act
        RouteValueDictionary url = _filteredUrl.AddMonthFilter(new DateTime(2017, 02, 01));

        url["DateTo"].Should().Be(new DateTime(2017, 02, 21).ToString("yyyy-MM-dd"));
    }

    [Fact]
    public void ShouldReturnEmptyIfHasNullQueryUrlForAddMonthFilter()
    {
        // Arrange
        FilteredUrl filteredUrl = new(_timeProvider.Object);

        // Act
        RouteValueDictionary result = filteredUrl.AddMonthFilter(new DateTime(2017, 01, 01));

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ShouldReturnEmptyIfHasNullQueryUrlForAddCategoryFilter()
    {
        // Arrange
        FilteredUrl filteredUrl = new(_timeProvider.Object);

        // Act
        RouteValueDictionary result = filteredUrl.AddCategoryFilter("test");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ShouldReturnFalseIfHasNullQueryUrlForHasNoCategoryFilter()
    {
        // Arrange
        FilteredUrl filteredUrl = new(_timeProvider.Object);

        // Act
        bool result = filteredUrl.HasNoCategoryFilter();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShouldReturnEmptyIfHasNullQueryUrlForHasNoDateFilter()
    {
        // Arrange
        FilteredUrl filteredUrl = new(_timeProvider.Object);

        // Act
        bool result = filteredUrl.HasNoDateFilter();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ShouldReturnEmptyIfHasNullQueryUrlForWithoutCategory()
    {
        // Arrange
        FilteredUrl filteredUrl = new(_timeProvider.Object);

        // Act
        RouteValueDictionary result = filteredUrl.WithoutCategory();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ShouldReturnEmptyIfHasNullQueryUrlForWithoutDateFilter()
    {
        // Arrange
        FilteredUrl filteredUrl = new(_timeProvider.Object);

        // Act
        RouteValueDictionary result = filteredUrl.WithoutDateFilter();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ShouldReturnEmptyIfHasNullQueryUrlForWithoutTagFilter()
    {
        // Arrange
        FilteredUrl filteredUrl = new(_timeProvider.Object);

        // Act
        RouteValueDictionary result = filteredUrl.WithoutTagFilter();

        // Assert
        Assert.Empty(result);
    }
}