using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ProfileFactory
    {
        private readonly ISimpleTagParserContainer _parser;
        private readonly MarkdownWrapper _markdownWrapper;

        public ProfileFactory(ISimpleTagParserContainer parser, MarkdownWrapper markdownWrapper)
        {
            _parser = parser;
            _markdownWrapper = markdownWrapper;
        }

        public virtual ProcessedProfile Build(Profile profile)
        {
            var htmlBody = _markdownWrapper.ConvertToHtml(profile.Body);
            var processedBody = _parser.ParseAll(htmlBody);

            return new ProcessedProfile(profile.Type, profile.Title, profile.Slug, profile.Subtitle, profile.Teaser,
                profile.Image, processedBody, profile.BackgroundImage, profile.Icon, profile.Breadcrumbs);
        }
    }
}