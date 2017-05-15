using System;
using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedNews : IProcessedContentType
    {
        public readonly string Title;
        public readonly string Slug;
        public readonly string Teaser;
        public readonly string Image;
        public readonly string ThumbnailImage;
        public readonly string Body;
        public readonly List<Crumb> Breadcrumbs;
        public readonly DateTime SunriseDate;
        public readonly DateTime SunsetDate;
        public readonly List<Alert> Alerts;
        public readonly List<string> Tags;

        public ProcessedNews(string title, string slug, string teaser, string image, string thumbnailImage, string body, List<Crumb> breadcrumbs, DateTime sunriseDate, DateTime sunsetDate, List<Alert> alerts, List<string> tags )
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Image = image;
            ThumbnailImage = thumbnailImage;
            Body = body;
            Breadcrumbs = breadcrumbs;
            SunriseDate = sunriseDate;
            SunsetDate = sunsetDate;
            Alerts = alerts;
            Tags = tags;
        }
    }
}
