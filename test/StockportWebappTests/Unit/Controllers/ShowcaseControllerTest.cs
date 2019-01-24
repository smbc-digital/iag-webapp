using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Net;
using StockportWebapp.Config;
using StockportWebapp.ProcessedModels;
using System;
using System.Threading.Tasks;
using StockportWebapp.Repositories;
using StockportWebapp.FeatureToggling;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class ShowcaseControllerTest
    {
        private readonly ShowcaseController _controller;
        private readonly Mock<FeatureToggles> _mockFeatureToggles = new Mock<FeatureToggles>();
        private readonly Mock<IProcessedContentRepository> _mockContentRepository = new Mock<IProcessedContentRepository>();

        public ShowcaseControllerTest()
        {
            _controller = new ShowcaseController(_mockContentRepository.Object, new Mock<ILogger<ShowcaseController>>().Object, new Mock<IApplicationConfiguration>().Object, _mockFeatureToggles.Object);
        }

        [Fact]
        public async Task ItReturnsShowcaseWithProcessedBody()
        {
            const string showcaseSlug = "showcase-slug";
            var alerts = new List<Alert> {new Alert("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty)};
            var showcase = new ProcessedShowcase("Test showcase",
                showcaseSlug,
                "showcase teaser",
                "showcase subheading",
                "event category",
                "events Category Or Tag",
                "event subheading",
                "news subheading",
                "news category",
                "news type",
                "body subheading",
                "body",
                null,
                "af981b9771822643da7a03a9ae95886f/picture.jpg",
                new List<SubItem>
                {
                    new SubItem("slug",
                        "title",
                        "teaser",
                        "icon",
                        "type",
                        "image.jpg",
                        new List<SubItem>())
                },
                new List<Crumb>
                {
                    new Crumb("title", "slug", "type")
                },
                new List<Consultation>(),
                new List<SocialMediaLink>(),
                new List<Event>(),
                "",
                "",
                alerts,
                new List<SubItem>(),
                null,
                null,
                null,
                new CallToActionBanner(),
                new FieldOrder(),
                "Key Fact Subheading",
                "fa-icon",
                "",
                null,
                "",
                "",
                "");

            _mockContentRepository
                .Setup(_ => _.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse(200, showcase, string.Empty));

            var showcasePage = await _controller.Showcase(showcaseSlug) as ViewResult;;
            var processedShowcase = showcasePage.Model as ProcessedShowcase;

            processedShowcase.Title.Should().Be("Test showcase");
            processedShowcase.Slug.Should().Be("showcase-slug");
            processedShowcase.Teaser.Should().Be("showcase teaser");
            processedShowcase.Subheading.Should().Be("showcase subheading");
            processedShowcase.EventCategory.Should().Be("event category");
            processedShowcase.EventSubheading.Should().Be("event subheading");
            processedShowcase.Breadcrumbs.Count().Should().Be(1);
            processedShowcase.SecondaryItems.Count().Should().Be(1);
            processedShowcase.Alerts.Count().Should().Be(1);
            processedShowcase.KeyFactSubheading.Should().Be("Key Fact Subheading");
            processedShowcase.Icon.Should().Be("fa-icon");
        }

        [Fact]
        public async Task Returns404WhenShowcaseNotFound()
        {
            _mockContentRepository
                .Setup(_ => _.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = await _controller.Showcase("not-found-slug") as HttpResponse;;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ReturnsEmptyListIfAllTopicsExpired()
        {
            _mockContentRepository
                .Setup(_ => _.Get<Showcase>(It.IsAny<string>(), It.IsAny<List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = await _controller.Showcase("not-found-slug") as HttpResponse;;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}