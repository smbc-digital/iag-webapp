using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.AmazonSES;
using StockportWebapp.Controllers;
using StockportWebapp.ViewDetails;
using Xunit;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using HttpResponse = StockportWebapp.Http.HttpResponse;
using StockportWebapp.FeatureToggling;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class ContactUsControllerTest
    {
        private readonly ContactUsController _controller;
        private readonly Mock<IHttpEmailClient> _mockEmailClient;
        private readonly Mock<ILogger<ContactUsController>> _mockLogger;
        private readonly Mock<IApplicationConfiguration> _configuration;
        private readonly BusinessId _businessId;
        private readonly string _userEmail = "contactme@email.com";
        private readonly ContactUsDetails _validContactDetails;
        private readonly string _userName = "name";
        private readonly string _emailSubject = "Drugs and Alcohol";
        private readonly string _emailBody = "A body";
        private string _serviceEmails = "service@email.com, another@email.com";

        private const string Path = "/page-with-contact-us-form";
        private readonly string _url = $"http://page.com{Path}";
        private readonly string _title = "Title";
        private Mock<IRepository> _repository = new Mock<IRepository>();
        private readonly ContactUsId _contactUsId;
        
        public ContactUsControllerTest()
        {
            _mockEmailClient = new Mock<IHttpEmailClient>();
            _mockLogger = new Mock<ILogger<ContactUsController>>();
            _configuration = new Mock<IApplicationConfiguration>();

            _configuration.Setup(a => a.GetEmailEmailFrom(It.IsAny<string>()))
                .Returns(AppSetting.GetAppSetting("businessid:Email:EmailFrom"));

            _businessId = new BusinessId("businessid");

            _contactUsId = new ContactUsId("name", "slug", "email");
          
            _repository.Setup(o => o.Get<ContactUsId>(It.IsAny<string>(), It.IsAny<List<Query>>()))
               .ReturnsAsync(HttpResponse.Successful(200, _contactUsId));

            _controller = new ContactUsController(_repository.Object, _mockEmailClient.Object, _mockLogger.Object, _configuration.Object, _businessId);
            _validContactDetails = new ContactUsDetails(_userName, _userEmail, _emailSubject,
                _emailBody, _serviceEmails,_title);

            var request = new Mock<HttpRequest>();
            var context = new Mock<HttpContext>();
            var headerDictionary = new HeaderDictionary { { "referer", _url } };
            request.Setup(r => r.Headers).Returns(headerDictionary);
            context.Setup(c => c.Request).Returns(request.Object);
            _controller.ControllerContext = new ControllerContext(new ActionContext(context.Object, new RouteData(), new ControllerActionDescriptor()));
        }

        [Fact]
        public async Task ItSendsAnEmailToService()
        {
            await _controller.Contact(_validContactDetails);

            _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()));
        }

        [Fact]
        public async Task DynamicContactUsPostShouldReturnARedirectActionIfFeatureToggleOn()
        {
            var pageResult = await _controller.Contact(_validContactDetails) as RedirectResult;;

            pageResult.Url.Should().Contain(Path);
        }

        [Fact]
        public async Task ShouldNotSendEmailWhenInvalid()
        {
            var invalidDetails = new ContactUsDetails { Email = "nam", Name = "name" };
            _controller.ModelState.AddModelError("", "Error");

            await _controller.Contact(invalidDetails);

            _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()), Times.Never);
        }

        [Fact]
        public async Task ShouldCreateTheEmailBodyFromContactUsDetailsForPostIfItIsValid()
        {
            await _controller.Contact(_validContactDetails);

            _mockEmailClient.Verify(client => client.SendEmailToService(It.Is<EmailMessage>(
                message => !string.IsNullOrEmpty(message.Subject) &&
                message.Body.Contains(_userName) && message.Body.Contains(_userEmail) && message.Body.Contains(_emailSubject) && message.Body.Contains(_emailBody) && message.Body.Contains(_url)
            )));
        }

        [Fact]
        public async Task ShouldSendSentStatusBackInTheRedirectAsTrueIfMessageValidAndIsSentSuccessfully()
        {
            _mockEmailClient.Setup(o => o.SendEmailToService(It.IsAny<EmailMessage>())).ReturnsAsync(HttpStatusCode.OK);

            var pageResult = await _controller.Contact(_validContactDetails);

            pageResult.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = pageResult as RedirectToActionResult;

            redirectResult.ActionName.Should().Be("ThankYouMessage");
            redirectResult.RouteValues["referer"].Should().Be(Path);
        }

        [Fact]
        public async Task ShouldSendSentStatusBackInTheRedirectAsFalseIfMessageValidButIsNotSentSuccessfully()
        {
            _mockEmailClient.Setup(o => o.SendEmailToService(It.IsAny<EmailMessage>())).Returns(Task.FromResult(HttpStatusCode.BadRequest));

            var pageResult = await _controller.Contact(_validContactDetails) as RedirectResult;;

            pageResult.Url.Should().Contain("message=We have been unable to process the request. Please try again later.");
        }

        [Fact]
        public async Task ShouldSendSentStatusBackInTheRedirectAsFalseIfMessageInvalid()
        {
            var invalidDetails = new ContactUsDetails { Email = "nam", Name = "name" };
            _controller.ModelState.AddModelError("Name", "an invalid name was provided");
            _controller.ModelState.AddModelError("Email", "an invalid email was provided");

            var pageResult = await _controller.Contact(invalidDetails) as RedirectResult;;

            pageResult.Url.Should().Contain("message=an invalid name was provided<br />an invalid email was provided<br />");
        }

        [Fact]
        public async Task ShouldShowAThankYouPageWithTheReferingUrlPassed()
        {
            var referer = "this-is-a-referer";

            var pageResult = await _controller.ThankYouMessage(referer) as ViewResult;;

            pageResult.Model.Should().Be(referer);
            pageResult.ViewName.Should().Be("ThankYouMessage");
        }

        [Fact]
        public async Task ShouldAddPageTitleToSubject()
        {
            await _controller.Contact(_validContactDetails);

            _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()));
        }
    }
}