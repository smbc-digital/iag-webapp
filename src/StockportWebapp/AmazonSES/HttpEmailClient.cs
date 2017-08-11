using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using StockportWebapp.Builders;
using StockportWebapp.Models;

namespace StockportWebapp.AmazonSES
{
    public interface IHttpEmailClient
    {        
        Task<HttpStatusCode> SendEmailToService(EmailMessage emailMessage);
    }

    public class HttpEmailClient : IHttpEmailClient
    {
        private readonly IAmazonSimpleEmailService _amazonSimpleEmailService;
        private readonly ILogger<HttpEmailClient> _logger;
        private readonly IEmailBuilder _emailBuilder;
        private readonly bool _sendAmazonEmails;

        public HttpEmailClient(ILogger<HttpEmailClient> logger,IEmailBuilder emailBuilder, IAmazonSimpleEmailService amazonSimpleEmailService, bool sendAmazonEmails)
        {
            _logger = logger;
            _emailBuilder = emailBuilder;
            _amazonSimpleEmailService = amazonSimpleEmailService;
            _sendAmazonEmails = sendAmazonEmails;
        }

        public async Task<HttpStatusCode> SendEmailToService(EmailMessage emailMessage)
        {
            if (string.IsNullOrEmpty(emailMessage.ToEmail))
            {
                _logger.LogError("ToEmail can not be null or empty. No email has been sent.");
                return HttpStatusCode.InternalServerError;
            }

            var result =  await SendEmail(emailMessage);

            return result.HttpStatusCode;
        }

        private async Task<SendRawEmailResponse> SendEmail(EmailMessage emailMessage)
        {
            var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(_emailBuilder.BuildMessageToStream(emailMessage)) };

            try
            {
                SendRawEmailResponse response = new SendRawEmailResponse { HttpStatusCode = HttpStatusCode.OK };

                if (_sendAmazonEmails) {
                    response = await _amazonSimpleEmailService.SendRawEmailAsync(sendRequest);
                }

                LogResponse(response);

                return response;
            }
            catch (Exception exception)
            {
                _logger.LogError($"An error occurred trying to send an email to Amazon SES. \n{exception.Message}");
                return new SendRawEmailResponse {HttpStatusCode = HttpStatusCode.BadRequest};
            }
        }

        private void LogResponse(SendRawEmailResponse response)
        {
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"An email was sent to Amazon SES with message id: {response.MessageId} and request id {response.ResponseMetadata?.RequestId}");
            }
            else
            {
                _logger.LogWarning($"There was a problem sending an email, message id: {response.MessageId} and request id: {response.ResponseMetadata?.RequestId} and status code {response.HttpStatusCode}");
            }
        }
    }
}