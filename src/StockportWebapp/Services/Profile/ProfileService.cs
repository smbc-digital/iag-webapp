using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.Repositories.Responses;
using StockportWebapp.Services.Profile;
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

        public ProfileService(IRepository repository, ISimpleTagParserContainer parser, MarkdownWrapper markdownWrapper, IDynamicTagParser<Alert> alertsInlineTagParser)
        {
            _repository = repository;
            _parser = parser;
            _markdownWrapper = markdownWrapper;
            _alertsInlineTagParser = alertsInlineTagParser;
        }

        public async Task<ProfileEntity> GetProfile(string slug)
        {
            var response = await _repository.Get<ProfileResponse>(slug);

            if (response.StatusCode == 200)
            {
                var profile = response.Content as ProfileResponse;

                var processedBody = _parser.ParseAll(profile.Body, profile.Title);
                processedBody = _markdownWrapper.ConvertToHtml(processedBody);
                processedBody = _alertsInlineTagParser.Parse(processedBody, profile.Alerts);

                return new ProfileEntity
                {
                    Title = profile.Title,
                    Slug = profile.Slug,
                    LeadParagraph = profile.LeadParagraph,
                    Teaser = profile.Teaser,
                    Image = profile.Image,
                    Body = processedBody,
                    Breadcrumbs = profile.Breadcrumbs,
                    Alerts = profile.Alerts,
                    DidYouKnowSection = profile.DidYouKnowSection,
                    FieldOrder = profile.FieldOrder
                };
            }

            return null;
        }
    }
}