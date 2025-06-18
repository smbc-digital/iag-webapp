using ReverseMarkdown.Converters;

namespace StockportWebappTests_Unit.Unit.Controllers;

public class ShowcaseControllerTest
{
    private readonly ShowcaseController _controller;

    private readonly Mock<IProcessedContentRepository> _mockContentRepository = new();

    public ShowcaseControllerTest() =>
        _controller = new(_mockContentRepository.Object);

    [Fact]
    public async Task ItReturnsShowcaseWithProcessedBody()
    {
        // Arrange
        List<Alert> alerts = new()
        {
            new("title",
                "body",
                Severity.Information,
                new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                string.Empty,
                false,
                string.Empty)
        };

        ProcessedShowcase showcase = new(
            "Test showcase",
            "showcase-slug",
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
                    "teaser image",
                    "icon",
                    "type",
                    "image.jpg",
                    new List<SubItem>(),
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
            new SpotlightOnBanner("test", new MediaAsset(), "test", "test", "test", new DateTime()));

        _mockContentRepository
            .Setup(repo => repo.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse(200, showcase, string.Empty));

        // Act
        ViewResult showcasePage = await _controller.Showcase("showcase-slug") as ViewResult; ;
        ProcessedShowcase processedShowcase = showcasePage.Model as ProcessedShowcase;

        // Assert
        Assert.Equal("Test showcase", processedShowcase.Title);
        Assert.Equal("showcase-slug", processedShowcase.Slug);
        Assert.Equal("showcase teaser", processedShowcase.Teaser);
        Assert.Equal("showcase subheading", processedShowcase.Subheading);
        Assert.Equal("event category", processedShowcase.EventCategory);
        Assert.Equal("event subheading", processedShowcase.EventSubheading);
        Assert.Single(processedShowcase.Breadcrumbs);
        Assert.Single(processedShowcase.SecondaryItems);
        Assert.Single(processedShowcase.Alerts);
        Assert.Equal("fa-icon", processedShowcase.Icon);
    }

    [Fact]
    public async Task Returns404WhenShowcaseNotFound()
    {
        // Arrange
        _mockContentRepository
            .Setup(repo => repo.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        HttpResponse response = await _controller.Showcase("not-found-slug") as HttpResponse; ;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }

    [Fact]
    public async Task ReturnsEmptyListIfAllTopicsExpired()
    {
        // Arrange
        _mockContentRepository
            .Setup(repo => repo.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

        // Act
        HttpResponse response = await _controller.Showcase("not-found-slug") as HttpResponse; ;

        // Assert
        Assert.Equal(404, response.StatusCode);
    }
}