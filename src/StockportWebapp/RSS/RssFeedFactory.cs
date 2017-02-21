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
                Copyright = string.Concat("Copyright " , DateTime.Today.ToString("yyyy"),", Stockport Council"),
                Title = "Stockport Council News Feed",
                Description = "Stockport Council News Feed"
        };            

            foreach (var rssItem in rssItemsList)
            {
                if (typeof(T) == typeof(News))
                {
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
                        Body = eventItem.Teaser + "<br /> Location: " + eventItem.Location + "<br /> Fee: " + eventItem.Fee + "<br /> Event Date and Time: " + eventItem.EventDate.ToString("dd/MM/yyyy") + " " + eventItem.StartTime + " - " + eventItem.EndTime,
                        Link = new Uri(host + eventItem.Slug),
                        Permalink = new Uri(host + eventItem.Slug).AbsoluteUri,
                        PublishDate = eventItem.UpdatedAt,                
                        Author = new Author { Name = "Stockport Council", Email = email }                        
                    };
                    feed.Items.Add(item);
                }
            }
            return feed.Serialize();
        }
    }
}
