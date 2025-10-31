namespace StockportWebappTests_Unit.Unit.Controllers;

public class HomeControllerTest
{
    private readonly HomeController _controller;
    private readonly Mock<IApplicationConfiguration> _config = new();
    private readonly Mock<INewsService> _newsService = new();
    private readonly Mock<IEventsService> _eventsService = new();
    private readonly Mock<IHomepageService> _homepageService = new();
    private readonly Mock<IStockportApiEventsService> _stockportApiService = new();

    private readonly List<SubItem> _featuredTasks = new()
    {
        new SubItem("slug featuredTasks",
                    "featured Tasks",
                    "teaser Featured Tasks",
                    "teaser image featured tasks",
                    "fa fa-home",
                    string.Empty,
                    "image",
                    new List<SubItem>(),
                    EColourScheme.Teal)
    };
    
    private readonly List<SubItem> _featuredTopics = new()
    {
        new SubItem("Council Tax",
                    "council-tax",
                    "How to pay, discounts",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    new List<SubItem>(),
                    EColourScheme.Teal)
    };

    private readonly List<Alert> _alerts = new()
    {
        new Alert("title",
                "body",
                Severity.Information,
                new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                string.Empty,
                false,
                string.Empty)
    };

    private readonly List<CarouselContent> _carouselContents = new()
    {
        new CarouselContent("Carousel Title",
                            "Carousel Teaser",
                            "Carousel Image",
                            "Carousel Url",
                            new DateTime())
    };

    private readonly CarouselContent _campaignBanner = new("Campaign Title",
                                                        "Campaign Teaser",
                                                        "Campaign Image",
                                                        "Campaign Url",
                                                        new DateTime());

    private readonly News _newsContent = new("title",
                                            "slug",
                                            "teaser",
                                            "image",
                                            "thumbnail",
                                            "hero image caption",
                                            "body",
                                            new DateTime(2015, 9, 10),
                                            "test",
                                            new DateTime(2015, 9, 20),
                                            new DateTime(2015, 9, 15),
                                            new List<Alert>(),
                                            new List<string>(),
                                            new List<InlineQuote>(),
                                            null,
                                            string.Empty,
                                            new List<TrustedLogo>(),
                                            null,
                                            string.Empty,
                                            new List<Event>());

    private readonly Event _eventsContent = new()
    {
        Title = "title",
        EventDate = new DateTime(2017, 01, 01),
        Featured = true
    };

    private readonly CallToActionBanner _callToActionBanner = new();

    public HomeControllerTest()
    {
        ProcessedHomepage homePageContent = new("Title",
                                                "heading",
                                                "summary",
                                                _featuredTasks,
                                                _featuredTopics,
                                                _alerts,
                                                _carouselContents,
                                                "image.jpg",
                                                "foregroundimage.jpg",
                                                string.Empty,
                                                string.Empty,
                                                string.Empty,
                                                new List<News>(),
                                                "homepage text",
                                                string.Empty,
                                                "meta description",
                                                _campaignBanner,
                                                _callToActionBanner,
                                                _callToActionBanner,
                                                new List<SpotlightOnBanner>());

        _homepageService
            .Setup(service => service.GetHomepage())
            .ReturnsAsync(homePageContent);

        _newsService
            .Setup(service => service.GetLatestNewsItem())
            .ReturnsAsync(_newsContent);

        AppSetting appSetting = AppSetting.GetAppSetting("email_alerts_url");
        _config
            .Setup(conf => conf.GetEmailAlertsUrl("stockportgov"))
            .Returns(appSetting);
        
        _config
            .Setup(conf => conf.GetEmailAlertsNewSubscriberUrl("stockportgov"))
            .Returns(AppSetting.GetAppSetting("email_alerts_url"));
        
        _controller = new HomeController(new BusinessId("stockportgov"),
                                        _config.Object,
                                        _newsService.Object,
                                        _eventsService.Object,
                                        _homepageService.Object,
                                        _stockportApiService.Object);
    }

