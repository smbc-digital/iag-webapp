namespace StockportWebappTests_Unit.Unit.Controllers;

public class NewsControllerTest
{
    private NewsController _controller;
    private readonly Mock<IRepository> _repository = new();
    private readonly Mock<IProcessedContentRepository> _processedContentRepository = new();
    private readonly Mock<IRssFeedFactory> _mockRssFeedFactory = new();
    private readonly Mock<ILogger<NewsController>> _logger = new();
    private readonly Mock<IApplicationConfiguration> _config = new();
    private const string BusinessId = "businessId";
    private const string EmailAlertsTopicId = "test-id";
    private const bool EmailAlertsOn = true;
    private readonly Mock<IFilteredUrl> _filteredUrl = new();
    private readonly Mock<IFeatureManager> _featureManager = new();

    private static readonly News NewsItemWithImages = new(
        "Another news article",
        "another-news-article",
        "This is another news article",
        "type",
        "hero-image.png",
        "image.jpg",
        "thumbnail.jpg",
        "hero image caption",
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
        new List<Crumb>(),
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
        "in partnership with",
        new List<TrustedLogo>(),
        null,
        "dance",
        new List<Event>()
    );

    private static readonly News NewsItemWithoutImages = new(
        "News 26th Aug",
        "news-26th-aug",
        "test",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        "test",
        new List<Crumb>(),
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
        new List<Event>()
    );

    private readonly ProcessedNews _processedNewsArticle = new(
        "Another news article",
        "another-news-article",
        "This is another news article",
        "purpose",
        "hero-image.png",
        "image.jpg",
        "thumbnail.jpg",
        "hero image caption",
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
        new List<Crumb>(),
        new DateTime(2015, 9, 10),
        "test",
        new DateTime(2015, 9, 20),
        new DateTime(2015, 9, 15),
        new List<Alert>(),
        new List<string> { "Events", "Bramall Hall" },
        new List<InlineQuote>(),
        null,
        "logo area title",
        new List<TrustedLogo>(),
        null,
        "dance-category",
        new List<Event>()
    );

    private readonly List<News> _listOfNewsItems = new() 
    {
        NewsItemWithoutImages,
        NewsItemWithImages
    };

    private readonly Newsroom _newsRoom;
    private readonly Newsroom _emptyNewsRoom;
    public const int MaxNumberOfItemsPerPage = 15;
    
