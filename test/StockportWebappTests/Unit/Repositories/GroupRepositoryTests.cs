using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using System.Net;

namespace StockportWebappTests.Unit.Repositories
{
    public class GroupRepositoryTests
    {
        private readonly GroupRepository _groupRepository;
        private readonly Mock<ILogger<GroupRepository>> _logger;
        private readonly Mock<IHttpEmailClient> _emailClient;


        public GroupRepositoryTests()
        {
            _logger = new Mock<ILogger<GroupRepository>>();
            _emailClient = new Mock<IHttpEmailClient>();
            var applicationConfiguration = new Mock<IApplicationConfiguration>();

            applicationConfiguration.Setup(a => a.GetGroupSubmissionEmail(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));
            applicationConfiguration.Setup(a => a.GetEmailEmailFrom(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("GroupSubmissionEmail"));

            _groupRepository = new GroupRepository(_logger.Object, _emailClient.Object, applicationConfiguration.Object, new BusinessId("businessId"));
        }

        [Fact]
        public void ItShouldBuildAEmailBodyFromFormContent()
        {

            var groupSubmission = new GroupSubmission()
            {
                Address = "Address",
                Categories = new List<string>(),
                Name = "Group",
                Email = "email",
                PhoneNumber = "phone",
                Website = "http://www.group.org",
                Description = "Description",
                Category1 = "Category"
            };

            var response = _groupRepository.GenerateEmailBody(groupSubmission);

            response.Should().Contain("Group name: Group");         
            response.Should().Contain("Location: Address");
            response.Should().Contain("Group website: http://www.group.org");
            response.Should().Contain("Group description: Description");
            response.Should().Contain("Group email address: email");
            response.Should().Contain("Group categories: Category");
            response.Should().Contain("Group phone number: phone");
        }

        [Fact]
        public void ItShouldBuildAEmailBodyWithImageFromFormContent()
        {
            var groupSubmission = new GroupSubmission() { Image = new FormFile(null, 0, 0, "name", "filename.jpg") };

            var response = _groupRepository.GenerateEmailBody(groupSubmission);

            response.Should().Contain("Event image: " + groupSubmission.Image.FileName);
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
                Category1 = "Category"
            };
            var response = await _groupRepository.SendEmailMessage(groupSubmission);

            response.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void ItShouldLogThatAnEmailWasSent()
        {
            var groupSubmission = new GroupSubmission { Email = "test@testing.xyz" };
            await _groupRepository.SendEmailMessage(groupSubmission);

            LogTesting.Assert(_logger, LogLevel.Information, "Sending group submission form email");
        }
    }
}
