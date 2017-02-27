using System;
using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class ProcessedEvents : IProcessedContentType
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
        public DateTime EventDate { get; }
        public string StartTime { get; }
        public string EndTime { get; }
        public List<Crumb> Breadcrumbs { get; }
        public List<string> Categories { get; }
        public MapPosition MapPosition { get; }
        public string BookingInformation { get; set; }

        public Group Group { get; set; }

        public ProcessedEvents(string title, string slug, string teaser, string imageUrl, string thumbnailImageUrl, string description, 
                               string fee, string location,string submittedBy, DateTime eventDate, string startTime, string endTime, 
                               List<Crumb> breadcrumbs, List<string> categories, MapPosition mapPosition, string bookingInformation, Group group )
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
            EventDate = eventDate;
            StartTime = startTime;
            EndTime = endTime;
            Breadcrumbs = breadcrumbs;
            Categories = categories;
            MapPosition = mapPosition;
            BookingInformation = bookingInformation;
            Group = group;
        }
    }
}