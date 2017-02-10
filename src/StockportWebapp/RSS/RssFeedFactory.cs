using System;
using System.Collections.Generic;
using StockportWebapp.Models;
using WilderMinds.RssSyndication;

namespace StockportWebapp.RSS
{
    public interface IRssFeedFactory
    {
        string BuildRssFeed<T>(IEnumerable<T> rssItemsList , string host, string email);        
    }

    public class RssFeedFactory : IRssFeedFactory
    {
        public string BuildRssFeed<T>(IEnumerable<T> rssItemsList, string host, string email)
        {
            var feed = new Feed
            {
               
                Link = new Uri(host),
                Copyright = string.Concat("Copyright " , DateTime.Today.ToString("yyyy"),", Stockport Council")
            };            

            foreach (var rssItem in rssItemsList)
            {
                if (typeof(T) == typeof(News))
                {
                    feed.Title = "Stockport Council News Feed";
                    feed.Description = "Stockport Council News Feed";
                    var newsItem = rssItem as News;
                    var item = new Item
                    {
                        Title = newsItem.Title,
                        Body = newsItem.Teaser,
                        Link = new Uri(host + newsItem.Slug),
                        Permalink = new Uri(host + newsItem.Slug).AbsoluteUri,
                        PublishDate = newsItem.SunriseDate,
                        Author = new Author {Name = "Stockport Council", Email = email},
                    };
                    feed.Items.Add(item);
                }

                if (typeof(T) == typeof(Event))
                {
                    feed.Title = "Stockport Council Events Feed";
                    feed.Description = "Stockport Council Events Feed";
                    var eventItem = rssItem as Event;
                    var item = new Item
                    {
                        Title = eventItem.Title,
                        Body = eventItem.Teaser,
                        Link = new Uri(host + eventItem.Slug),
                        Permalink = new Uri(host + eventItem.Slug).AbsoluteUri,
                        PublishDate = eventItem.EventDate,                        
                        Author = new Author { Name = "Stockport Council", Email = email }
                    };
                    feed.Items.Add(item);
                }
            }
            return feed.Serialize();
        }
    }
}
