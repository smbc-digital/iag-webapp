using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebappTests.Unit.Fake;
using Xunit;
using System.Net;
using StockportWebapp.ProcessedModels;

namespace StockportWebappTests.Unit.Controllers
{
    public class ShowcaseControllerTest
    {
        private readonly ShowcaseController _controller;
        private readonly FakeProcessedContentRepository _fakeRepository;

        public ShowcaseControllerTest()
        {
            _fakeRepository = new FakeProcessedContentRepository();
            _controller = new ShowcaseController(_fakeRepository, new Mock<ILogger<ShowcaseController>>().Object);
        }

        [Fact]
        public void ItReturnsShowcaseWithProcessedBody()
        {
            const string showcaseSlug = "showcase-slug";
            var showcase = new ProcessedShowcase("Test showcase", showcaseSlug, "showcase teaser",
                                                 "showcase subheading", "event category", "event subheading", "news subheading", "news category", "news type", null, "af981b9771822643da7a03a9ae95886f/picture.jpg",
                                                 new List<SubItem> { new SubItem("slug", "title", "teaser", "icon", "type", "image.jpg", new List<SubItem>()) }, new List<Crumb> { new Crumb("title", "slug", "type") }, new List<Consultation>(), new List<SocialMediaLink>(), new List<Event>());

            _fakeRepository.Set(new HttpResponse(200, showcase, string.Empty));

            var showcasePage = AsyncTestHelper.Resolve(_controller.Showcase(showcaseSlug)) as ViewResult;
            var processedShowcase = showcasePage.Model as ProcessedShowcase;

            processedShowcase.Title.Should().Be("Test showcase");
            processedShowcase.Slug.Should().Be("showcase-slug");
            processedShowcase.Teaser.Should().Be("showcase teaser");
            processedShowcase.Subheading.Should().Be("showcase subheading");
            processedShowcase.EventCategory.Should().Be("event category");
            processedShowcase.EventSubheading.Should().Be("event subheading");
            processedShowcase.Breadcrumbs.Count().Should().Be(1);
            processedShowcase.FeaturedItems.Count().Should().Be(1);
        }

        [Fact]
        public void Returns404WhenShowcaseNotFound()
        {
            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = AsyncTestHelper.Resolve(_controller.Showcase("not-found-slug")) as HttpResponse;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public void ReturnsEmptyListIfAllTopicsExpired()
        {
            _fakeRepository.Set(new HttpResponse((int)HttpStatusCode.NotFound, null, string.Empty));

            var response = AsyncTestHelper.Resolve(_controller.Showcase("not-found-slug")) as HttpResponse;

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}