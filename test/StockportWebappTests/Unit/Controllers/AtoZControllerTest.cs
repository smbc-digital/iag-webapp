using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class AtoZControllerTest
    {
        private readonly Mock<IRepository> _repository;
        private readonly AtoZController _controller;

        public AtoZControllerTest()
        {
            _repository = new Mock<IRepository>();
            _controller = new AtoZController(_repository.Object);
        }

        [Fact]
        public void ItReturnsAnAtoZListing()
        {
            var atoz = new List<AtoZ> { new AtoZ("title", "slug", "teaser", "type") };
            var response = new HttpResponse((int)HttpStatusCode.OK, atoz, string.Empty);

            _repository.Setup(o => o.Get<List<AtoZ>>(It.IsAny<string>(), null))
                .ReturnsAsync(response);

            var view = AsyncTestHelper.Resolve(_controller.Index("v")) as ViewResult;
            var model = view.ViewData.Model as AtoZViewModel;

            model.CurrentLetter.Should().Be("V");
            model.Items.Should().HaveCount(1);
            model.Items[0].Title.Should().Be("title");
            model.Items[0].NavigationLink.Should().Contain("slug");
            model.Items[0].Teaser.Should().Be("teaser");
        }

        [Fact]
        public void RedirectsTo500ErrorIfUnauthorised()
        {
            var response = new HttpResponse((int)HttpStatusCode.Unauthorized, string.Empty, string.Empty);

            _repository.Setup(o => o.Get<List<AtoZ>>(It.IsAny<string>(), null))
                .ReturnsAsync(response);

            var result = AsyncTestHelper.Resolve(_controller.Index("v")) as RedirectToActionResult;
            result.ControllerName.Should().Be("Error");
            result.ActionName.Should().Be("500");
        }

        [Fact]
        public void GetsABlankAtoZWhenNotFoundAtoZListing()
        {
            _repository.Setup(o => o.Get<List<AtoZ>>("a", null))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.NotFound, "error", string.Empty));

            var response = AsyncTestHelper.Resolve(_controller.Index("a")) as ViewResult;

            response.ViewData["Error"].Should().Be("error");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("abc")]
        [InlineData("$")]
        [InlineData("abc")]
        [InlineData("not a letter")]
        [InlineData("$Not a letter")]
        public void ShouldReturnANotFoundPageIfTheSearchTermIsNotInTheAlphabet(string searchTerm)
        {
            var response = AsyncTestHelper.Resolve(_controller.Index(searchTerm));

            response.Should().BeOfType<NotFoundResult>();
            _repository.Verify(o => o.Get<List<AtoZ>>(It.IsAny<string>(), null), Times.Never);
        }     
    }
}