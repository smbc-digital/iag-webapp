using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using StockportWebapp.AmazonSES;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Builders;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.AmazonSES
{
    public class HttpEmailClientTest
    {
        private readonly Mock<ILogger<HttpEmailClient>> _mockLogger;
        private readonly Mock<IEmailBuilder> _emailBuilder;
        private readonly Mock<IAmazonSimpleEmailService> _amazonEmailService;

        public HttpEmailClientTest()
        {
            _mockLogger = new Mock<ILogger<HttpEmailClient>>();
            _emailBuilder = new Mock<IEmailBuilder>();
            _amazonEmailService = new Mock<IAmazonSimpleEmailService>();
        }

        [Fact]
        public void ItShouldReturnA500AndLogItIfTheServiceEmailIsNullOrEmpty()
        {
            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object);
            var emailMessage = new EmailMessage("subject", "body", "", "user@email.com", new List<IFormFile>());

            var httpStatusCode = AsyncTestHelper.Resolve(emailClient.SendEmailToService(emailMessage));
            httpStatusCode.Should().Be(HttpStatusCode.InternalServerError);

            LogTesting.Assert(_mockLogger, LogLevel.Error, "ServiceEmail can not be null or empty. No email has been sent.");
        }

        [Fact]
        public void ItShouldReturnA200IfTheStatusCodeIsOkAndLogTheResponse()
        {
            _amazonEmailService.Setup(o => o.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SendRawEmailResponse { HttpStatusCode = HttpStatusCode.OK, MessageId = "test", ResponseMetadata = new ResponseMetadata { RequestId = "test" } });

            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object);
            var emailMessage = new EmailMessage("subject", "body", "service@mail.com", "user@email.com", new List<IFormFile>());

            var httpStatusCode = AsyncTestHelper.Resolve(emailClient.SendEmailToService(emailMessage));

            httpStatusCode.Should().Be(HttpStatusCode.OK);
            LogTesting.Assert(_mockLogger, LogLevel.Information, "An email was sent to Amazon SES with message id: test and request id test");
        }


        [Fact]
        public void ItShouldReturnA500IfTheEmailServiceThrowsAnException()
        {
            _amazonEmailService.Setup(o => o.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SendRawEmailResponse { HttpStatusCode = HttpStatusCode.InternalServerError, MessageId = "test", ResponseMetadata = new ResponseMetadata { RequestId = "test" } });

            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object);
            var emailMessage = new EmailMessage("subject", "body", "service@mail.com", "user@email.com", new List<IFormFile>());

            var httpStatusCode = AsyncTestHelper.Resolve(emailClient.SendEmailToService(emailMessage));

            httpStatusCode.Should().Be(HttpStatusCode.InternalServerError);
            LogTesting.Assert(_mockLogger, LogLevel.Warning, $"There was a problem sending an email, message id: test and request id: test and status code {HttpStatusCode.InternalServerError}");
        }

        [Fact]
        public void ItShouldReturnABadRequestIfAmazonSesReturnsAnException()
        {
            _amazonEmailService.Setup(
                    o => o.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("There was an error"));

            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object);
            var emailMessage = new EmailMessage("subject", "body", "service@mail.com", "user@email.com", new List<IFormFile>());

            var httpStatusCode = AsyncTestHelper.Resolve(emailClient.SendEmailToService(emailMessage));

            httpStatusCode.Should().Be(HttpStatusCode.BadRequest);
            LogTesting.Assert(_mockLogger, LogLevel.Error, "An error occurred trying to send an email to Amazon SES. \nThere was an error");
        }
    }
}