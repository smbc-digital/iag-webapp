using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Parsers
{
    public class S3BucketSearchTagParser : IDynamicTagParser<S3BucketSearch>
    {
        private readonly IViewRender _viewRenderer;
        private readonly ILogger<S3BucketSearch> _logger;

        public S3BucketSearchTagParser(IViewRender viewRenderer, ILogger<S3BucketSearch> logger)
        {
            _viewRenderer = viewRenderer;
            _logger = logger;
        }

        protected Regex TagRegex => new Regex("{{(Search:((.*?)\\/)*)}}", RegexOptions.Compiled);

        public string Parse(string content, IEnumerable<S3BucketSearch> searches)
        {
            var matches = TagRegex.Matches(content);

            foreach (Match match in matches)
            {
                var search = searches.FirstOrDefault();

                if (search != null)
                {
                    search.SearchFolder = match.Groups[0].ToString().Replace("{{Search:", "").Replace("}}", "");
                    var searchHtml = _viewRenderer.Render("S3Bucket", search);
                    content = TagRegex.Replace(content, searchHtml, 1);
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
