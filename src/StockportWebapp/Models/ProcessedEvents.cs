using System;
using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class ProcessedEvents : IProcessedContentType
    {
        public string Title { get; }
        public string Slug { get; }
        public string Teaser { get; }
        public string Image { get; }
        public string ThumbnailImage { get; }
        public string Description { get; set; }
        public string Fee { get; }
        public string Location { get; }
        public string SubmittedBy { get; }
        public string Longitude { get; }
        public string Latitude { get; }
        public bool Featured { get; }
        public DateTime EventDate { get; }
        public string StartTime { get; }
        public string EndTime { get; }
        public readonly List<Crumb> Breadcrumbs;

        public ProcessedEvents(string title, string slug, string teaser, string image, string thumbnailImage, string description, string fee, string location,
            string submittedBy, string longitude, string latitude, bool featured, DateTime eventDate, string startTime, string endTime, List<Crumb> breadcrumbs)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            Image = image;
            Description = description;
            ThumbnailImage = thumbnailImage;
            Fee = fee;
            Location = location;
            SubmittedBy = submittedBy;
            Longitude = longitude;
            Latitude = latitude;
            Featured = featured;
            EventDate = eventDate;
            StartTime = startTime;
            EndTime = endTime;
            Breadcrumbs = breadcrumbs;
        }
    }
}