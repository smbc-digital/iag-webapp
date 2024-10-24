﻿namespace StockportWebapp.Models.ProcessedModels;
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
    public string BookingInformation { get; set; }
    public readonly List<Alert> Alerts;
    public readonly List<Alert> GlobalAlerts = new List<Alert>();
    public MapDetails MapDetails { get; set; }
    public List<GroupBranding> TrustedLogos { get; set; } = new List<GroupBranding>();
    public string PhoneNumber { get; }
    public string Email { get; }
    public string Website { get; }

    public string MetaDescription { get; set; }


    public Group Group { get; set; }

    public ProcessedEvents(string title, string slug, string teaser, string imageUrl, string thumbnailImageUrl, string description,
                            string fee, string location, string submittedBy, DateTime eventDate, string startTime, string endTime,
                            List<Crumb> breadcrumbs, List<string> categories, MapDetails mapDetails, string bookingInformation, Group group, List<Alert> alerts, string accessibleTransportLink, List<GroupBranding> trustedLogos, string phoneNumber, string email, string website, string metaDescription)
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
        BookingInformation = bookingInformation;
        Group = group;
        Alerts = alerts;
        MapDetails = mapDetails;
        TrustedLogos = trustedLogos;
        PhoneNumber = phoneNumber;
        Email = email;
        Website = website;
        MetaDescription = metaDescription;
    }

    public bool IsAlertDisplayed(Alert alert)
        => alert.SunriseDate <= EventDate && alert.SunsetDate >= EventDate;
}