﻿using System;
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
using StockportWebappTests_Unit.Helpers;

namespace StockportWebappTests_Unit.Unit.Utils
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