    public NewsControllerTest()
    {
        _newsRoom = new(_listOfNewsItems,
                        null,
                        null,
                        null,
                        null,
                        new OrderedList<Alert>(),
                        EmailAlertsOn,
                        EmailAlertsTopicId,
                        new List<string>(),
                        new List<DateTime>(),
                        new List<int>(),
                        null);
        
        _emptyNewsRoom = new(new List<News>(),
                            null,
                            null,
                            null,
                            null,
                            new OrderedList<Alert>(),
                            EmailAlertsOn,
                            EmailAlertsTopicId,
                            new List<string>(),
                            new List<DateTime>(),
                            new List<int>(),
                            null);

        HttpResponse responseListing = new(200, _newsRoom, string.Empty);
        HttpResponse responseDetail = new(200, _processedNewsArticle, string.Empty);
        HttpResponse emptyResponsListing = new(200, _emptyNewsRoom, string.Empty);

        _repository
            .Setup(repo => repo.Get<Newsroom>(It.IsAny<string>(), It.Is<List<Query>>(l => l.Count.Equals(0))))
            .ReturnsAsync(responseListing);

        _repository
            .Setup(repo => repo.GetLatest<List<News>>(7))
            .ReturnsAsync(HttpResponse.Successful(200, _listOfNewsItems));

        _processedContentRepository
            .Setup(repo => repo.Get<News>("another-news-article", null))
            .ReturnsAsync(responseDetail);

        _mockRssFeedFactory
            .Setup(factory => factory.BuildRssFeed(It.IsAny<List<News>>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns("rss fun");

        _config
            .Setup(conf => conf.GetRssEmail(BusinessId))
            .Returns(AppSetting.GetAppSetting("rss-email"));
        
        _config
            .Setup(conf => conf.GetEmailAlertsNewSubscriberUrl(BusinessId))
            .Returns(AppSetting.GetAppSetting("email-alerts-url"));

        _featureManager
            .Setup(feature => feature.IsEnabledAsync("NewsRedesign"))
            .ReturnsAsync(true);
        
        _controller = new(_repository.Object,
                        _processedContentRepository.Object,
                        _mockRssFeedFactory.Object,
                        _logger.Object,
                        _config.Object,
                        new BusinessId(BusinessId),
                        _filteredUrl.Object,
                        _featureManager.Object);
    }

    [Fact]
    public async Task Index_ShouldReturnANewsListingPageWithTwoItems()
    {
        // Act
        ViewResult actionResponse = await _controller.Index(new NewsroomViewModel(), 1, MaxNumberOfItemsPerPage) as ViewResult;
        NewsroomViewModel viewModel = actionResponse.ViewData.Model as NewsroomViewModel;
        Newsroom news = viewModel.Newsroom;

        // Assert
        Assert.Equal(2, news.News.Count);
        Assert.Equal(NewsItemWithoutImages, news.News[0]);
        Assert.Equal(NewsItemWithImages, news.News[1]);
        Assert.Equal(EmailAlertsTopicId, news.EmailAlertsTopicId);
        Assert.Equal(EmailAlertsOn, news.EmailAlerts);
    }

    [Fact]
    public async Task Index_ShouldReturnAListOfNewsArticlesForATagAndACategory()
    {
        // Arrange
        _repository
            .Setup(repo=> repo.Get<Newsroom>(string.Empty, It.Is<List<Query>>(l => l.Contains(new Query("tag", "Events")) && l.Contains(new Query("Category", "A Category")))))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _newsRoom));

        // Act
        ViewResult actionResponse = await _controller.Index(new NewsroomViewModel
            {
                Tag = "Events",
                Category = "A Category"
            },
            1, MaxNumberOfItemsPerPage) as ViewResult;

        NewsroomViewModel viewModel = actionResponse.ViewData.Model as NewsroomViewModel;
        Newsroom news = viewModel.Newsroom;

        // Assert
        Assert.Equal(2, news.News.Count);
        Assert.Equal(NewsItemWithoutImages, news.News[0]);
        Assert.Equal(NewsItemWithImages, news.News[1]);
        Assert.Equal(EmailAlertsOn, news.EmailAlerts);
        Assert.Equal(EmailAlertsTopicId, news.EmailAlertsTopicId);
    }

