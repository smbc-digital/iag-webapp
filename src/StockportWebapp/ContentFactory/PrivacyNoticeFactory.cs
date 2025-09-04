namespace StockportWebapp.ContentFactory;

public class PrivacyNoticeFactory(MarkdownWrapper markdownWrapper)
{
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedPrivacyNotice Build(PrivacyNotice privacyNotice) =>
        new(privacyNotice.Slug,
            privacyNotice.Title,
            privacyNotice.Category,
            _markdownWrapper.ConvertToHtml(privacyNotice.Purpose),
            _markdownWrapper.ConvertToHtml(privacyNotice.TypeOfData),
            _markdownWrapper.ConvertToHtml(privacyNotice.Legislation),
            _markdownWrapper.ConvertToHtml(privacyNotice.Obtained),
            _markdownWrapper.ConvertToHtml(privacyNotice.ExternallyShared),
            _markdownWrapper.ConvertToHtml(privacyNotice.RetentionPeriod),
            privacyNotice.OutsideEu,
            privacyNotice.AutomatedDecision,
            privacyNotice.Breadcrumbs,
            privacyNotice.ParentTopic);
}