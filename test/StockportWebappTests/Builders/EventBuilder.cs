using StockportWebapp.Models;
using System;
using System.Collections.Generic;

namespace StockportWebappTests_Unit.Builders
{
    internal class EventBuilder
    {
        private string _title = "title";
        private string _slug = "slug";
        private string _teaser = "teaser";
        private string _imageUrl = "url";
        private string _thumbnailImageUrl = "url";
        private string _description = "description";
        private string _fee = "fee";
        private string _location = "location";
        private string _submittedBy = "person";
        private DateTime _eventDate = DateTime.MaxValue;
        private string _startTime = "00:00";
        private string _endTime = "12:00";
        private List<Crumb> _breadcrumbs = new List<Crumb>();
        private List<Document> _documents = new List<Document>();
        private List<string> _categories = new List<string>();
        private MapPosition _mapPosition = new MapPosition();
        private bool _featured = false;
        private string _bookingInformation = "info";
        private DateTime _updatedAt = DateTime.MinValue;
        private List<string> _tags = new List<string>();
        private Group _group = new GroupBuilder().Build();
        private List<Alert> _alerts = new List<Alert>();
        private EventFrequency _eventFrequency = EventFrequency.None;
        private int _occurences = 0;
        private List<EventCategory> _eventCategories = new List<EventCategory>();
        private string _accessibleTransportLink = "link";

        public Event Build()
        {
            return new Event
            {
                Title = _title,
                Slug = _slug,
                Teaser = _teaser,
                ImageUrl = _imageUrl,
                ThumbnailImageUrl = _thumbnailImageUrl,
                Description = _description,
                Fee = _fee,
                Location = _location,
                SubmittedBy = _submittedBy,
                EventDate = _eventDate,
                StartTime = _startTime,
                EndTime = _endTime,
                Breadcrumbs = _breadcrumbs,
                Documents = _documents,
                Categories = _categories,
                MapPosition = _mapPosition,
                Featured = _featured,
                BookingInformation = _bookingInformation,
                UpdatedAt = _updatedAt,
                Tags = _tags,
                Group = _group,
                Alerts = _alerts,
                EventFrequency = _eventFrequency,
                Occurences = _occurences,
                EventCategories = _eventCategories,
                AccessibleTransportLink = _accessibleTransportLink
            };
        }
    }
}
