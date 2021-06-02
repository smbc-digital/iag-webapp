using Xunit;
using FluentAssertions;
using StockportWebapp.Utils;
using Moq;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace StockportWebappTests_Unit.Unit.Utils
{
    public class LoggedInHelperTests
    {
        private readonly Mock<IJwtDecoder> _jwtDecoder = new Mock<IJwtDecoder>();
        private readonly Mock<IHttpContextAccessor> _context = new Mock<IHttpContextAccessor>();
        private readonly Mock<ILogger<LoggedInHelper>> _logger = new Mock<ILogger<LoggedInHelper>>();
        private readonly LoggedInHelper _loggedInHelper;

        public LoggedInHelperTests()
        {
            _jwtDecoder.Setup(_ => _.Decode(It.IsAny<string>())).Returns(new LoggedInPerson { Email = "email", Name = "name" });
            _loggedInHelper = new LoggedInHelper(_context.Object, new StockportWebapp.Config.CurrentEnvironment("local"), _jwtDecoder.Object, _logger.Object);
        }

        [Fact]
        public void ShouldReturnLoggedInPersonIfCookieExists()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Cookies = MockRequestCookieCollection("jwtCookie", "test");

            // Mocks
            _context.Setup(_ => _.HttpContext).Returns(httpContext);

            // Act
            var loggedInPerson = _loggedInHelper.GetLoggedInPerson();

            // Assert
            loggedInPerson.Name.Should().Be("name");
            loggedInPerson.Email.Should().Be("email");
        }

        [Fact]
        public void ShouldReturnEmptyLoggedInPersonIfNoCookieExists()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Mocks
            _context.Setup(_ => _.HttpContext).Returns(httpContext);

            // Act
            var loggedInPerson = _loggedInHelper.GetLoggedInPerson();

            // Assert
            loggedInPerson.Name.Should().BeNull();
            loggedInPerson.Email.Should().BeNull();
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
    }
}
