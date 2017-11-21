using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Logging;
using StockportWebapp.Builders;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.AmazonSES
{
    public interface IHttpEmailClient
    {        
        Task<HttpStatusCode> SendEmailToService(EmailMessage emailMessage);
        string GenerateEmailBodyFromHtml<T>(T details, string templateName = null);
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

        public string GenerateEmailBodyFromHtml<T>(T details, string templateName = null)
        {
            var template = !string.IsNullOrEmpty(templateName) ? templateName : typeof(T).Name;

            var layout = GetEmailTemplateForLayout();
            var body = GetEmailTemplateForBody(template);

            foreach (var property in typeof(T).GetProperties())
            {
                var tag = $"{{{{ {property.Name.ToLower()} }}}}";
                tag = tag.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
                var value = property.GetValue(details, null) == null ? string.Empty : property.GetValue(details, null);

                if (property.PropertyType == typeof(List<string>))
                {
                    if (value is List<string> items) value = string.Join(", ", items.ToArray()).Trim().TrimEnd(',');
                }
                else
                {
                    value = value.ToString().Replace("\r\n", "<br />");
                }

                body = body.Replace(tag, value.ToString());
            }

            var result = layout.Replace("{{ MAIN_BODY }}", body);

            return result;
        }

        private static string GetEmailTemplateForLayout()
        {
            return new FileReader().GetStringResponseFromFile("StockportWebapp.Emails.Templates._Layout.html");
        }

        private static string GetEmailTemplateForBody(string template)
        {
            return new FileReader().GetStringResponseFromFile($"StockportWebapp.Emails.Templates.{template}.html");
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