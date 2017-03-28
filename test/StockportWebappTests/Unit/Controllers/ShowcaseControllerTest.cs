using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Parsers;
using StockportWebapp.ViewModels;
using StockportWebappTests.Unit.Fake;
using Xunit;
using Helper = StockportWebappTests.TestHelper;
using static StockportWebapp.Models.LiveChat;
using System;
using System.Net;

namespace StockportWebappTests.Unit.Controllers
{
    public class ShowcaseControllerTest
    {
        private readonly ShowcaseController _controller;
        private readonly FakeProcessedContentRepository _fakeRepository;
        private readonly Mock<IContactUsMessageTagParser> _contactUsMessageParser;

        private const string DefaultMessage = "A default message";

        public ShowcaseControllerTest()
        {
            _fakeRepository = new FakeProcessedContentRepository();

            _contactUsMessageParser = new Mock<IContactUsMessageTagParser>();

            _controller = new ShowcaseController(_fakeRepository, new Mock<ILogger<ShowcaseController>>().Object, _contactUsMessageParser.Object);
        }

        [Fact]
        public void ItReturnsShowcaseWithProcessedBody()
        {
            const string ShowcaseSlug = "showcase-slug";
            var Showcase = new ProcessedShowcase("Test showcase", ShowcaseSlug, "showcase teaser",
                                                 "showcase subheading", "af981b9771822643da7a03a9ae95886f/picture.jpg",
                                                 null, new List<Crumb>() { new Crumb("title", "slug", "type") });

            _fakeRepository.Set(new HttpResponse(200, Showcase, string.Empty));

            var ShowcasePage = AsyncTestHelper.Resolve(_controller.Showcase(ShowcaseSlug)) as ViewResult;
            var processedShowcase = ShowcasePage.Model as ProcessedShowcase;

            processedShowcase.Title.Should().Be("Test showcase");
            processedShowcase.Slug.Should().Be("Test showcase");
            processedShowcase.Teaser.Should().Be("Test showcase");
            processedShowcase.Subheading.Should().Be("Test showcase");
            processedShowcase.Breadcrumbs.Count().Should().Be(1);
            processedShowcase.FeaturedItems.Count().Should().Be(0);
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