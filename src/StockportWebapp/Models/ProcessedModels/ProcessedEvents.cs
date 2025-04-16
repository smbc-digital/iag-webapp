namespace StockportWebapp.Models.ProcessedModels;
public class ProcessedEvents(string title,
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
                            MapDetails mapDetails,
                            string bookingInformation,
                            Group group,
                            List<Alert> alerts,
                            string accessibleTransportLink,
                            string logoAreaTitle,
                            List<GroupBranding> eventBranding,
                            string phoneNumber,
                            string email,
                            string website,
                            string facebook,
                            string instagram,
                            string linkedIn,
                            string metaDescription,
                            string duration,
                            string languages,
                            List<ProcessedEvents> relatedEvents,
                            IEnumerable<CallToActionBanner> callToActionBanners) : IProcessedContentType
{
    public string Title { get; } = title;
    public string Slug { get; } = slug;
    public string Teaser { get; } = teaser;
    public string ImageUrl { get; } = imageUrl;
    public string ThumbnailImageUrl { get; } = thumbnailImageUrl;
    public string Description { get; set; } = description;
    public string Fee { get; } = fee;
    public bool? Free { get; } = free;
    public string Location { get; } = location;
    public string SubmittedBy { get; } = submittedBy;
    public DateTime EventDate { get; } = eventDate;
    public string StartTime { get; } = startTime;
    public string EndTime { get; } = endTime;
    public List<Crumb> Breadcrumbs { get; } = breadcrumbs;
    public string BookingInformation { get; set; } = bookingInformation;
    public readonly List<Alert> Alerts = alerts;
    public readonly List<Alert> GlobalAlerts = new();
    public MapDetails MapDetails { get; set; } = mapDetails;
    public string LogoAreaTitle { get; } = logoAreaTitle;
    public List<GroupBranding> EventBranding { get; set; } = eventBranding;
    public string PhoneNumber { get; } = phoneNumber;
    public string Email { get; } = email;
    public string Website { get; } = website;
    public string Facebook { get; } = facebook;
    public string Instagram { get; } = instagram;
    public string LinkedIn { get; } = linkedIn;
    public string MetaDescription { get; set; } = metaDescription;
    public Group Group { get; set; } = group;
    public string Duration { get; } = duration;
    public string Languages { get; } = languages;
    public List<ProcessedEvents> RelatedEvents { get; set; } = relatedEvents;
    public IEnumerable<CallToActionBanner> CallToActionBanners{ get; set; } = callToActionBanners;

    public bool IsAlertDisplayed(Alert alert)
        => alert.SunriseDate <= EventDate && alert.SunsetDate >= EventDate;
}