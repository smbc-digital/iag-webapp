using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Middleware;
using StockportWebappTests_Unit.Helpers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Middleware
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
        public void ShouldSetToDefaultBusinessIdIfBusinessIdIsNoInHeader()
        {
            var context = new DefaultHttpContext();
            _businessIdMiddleware.Invoke(context, _businessId);

            _businessId.ToString().Should().Be("stockportgov");
        }

        [Fact]
        public void ShouldLogWarningIfNoBusinessIdIsSet()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/";
            _businessIdMiddleware.Invoke(context, _businessId);

            LogTesting.Assert(_logger, LogLevel.Information, "BUSINESS-ID has not been set, setting to default");
        }

        [Fact]
        public void ShouldNotLogIfNoBusinessIdIsSetAndIsAsset()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/assets/test.js";
            _businessIdMiddleware.Invoke(context, _businessId);

            LogTesting.DoesNotContain(_logger, LogLevel.Information, "BUSINESS-ID has not been set, setting to default");
        }

        [Fact]
        public void ShouldNotLogIfNoBusinessIdIsSetAndIsHealthCheck()
        {
            var context = new DefaultHttpContext();
            context.Request.Path = "/_healthcheck";
            _businessIdMiddleware.Invoke(context, _businessId);

            LogTesting.DoesNotContain(_logger, LogLevel.Information, "BUSINESS-ID has not been set, setting to default");
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