    [Fact]
    public async Task Index_Should_ReturnHomeView_WithFeaturedTasks()
    {
        // Arrange
        _eventsService
            .Setup(service => service.GetLatestFeaturedEventItem())
            .ReturnsAsync(_eventsContent);

        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel page = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        SubItem featuredTask = page.HomepageContent.FeaturedTasks.First();

        Assert.Equal("heading", page.HomepageContent.FeaturedTasksHeading);
        Assert.Equal("summary", page.HomepageContent.FeaturedTasksSummary);
        Assert.Single(page.HomepageContent.FeaturedTasks);
        Assert.Equal("featured Tasks", featuredTask.Title);
        Assert.Contains("slug featuredTasks", featuredTask.NavigationLink);
        Assert.Equal("teaser Featured Tasks", featuredTask.Teaser);
        Assert.Equal("fa fa-home", featuredTask.Icon);
        Assert.Equal("image", featuredTask.Image);
    }

    [Fact]
    public async Task Index_Should_ReturnHomeView_WithFeaturedTopics()
    {
        // Arrange
        _eventsService
            .Setup(service => service.GetLatestFeaturedEventItem())
            .ReturnsAsync(_eventsContent);

        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel page = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        SubItem featuredTopic = page.HomepageContent.FeaturedTopics.First();

        Assert.Single(page.HomepageContent.FeaturedTopics);
        Assert.Equal("council-tax", featuredTopic.Title);
        Assert.Equal("How to pay, discounts", featuredTopic.Teaser);
        Assert.Empty(featuredTopic.SubItems);
    }

    [Fact]
    public async Task Index_Should_ReturnHomeView_WithAlerts()
    {
        // Arrange
        _eventsService
            .Setup(service => service.GetLatestFeaturedEventItem())
            .ReturnsAsync(_eventsContent);

        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel page = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        Alert alert = page.HomepageContent.Alerts.First();

        Assert.Single(page.HomepageContent.Alerts);
        Assert.Equal("title", alert.Title);
        Assert.Contains("body", alert.Body);
        Assert.Equal(Severity.Information, alert.Severity);
    }

    [Fact]
    public async Task Index_Should_ReturnHomeView_WithCarouselContents()
    {
        // Arrange
        _eventsService
            .Setup(service => service.GetLatestFeaturedEventItem())
            .ReturnsAsync(_eventsContent);

        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel page = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        CarouselContent carouselContent = page.HomepageContent.CarouselContents.First();

        Assert.Single(page.HomepageContent.CarouselContents);
        Assert.Equal("Carousel Title", carouselContent.Title);
        Assert.Equal("Carousel Teaser", carouselContent.Teaser);
        Assert.Equal("Carousel Image", carouselContent.Image);
        Assert.Equal("Carousel Url", carouselContent.URL);
    }

    [Fact]
    public async Task Index_Should_ReturnHomeView_WithBackground()
    {
        // Arrange
        _eventsService
            .Setup(service => service.GetLatestFeaturedEventItem())
            .ReturnsAsync(_eventsContent);

        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel page = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        Assert.Equal("image.jpg", page.HomepageContent.BackgroundImage);
        Assert.Equal("homepage text", page.HomepageContent.FreeText);
        Assert.Equal("meta description", page.HomepageContent.MetaDescription);
    }

    [Fact]
    public async Task Index_Should_ReturnHomeView_WithFeaturedNews()
    {
        // Arrange
        _eventsService
            .Setup(service => service.GetLatestFeaturedEventItem())
            .ReturnsAsync(_eventsContent);

        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel page = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        Assert.Equal("title", page.FeaturedNews.Title);
        Assert.Equal("slug", page.FeaturedNews.Slug);
        Assert.Equal(new(2015, 9, 10), page.FeaturedNews.SunriseDate);
        Assert.Equal(new(2015, 9, 20), page.FeaturedNews.SunsetDate);
    }

    [Fact]
    public async Task Index_Should_ReturnHomeView_WithFeaturedEvent()
    {
        // Arrange
        _eventsService
            .Setup(service => service.GetLatestFeaturedEventItem())
            .ReturnsAsync(_eventsContent);

        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel page = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        Assert.Equal("title", page.FeaturedEvent.Title);
        Assert.Equal(new DateTime(2017, 01, 01), page.FeaturedEvent.EventDate);
    }

    [Fact]
    public async Task Index_Should_ReturnHomeView_WithCampaignBanner()
    {
        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel page = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        Assert.Equal("Campaign Title", page.HomepageContent.CampaignBanner.Title);
        Assert.Equal("Campaign Teaser", page.HomepageContent.CampaignBanner.Teaser);
        Assert.Equal("Campaign Image", page.HomepageContent.CampaignBanner.Image);
        Assert.Equal("Campaign Url", page.HomepageContent.CampaignBanner.URL);
    }

