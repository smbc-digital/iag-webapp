﻿namespace StockportWebapp.Models.ProcessedModels;
[ExcludeFromCodeCoverage]
public class ProcessedPrivacyNotice : IProcessedContentType
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public string Purpose { get; set; }
    public string TypeOfData { get; set; }
    public string Legislation { get; set; }
    public string Obtained { get; set; }
    public string ExternallyShared { get; set; }
    public string RetentionPeriod { get; set; }
    public bool OutsideEu { get; set; }
    public bool AutomatedDecision { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public Topic ParentTopic { get; set; }
    public readonly string NavigationLink;

    public ProcessedPrivacyNotice() { }

    public ProcessedPrivacyNotice(string slug, string title, string category, string purpose, string typeOfData, string legislation, string obtained, string externallyShared, string retentionPeriod, bool outsideEu, bool automatedDecision, IEnumerable<Crumb> breadcrumbs, Topic parentTopic)
    {
        Slug = slug;
        Title = title;
        Category = category;
        Purpose = purpose;
        TypeOfData = typeOfData;
        Legislation = legislation;
        Obtained = obtained;
        ExternallyShared = externallyShared;
        RetentionPeriod = retentionPeriod;
        OutsideEu = outsideEu;
        AutomatedDecision = automatedDecision;
        Breadcrumbs = breadcrumbs;
        ParentTopic = parentTopic;
        NavigationLink = TypeRoutes.GetUrlFor("privacy-notice", slug);
    }
}