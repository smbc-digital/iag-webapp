using System.Threading.Tasks;
using StockportWebapp.ContentFactory.Trivia;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.Repositories.Responses;
using StockportWebapp.Services.Profile.Entities;
using StockportWebapp.Utils;

namespace StockportWebapp.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository _repository;
        private readonly ISimpleTagParserContainer _parser;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
        private readonly IDynamicTagParser<InlineQuote> _inlineQuotesTagParser;
        private readonly ITriviaFactory _triviaFactory;

        public ProfileService(
            IRepository repository, 
            ISimpleTagParserContainer parser, 
            MarkdownWrapper markdownWrapper, 
            IDynamicTagParser<Alert> alertsInlineTagParser,
            ITriviaFactory triviaFactory,
            IDynamicTagParser<InlineQuote> inlineQuotesTagParser)
        {
            _repository = repository;
            _parser = parser;
            _markdownWrapper = markdownWrapper;
            _alertsInlineTagParser = alertsInlineTagParser;
            _triviaFactory = triviaFactory;
            _inlineQuotesTagParser = inlineQuotesTagParser;
        }

        public async Task<ProfileEntity> GetProfile(string slug)
        {
            var response = await _repository.Get<ProfileResponse>(slug);

            if (response.StatusCode == 200)
            {
                var profile = response.Content as ProfileResponse;
                var triviaSection = _triviaFactory.Build(profile.TriviaSection);

                var processedBody = _parser.ParseAll(profile.Body, profile.Title, false);
                processedBody = _markdownWrapper.ConvertToHtml(processedBody);
                processedBody = _alertsInlineTagParser.Parse(processedBody, profile.Alerts);
                processedBody = _inlineQuotesTagParser.Parse(processedBody, profile.InlineQuotes);

                return new ProfileEntity
                {
                    Title = profile.Title,
                    Slug = profile.Slug,
                    Teaser = profile.Teaser,
                    Quote = profile.Quote,
                    Image = profile.Image,
                    Body = processedBody,
                    Breadcrumbs = profile.Breadcrumbs,
                    Alerts = profile.Alerts,
                    TriviaSubheading = profile.TriviaSubheading,
                    TriviaSection = triviaSection,
                    Subtitle = profile.Subtitle,
                    InlineQuotes = profile.InlineQuotes,
                    EventsBanner = profile.EventsBanner
                };
            }

            return null;
        }
    }
}