using Xunit;
using FluentAssertions;
using StockportWebapp.Utils;
using Moq;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.Internal;
using System.Collections.Generic;

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
            httpContext.Request.Cookies = new RequestCookieCollection(new Dictionary<string, string>() { { "jwtCookie", "test" } });

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
    }
}
