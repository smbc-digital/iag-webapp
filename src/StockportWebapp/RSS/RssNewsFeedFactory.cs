using System;
using System.Collections.Generic;
using StockportWebapp.Models;
using WilderMinds.RssSyndication;

namespace StockportWebapp.RSS
{
    public interface IRssNewsFeedFactory
    {
        string BuildRssFeed(List<News> news, string host, string email);        
    }

    public class RssNewsFeedFactory : IRssNewsFeedFactory
    {
        public string BuildRssFeed(List<News> news, string host, string email)
        {
            var feed = new Feed
            {
                Title = "Stockport Council News Feed",
                Description = "Stockport Council News Feed",
                Link = new Uri(host),
                Copyright = string.Concat("Copyright " , DateTime.Today.ToString("yyyy"),", Stockport Council")
            };            

            foreach (var newsItem in news)
            {
                var item = new Item
                {                    
                    Title = newsItem.Title,
                    Body =  newsItem.Teaser,
                    Link =  new Uri(host + newsItem.Slug) ,
                    Permalink = new Uri(host + newsItem.Slug).AbsoluteUri,
                    PublishDate = newsItem.SunriseDate,
                    Author = new Author { Name = "Stockport Council", Email = email }        
                };               
                feed.Items.Add(item);
            }
            return feed.Serialize();
        }
    }
}
