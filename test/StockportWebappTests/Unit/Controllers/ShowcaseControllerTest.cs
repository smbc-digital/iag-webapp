namespace StockportWebappTests_Unit.Unit.Controllers;

public class ShowcaseControllerTest
{
    private readonly ShowcaseController _controller;

    private readonly Mock<IProcessedContentRepository> _mockContentRepository = new();

    public ShowcaseControllerTest() => _controller = new ShowcaseController(_mockContentRepository.Object, new Mock<IApplicationConfiguration>().Object);

    [Fact]
    public async Task ItReturnsShowcaseWithProcessedBody()
    {
        const string showcaseSlug = "showcase-slug";
        List<Alert> alerts = new() { new("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                        new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false, string.Empty) };
        ProcessedShowcase showcase = new(
            "Test showcase",
            showcaseSlug,
            "showcase teaser",
            "showcase metaDescription",
            "showcase subheading",
            "event category",
            "events Category Or Tag",
            "event subheading",
            "news subheading",
            "news category",
            "news type",
            "body subheading",
            "body",
            null,
            "af981b9771822643da7a03a9ae95886f/picture.jpg",
            new List<SubItem>
            {
                new("slug",
                    "title",
                    "teaser",
                    "icon",
                    "type",
                    "contentType",
                    "image.jpg",
                    0,
                    "body text",
                    new List<SubItem>(),
                    string.Empty,
                    string.Empty,
                    EColourScheme.Teal)
            },
            new List<Crumb>
            {
                new("title", "slug", "type")
            },
            string.Empty,
            new List<SocialMediaLink>(),
            new List<Event>(),
            "email alerts topic id",
            "emailAlertsText",
            alerts,
            new List<SubItem>(),
            string.Empty,
            new List<SubItem>(),
            null,
            null,
            new CallToActionBanner(),
            new FieldOrder(),
            "fa-icon",
            string.Empty,
            null,
            string.Empty,
            string.Empty,
            string.Empty,
            new Video(),
            new SpotlightBanner("test", "test", "test"));

        _mockContentRepository
            .Setup(_ => _.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, showcase, string.Empty));

        ViewResult showcasePage = await _controller.Showcase(showcaseSlug) as ViewResult; ;
        ProcessedShowcase processedShowcase = showcasePage.Model as ProcessedShowcase;

        processedShowcase.Title.Should().Be("Test showcase");
        processedShowcase.Slug.Should().Be("showcase-slug");
        processedShowcase.Teaser.Should().Be("showcase teaser");
        processedShowcase.Subheading.Should().Be("showcase subheading");
        processedShowcase.EventCategory.Should().Be("event category");
        processedShowcase.EventSubheading.Should().Be("event subheading");
        processedShowcase.Breadcrumbs.Count().Should().Be(1);
        processedShowcase.SecondaryItems.Count().Should().Be(1);
        processedShowcase.Alerts.Count().Should().Be(1);
        processedShowcase.Icon.Should().Be("fa-icon");
    }

    [Fact]
    public async Task Returns404WhenShowcaseNotFound()
    {
        _mockContentRepository
            .Setup(_ => _.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        HttpResponse response = await _controller.Showcase("not-found-slug") as HttpResponse; ;

        response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ReturnsEmptyListIfAllTopicsExpired()
    {
        _mockContentRepository
            .Setup(_ => _.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        HttpResponse response = await _controller.Showcase("not-found-slug") as HttpResponse; ;

        response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}