    [Fact]
    public async Task Index_Should_ReturnHomepage_WhenThereAreNoEvents()
    {
        // Arrange
        _eventsService
            .Setup(service => service.GetLatestEventsItem())
            .ReturnsAsync((Event)null);

        // Act
        ViewResult indexPage = await _controller.Index() as ViewResult;
        HomepageViewModel response = indexPage.ViewData.Model as HomepageViewModel;

        // Assert
        Assert.NotNull(response.HomepageContent);
        Assert.Null(response.FeaturedEvent);
    }

    [Fact]
    public async Task Index_Should_ReturnNotFound_IfHomepageIsNull()
    {
        // Arrange
        _homepageService
            .Setup(service => service.GetHomepage())
            .ReturnsAsync((ProcessedHomepage)null);

        // Act
        NotFoundResult response = await _controller.Index() as NotFoundResult;

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Index_Should_CallApiService_IfEventsCategoryNotEmpty()
    {
        // Arrange
        ProcessedHomepage homePageContent = new("Title",
                                                "heading",
                                                "summary",
                                                new List<SubItem>(),
                                                new List<SubItem>(),
                                                new List<Alert>(),
                                                new List<CarouselContent>(),
                                                "image.jpg",
                                                "foregroundImage.jpg",
                                                string.Empty,
                                                string.Empty,
                                                string.Empty,
                                                new List<News>(),
                                                "homepage text",
                                                "unittest",
                                                "meta description",
                                                _campaignBanner,
                                                _callToActionBanner,
                                                _callToActionBanner,
                                                new List<SpotlightOnBanner>());

        _homepageService
            .Setup(service => service.GetHomepage())
            .ReturnsAsync(homePageContent);
        
        _stockportApiService
            .Setup(service => service.GetEventsByCategory("unittest", true))
            .ReturnsAsync(new List<Event> { new EventBuilder().Build() });

        // Act
        await _controller.Index();

        // Assert
        _stockportApiService.Verify(service => service.GetEventsByCategory(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async Task Index_ShouldNot_CallApiService_IfCategoryIsNotEmpty()
    {
        // Act
        await _controller.Index();

        // Assert
        _stockportApiService.Verify(service => service.GetEventsByCategory(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task EmailSubscribe_Should_RedirectToConfiguredUrlWithEmailAddress()
    {
        // Act
        RedirectResult result = await _controller.EmailSubscribe("me@email.com", string.Empty, string.Empty) as RedirectResult;

        // Assert
        Assert.IsType<RedirectResult>(result);
        _config.Verify(conf => conf.GetEmailAlertsUrl("stockportgov"), Times.Once);
        Assert.Equal("email_alerts_url?email=me@email.com", result.Url);
    }

    [Fact]
    public async Task EmailSubscribe_Should_ReturnNotFound_IfEmailConfigurationIsMissing()
    {
        // Arrange
        AppSetting appSetting = AppSetting.GetAppSetting(null);
        _config
            .Setup(conf => conf.GetEmailAlertsUrl("stockportgov"))
            .Returns(appSetting);

        // Act
        StatusCodeResult response = await _controller.EmailSubscribe("me@email.com", string.Empty, string.Empty) as StatusCodeResult; ;

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task EmailSubscribe_Should_RedirectToConfiguredUrlWithEmailAlertsTopicId()
    {
        // Act
        RedirectResult result = await _controller.EmailSubscribe(string.Empty, "test@email.com", string.Empty) as RedirectResult;

        // Assert
        Assert.IsType<RedirectResult>(result);
        Assert.Equal("email_alerts_url?topic_id=test@email.com", result.Url);
        _config.Verify(conf => conf.GetEmailAlertsNewSubscriberUrl("stockportgov"), Times.Once);
    }

    [Fact]
    public async Task EmailSubscribe_Should_RedirectToConfiguredUrlWithMailingListId()
    {
        // Act
        RedirectResult result = await _controller.EmailSubscribe(string.Empty, string.Empty, "123") as RedirectResult;

        // Assert
        Assert.IsType<RedirectResult>(result);
        Assert.Equal("email_alerts_url?topic_id=123", result.Url);
        _config.Verify(conf => conf.GetEmailAlertsNewSubscriberUrl("stockportgov"), Times.Once);
    }
}