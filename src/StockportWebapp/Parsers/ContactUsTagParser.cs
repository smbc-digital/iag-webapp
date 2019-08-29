using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using StockportWebapp.Utils;
using StockportWebapp.ViewDetails;

namespace StockportWebapp.Parsers
{
    public class ContactUsTagParser : ISimpleTagParser
    {
        private readonly IViewRender _viewRenderer;
        private readonly ILogger<ContactUsTagParser> _logger;
        private readonly TagReplacer _tagReplacer;
        private string _articleTitle;

        private const string UnableToRenderFormError = "<p>This contact form is temporarily unavailable. Please check back later.</p>";

        public ContactUsTagParser(IViewRender viewRenderer, ILogger<ContactUsTagParser> logger)
        {
            _viewRenderer = viewRenderer;
            _logger = logger;
            _tagReplacer = new TagReplacer(GenerateHtml, TagRegex);
        }

        protected Regex TagRegex => new Regex("{{CONTACT-US:(\\s*[a-zA-Z0-9]*[^}]*)}}", RegexOptions.Compiled);

        public static Regex ContactUsMessageTagRegex => new Regex("<!-- ##CONTACT_US_MESSAGE## -->", RegexOptions.Compiled);

        protected string GenerateHtml(string serviceEmailId)
        {
            if (string.IsNullOrEmpty(serviceEmailId))
            {
                _logger.LogError("The service email ID in this CONTACT-US tag is invalid and this contact form will not render.");
                return UnableToRenderFormError;
            }
            var renderResult = _viewRenderer.Render("ContactUs", new ContactUsDetails(serviceEmailId, _articleTitle));
            return string.Concat(ContactUsMessageTagRegex.ToString(), renderResult);
        }

        public string Parse(string body, string title)
        {
            _articleTitle = title;
            return _tagReplacer.ReplaceAllTags(body);
        }
    }
}