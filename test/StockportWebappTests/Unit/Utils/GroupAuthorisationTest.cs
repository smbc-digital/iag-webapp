using FluentAssertions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Filters;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;
using HostString = Microsoft.AspNetCore.Http.HostString;
using PathString = Microsoft.AspNetCore.Http.PathString;
using QueryString = Microsoft.AspNetCore.Http.QueryString;

namespace StockportWebappTests_Unit.Unit.Utils
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
        private readonly Mock<ILoggedInHelper> _loggedInHelper = new Mock<ILoggedInHelper>();

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

            // Mocks
            _applicationConfigurationMock.Setup(c => c.GetMyAccountUrl()).Returns("www.loginpage.com");
            _loggedInHelper.Setup(o => o.GetLoggedInPerson()).Returns(new LoggedInPerson());

            var groupAuthorisation = new GroupAuthorisation(_applicationConfigurationMock.Object, _loggedInHelper.Object);

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
            _context.Request.Cookies = MockRequestCookieCollection("jwtCookie", "test");

            // Mocks
            _applicationConfigurationMock.Setup(c => c.GetMyAccountUrl()).Returns("www.loginpage.com");
            _loggedInHelper.Setup(o => o.GetLoggedInPerson()).Returns(new LoggedInPerson() { Email = "test", Name = "test" });

            var groupAuthorisation = new GroupAuthorisation(_applicationConfigurationMock.Object, _loggedInHelper.Object);

            // Act
            groupAuthorisation.OnActionExecuting(_actionExcecutingContext);
            var loggedInPerson = _actionExcecutingContext.ActionArguments["loggedInPerson"] as LoggedInPerson;

            // Assert
            loggedInPerson.Email.Should().Be("test");
            loggedInPerson.Name.Should().Be("test");
        }

        private static IRequestCookieCollection MockRequestCookieCollection(string key, string value)
        {
            var requestFeature = new HttpRequestFeature();
            var featureCollection = new FeatureCollection();

            requestFeature.Headers = new Microsoft.AspNetCore.Http.HeaderDictionary();
            requestFeature.Headers.Add(HeaderNames.Cookie, new StringValues(key + "=" + value));

            featureCollection.Set<IHttpRequestFeature>(requestFeature);

            var cookiesFeature = new RequestCookiesFeature(featureCollection);

            return cookiesFeature.Cookies;
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
