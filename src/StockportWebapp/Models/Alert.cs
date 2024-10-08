﻿namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Alert
{
    public string Title { get; }
    public string SubHeading { get; }
    public string Body { get; }
    public string Severity { get; }
    public DateTime SunriseDate { get; }
    public DateTime SunsetDate { get; }
    public string Slug { get; }
    public bool IsStatic { get; }
    public string ImageUrl { get; }

    public Alert(string title, string subHeading, string body, string severity, DateTime sunriseDate, DateTime sunsetDate, string slug, bool isStatic, string imageUrl) 
    {
        Title = title;
        SubHeading = subHeading;
        Body = MarkdownWrapper.ToHtml(body);
        Severity = severity;
        SunriseDate = sunriseDate;
        SunsetDate = sunsetDate;
        Slug = slug;
        IsStatic = isStatic;
        ImageUrl = imageUrl;
    }
}

[ExcludeFromCodeCoverage]
public static class Severity
{
    public const string Warning = "Warning";
    public const string Error = "Error";
    public const string Information = "Information";
    public const string Success = "Success";
    public const string Condolence = "Condolence";
}