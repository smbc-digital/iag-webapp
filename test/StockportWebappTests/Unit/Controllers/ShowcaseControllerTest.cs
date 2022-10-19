using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class ShowcaseControllerTest
    {
        private readonly ShowcaseController _controller;

        private readonly Mock<IProcessedContentRepository> _mockContentRepository = new Mock<IProcessedContentRepository>();

        public ShowcaseControllerTest()
        {
            _controller = new ShowcaseController(_mockContentRepository.Object, new Mock<IApplicationConfiguration>().Object);
        }

        [Fact]
        public async Task ItReturnsShowcaseWithProcessedBody()
        {
            const string showcaseSlug = "showcase-slug";
            var alerts = new List<Alert> {new Alert("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                                 new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false)};
            var showcase = new ProcessedShowcase(
                "Test showcase",
                showcaseSlug,
                "showcase teaser",
                "showcase metaDescription",
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
                "",
                new List<SocialMediaLink>(),
                new List<Event>(),
                "email alerts topic id",
                "emailAlertsText",
                alerts,
                new List<SubItem>(),
                "",
                new List<SubItem>(),
                null,
                null,
                new CallToActionBanner(),
                new FieldOrder(),
                "fa-icon",
                "",
                null,
                "",
                "",
                "",
                new Video(),
                new SpotlightBanner("test", "test", "test"));

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