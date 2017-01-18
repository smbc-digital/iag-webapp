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
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Controllers
{
    public class ContactUsControllerTest
    {
        private readonly ContactUsController _controller;
        private readonly Mock<IHttpEmailClient> _mockEmailClient;
        private readonly Mock<ILogger<ContactUsController>> _mockLogger;
        private readonly string _userEmail = "contactme@email.com";
        private readonly ContactUsDetails _validContactDetails;
        private readonly string _userName = "name";
        private readonly string _emailSubject = "Drugs and Alcohol";
        private readonly string _emailBody = "A body";
        private string _serviceEmails = "service@email.com, another@email.com";

        private const string Path = "/page-with-contact-us-form";
        private readonly string _url = $"http://page.com{Path}";
        private readonly string _title = "Title";

        public ContactUsControllerTest()
        {
            _mockEmailClient = new Mock<IHttpEmailClient>();
            _mockLogger = new Mock<ILogger<ContactUsController>>();
            _controller = new ContactUsController(_mockEmailClient.Object, _mockLogger.Object);
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
        public void ItSendsAnEmailToService()
        {
            AsyncTestHelper.Resolve(_controller.Contact(_validContactDetails));

            _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()));
        }

        [Fact]
        public void DynamicContactUsPostShouldReturnARedirectActionIfFeatureToggleOn()
        {
            var pageResult = AsyncTestHelper.Resolve(_controller.Contact(_validContactDetails)) as RedirectResult;

            pageResult.Url.Should().Contain(Path);
        }

        [Fact]
        public void ShouldNotSendEmailWhenInvalid()
        {
            var invalidDetails = new ContactUsDetails { Email = "nam", Name = "name" };
            _controller.ModelState.AddModelError("", "Error");

            AsyncTestHelper.Resolve(_controller.Contact(invalidDetails));

            _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()), Times.Never);
        }

        [Fact]
        public void ShouldCreateTheEmailBodyFromContactUsDetailsForPostIfItIsValid()
        {
            AsyncTestHelper.Resolve(_controller.Contact(_validContactDetails));

            _mockEmailClient.Verify(client => client.SendEmailToService(It.Is<EmailMessage>(
                message => !string.IsNullOrEmpty(message.Subject) &&
                message.Body.Contains(_userName) && message.Body.Contains(_userEmail) && message.Body.Contains(_emailSubject) && message.Body.Contains(_emailBody) && message.Body.Contains(_url)
            )));
        }

        [Fact]
        public void ShouldSendSentStatusBackInTheRedirectAsTrueIfMessageValidAndIsSentSuccessfully()
        {
            _mockEmailClient.Setup(o => o.SendEmailToService(It.IsAny<EmailMessage>())).ReturnsAsync(HttpStatusCode.OK);

            var pageResult = AsyncTestHelper.Resolve(_controller.Contact(_validContactDetails));

            pageResult.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = pageResult as RedirectToActionResult;

            redirectResult.ActionName.Should().Be("ThankYouMessage");
            redirectResult.RouteValues["referer"].Should().Be(Path);
        }

        [Fact]
        public void ShouldSendSentStatusBackInTheRedirectAsFalseIfMessageValidButIsNotSentSuccessfully()
        {
            _mockEmailClient.Setup(o => o.SendEmailToService(It.IsAny<EmailMessage>())).Returns(Task.FromResult(HttpStatusCode.BadRequest));

            var pageResult = AsyncTestHelper.Resolve(_controller.Contact(_validContactDetails)) as RedirectResult;

            pageResult.Url.Should().Contain("message=We have been unable to process the request. Please try again later.");
        }

        [Fact]
        public void ShouldSendSentStatusBackInTheRedirectAsFalseIfMessageInvalid()
        {
            var invalidDetails = new ContactUsDetails { Email = "nam", Name = "name" };
            _controller.ModelState.AddModelError("Name", "an invalid name was provided");
            _controller.ModelState.AddModelError("Email", "an invalid email was provided");

            var pageResult = AsyncTestHelper.Resolve(_controller.Contact(invalidDetails)) as RedirectResult;

            pageResult.Url.Should().Contain("message=an invalid name was provided<br />an invalid email was provided<br />");
        }

        [Fact]
        public void ShouldShowAThankYouPageWithTheReferingUrlPassed()
        {
            var referer = "this-is-a-referer";

            var pageResult = AsyncTestHelper.Resolve(_controller.ThankYouMessage(referer)) as ViewResult;

            pageResult.Model.Should().Be(referer);
            pageResult.ViewName.Should().Be("ThankYouMessage");
        }

        [Fact]
        public void ShouldAddPageTitleToSubject()
        {
            AsyncTestHelper.Resolve(_controller.Contact(_validContactDetails));

            _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()));
        }
    }
}