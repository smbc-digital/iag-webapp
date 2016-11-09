using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using StockportWebapp.AmazonSES;
using StockportWebappTests.Unit.Fake;
using Xunit;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Config;

namespace StockportWebappTests.Unit.AmazonSES
{
    public class HttpEmailClientTest
    {
        private readonly FakeResponseHandler _fakeResponseHandler;
        private const string Host = "www.amazon.com";
        private HttpEmailClient _emailClient;
        private readonly Mock<AmazonAuthorizationHeader> _mockAuthHeader;
        private readonly Mock<ILogger<HttpEmailClient>> _mockLogger;

        private readonly HttpResponseMessage _dummyOkResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(TestHelper.AnyString)
        };

        private readonly Uri _amazonUri;
        private readonly Mock<IEmailConfigurationBuilder> _mockEmailConfiguration;
        private readonly BusinessId _businessId;

        public HttpEmailClientTest()
        {
            var config = new AmazonSesClientConfiguration(AppSetting.GetAppSetting(Host), AppSetting.GetAppSetting("region"), AppSetting.GetAppSetting("emailFrom"),
                new AmazonSESKeys("access", "secret"));

            _businessId = new BusinessId("businessId");
            _mockEmailConfiguration = new Mock<IEmailConfigurationBuilder>();
            _mockEmailConfiguration.Setup(o => o.Build(_businessId.ToString())).Returns(config);

            _amazonUri = new Uri($"https://{Host}");

            _fakeResponseHandler = new FakeResponseHandler();

            _mockAuthHeader = new Mock<AmazonAuthorizationHeader>();
            _mockLogger = new Mock<ILogger<HttpEmailClient>>();
        }

