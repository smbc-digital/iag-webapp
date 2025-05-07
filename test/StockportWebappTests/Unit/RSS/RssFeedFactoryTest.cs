namespace StockportWebappTests_Unit.Unit.RSS;

public class RssFeedFactoryTest
{
    private readonly RssFeedFactory _rssFeedFactory;

    public RssFeedFactoryTest() =>
        _rssFeedFactory = new RssFeedFactory();

    [Fact]
    public void CreatesChannelOnRss()
    {
        // Act
        string rss = _rssFeedFactory.BuildRssFeed(new List<News>(), "http://localhost:5000/news", "email@test.email");

        XDocument xmlDoc = XDocument.Parse(rss);
        XElement rootNode = xmlDoc.Root;
        XElement channelNode = rootNode.Element("channel");

        // Assert
        Assert.Equal("Stockport Council News Feed", channelNode.Element("title").Value);
        Assert.Equal("http://localhost:5000/news", channelNode.Element("link").Value);
        Assert.Equal("Stockport Council News Feed", channelNode.Element("description").Value);
    }

    [Fact]
    public void CreateNewsItemsForRssFeed()
    {
        // Arrange
        List<News> news = new()
        {
            new("news item  1",
                "item1-slug",
                "teser-item1",
                "purpose",
                string.Empty,
                string.Empty,
                string.Empty,
                new List<Crumb>(),
                new DateTime(2016, 09, 01),
                new DateTime(2016, 09, 01),
                new DateTime(2016, 09, 01),
                new List<Alert>(),
                new List<string>(),
                new List<Document>(),
                new List<Profile>()
            ),
            new("news item  2",
                "item2-slug",
                "teser-item2",
                "purpose",
                string.Empty,
                string.Empty,
                string.Empty,
                new List<Crumb>(),
                new DateTime(2016, 09, 01),
                new DateTime(2016, 09, 01),
                new DateTime(2016, 09, 01),
                new List<Alert>(),
                new List<string>(),
                new List<Document>(),
                new List<Profile>()
            ),
            new("news item  3",
                "item3-slug",
                "teser-item3",
                "purpose",
                string.Empty,
                string.Empty,
                string.Empty,
                new List<Crumb>(),
                new DateTime(2016, 09, 01),
                new DateTime(2016, 09, 01),
                new DateTime(2016, 09, 01),
                new List<Alert>(),
                new List<string>(),
                new List<Document>(),
                new List<Profile>()
            ),
            new("news item  4",
                "item4-slug",
                "teser-item4",
                "purpose",
                string.Empty,
                string.Empty,
                string.Empty,
                new List<Crumb>(),
                new DateTime(2016, 09, 01),
                new DateTime(2016, 09, 01),
                new DateTime(2016, 09, 01),
                new List<Alert>(),
                new List<string>(),
                new List<Document>(),
                new List<Profile>()
            )
        };

        // Act
        string rss = _rssFeedFactory.BuildRssFeed(news, "http://localhost", "email@test.email");

        // Assert
        XDocument xmlDoc = XDocument.Parse(rss);
        XElement rootNode = xmlDoc.Root;
        XElement channelNode = rootNode.Element("channel");
        IEnumerable<XElement> itemNodes = channelNode.Elements("item");

        Assert.Equal("news item  4", itemNodes.ToList()[0].Element("title").Value);
        Assert.Equal(4, itemNodes.Count());
    }

    [Fact]
    public void CreateEventItemsForRssFeed()
    {
        // Arrange
        List<Event> events = new()
        {
            new()
            {
                Title = "Event Title 1",
                Description = "Event Description 1",
                Breadcrumbs = new List<Crumb>(),
                EventDate = new DateTime(2017, 08, 01),
                StartTime = "10:00",
                EndTime = "17:00",
                Fee = "Free",
                Documents = new List<Document>(),
                Location = "Stoppford House",
                UpdatedAt = new DateTime(2017, 12, 25)
            },
            new()
            {
                Title = "Event Title 2",
                Description = "Event Description 3",
                Breadcrumbs = new List<Crumb>(),
                EventDate = new DateTime(2017, 08, 01),
                StartTime = "10:00",
                EndTime = "17:00",
                Fee = "Free",
                Documents = new List<Document>(),
                Location = "Stoppford House",
                UpdatedAt = new DateTime(2017, 12, 25)
            },
            new()
            {
                Title = "Event Title 3",
                Description = "Event Description 3",
                Breadcrumbs = new List<Crumb>(),
                EventDate = new DateTime(2017, 08, 01),
                StartTime = "10:00",
                EndTime = "17:00",
                Fee = "Free",
                Documents = new List<Document>(),
                Location = "Stoppford House",
                UpdatedAt = new DateTime(2017, 12, 25)
            },
            new()
            {
                Title = "Event Title 4",
                Description = "Event Description 4",
                Breadcrumbs = new List<Crumb>(),
                EventDate = new DateTime(2017, 08, 01),
                StartTime = "10:00",
                EndTime = "17:00",
                Fee = "Free",
                Documents = new List<Document>(),
                Location = "Stoppford House",
                UpdatedAt = new DateTime(2017, 12, 25)
            }
        };

        // Act
        string rss = _rssFeedFactory.BuildRssFeed(events, "http://localhost", "email@test.email");

        XDocument xmlDoc = XDocument.Parse(rss);
        XElement rootNode = xmlDoc.Root;
        XElement channelNode = rootNode.Element("channel");
        IEnumerable<XElement> itemNodes = channelNode.Elements("item");

        // Assert
        Assert.Equal("Event Title 4", itemNodes.ToList()[0].Element("title").Value);
        Assert.Contains("25 Dec 2017", itemNodes.ToList()[0].Element("pubDate").Value);
        Assert.Equal(4, itemNodes.Count());
    }
}