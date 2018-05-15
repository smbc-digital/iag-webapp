using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using StockportWebapp.Repositories;

namespace StockportWebapp.Parsers
{
    public class PrivacyNoticeTagParser : IDynamicTagParser<PrivacyNotice>
    {
        private readonly IViewRender _viewRenderer;
        private readonly ILogger<PrivacyNotice> _logger;

        public PrivacyNoticeTagParser(IViewRender viewRenderer, ILogger<PrivacyNotice> logger)
        {
            _viewRenderer = viewRenderer;
            _logger = logger;
        }

        protected Regex TagRegex => new Regex("{{PrivacyNotice:(.*?)}}", RegexOptions.Compiled);

        public string Parse(string content, IEnumerable<PrivacyNotice> PrivacyNotices)
        {
            var matches = TagRegex.Matches(content);

            foreach (Match match in matches)
            {
                var tagDataIndex1 = 1;

                var privacyNoticeSlug1 = match.Groups[tagDataIndex1].Value;

                var privacyNotices = PrivacyNotices.Where(s => s.Title.Replace(" ", "") == privacyNoticeSlug1);
                if (privacyNotices != null)
                {
                    var privacyNoticeHtml = _viewRenderer.Render("PrivacyNotice", privacyNotices);
                    content = TagRegex.Replace(content, privacyNoticeHtml, 1);
                }
                else
                {
              //      _logger.LogWarning($"The Alerts Title {AlertsInlineTitle} could not be found and will be removed");
                }
            }
            return RemoveEmptyTags(content);
        }

        private string RemoveEmptyTags(string content)
        {
            return TagRegex.Replace(content, string.Empty);
        }
    }
}