﻿namespace StockportWebapp.RSS;

public interface IRssFeedFactory
{
    string BuildRssFeed<T>(IEnumerable<T> rssItemsList, string host, string email);
}

public class RssFeedFactory : IRssFeedFactory
{
    public string BuildRssFeed<T>(IEnumerable<T> rssItemsList, string host, string email)
    {
        Feed feed = new()
        {
            Link = new Uri(host),
            Copyright = string.Concat("Copyright ", DateTime.Today.ToString("yyyy"), ", Stockport Council"),
            Title = "Stockport Council News Feed",
            Description = "Stockport Council News Feed"
        };

        foreach (T rssItem in rssItemsList)
        {
            if (typeof(T).Equals(typeof(News)))
            {
                News newsItem = rssItem as News;
                Item item = new()
                {
                    Title = newsItem.Title,
                    Body = newsItem.Teaser,
                    Link = new Uri(host + newsItem.Slug),
                    Permalink = new Uri(host + newsItem.Slug).AbsoluteUri,
                    PublishDate = newsItem.SunriseDate,
                    Author = new Author { Name = "Stockport Council", Email = email },
                };
                feed.Items.Add(item);
            }

            if (typeof(T).Equals(typeof(Event)))
            {
                feed.Title = "Stockport Council Events Feed";
                feed.Description = "Stockport Council Events Feed";

                Event eventItem = rssItem as Event;
                Item item = new()
                {
                    Title = eventItem.Title,
                    Body = $"{eventItem.Teaser}<br /> Location: {eventItem.Location}<br /> Fee: {eventItem.Fee}<br /> Event Date and Time: {eventItem.EventDate:dd/MM/yyyy} {eventItem.StartTime} - {eventItem.EndTime}",
                    Link = new Uri(host + eventItem.Slug),
                    Permalink = new Uri(host + eventItem.Slug).AbsoluteUri,
                    PublishDate = eventItem.UpdatedAt,
                    Author = new Author { Name = "Stockport Council", Email = email }
                };
                feed.Items.Add(item);
            }
        }

        feed.Items = feed.Items.OrderBy(p => p.PublishDate).Reverse().ToList();

        return feed.Serialize();
    }
}