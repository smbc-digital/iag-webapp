namespace StockportWebapp.Models;

public class Event
{
    public string Title { get; set; }
    public string Slug { get; set; }

    public string MetaDescription { get; set; }
    public string Teaser { get; set; }
    public string ImageUrl { get; set; }
    public string ThumbnailImageUrl { get; set; }
    public string Description { get; set; }
    public string Fee { get; set; }
    public string Location { get; set; }
    public string SubmittedBy { get; set; }
    public DateTime EventDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public List<Crumb> Breadcrumbs { get; set; }
    public List<Document> Documents { get; set; }
    public List<string> Categories { get; set; }
    public MapPosition MapPosition { get; set; }
    public bool Featured { get; set; }
    public string BookingInformation { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<string> Tags { get; set; }
    public Group Group { get; set; }
    public List<Alert> Alerts { get; set; }
    public EventFrequency EventFrequency { get; set; }
    public int Occurences { get; set; }
    public List<EventCategory> EventCategories { get; set; }
    public string AccessibleTransportLink { get; set; }
}
