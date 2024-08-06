namespace StockportWebapp.ContentFactory;

public class PrivacyNoticeFactory
{
    private readonly MarkdownWrapper _markdownWrapper;

    public PrivacyNoticeFactory(MarkdownWrapper markdownWrapper) 
        => _markdownWrapper = markdownWrapper;

    public virtual ProcessedPrivacyNotice Build(PrivacyNotice privacyNotice)
    {
        string typeOfDataHtml = _markdownWrapper.ConvertToHtml(privacyNotice.TypeOfData);
        string purposeHtml = _markdownWrapper.ConvertToHtml(privacyNotice.Purpose);
        string externallySharedHtml = _markdownWrapper.ConvertToHtml(privacyNotice.ExternallyShared);
        string obtainedHtml = _markdownWrapper.ConvertToHtml(privacyNotice.Obtained);
        string retentionPeriodHtml = _markdownWrapper.ConvertToHtml(privacyNotice.RetentionPeriod);
        string legistationHtml = _markdownWrapper.ConvertToHtml(privacyNotice.Legislation);

        ProcessedPrivacyNotice processedPrivacyNotice = new(privacyNotice.Slug, privacyNotice.Title, privacyNotice.Category, purposeHtml, typeOfDataHtml, legistationHtml, obtainedHtml, externallySharedHtml, retentionPeriodHtml, privacyNotice.OutsideEu, privacyNotice.AutomatedDecision, privacyNotice.Breadcrumbs, privacyNotice.ParentTopic);

        return processedPrivacyNotice;
    }
}