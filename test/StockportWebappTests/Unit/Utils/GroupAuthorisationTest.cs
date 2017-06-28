using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Filters;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class GroupAuthorisationTest
    {
        private readonly Mock<IApplicationConfiguration> _applicationConfigurationMock = new Mock<IApplicationConfiguration>();
        private ModelStateDictionary _modelState;
        private ActionExecutingContext _actionExcecutingContext;
        private readonly DefaultHttpContext _context = new DefaultHttpContext();
        private readonly Mock<IJwtDecoder> _decoder = new Mock<IJwtDecoder>();
        private readonly CurrentEnvironment _environment = new CurrentEnvironment("TEST");
        private readonly Mock<ILogger<GroupAuthorisation>> _logger = new Mock<ILogger<GroupAuthorisation>>();

        [Fact]
        public void ShouldRedirectIfNoCookie()
        {
            // Arrange
            SetUpParameters();
            var redirectUrl = "www.test.com";
            _context.Request.Host = new HostString(redirectUrl);
            _context.Request.Path = new PathString("/");
            _context.Request.Scheme = "http";
            _context.Request.QueryString = new QueryString("");
            _applicationConfigurationMock.Setup(c => c.GetMyAccountUrl()).Returns("www.loginpage.com");
            var groupAuthorisation = new GroupAuthorisation(_applicationConfigurationMock.Object, _decoder.Object, _environment, _logger.Object);

            // Act
            groupAuthorisation.OnActionExecuting(_actionExcecutingContext);
            var result = _actionExcecutingContext.Result as RedirectResult;

            // Assert
            result.Should().NotBeNull();
            result.Url.Should().NotBeNullOrEmpty();
            result.Url.Should().Be("www.loginpage.com?returnUrl=http://" + redirectUrl + "/");

        }

        [Fact]
        public void ShouldNotRedirectIfCookieExists()
        {
            // Arrange
            SetUpParameters();
            var redirectUrl = "www.test.com";
            _context.Request.Host = new HostString(redirectUrl);
            _context.Request.Path = new PathString("/");
            _context.Request.Scheme = "http";
            _context.Request.QueryString = new QueryString("");
            _context.Request.Cookies = new RequestCookieCollection(new Dictionary<string, string>() { { "jwtCookie", "test" } });
            _applicationConfigurationMock.Setup(c => c.GetMyAccountUrl()).Returns("www.loginpage.com");
            _decoder.Setup(d => d.Decode(It.IsAny<string>())).Returns(new LoggedInPerson() {Email = "test", Name = "test"});
            var groupAuthorisation = new GroupAuthorisation( _applicationConfigurationMock.Object, _decoder.Object, _environment, _logger.Object);

            // Act
            groupAuthorisation.OnActionExecuting(_actionExcecutingContext);
            var loggedInPerson = _actionExcecutingContext.ActionArguments["loggedInPerson"] as LoggedInPerson;

            // Assert
            loggedInPerson.Email.Should().Be("test");
            loggedInPerson.Name.Should().Be("test");
        }

        private void SetUpParameters()
        {
            _modelState = new ModelStateDictionary();

            var actionContext = new ActionContext(
                _context,
                new Mock<RouteData>().Object,
                new Mock<ActionDescriptor>().Object,
                _modelState);

            _actionExcecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            new Mock<Controller>());
        }
    }
}
