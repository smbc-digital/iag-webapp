namespace StockportWebapp.Models.ProcessedModels;
public class ProcessedEvents : IProcessedContentType
{
    public string Title { get; }
    public string Slug { get; }
    public string Teaser { get; }
    public string ImageUrl { get; }
    public string ThumbnailImageUrl { get; }
    public string Description { get; set; }
    public string Fee { get; }
    public bool? Free { get; }
    public string Location { get; }
    public string SubmittedBy { get; }
    public DateTime EventDate { get; }
    public string StartTime { get; }
    public string EndTime { get; }
    public List<Crumb> Breadcrumbs { get; }
    public List<string> Categories { get; }
    public string BookingInformation { get; set; }
    public readonly List<Alert> Alerts;
    public readonly List<Alert> GlobalAlerts = new();
    public MapDetails MapDetails { get; set; }
    public List<GroupBranding> EventBranding { get; set; } = new();
    public string PhoneNumber { get; }
    public string Email { get; }
    public string Website { get; }
    public string Facebook { get; }
    public string Instagram { get; }
    public string Linkedin { get; }
    public string MetaDescription { get; set; }
    public Group Group { get; set; }
    public string Duration { get; }
    public string Languages { get; }
    public List<ProcessedEvents> RelatedEvents { get; set; }

    public ProcessedEvents(string title,
                        string slug,
                        string teaser,
                        string imageUrl,
                        string thumbnailImageUrl,
                        string description,
                        string fee,
                        bool? free,
                        string location,
                        string submittedBy,
                        DateTime eventDate,
                        string startTime,
                        string endTime,
                        List<Crumb> breadcrumbs,
                        List<string> categories,
                        MapDetails mapDetails,
                        string bookingInformation,
                        Group group,
                        List<Alert> alerts,
                        string accessibleTransportLink,
                        List<GroupBranding> eventBranding,
                        string phoneNumber,
                        string email,
                        string website,
                        string facebook,
                        string instagram,
                        string linkedin,
                        string metaDescription,
                        string duration,
                        string languages,
                        List<ProcessedEvents> relatedEvents)
    {
        Title = title;
        Slug = slug;
        Teaser = teaser;
        ImageUrl = imageUrl;
        Description = description;
        ThumbnailImageUrl = thumbnailImageUrl;
        Fee = fee;
        Free = free;
        Location = location;
        SubmittedBy = submittedBy;
        EventDate = eventDate;
        StartTime = startTime;
        EndTime = endTime;
        Breadcrumbs = breadcrumbs;
        Categories = categories;
        BookingInformation = bookingInformation;
        Group = group;
        Alerts = alerts;
        MapDetails = mapDetails;
        EventBranding = eventBranding;
        PhoneNumber = phoneNumber;
        Email = email;
        Website = website;
        Facebook = facebook;
        Instagram = instagram;
        Linkedin = linkedin;
        MetaDescription = metaDescription;
        Duration = duration;
        Languages = languages;
        RelatedEvents = relatedEvents;
    }

    public bool IsAlertDisplayed(Alert alert)
        => alert.SunriseDate <= EventDate && alert.SunsetDate >= EventDate;
}