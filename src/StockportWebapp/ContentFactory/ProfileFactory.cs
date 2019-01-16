using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ProfileFactory
    {
        private readonly ISimpleTagParserContainer _parser;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;

        public ProfileFactory(ISimpleTagParserContainer parser, MarkdownWrapper markdownWrapper, IDynamicTagParser<Alert> alertsInlineTagParser)
        {
            _parser = parser;
            _markdownWrapper = markdownWrapper;
            _alertsInlineTagParser = alertsInlineTagParser;
        }

        public virtual ProcessedProfile Build(Profile profile)
        {
            //var htmlBody = _markdownWrapper.ConvertToHtml(profile.Body);
            var processedBody = _parser.ParseAll(profile.Body, profile.Title);
            processedBody = _markdownWrapper.ConvertToHtml(processedBody);
            processedBody = _alertsInlineTagParser.Parse(processedBody, profile.Alerts);

            return new ProcessedProfile(profile.Type,
                profile.Title,
                profile.Slug,
                profile.Subtitle,
                profile.Teaser,
                profile.Quote,
                profile.Image,
                processedBody,
                profile.BackgroundImage,
                profile.Icon,
                profile.Breadcrumbs,
                profile.Alerts);
        }
    }
}