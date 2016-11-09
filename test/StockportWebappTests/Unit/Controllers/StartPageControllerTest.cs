using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebappTests.Unit.Fake;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;

namespace StockportWebappTests.Unit.Controllers
{
    public class StartPageControllerTest
    {
        private readonly StartPageController _controller;
        private readonly Mock<IRepository> _repository;

        public StartPageControllerTest()
        {
            // declarations
            _repository = new Mock<IRepository>();

            // data
            var alerts = new List<Alert> { new Alert("title", "subHeading", "body", "severity") };
            var startPage = new StartPage(
                "start-page",
                "Start Page",
                "This is a summary ",
                "An upper body",
                "Start now",
                "http://start.com",
                "Lower body",
                new List<Crumb>
                {
                    new Crumb("title", "slug", "type")
                },
                "image.jpg",
                "icon",
                alerts
            );

            // setup mocks
            _repository.Setup(o => o.Get<StartPage>("start-page", null)).ReturnsAsync(new HttpResponse(200, startPage, string.Empty));
            _repository.Setup(o => o.Get<StartPage>("doesnt-exist", null)).ReturnsAsync(new HttpResponse(404, null, "No start-page found for 'doesnt-exist'"));

            // objects
            _controller = new StartPageController(_repository.Object);
        }

        [Fact]
        public void GetAStartPage()
        {
            var indexPage = AsyncTestHelper.Resolve(_controller.Index("start-page")) as ViewResult;
            var result = indexPage.ViewData.Model as StartPage;

            result.Title.Should().Be("Start Page");
            result.Slug.Should().Be("start-page");
            result.Summary.Should().Be("This is a summary ");
            result.UpperBody.Should().Be(MarkdownWrapper.ToHtml("An upper body"));
            result.FormLinkLabel.Should().Be("Start now");
            result.FormLink.Should().Be("http://start.com");
            result.LowerBody.Should().Be(MarkdownWrapper.ToHtml("Lower body"));
            result.Breadcrumbs.Should().HaveCount(1);
            result.Alerts.First().Title.Should().Be("title");
            result.Alerts.First().Body.Should().Contain("body");
            result.Alerts.First().SubHeading.Should().Be("subHeading");
            result.Alerts.First().Severity.Should().Be("severity");
        }

        [Fact]
        public void GivesNotFoundOnRequestForNonExistentStartPage()
        {
            var result = AsyncTestHelper.Resolve(_controller.Index("doesnt-exist")) as StatusCodeResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
