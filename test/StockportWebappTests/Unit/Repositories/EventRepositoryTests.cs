using System;
using System.Net;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Repositories;


namespace StockportWebappTests.Unit.Repositories
{
    public class EventRepositoryTests
    {
        private readonly EventsRepository _eventsRepository;
        private readonly Mock<ILogger<EventsRepository>> _logger;
        private readonly Mock<IHttpEmailClient> _emailClient;
        private readonly Mock<IApplicationConfiguration> _applicationConfiguration;

        public EventRepositoryTests()
        {
            _logger = new Mock<ILogger<EventsRepository>>();
            _emailClient = new Mock<IHttpEmailClient>();
            _applicationConfiguration = new Mock<IApplicationConfiguration>();
            _applicationConfiguration.Setup(a => a.GetEventSubmissionEmail(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("EventSubmissionEmail"));
            _eventsRepository = new EventsRepository(_logger.Object, _emailClient.Object,_applicationConfiguration.Object, new BusinessId("businessId"));
        }

        [Fact]
        public void ItShouldBuildAEmailBodyFromFormContent()
        {
            var eventSubmission = new EventSubmission("title", "teaser", new DateTime(2016, 01, 01), "start time", "end time", new DateTime(2016, 01, 01), "frequency",
                "fee", "location", "submitted by", null, "description", null);

            var response = _eventsRepository.GenerateEmailBody(eventSubmission);

            response.Should().Contain("Title: title");
            response.Should().Contain("Teaser: teaser");
            response.Should().Contain(string.Concat("Event Date: ", new DateTime(2016, 01, 01).ToString("dddd dd MMMM yyyy")));
            response.Should().Contain("Start Time: start time");
            response.Should().Contain("End Time: end time");
            response.Should().Contain("Frequency: frequency");
            response.Should().Contain("Fee: fee");
            response.Should().Contain("Location: location");
            response.Should().Contain("Submitted By: submitted by");
            response.Should().Contain("Description: description");
        }

        [Fact]
        public void ItShouldBuildAEmailBodyWithImageFromFormContent()
        {
            var eventSubmission = new EventSubmission {Image = new FormFile(null, 0, 0, "name", "filename.jpg")};

            var response = _eventsRepository.GenerateEmailBody(eventSubmission);

            response.Should().Contain("Image: " + eventSubmission.Image.FileName);
        }

        [Fact]
        public void ItShouldBuildAEmailBodyWithAttachmentFromFormContent()
        {
            var eventSubmission = new EventSubmission { Attachment = new FormFile(null, 0, 0, "name", "filename.jpg") };

            var response = _eventsRepository.GenerateEmailBody(eventSubmission);

            response.Should().Contain("Attachment: " + eventSubmission.Attachment.FileName);
        }

        [Fact]
        public async void ItShouldSendAnEmailAndReturnAStatusCodeOf200()
        {
            var eventSubmission = new EventSubmission {SubmitterEmail = "test@testing.xyz"};

            _emailClient.Setup(
                e =>
                    e.SendEmailToService(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        "test@testing.xyz")).ReturnsAsync(HttpStatusCode.OK);

            var response = await _eventsRepository.SendEmailMessage(eventSubmission);

            response.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void ItShouldLogThatAnEmailWasSent()
        {
            var eventSubmission = new EventSubmission { SubmitterEmail = "test@testing.xyz" };
            await _eventsRepository.SendEmailMessage(eventSubmission);

            LogTesting.Assert(_logger, LogLevel.Information, "Sending event submission form email");
        }
    }
}
