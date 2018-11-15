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
using System.Threading.Tasks;

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
        public async Task ItShouldReturnA500AndLogItIfTheServiceEmailIsNullOrEmpty()
        {
            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, true);
            var emailMessage = new EmailMessage("subject", "body", "", "", "user@email.com", new List<IFormFile>());

            var httpStatusCode = await emailClient.SendEmailToService(emailMessage);
            httpStatusCode.Should().Be(HttpStatusCode.InternalServerError);

            LogTesting.Assert(_mockLogger, LogLevel.Error, "ToEmail can not be null or empty. No email has been sent.");
        }

        [Fact]
        public async Task ItShouldReturnA200IfTheStatusCodeIsOkAndLogTheResponse()
        {
            _amazonEmailService.Setup(o => o.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SendRawEmailResponse { HttpStatusCode = HttpStatusCode.OK, MessageId = "test", ResponseMetadata = new ResponseMetadata { RequestId = "test" } });

            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, true);
            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "service@mail.com", "user@email.com", new List<IFormFile>());

            var httpStatusCode = await emailClient.SendEmailToService(emailMessage);

            httpStatusCode.Should().Be(HttpStatusCode.OK);
            LogTesting.Assert(_mockLogger, LogLevel.Information, "An email was sent to Amazon SES with message id: test and request id test");
        }


        [Fact]
        public async Task ItShouldReturnA500IfTheEmailServiceThrowsAnException()
        {
            _amazonEmailService.Setup(o => o.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SendRawEmailResponse { HttpStatusCode = HttpStatusCode.InternalServerError, MessageId = "test", ResponseMetadata = new ResponseMetadata { RequestId = "test" } });

            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, true);
            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "service@mail.com", "user@email.com", new List<IFormFile>());

            var httpStatusCode = await emailClient.SendEmailToService(emailMessage);

            httpStatusCode.Should().Be(HttpStatusCode.InternalServerError);
            LogTesting.Assert(_mockLogger, LogLevel.Warning, $"There was a problem sending an email, message id: test and request id: test and status code {HttpStatusCode.InternalServerError}");
        }

        [Fact]
        public async Task ItShouldReturnABadRequestIfAmazonSesReturnsAnException()
        {
            _amazonEmailService.Setup(
                    o => o.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("There was an error"));

            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, true);
            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "service@mail.com", "user@email.com", new List<IFormFile>());

            var httpStatusCode = await emailClient.SendEmailToService(emailMessage);

            httpStatusCode.Should().Be(HttpStatusCode.BadRequest);
            LogTesting.Assert(_mockLogger, LogLevel.Error, "An error occurred trying to send an email to Amazon SES. \nThere was an error");
        }

        [Fact]
        public async Task ItShouldReturnAOk_When_SendAmazonEmail_SetToFalse()
        {

            var emailClient = new HttpEmailClient(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, false);
            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "service@mail.com", "user@email.com", new List<IFormFile>());

            var httpStatusCode = await emailClient.SendEmailToService(emailMessage);

            httpStatusCode.Should().Be(HttpStatusCode.OK);
            _amazonEmailService.Verify(_ => _.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()),Times.Never);
        }
    }
}