using System;
using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Models;
using StockportWebapp.RSS;
using Xunit;
using System.Xml.Linq;
using FluentAssertions;

namespace StockportWebappTests.Unit.RSS
{
    public class RssFeedFactoryTest
    {
        private readonly RssNewsFeedFactory _rssFeedFactory;

        public RssFeedFactoryTest()
        {
            _rssFeedFactory = new RssNewsFeedFactory();
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
            news.Add(new News("news item  1", "item1-slug", "teser-item1", "", "", "", new List<Crumb>(),
                new DateTime(2016, 09, 01), new DateTime(2016, 09, 01), new List<Alert>(), new List<string>(),new List<Document>()));
            news.Add(new News("news item  2", "item2-slug", "teser-item2", "", "", "", new List<Crumb>(),
                new DateTime(2016, 09, 01), new DateTime(2016, 09, 01), new List<Alert>(), new List<string>(),new List<Document>()));
            news.Add(new News("news item  3", "item3-slug", "teser-item3", "", "", "", new List<Crumb>(),
                new DateTime(2016, 09, 01), new DateTime(2016, 09, 01), new List<Alert>(), new List<string>(), new List<Document>()));
            news.Add(new News("news item  4", "item4-slug", "teser-item4", "", "", "", new List<Crumb>(),
                new DateTime(2016, 09, 01), new DateTime(2016, 09, 01), new List<Alert>(), new List<string>(), new List<Document>()));

            var rss = _rssFeedFactory.BuildRssFeed(news,"http://localhost", "email@test.email");

            var xmlDoc = XDocument.Parse(rss);

            var rootNode = xmlDoc.Root;
            var channelNode = rootNode.Element("channel");
            var itemNodes = channelNode.Elements("item");

            itemNodes.ToList()[0].Element("title").Value.Should().Be("news item  1");
            itemNodes.Count().Should().Be(4);
        }
    }
}
