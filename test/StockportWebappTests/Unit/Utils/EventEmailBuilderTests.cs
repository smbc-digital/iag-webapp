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
using StockportWebapp.Utils;
using StockportWebapp.Emails.Models;

namespace StockportWebappTests.Unit.Utils
{
    public class EventEmailBuilderTests
    {
        private readonly EventEmailBuilder _eventEmailBuilder;
        private readonly Mock<ILogger<EventEmailBuilder>> _logger;
        private readonly Mock<IHttpEmailClient> _emailClient;
       

        public EventEmailBuilderTests()
        {
            _logger = new Mock<ILogger<EventEmailBuilder>>();
            _emailClient = new Mock<IHttpEmailClient>();
            var applicationConfiguration = new Mock<IApplicationConfiguration>();

            applicationConfiguration.Setup(a => a.GetEventSubmissionEmail(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("EventSubmissionEmail"));
            applicationConfiguration.Setup(a => a.GetEmailEmailFrom(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("EventSubmissionEmail"));

            _eventEmailBuilder = new EventEmailBuilder(_logger.Object, _emailClient.Object, applicationConfiguration.Object, new BusinessId("businessId"));
        }

        [Fact]
        public void ItShouldBuildAEmailBodyFromFormContent()
        {
            var eventSubmission = new EventAdd
            {
                Title = "title",
                EventDate = new DateTime(2016, 01, 01).ToString("dddd dd MMMM yyyy"),
                StartTime = new DateTime(2017, 01, 01, 15, 00, 00).ToString("HH:mm"),
                EndDate = new DateTime(2017, 01, 01, 18, 00, 00).ToString("dddd dd MMMM yyyy"),
                EndTime = new DateTime(2016, 01, 01, 18, 00, 00).ToString("HH:mm"),
                Frequency = "frequency",
                Fee = "fee",
                Location = "location",
                SubmittedBy = "submitted by",
                Description = "description",
                SubmitterEmail = "email",
                Categories = "category 1"
            };

            var response = _eventEmailBuilder.GenerateEmailBodyFromHtml(eventSubmission);

            response.Should().Contain("Event name: title");
            response.Should().Contain(string.Concat("Event date: ", new DateTime(2016, 01, 01).ToString("dddd dd MMMM yyyy")));
            response.Should().Contain("Start time: 15:00");
            response.Should().Contain("End time: 18:00");
            response.Should().Contain("Frequency: frequency");
            response.Should().Contain("Price: fee");
            response.Should().Contain("Location: location");
            response.Should().Contain("Organiser name: submitted by");
            response.Should().Contain("Description: description");
            response.Should().Contain("Organiser email address: email");
            response.Should().Contain("Categories: category 1");          
        }

        [Fact]
        public void ItShouldBuildAEmailBodyWithImageFromFormContent()
        {
            var eventSubmission = new EventAdd { ImagePath = "filename.jpg" };

            var response = _eventEmailBuilder.GenerateEmailBodyFromHtml(eventSubmission);

            response.Should().Contain("Event image: " + eventSubmission.ImagePath);
        }

        [Fact]
        public void ItShouldBuildAEmailBodyWithAttachmentFromFormContent()
        {
            var eventSubmission = new EventAdd { AttachmentPath = "filename.jpg" };

            var response = _eventEmailBuilder.GenerateEmailBodyFromHtml(eventSubmission);

            response.Should().Contain("Additional event document: " + eventSubmission.AttachmentPath);
        }

        [Fact]
        public async void ItShouldSendAnEmailAndReturnAStatusCodeOf200()
        {
            _emailClient.Setup(e => e.SendEmailToService(It.Is<EmailMessage>(message => message.ToEmail == AppSetting.GetAppSetting("EventSubmissionEmail").ToString()))).ReturnsAsync(HttpStatusCode.OK);

            var response = await _eventEmailBuilder.SendEmailAddNew(new EventSubmission() {EventDate = new DateTime(2017, 9, 9) });

            response.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void ItShouldLogThatAnEmailWasSent()
        {
            var eventSubmission = new EventSubmission { SubmitterEmail = "test@testing.xyz", EventDate = new DateTime(2017,9,9)};
            await _eventEmailBuilder.SendEmailAddNew(eventSubmission);

            LogTesting.Assert(_logger, LogLevel.Information, "Sending event submission form email");
        }


        [Fact]
        public async void ItShouldReturnFeaturedEventsFirst()
        {
            var eventSubmission = new EventSubmission { SubmitterEmail = "test@testing.xyz", EventDate = new DateTime(2017, 9, 9) };
            await _eventEmailBuilder.SendEmailAddNew(eventSubmission);

            LogTesting.Assert(_logger, LogLevel.Information, "Sending event submission form email");
        }
    }
}
