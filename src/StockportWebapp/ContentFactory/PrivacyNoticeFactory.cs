using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var legislationHtml = _markdownWrapper.ConvertToHtml(privacyNotice.Legislation);
            var externallySharedHtml = _markdownWrapper.ConvertToHtml(privacyNotice.ExternallyShared);

            var processedPrivacyNotice = new ProcessedPrivacyNotice(privacyNotice.Slug, privacyNotice.Title, privacyNotice.Directorate, privacyNotice.ActivitiesAsset, privacyNotice.TransactionsActivity, privacyNotice.Purpose, typeOfDataHtml, legislationHtml, privacyNotice.Obtained, externallySharedHtml, privacyNotice.RetentionPeriod, privacyNotice.Conditions, privacyNotice.ConditionsSpecial, privacyNotice.UrlOne, privacyNotice.UrlTwo, privacyNotice.UrlThree);

            return processedPrivacyNotice;
        }
    }
}
