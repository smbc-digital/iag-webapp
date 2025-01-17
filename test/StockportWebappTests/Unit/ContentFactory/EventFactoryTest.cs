namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class EventFactoryTest
{
    private readonly EventFactory _factory;
    private readonly Mock<MarkdownWrapper> _markdownWrapper = new();
    private readonly Mock<ITagParserContainer> _tagParserContainer = new();
    private readonly Mock<IDynamicTagParser<Document>> _documentTagParser = new();
    private readonly Event _event;
    private const string Title = "This is the event";
    private const string Description = "The event";
    private const string Slug = "event-of-the-century";
    private const string Teaser = "Read more for the event";
    private const string Image = "image.jpg";
    private const string ThumbnailImage = "thumbnail.jpg";
    private const string Fee = "Free";
    private const string Location = "Bramall Hall, Carpark, SK7 6HG";
    private const string SubmittedBy = "Friends of Stockport";
    private const string StartTime = "10:00";
    private const string EndTime = "17:00";
    private readonly DateTime _eventDate = new(2016, 12, 30);
    private readonly List<Crumb> _breadcrumbs = new();
    private readonly string _bookingInformation = "Booking information";
    private readonly List<Alert> _alerts = new()
        {
            new Alert("title",
                    "subHeading",
                    "body",
                    "severity",
                    new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc),
                    string.Empty,
                    false,
                    string.Empty)
        };

    public EventFactoryTest()
    {
        _factory = new EventFactory(_tagParserContainer.Object, _markdownWrapper.Object);
        _event = new Event
        {
            Title = Title,
            Slug = Slug,
            Teaser = Teaser,
            ImageUrl = Image,
            ThumbnailImageUrl = ThumbnailImage,
            Description = Description,
            Fee = Fee,
            Location = Location,
            SubmittedBy = SubmittedBy,
            EventDate = _eventDate,
            StartTime = StartTime,
            EndTime = EndTime,
            Breadcrumbs = _breadcrumbs,
            BookingInformation = _bookingInformation,
            Alerts = _alerts
        };

        _tagParserContainer
            .Setup(parser => parser.ParseAll(Description,
                It.IsAny<string>(),
                It.IsAny<bool>(),
                null,
                null,
                null,
                null,
                null,
                null,
                It.IsAny<bool>()))
            .Returns(Description);
        
        _markdownWrapper
            .Setup(parser => parser.ConvertToHtml(Description))
            .Returns(Description);
        
        _documentTagParser
            .Setup(parser => parser.Parse(Description, _event.Documents, It.IsAny<bool>()))
            .Returns(Description);
    }

    [Fact]
    public void ShouldSetTheCorrespondingFieldsForAProcessedEvent()
    {
        // Act
        ProcessedEvents result = _factory.Build(_event);
        
        // Assert
        Assert.Equal("This is the event", result.Title);
        Assert.Equal("The event", result.Description);
        Assert.Equal("event-of-the-century", result.Slug);
        Assert.Equal("Read more for the event", result.Teaser);
        Assert.Equal("Free", result.Fee);
        Assert.Equal("Bramall Hall, Carpark, SK7 6HG", result.Location);
        Assert.Equal("Friends of Stockport", result.SubmittedBy);
        Assert.Equal(new DateTime(2016, 12, 30), result.EventDate);
        Assert.Equal("10:00", result.StartTime);
        Assert.Equal("17:00", result.EndTime);
        Assert.Equal("Booking information", result.BookingInformation);
        Assert.Equal(_alerts[0].Title, result.Alerts[0].Title);
        Assert.Equal(_alerts[0].Body, result.Alerts[0].Body);
        Assert.Equal(_alerts[0].Severity, result.Alerts[0].Severity);
        Assert.Equal(_alerts[0].SubHeading, result.Alerts[0].SubHeading);
        Assert.Equal(_alerts[0].SunriseDate, result.Alerts[0].SunriseDate);
        Assert.Equal(_alerts[0].SunsetDate, result.Alerts[0].SunsetDate);
    }

    [Fact]
    public void ShouldProcessDescriptionWithMarkdown()
    {
        // Act
        _factory.Build(_event);

        // Assert
        _markdownWrapper.Verify(wrapper => wrapper.ConvertToHtml(Description), Times.Once);
    }

    [Fact]
    public void ShouldPassTitleToAllSimpleParsersWhenBuilding()
    {
        // Act
        _factory.Build(_event);

        // Assert
        _tagParserContainer.Verify(parser => parser.ParseAll(Description,
                                                            _event.Title,
                                                            It.IsAny<bool>(),
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            null,
                                                            It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public void ShouldProcessDescriptionWithDocumentParser()
    {
        // Act
        _factory.Build(_event);

        // Assert
        _tagParserContainer.Verify(parser => parser.ParseAll(Description,
                                                _event.Title,
                                                It.IsAny<bool>(),
                                                null,
                                                _event.Documents,
                                                null,
                                                null,
                                                null,
                                                null,
                                                It.IsAny<bool>()), Times.Once);
    }
}