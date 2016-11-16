using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Controllers;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class ErrorControllerTest
    {
        private readonly ErrorController _controller;

        public ErrorControllerTest()
        {
            _controller = new ErrorController();
        }

        [Fact]
        public void ShouldTellUsSomethingsMissingIfAPageWasNotFound()
        {
            var result = AsyncTestHelper.Resolve(_controller.Error("404")) as ViewResult;

            result.ViewData[@"ErrorHeading"].Should().Be("Something's missing");
        }

        [Fact]
        public void ShouldTellUsSomethingIsWrongIfADifferentErrorOccurred()
        {
            var result = AsyncTestHelper.Resolve(_controller.Error("500")) as ViewResult;

            result.ViewData[@"ErrorHeading"].Should().Be("Something went wrong");
        }
    }
}