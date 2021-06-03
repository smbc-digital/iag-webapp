using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using StockportWebappTests_Unit.Helpers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Utils
{
    public class GroupEmailBuilderTests
    {
        private readonly GroupEmailBuilder _groupEmailBuilder;
        private readonly Mock<ILogger<GroupEmailBuilder>> _logger;
        private readonly Mock<IHttpEmailClient> _emailClient;
        private readonly Mock<IApplicationConfiguration> _applicationConfiguration;


        public GroupEmailBuilderTests()
        {
            _logger = new Mock<ILogger<GroupEmailBuilder>>();
            _emailClient = new Mock<IHttpEmailClient>();
            _applicationConfiguration = new Mock<IApplicationConfiguration>();

            _applicationConfiguration.Setup(a => a.GetGroupSubmissionEmail(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));
            _applicationConfiguration.Setup(a => a.GetEmailEmailFrom(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));

            _groupEmailBuilder = new GroupEmailBuilder(_logger.Object, _emailClient.Object, _applicationConfiguration.Object, new BusinessId("businessId"));
        }

        [Fact]
        public async void ItShouldSendAnEmailAndReturnAStatusCodeOf200()
        {
            _emailClient.Setup(e => e.SendEmailToService(It.Is<EmailMessage>(message => message.ToEmail == AppSetting.GetAppSetting("GroupSubmissionEmail").ToString()))).ReturnsAsync(HttpStatusCode.OK);

            var groupSubmission = new GroupSubmission()
            {
                Address = "Address",
                Categories = new List<string>(),
                Name = "Group",
                Email = "email",
                PhoneNumber = "phone",
                Website = "http://www.group.org",
                Description = "Description",
                CategoriesList = "Category"
            };
            var response = await _groupEmailBuilder.SendEmailAddNew(groupSubmission);

            response.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void ItShouldLogThatAnEmailWasSent()
        {
            var groupSubmission = new GroupSubmission { Email = "test@testing.xyz" };
            await _groupEmailBuilder.SendEmailAddNew(groupSubmission);

            LogTesting.Assert(_logger, LogLevel.Information, "Sending group submission form email");
        }
    }
}
