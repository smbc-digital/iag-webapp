using StockportWebapp.Models;

namespace StockportWebappTests_Unit.Builders
{
    internal class EventBuilder
    {
        private const string Title = "title";
        private const string Slug = "slug";
        private const string Teaser = "teaser";
        private const string ImageUrl = "url";
        private const string ThumbnailImageUrl = "url";
        private const string Description = "description";
        private const string Fee = "fee";
        private const string Location = "location";
        private const string SubmittedBy = "person";
        private readonly DateTime _eventDate = DateTime.MaxValue;
        private const string StartTime = "00:00";
        private const string EndTime = "12:00";
        private readonly List<Crumb> _breadcrumbs = new List<Crumb>();
        private readonly List<Document> _documents = new List<Document>();
        private readonly List<string> _categories = new List<string>();
        private readonly MapPosition _mapPosition = new MapPosition();
        private const bool Featured = false;
        private const string BookingInformation = "info";
        private readonly DateTime _updatedAt = DateTime.MinValue;
        private readonly List<string> _tags = new List<string>();
        private readonly Group _group = new GroupBuilder().Build();
        private readonly List<Alert> _alerts = new List<Alert>();
        private const EventFrequency EventFrequency = StockportWebapp.Models.EventFrequency.None;
        private const int Occurences = 0;
        private readonly List<EventCategory> _eventCategories = new List<EventCategory>();
        private const string AccessibleTransportLink = "link";

        public Event Build()
        {
            return new Event
            {
                Title = Title,
                Slug = Slug,
                Teaser = Teaser,
                ImageUrl = ImageUrl,
                ThumbnailImageUrl = ThumbnailImageUrl,
                Description = Description,
                Fee = Fee,
                Location = Location,
                SubmittedBy = SubmittedBy,
                EventDate = _eventDate,
                StartTime = StartTime,
                EndTime = EndTime,
                Breadcrumbs = _breadcrumbs,
                Documents = _documents,
                Categories = _categories,
                MapPosition = _mapPosition,
                Featured = Featured,
                BookingInformation = BookingInformation,
                UpdatedAt = _updatedAt,
                Tags = _tags,
                Group = _group,
                Alerts = _alerts,
                EventFrequency = EventFrequency,
                Occurences = Occurences,
                EventCategories = _eventCategories,
                AccessibleTransportLink = AccessibleTransportLink
            };
        }
    }
}
