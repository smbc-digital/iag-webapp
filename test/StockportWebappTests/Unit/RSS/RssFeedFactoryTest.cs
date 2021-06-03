using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Castle.Components.DictionaryAdapter;
using FluentAssertions;
using StockportWebapp.Models;
using StockportWebapp.RSS;
using Xunit;

namespace StockportWebappTests_Unit.Unit.RSS
{
    public class RssFeedFactoryTest
    {
        private readonly RssFeedFactory _rssFeedFactory;

        public RssFeedFactoryTest()
        {
            _rssFeedFactory = new RssFeedFactory();
        }

        [Fact]
        public void CreatesChannelOnRss()
        {
            var rss = _rssFeedFactory.BuildRssFeed(new List<News>(), "http://localhost:5000/news", "email@test.email");

            var xmlDoc = XDocument.Parse(rss);

            var rootNode = xmlDoc.Root;

            var channelNode = rootNode.Element("channel");

            channelNode.Element("title").Value.Should().Be("Stockport Council News Feed");
            channelNode.Element("link").Value.Should().Be("http://localhost:5000/news");
            channelNode.Element("description").Value.Should().Be("Stockport Council News Feed");
        }

        [Fact]
        public void CreateNewsItemsForRssFeed()
        {
            var news = new List<News>();
            news.Add(new News("news item  1", "item1-slug", "teser-item1", "purpose", "", "", "", new List<Crumb>(),
                new DateTime(2016, 09, 01), new DateTime(2016, 09, 01), new List<Alert>(), new List<string>(),new List<Document>()));
            news.Add(new News("news item  2", "item2-slug", "teser-item2", "purpose", "", "", "", new List<Crumb>(),
                new DateTime(2016, 09, 01), new DateTime(2016, 09, 01), new List<Alert>(), new List<string>(),new List<Document>()));
            news.Add(new News("news item  3", "item3-slug", "teser-item3", "purpose", "", "", "", new List<Crumb>(),
                new DateTime(2016, 09, 01), new DateTime(2016, 09, 01), new List<Alert>(), new List<string>(), new List<Document>()));
            news.Add(new News("news item  4", "item4-slug", "teser-item4", "purpose", "", "", "", new List<Crumb>(),
                new DateTime(2016, 09, 01), new DateTime(2016, 09, 01), new List<Alert>(), new List<string>(), new List<Document>()));

            var rss = _rssFeedFactory.BuildRssFeed(news,"http://localhost", "email@test.email");

            var xmlDoc = XDocument.Parse(rss);

            var rootNode = xmlDoc.Root;
            var channelNode = rootNode.Element("channel");
            var itemNodes = channelNode.Elements("item");

            itemNodes.ToList()[0].Element("title").Value.Should().Be("news item  4");
            itemNodes.Count().Should().Be(4);
        }

        [Fact]
        public void CreateEventItemsForRssFeed()
        {
            var events = new List<Event>();
            events.Add(new Event()
            {
                Title = "Event Title 1", Description = "Event Description 1", Categories = new List<string>(),
                Breadcrumbs = new EditableList<Crumb>(), EventDate = new DateTime(2017,08,01),StartTime = "10:00",
                EndTime="17:00",Fee="Free",Documents = new List<Document>(),Location = "Stoppford House", UpdatedAt = new DateTime(2017,12,25)
            });
            events.Add(new Event()
            {
                Title = "Event Title 2",
                Description = "Event Description 3",
                Categories = new List<string>(),
                Breadcrumbs = new EditableList<Crumb>(),
                EventDate = new DateTime(2017, 08, 01),
                StartTime = "10:00",
                EndTime = "17:00",
                Fee = "Free",
                Documents = new List<Document>(),
                Location = "Stoppford House",
                UpdatedAt = new DateTime(2017, 12, 25)
            });

            events.Add(new Event()
            {
                Title = "Event Title 3",
                Description = "Event Description 3",
                Categories = new List<string>(),
                Breadcrumbs = new EditableList<Crumb>(),
                EventDate = new DateTime(2017, 08, 01),
                StartTime = "10:00",
                EndTime = "17:00",
                Fee = "Free",
                Documents = new List<Document>(),
                Location = "Stoppford House",
                UpdatedAt = new DateTime(2017, 12, 25)
            });

            events.Add(new Event()
            {
                Title = "Event Title 4",
                Description = "Event Description 4",
                Categories = new List<string>(),
                Breadcrumbs = new EditableList<Crumb>(),
                EventDate = new DateTime(2017, 08, 01),
                StartTime = "10:00",
                EndTime = "17:00",
                Fee = "Free",
                Documents = new List<Document>(),
                Location = "Stoppford House",
                UpdatedAt = new DateTime(2017, 12, 25)
            });

            var rss = _rssFeedFactory.BuildRssFeed(events, "http://localhost", "email@test.email");

            var xmlDoc = XDocument.Parse(rss);

            var rootNode = xmlDoc.Root;
            var channelNode = rootNode.Element("channel");
            var itemNodes = channelNode.Elements("item");

            itemNodes.ToList()[0].Element("title").Value.Should().Be("Event Title 4");
            itemNodes.ToList()[0].Element("pubDate").Value.Should().Contain("25 Dec 2017");
            itemNodes.Count().Should().Be(4);
            
        }
    }
}
