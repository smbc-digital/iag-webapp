using System;
using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Event
    {
        public string Title { get; }
        public string Slug { get; }
        public string Teaser { get; }
        public string ImageUrl { get; }
        public string ThumbnailImageUrl { get; }
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
        public List<Crumb> Breadcrumbs { get; }

        public Event(string title, string slug, string teaser, string imageUrl, string thumbnailImageUrl, string description, string fee, string location, 
            string submittedBy, string longitude, string latitude, bool featured, DateTime eventDate, string startTime, string endTime, List<Crumb> breadcrumbs)
        {
            Title = title;
            Slug = slug;
            Teaser = teaser;
            ImageUrl = imageUrl;
            Description = description;
            ThumbnailImageUrl = thumbnailImageUrl;
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

    public class NullEvent : Event
    {
        public NullEvent()
            : base(
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                false, new DateTime(), string.Empty, string.Empty, new List<Crumb>())
        { }
    }
}
