using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.ContentFactory;
using StockportWebapp.Http;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.Services.Showcase.Entities;

namespace StockportWebapp.Services.Showcase
{
    public class ShowcaseService : IShowcaseService
    {
        private readonly IProcessedContentRepository _repository;
        private readonly ContentTypeFactory _factory;

        public ShowcaseService(IProcessedContentRepository repository, ContentTypeFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public async Task<ShowcaseEntity> GetShowcase(string slug)
        {
            var response = await _repository.Get<Models.Showcase>(slug);

            if (response.IsSuccessful())
            {
                var showcase = response.Content as Models.Showcase;

                var showcaseEntity = new ShowcaseEntity
                {
                    Title = showcase.Title,
                    Slug = showcase.Slug,
                    Teaser = showcase.Teaser,
                    Subheading = showcase.Subheading,
                    EventCategory = showcase.EventCategory,
                    EventSubheading = showcase.EventSubheading,
                    HeroImageUrl = showcase.HeroImageUrl,
                    Breadcrumbs = showcase.Breadcrumbs,
                    SecondaryItems = showcase.SecondaryItems,
                    Consultations = showcase.Consultations,
                    SocialMediaLinks = showcase.SocialMediaLinks,
                    Events = showcase.Events,
                    NewsSubheading = showcase.NewsSubheading,
                    NewsCategoryTag = showcase.NewsCategoryTag,
                    NewsCategoryOrTag = showcase.NewsCategoryOrTag,
                    BodySubheading = showcase.BodySubheading,
                    Body = showcase.Body,
                    NewsArticle = showcase.NewsArticle,
                    EventsCategoryOrTag = showcase.EventsCategoryOrTag,
                    EmailAlertsTopicId = showcase.EmailAlertsTopicId,
                    EmailAlertsText = showcase.EmailAlertsText,
                    Alerts = showcase.Alerts,
                    KeyFacts = showcase.KeyFacts,
                    PrimaryItems = showcase.PrimaryItems,
                    Profile = showcase.Profile,
                    Profiles = showcase.Profiles,
                    FieldOrder = showcase.FieldOrder,
                    KeyFactSubheading = showcase.KeyFactSubheading,
                    Icon = showcase.Icon,
                    DidYouKnowSection = (List<ProcessedInformationItem>)_factory.Build(showcase.DidYouKnowSection),
                    KeyFactsSection = showcase.KeyFactsSection,
                    CallToActionBanner = showcase.CallToActionBanner
                };

                return showcaseEntity;
            }

            return null;
        }
    }
}
