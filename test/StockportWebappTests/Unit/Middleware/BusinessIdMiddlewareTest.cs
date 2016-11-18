using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Middleware;
using Xunit;

namespace StockportWebappTests.Unit.Middleware
{
    public class BusinessIdMiddlewareTest
    {
        private readonly BusinessIdMiddleware _businessIdMiddleware;
        private readonly BusinessId _businessId;
        private readonly Mock<ILogger<BusinessIdMiddleware>> _logger;

        public BusinessIdMiddlewareTest()
        {
            var requestDelegate = new Mock<RequestDelegate>();
            _logger = new Mock<ILogger<BusinessIdMiddleware>>();
            _businessId = new BusinessId();

            _businessIdMiddleware = new BusinessIdMiddleware(requestDelegate.Object, _logger.Object);
        }

        [Fact]
        public void ShouldSetBusinessIdIfBusinessIdIsInHeader()
        {
            var context = new DefaultHttpContext();
            const string businessIdString = "business-id";
            context.Request.Headers["BUSINESS-ID"] = businessIdString;
            _businessIdMiddleware.Invoke(context, _businessId);

            _businessId.ToString().Should().Be(businessIdString);
        }

        [Fact]
        public void ShouldNotSetBusinessIdIfBusinessIdIsNoInHeader()
        {
            var context = new DefaultHttpContext();
            _businessIdMiddleware.Invoke(context, _businessId);

            _businessId.ToString().Should().Be("NOT SET");
        }

        [Fact]
        public void ShouldLogErrorIfNoBusinessIdIsSet()
        {
            var context = new DefaultHttpContext();
            _businessIdMiddleware.Invoke(context, _businessId);

            LogTesting.Assert(_logger, LogLevel.Error, "BUSINESS-ID has not been set");
        }

        [Fact]
        public void ShouldLogInformationIfBusniessIdIsSet()
        {
            var context = new DefaultHttpContext();
            const string businessIdString = "business-id";
            context.Request.Headers["BUSINESS-ID"] = businessIdString;
            _businessIdMiddleware.Invoke(context, _businessId);

            LogTesting.Assert(_logger, LogLevel.Information, "BUSINESS-ID has been set to: business-id");
        }
    }
}
