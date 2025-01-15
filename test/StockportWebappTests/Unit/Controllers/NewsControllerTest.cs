namespace StockportWebappTests_Unit.Unit.Controllers;

public class NewsControllerTest
{
    private NewsController _controller;
    private Mock<IRepository> _repository = new();
    private readonly Mock<IProcessedContentRepository> _processedContentRepository = new();
    private readonly Mock<IRssFeedFactory> _mockRssFeedFactory = new();
    private readonly Mock<ILogger<NewsController>> _logger = new();
    private readonly Mock<IApplicationConfiguration> _config = new();
    private const string BusinessId = "businessId";
    private const string EmailAlertsTopicId = "test-id";
    private const bool EmailAlertsOn = true;
    private readonly Mock<IFilteredUrl> _filteredUrl = new();

    private static readonly News NewsItemWithImages = new("Another news article",
        "another-news-article",
        "This is another news article",
        "type",
        "image.jpg",
        "thumbnail.jpg",
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
        new List<Crumb>(),
        new DateTime(2015, 9, 10),
        new DateTime(2015, 9, 20),
        new DateTime(2015, 9, 15),
        new List<Alert>(),
        new List<string>(),
        new List<Document>(),
        new List<Profile>()
    );

    private static readonly News NewsItemWithoutImages = new("News 26th Aug",
        "news-26th-aug",
        "test",
        "",
        "",
        "",
        "test",
        new List<Crumb>(),
        new DateTime(2015, 9, 10),
        new DateTime(2015, 9, 20),
        new DateTime(2015, 9, 15),
        new List<Alert>(),
        new List<string>(),
        new List<Document>(),
        new List<Profile>()
    );

    private readonly ProcessedNews _processedNewsArticle = new("Another news article",
        "another-news-article",
        "This is another news article",
        "purpose",
        "image.jpg",
        "thumbnail.jpg",
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
        new List<Crumb>(),
        new DateTime(2015, 9, 10),
        new DateTime(2015, 9, 20),
        new DateTime(2015, 9, 15),
        new List<Alert>(),
        new List<string> { "Events", "Bramall Hall" }
    );

    private readonly List<News> _listOfNewsItems = new() { NewsItemWithoutImages, NewsItemWithImages };
    private readonly Newsroom _newsRoom;
    private readonly Newsroom _emptyNewsRoom;
    public const int MaxNumberOfItemsPerPage = 15;
    
    public NewsControllerTest()
    {
        _newsRoom = new(_listOfNewsItems,
                        new OrderedList<Alert>(),
                        EmailAlertsOn,
                        EmailAlertsTopicId,
                        new List<string>(),
                        new List<DateTime>());
        
        _emptyNewsRoom = new(new List<News>(),
                            new OrderedList<Alert>(),
                            EmailAlertsOn,
                            EmailAlertsTopicId,
                            new List<string>(),
                            new List<DateTime>());

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

        _controller = new(_repository.Object,
                        _processedContentRepository.Object,
                        _mockRssFeedFactory.Object,
                        _logger.Object,
                        _config.Object,
                        new BusinessId(BusinessId),
                        _filteredUrl.Object);
    }

    [Fact]
    public async Task Index_ItReturnsANewsListingPageWithTwoItems()
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
    public async Task Detail_ItReturnsANewsPageWithImageDocumentssAndLatestNews()
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
                                        _filteredUrl.Object);

        // Act
        ViewResult response = await controller.Detail("another-news-article") as ViewResult;
        NewsViewModel model = response.Model as NewsViewModel;

        // Arrange
        Assert.Equal("another-news-article", model.NewsItem.Slug);
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
                                        _filteredUrl.Object);

        // Act
        HttpResponse response = await controller.Index(new NewsroomViewModel(), 1, MaxNumberOfItemsPerPage) as HttpResponse;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task Detail_ShouldReturn404NotFoundForNewsArticleThatdoesNotExist()
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
                                        _filteredUrl.Object);

        // Act
        ContentResult response = await _controller.Rss() as ContentResult;

        // Assert
        Assert.Equal("application/rss+xml", response.ContentType);
        Assert.Equal("rss fun", response.Content);
        _mockRssFeedFactory.Verify(_ => _.BuildRssFeed(It.IsAny<List<News>>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
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
            .Setup(_ => _.Get<Newsroom>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, _emptyNewsRoom));

        NewsController controller = new(emptyRepository.Object,
                                        _processedContentRepository.Object,
                                        _mockRssFeedFactory.Object,
                                        _logger.Object,
                                        _config.Object,
                                        new BusinessId(BusinessId),
                                        _filteredUrl.Object);

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

    private NewsController SetUpController(int numNewsItems)
    {
        List<News> listofNewsItems = BuildNewsList(numNewsItems);

        Newsroom bigNewsRoom = new(listofNewsItems,
                                new OrderedList<Alert>(),
                                EmailAlertsOn,
                                EmailAlertsTopicId,
                                new List<string>(),
                                new List<DateTime>());

        _repository
            .Setup(_ => _.Get<Newsroom>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful((int)HttpStatusCode.OK, bigNewsRoom));

        return new(_repository.Object,
                                        _processedContentRepository.Object,
                                        _mockRssFeedFactory.Object,
                                        _logger.Object,
                                        _config.Object,
                                        new BusinessId(BusinessId),
                                        _filteredUrl.Object);
    }

    private List<News> BuildNewsList(int numberOfItems)
    {
        List<News> listofNewsItems = new();

        for (int i = 0; i < numberOfItems; i++)
        {
            News NewsItem = new("News Article " + i.ToString(),
                                "another-news-article",
                                "This is another news article",
                                "type",
                                "image.jpg",
                                "thumbnail.jpg",
                                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam gravida eu mauris in consectetur. Nullam nulla urna, sagittis a ex sit amet, ultricies rhoncus mauris. Quisque vel placerat turpis, vitae consectetur mauris.",
                                new List<Crumb>(),
                                new DateTime(2015, 9, 10),
                                new DateTime(2015, 9, 20),
                                new DateTime(2015, 9, 15),
                                new List<Alert>(),
                                new List<string>(),
                                new List<Document>(),
                                new List<Profile>());

            listofNewsItems.Add(NewsItem);
        }

        return listofNewsItems;
    }
}