        [Fact]
        public void ItSetsTheHttpRequestMethodToPost()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            _fakeResponseHandler.AddFakeResponse(_amazonUri, _dummyOkResponse);
            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService(TestHelper.AnyString, TestHelper.AnyString, TestHelper.AnyString,
                TestHelper.AnyString));

            _fakeResponseHandler.HttpRequest.Method.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void ItSetsXAmzDateHeaderFormattedAsISO8601()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            _fakeResponseHandler.AddFakeResponse(_amazonUri, _dummyOkResponse);
            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService(TestHelper.AnyString, TestHelper.AnyString, TestHelper.AnyString,
                TestHelper.AnyString));

            IEnumerable<string> customHeader = _fakeResponseHandler.HttpRequest.Headers.GetValues("X-Amz-Date");
            var headerValue = customHeader.FirstOrDefault();

            DateTime dateValue;
            var ISO8601format = "yyyyMMddTHHmmssZ";
            var isValidDateFormat = DateTime.TryParseExact(headerValue, ISO8601format, CultureInfo.CurrentCulture,
                DateTimeStyles.None, out dateValue);
            isValidDateFormat.Should().BeTrue();
        }

        [Fact]
        public void ItSetsContentOfTheRequestIncludingMessageBody()
        {
            _emailClient = new HttpEmailClient( _mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            var messageBody = "super important message body";
            _fakeResponseHandler.AddFakeResponse(_amazonUri, _dummyOkResponse);
            string expectedContent = "Message.Body.Text.Data=" + Uri.EscapeDataString(messageBody);

            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService("subject of message",
                messageBody, "service@email.com"));

            _fakeResponseHandler.RequestContent.Should().Contain(expectedContent);
        }

        [Fact]
        public void ItSetsContentOfTheRequestIncludingMessageSubject()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            var subject = "subject of message";
            _fakeResponseHandler.AddFakeResponse(_amazonUri, _dummyOkResponse);
            string expectedContent = "Message.Subject.Data=" + Uri.EscapeDataString(subject);

            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService(subject,
                "email message body", "service@email.com"));

            _fakeResponseHandler.RequestContent.Should().Contain(expectedContent);
        }

        [Fact]
        public void EmailIsCopiedToTheUserEmail()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            var userEmail = "user@email.com";
                
            _fakeResponseHandler.AddFakeResponse(_amazonUri, _dummyOkResponse);
            string expectedContent = "Destination.CcAddresses.member.1=" + Uri.EscapeDataString(userEmail);

            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService("subject of message",
                "this is the body in the message", "service@email.com", userEmail));

            _fakeResponseHandler.RequestContent.Should().Contain(expectedContent);
        }

        [Fact]
        public void EmailIsSentToTheServiceEmail()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            _fakeResponseHandler.AddFakeResponse(_amazonUri, _dummyOkResponse);
            var serviceEmail = "service@email.com, another@email.com";
            string expectedContent = $"Destination.ToAddresses.member.1={Uri.EscapeDataString("service@email.com")}&Destination.ToAddresses.member.2={Uri.EscapeDataString("another@email.com")}";

            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService("emailsubject",
                "emailbody", serviceEmail, "user@email"));

            _fakeResponseHandler.RequestContent.Should().Contain(expectedContent);
        }

        [Fact]
        public void EmailIsSentToAllServiceEmails()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            _fakeResponseHandler.AddFakeResponse(_amazonUri, _dummyOkResponse);
            var serviceEmail = "service@email.com";
            string expectedContent = $"Destination.ToAddresses.member.1=" + Uri.EscapeDataString(serviceEmail);

            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService("emailsubject",
                "emailbody", serviceEmail, "user@email"));

            _fakeResponseHandler.RequestContent.Should().Contain(expectedContent);
        }

        [Fact]
        public void IfServiceEmailIsEmptySomethingHasGoneWrong()
        {
            // This check can be moved to the contact us details model when the toggle has been removed. Empty serviceEmail is valid when using deprecated send email method.
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            _fakeResponseHandler.AddFakeResponse(_amazonUri, _dummyOkResponse);

            var httpStatusCode = AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService("emailsubject",
                "emailbody", string.Empty, "user@email"));

            httpStatusCode.Should().Be(HttpStatusCode.InternalServerError);
            LogTesting.Assert(_mockLogger, LogLevel.Error,
               $"ServiceEmail can not be null or empty. No email has been sent.");
        }

        [Fact]
        public void ItLogsTheAmazonMessageIdWhenTheStatusCodeIsOk()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            const string messageId = "test-message-id";
            const string requestId = "test-request-id";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("<SendEmailResponse xmlns=\"http://fakeurl/\">" +
                                            "<SendEmailResult>" +
                                            $"<MessageId>{messageId}</MessageId>" +
                                            "</SendEmailResult>" +
                                            "<ResponseMetadata>" +
                                            $"<RequestId>{requestId}</RequestId>" +
                                            "</ResponseMetadata>" +
                                            "</SendEmailResponse>")
            };

            _fakeResponseHandler.AddFakeResponse(_amazonUri, response);

            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService(TestHelper.AnyString,
                TestHelper.AnyString,
                TestHelper.AnyString,
                TestHelper.AnyString));

            LogTesting.Assert(_mockLogger, LogLevel.Information,
                $"An email was sent to Amazon SES with message id: {messageId} and request id {requestId}");
        }

        [Fact]
        public void ItLogsCouldntExtractWhenMessageOrRequestIdIsNotPresentForOkResponse()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            const string messageId = "test-message-id";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("<SendEmailResponse xmlns=\"http://fakeurl/\">" +
                                            "<SendEmailResult>" +
                                            $"<MessageId>{messageId}</MessageId>" +
                                            "</SendEmailResult>" +
                                            "<ResponseMetadata>" +
                                            "</ResponseMetadata>" +
                                            "</SendEmailResponse>")
            };

            _fakeResponseHandler.AddFakeResponse(_amazonUri, response);

            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService(TestHelper.AnyString,
                TestHelper.AnyString,
                TestHelper.AnyString,
                TestHelper.AnyString));

            LogTesting.Assert(_mockLogger, LogLevel.Warning, "Could not extract message id or request id from Amazon SES response.");
        }

        [Fact]
        public void ItLogsEntireErrorResponseFromAmazonForErrorResponse()
        {
            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, _mockEmailConfiguration.Object, _businessId);
            var errorResponse = new StringBuilder();
            errorResponse.Append("<ErrorResponse>");
            errorResponse.Append("<Error>");
            errorResponse.Append("<Type>Sender</Type>");
            errorResponse.Append("<Code>ValidationError</Code>");
            errorResponse.Append("<Message>Value null at 'message.subject' failed to satisfy constraint: Member must not be null</Message>");
            errorResponse.Append("</Error>");
            errorResponse.Append("<RequestId>test-request-id</RequestId>");
            errorResponse.Append("</ErrorResponse>");
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(errorResponse.ToString())
            };

            _fakeResponseHandler.AddFakeResponse(_amazonUri, response);

            AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService("subject","body","user@email"));

            LogTesting.Assert(_mockLogger, LogLevel.Error, $"An error occurred trying to send an email to Amazon SES. \n{errorResponse}");
        }

        [Fact]
        public void ShouldReturnA500AndLogItIfTheEmailConfigIsNotValid()
        {
            var invalidConfig = new AmazonSesClientConfiguration(AppSetting.GetAppSetting(null), AppSetting.GetAppSetting(null), AppSetting.GetAppSetting(null),
                new AmazonSESKeys(null, null));
            var mockEmailConfiguration = new Mock<IEmailConfigurationBuilder>();
            mockEmailConfiguration.Setup(o => o.Build(_businessId.ToString())).Returns(invalidConfig);

            _emailClient = new HttpEmailClient(_mockAuthHeader.Object, () => new HttpClient(_fakeResponseHandler), _mockLogger.Object, mockEmailConfiguration.Object, _businessId);

            var httpStatusCode = AsyncTestHelper.Resolve<HttpStatusCode>(_emailClient.SendEmailToService(TestHelper.AnyString, TestHelper.AnyString, TestHelper.AnyString,
                TestHelper.AnyString));
            httpStatusCode.Should().Be(HttpStatusCode.InternalServerError);

            LogTesting.Assert(_mockLogger, LogLevel.Error, $"The Amazon SES client configuration is not valid. {invalidConfig.ValidityToString()}");
        }
    }
}