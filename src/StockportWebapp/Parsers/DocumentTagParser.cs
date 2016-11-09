using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using System.Linq;

namespace StockportWebapp.Parsers
{
    public class DocumentTagParser : IDynamicTagParser<Document>
    {
        private readonly IViewRender _viewRenderer;
        private readonly ILogger<DocumentTagParser> _logger;

        public DocumentTagParser(IViewRender viewRenderer, ILogger<DocumentTagParser> logger)
        {
            _viewRenderer = viewRenderer;
            _logger = logger;
        }

        protected Regex TagRegex => new Regex("{{PDF:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

        public string Parse(string content, IEnumerable<Document> documents)
        {
            var matches = TagRegex.Matches(content);

            foreach (Match match in matches)
            {
                var tagDataIndex = 1;
                var fileName = match.Groups[tagDataIndex].Value;
                var document = GetDocumentMatchingFilename(documents, fileName);
                if (document != null)
                {
                    var documentHtml = _viewRenderer.Render("Document", document);
                    content = TagRegex.Replace(content, documentHtml, 1);
                }
                else
                {
                    _logger.LogWarning($"The document {fileName} could not be found and will be removed");
                }
            }
            return RemoveEmptyTags(content);
        }

        private string RemoveEmptyTags(string content)
        {
            return TagRegex.Replace(content, string.Empty);
        }

        private static Document GetDocumentMatchingFilename(IEnumerable<Document> documents, string fileName)
        {
            return documents.FirstOrDefault(s => s.FileName == fileName);
        }
    }
}