using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class PrivacyNoticeFactory
    {
        private readonly MarkdownWrapper _markdownWrapper;

        public PrivacyNoticeFactory(MarkdownWrapper markdownWrapper)
        {
            _markdownWrapper = markdownWrapper;
        }

        public virtual ProcessedPrivacyNotice Build(PrivacyNotice privacyNotice)
        {
            var typeOfDataHtml = _markdownWrapper.ConvertToHtml(privacyNotice.TypeOfData); 
            var purposeHtml = _markdownWrapper.ConvertToHtml(privacyNotice.Purpose);
            var externallySharedHtml = _markdownWrapper.ConvertToHtml(privacyNotice.ExternallyShared);
            var obtainedHtml = _markdownWrapper.ConvertToHtml(privacyNotice.Obtained);
            var retentionPeriodHtml = _markdownWrapper.ConvertToHtml(privacyNotice.RetentionPeriod);
            var legistationHtml = _markdownWrapper.ConvertToHtml(privacyNotice.Legislation);

            var processedPrivacyNotice = new ProcessedPrivacyNotice(privacyNotice.Slug, privacyNotice.Title, privacyNotice.Category, purposeHtml, typeOfDataHtml, legistationHtml, obtainedHtml, externallySharedHtml, retentionPeriodHtml, privacyNotice.OutsideEu, privacyNotice.AutomatedDecision, privacyNotice.UrlOne, privacyNotice.UrlTwo, privacyNotice.UrlThree, privacyNotice.Breadcrumbs, privacyNotice.ParentTopic);

            return processedPrivacyNotice;
        }
    }
}