    [Fact]
    public async Task Index_ShouldReturn404ForNoNewsItems()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<Newsroom>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(404, null, "not found"));
        
        NewsController controller = new(_repository.Object,
                                        _processedContentRepository.Object,
                                        _mockRssFeedFactory.Object,
                                        _logger.Object,
                                        _config.Object,
                                        new BusinessId(BusinessId),
                                        _filteredUrl.Object,
                                        null);

        // Act
        HttpResponse response = await controller.Index(new NewsroomViewModel(), 1, MaxNumberOfItemsPerPage) as HttpResponse;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task Index_ShouldReturnNewsItemsForADateFilter()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<Newsroom>(string.Empty,
                                            It.Is<List<Query>>(l => l.Contains(new Query("DateFrom", "2016-10-01")) && l.Contains(new Query("DateTo", "2016-11-01")))))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _newsRoom));

        // Act
        ViewResult actionResponse = await _controller.Index(
                    new NewsroomViewModel
                    {
                        DateFrom = new DateTime(2016, 10, 01),
                        DateTo = new DateTime(2016, 11, 01)
                    }, 1, MaxNumberOfItemsPerPage) as ViewResult;

        NewsroomViewModel viewModel = actionResponse.ViewData.Model as NewsroomViewModel;
        Newsroom news = viewModel.Newsroom;

        // Assert
        Assert.Equal(_newsRoom, news);
    }
    
    [Fact]
    public async Task Index_ShouldReturnEmptyPaginationForNoNewsItems()
    {
        // Arrange
        Mock<IRepository> emptyRepository = new();

        emptyRepository
            .Setup(repo => repo.Get<Newsroom>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _emptyNewsRoom));

        NewsController controller = new(emptyRepository.Object,
                                        _processedContentRepository.Object,
                                        _mockRssFeedFactory.Object,
                                        _logger.Object,
                                        _config.Object,
                                        new BusinessId(BusinessId),
                                        _filteredUrl.Object,
                                        null);

        // Act
        ViewResult actionResponse = await controller.Index(
            new NewsroomViewModel
            {
                DateFrom = null,
                DateTo = null
            }, 1, MaxNumberOfItemsPerPage) as ViewResult;

        NewsroomViewModel viewModel = actionResponse.ViewData.Model as NewsroomViewModel;

        // Assert
        Assert.Empty(viewModel.Newsroom.News);
        Assert.Equal(0, viewModel.Pagination.TotalItems);
    }

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(2, 1, 2, 1)]
    [InlineData(MaxNumberOfItemsPerPage, 1, MaxNumberOfItemsPerPage, 1)]
    [InlineData(MaxNumberOfItemsPerPage * 3, 1, MaxNumberOfItemsPerPage, 3)]
    [InlineData(MaxNumberOfItemsPerPage + 1, 2, 1, 2)]
    public async Task Index_PaginationShouldResultInCorrectNumItemsOnPageAndCorrectNumPages(
        int totalNumItems,
        int requestedPageNumber,
        int expectedNumItemsOnPage,
        int expectedNumPages)
    {
        // Arrange
        NewsController controller = SetUpController(totalNumItems);
        NewsroomViewModel model = new();

        // Act
        ViewResult actionResponse = await controller.Index(model, requestedPageNumber, MaxNumberOfItemsPerPage) as ViewResult;

        // Assert
        NewsroomViewModel viewModel = actionResponse.ViewData.Model as NewsroomViewModel;

        Assert.Equal(expectedNumItemsOnPage, viewModel.Newsroom.News.Count);
        Assert.Equal(expectedNumPages, model.Pagination.TotalPages);
    }

    [Theory]
    [InlineData(0, 50, 1)]
    [InlineData(5, MaxNumberOfItemsPerPage * 3, 3)]
    public async Task Index_IfSpecifiedPageNumIsImpossibleThenActualPageNumWillBeAdjustedAccordingly(
        int specifiedPageNumber,
        int numItems,
        int expectedPageNumber)
    {
        // Arrange
        NewsController controller = SetUpController(numItems);
        NewsroomViewModel model = new();

        // Act
        await controller.Index(model, specifiedPageNumber, MaxNumberOfItemsPerPage);

        // Assert
        Assert.Equal(expectedPageNumber, model.Pagination.CurrentPageNumber);
    }

    [Fact]
    public async Task Index_ShouldReturnEmptyPaginationObjectIfNoNewsArticlesExist()
    {
        // Arrange
        NewsController controller = SetUpController(0);
        NewsroomViewModel model = new();

        // Act
        await controller.Index(model, 0, MaxNumberOfItemsPerPage);

        // Assert
        Assert.NotNull(model.Pagination);
    }

    [Fact]
    public async Task Index_ShouldReturnCurrentURLForPagination()
    {
        // Arrange
        NewsController controller = SetUpController(10);
        NewsroomViewModel model = new();

        // Act
        await controller.Index(model, 0, MaxNumberOfItemsPerPage);

        // Assert
        Assert.NotNull(model.Pagination.CurrentUrl);
    }

    [Fact]
    public async Task Detail_ShouldReturnANewsPageWithImageDocumentsAndLatestNews()
    {
        // Act
        ViewResult actionResponse = await _controller.Detail("another-news-article") as ViewResult;
        NewsViewModel news = actionResponse.ViewData.Model as NewsViewModel;

        // Assert
        Assert.Equal("Another news article", news.NewsItem.Title);
        Assert.Equal("another-news-article", news.NewsItem.Slug);
        Assert.Equal("This is another news article", news.NewsItem.Teaser);
        Assert.Equal("image.jpg", news.NewsItem.Image);
        Assert.Equal("thumbnail.jpg", news.NewsItem.ThumbnailImage);
        Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.", news.NewsItem.Body);
        Assert.Equal(new DateTime(2015, 9, 10), news.NewsItem.SunriseDate);
        Assert.Equal(new DateTime(2015, 9, 20), news.NewsItem.SunsetDate);
        Assert.Equal(new DateTime(2015, 9, 15), news.NewsItem.UpdatedAt);
        Assert.Equal(2, news.NewsItem.Tags.Count);
        Assert.Equal("Events", news.NewsItem.Tags.First());
        Assert.Equal(2, news.GetLatestNews().Count);
    }

    [Fact]
    public async Task Detail_ShouldReturnANewsPageWithNoLatestNewsItems()
    {
        // Arrange
        _repository
            .Setup(repo => repo.GetLatest<List<News>>(7))
            .ReturnsAsync(new HttpResponse(404, null, "not found"));
        
        NewsController controller = new(_repository.Object,
                                        _processedContentRepository.Object,
                                        _mockRssFeedFactory.Object,
                                        _logger.Object, _config.Object,
                                        new BusinessId(BusinessId),
                                        _filteredUrl.Object,
                                        null);

        // Act
        ViewResult response = await controller.Detail("another-news-article") as ViewResult;
        NewsViewModel model = response.Model as NewsViewModel;

        // Arrange
        Assert.Equal("another-news-article", model.NewsItem.Slug);
    }

    [Fact]
    public async Task Detail_ShouldReturn404NotFoundForNewsArticleThatDoesNotExist()
    {
        // Arrange
        _processedContentRepository
            .Setup(repo => repo.Get<News>("this-news-article-does-not-exist", null))
            .ReturnsAsync(new HttpResponse(404, null, "not found"));

        // Act
        HttpResponse actionResponse = await _controller.Detail("this-news-article-does-not-exist") as HttpResponse;

        // Assert
        Assert.Equal(404, actionResponse.StatusCode);
    }

    [Fact]
    public async Task Rss_ShouldCreateRssFeedFromFactory()
    {
        // Arrange
        Mock<IRepository> repository = new();
        repository
            .Setup(repo => repo.Get<Newsroom>(It.IsAny<string>(), null))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _newsRoom));

        _controller = new NewsController(repository.Object,
                                        _processedContentRepository.Object,
                                        _mockRssFeedFactory.Object,
                                        _logger.Object,
                                        _config.Object,
                                        new BusinessId(BusinessId),
                                        _filteredUrl.Object,
                                        null);

        // Act
        ContentResult response = await _controller.Rss() as ContentResult;

        // Assert
        Assert.Equal("application/rss+xml", response.ContentType);
        Assert.Equal("rss fun", response.Content);
        _mockRssFeedFactory.Verify(rssFeedFactory => rssFeedFactory.BuildRssFeed(It.IsAny<List<News>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task NewsArchive_ShouldReturn404ForNoNewsItems()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<Newsroom>("/archive", It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(404, null, "not found"));

        // Act
        HttpResponse response = await _controller.NewsArchive(new NewsroomViewModel(), 1, MaxNumberOfItemsPerPage) as HttpResponse;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task NewsArchive_ShouldReturnViewResult_WithCorrectModel_WhenSuccessful()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<Newsroom>("/archive", It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _newsRoom));

        NewsroomViewModel model = new();

        // Act
        ViewResult result = await _controller.NewsArchive(model, 1, 10) as ViewResult;
        NewsroomViewModel viewModel = result.ViewData.Model as NewsroomViewModel;
        Newsroom news = viewModel.Newsroom;

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        NewsroomViewModel returnedModel = Assert.IsType<NewsroomViewModel>(viewResult.Model);

        Assert.Equal(_newsRoom, returnedModel.Newsroom);
    }

    [Fact]
    public async Task NewsArticle_ShouldReturn404_WhenInitialResponseFails()
    {
        // Arrange
        _processedContentRepository
            .Setup(repo => repo.Get<News>("this-news-article-does-not-exist", null))
            .ReturnsAsync(new HttpResponse(404, null, "not found"));

        // Act
        HttpResponse result = await _controller.NewsArticle("this-news-article-does-not-exist") as HttpResponse;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }

     [Fact]
    public async Task NewsArticle_ShouldReturnView_WithNewsViewModel_WhenSuccessful()
    {
        // Act
        ViewResult actionResponse = await _controller.NewsArticle("another-news-article") as ViewResult;
        NewsViewModel news = actionResponse.ViewData.Model as NewsViewModel;

        // Assert
        Assert.Equal("Another news article", news.NewsItem.Title);
        Assert.Equal("another-news-article", news.NewsItem.Slug);
        Assert.Equal("This is another news article", news.NewsItem.Teaser);
        Assert.Equal("image.jpg", news.NewsItem.Image);
        Assert.Equal("thumbnail.jpg", news.NewsItem.ThumbnailImage);
        Assert.Equal("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.", news.NewsItem.Body);
        Assert.Equal(new DateTime(2015, 9, 10), news.NewsItem.SunriseDate);
        Assert.Equal(new DateTime(2015, 9, 20), news.NewsItem.SunsetDate);
        Assert.Equal(new DateTime(2015, 9, 15), news.NewsItem.UpdatedAt);
        Assert.Equal(2, news.NewsItem.Tags.Count);
        Assert.Equal("Events", news.NewsItem.Tags.First());
        Assert.Equal(2, news.GetLatestNews().Count);
    }

    [Fact]
    public async Task NewsArticles_ShouldReturn404_WhenNoNewsArticlesExist()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<Newsroom>(string.Empty, It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(404, null, "not found"));

        // Act
        HttpResponse response = await _controller.NewsArticles(new NewsroomViewModel(), 1, MaxNumberOfItemsPerPage) as HttpResponse;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }
    
    [Fact]
    public async Task NewsArticles_ShouldReturnView_WithCorrectModel_WhenSuccessful()
    {
        // Act
        IActionResult result = await _controller.NewsArticles(new NewsroomViewModel(), 1, MaxNumberOfItemsPerPage);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        NewsroomViewModel returnedModel = Assert.IsType<NewsroomViewModel>(viewResult.Model);

        Newsroom addedNews = returnedModel.Newsroom;

        Assert.NotNull(addedNews);
        Assert.Null(addedNews.LatestArticle);
        Assert.NotNull(addedNews.LatestNews);
        Assert.True(addedNews.LatestNews.Items.Count <= 3);
        Assert.NotNull(addedNews.NewsItems);
    }

    [Fact]
    public async Task NewsArticles_ShouldReturnNewsItemsForADateFilter()
    {
        // Arrange
        _repository
            .Setup(repo => repo.Get<Newsroom>(string.Empty,
                                            It.Is<List<Query>>(l => l.Contains(new Query("DateFrom", "2016-10-01")) && l.Contains(new Query("DateTo", "2016-11-01")))))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _newsRoom));

        // Act
        ViewResult actionResponse = await _controller.NewsArticles(
                    new NewsroomViewModel
                    {
                        DateFrom = new DateTime(2016, 10, 01),
                        DateTo = new DateTime(2016, 11, 01)
                    }, 1, MaxNumberOfItemsPerPage) as ViewResult;

        NewsroomViewModel viewModel = actionResponse.ViewData.Model as NewsroomViewModel;
        Newsroom news = viewModel.Newsroom;

        // Assert
        Assert.Equal(_newsRoom, news);
    }
    
    private NewsController SetUpController(int numNewsItems)
    {
        List<News> listOfNewsItems = BuildNewsList(numNewsItems);

        Newsroom bigNewsRoom = new(listOfNewsItems,
                                null,
                                null,
                                null,
                                null,
                                new OrderedList<Alert>(),
                                EmailAlertsOn,
                                EmailAlertsTopicId,
                                new List<string>(),
                                new List<DateTime>(),
                                new List<int>(),
                                null);

        _repository
            .Setup(repo => repo.Get<Newsroom>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, bigNewsRoom));

        return new(_repository.Object,
                _processedContentRepository.Object,
                _mockRssFeedFactory.Object,
                _logger.Object,
                _config.Object,
                new BusinessId(BusinessId),
                _filteredUrl.Object,
                null);
    }

    private static List<News> BuildNewsList(int numberOfItems)
    {
        List<News> listofNewsItems = new();

        for (int i = 0; i < numberOfItems; i++)
        {
            News NewsItem = new("News Article " + i.ToString(),
                                "another-news-article",
                                "This is another news article",
                                "type",
                                "hero-image.png",
                                "image.jpg",
                                "thumbnail.jpg",
                                "hero image caption",
                                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
                                new List<Crumb>(),
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
                                "in partnership with",
                                new List<TrustedLogo>(),
                                null,
                                "dance",
                                new List<Event>());

            listofNewsItems.Add(NewsItem);
        }

        return listofNewsItems;
    }
}